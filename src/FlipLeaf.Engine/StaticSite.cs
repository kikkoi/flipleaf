﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlipLeaf.Core;

namespace FlipLeaf
{
    public class StaticSite : IStaticSite
    {
        private static readonly Dictionary<IInputSource, List<IPipeline>> _pipelines = new Dictionary<IInputSource, List<IPipeline>>();

        private static readonly char[] _directorySeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        private readonly ConcurrentBag<InputItems> _pageItems = new ConcurrentBag<InputItems>();

        public StaticSite()
        {
            RootDirectory = InputDirectory = Environment.CurrentDirectory;
        }

        public StaticSite(string directory)
        {
            RootDirectory = InputDirectory = Path.GetFullPath(directory);
            OutputDirectory = Path.Combine(RootDirectory, SiteConfiguration.Default.OutputDir);
        }

        public SiteConfiguration Configuration { get; private set; }

        /// <summary />
        public string RootDirectory { get; }

        public string InputDirectory { get; private set; }

        public string OutputDirectory { get; private set; }

        public IDictionary<string, object> Runtime { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public void SetOutputDirectory(string path)
        {
            OutputDirectory = Path.Combine(InputDirectory, path);

            Runtime["outputDir"] = OutputDirectory;
        }

        public void AddPipeline(IInputSource source, IPipeline pipeline)
        {
            if (!_pipelines.TryGetValue(source, out var sourcePipelines))
            {
                sourcePipelines = new List<IPipeline>();
                _pipelines[source] = sourcePipelines;
            }

            sourcePipelines.Add(pipeline);
        }

        public void LoadConfiguration()
        {
            var config = SiteConfiguration.LoadFromDisk(Path.Combine(InputDirectory, SiteConfiguration.DefaultFileName)) ?? SiteConfiguration.Default;

            if (!string.IsNullOrEmpty(config.InputDir))
            {
                InputDirectory = Path.Combine(InputDirectory, config.OutputDir);
                OutputDirectory = Path.Combine(RootDirectory, SiteConfiguration.Default.OutputDir);
            }

            if (!string.IsNullOrEmpty(config.OutputDir))
            {
                OutputDirectory = Path.Combine(InputDirectory, config.OutputDir);
            }

            Configuration = config;

            Configuration.ApplyTo(Runtime);
        }

        public async Task GenerateAsync()
        {
            var items = await Task
                .WhenAll(_pipelines.Select(pk => PrepareInputSource(pk.Key, pk.Value)))
                .ConfigureAwait(false);

            // Generate categories
            var categories = new Dictionary<string, List<InputItems>>(StringComparer.OrdinalIgnoreCase);

            foreach (var sourceItems in items)
            {
                foreach (var pageItems in sourceItems)
                {
                    PopulateCategories(pageItems, categories);
                }
            }

            Runtime["categories"] = categories;

            await Task
                .WhenAll(_pipelines.Select(pk => TransformInputSource(pk.Key, pk.Value)))
                .ConfigureAwait(false);
        }

        private Task<InputItems[]> PrepareInputSource(IInputSource inputSource, List<IPipeline> pipelines)
        {
            return Task.WhenAll(
                inputSource
                .Get(this)
                .Select(i =>
                {
                    foreach (var pipeline in pipelines)
                    {
                        if (pipeline.Accept(this, i))
                        {
                            return pipeline.PrepareAsync(this, i);
                        }
                    }

                    return Task.FromResult(InputItems.Empty);
                }));
        }

        private void PopulateCategories(InputItems pageItems, Dictionary<string, List<InputItems>> categories)
        {
            // register implicit catetegories
            if (pageItems.TryGetValue("path", out var pathObject) && pathObject is string path)
            {
                var dirName = Path.GetDirectoryName(path);

                foreach (var subDir in dirName.Split(_directorySeparators, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!categories.TryGetValue(subDir, out var categoryPages))
                    {
                        categoryPages = categories[subDir] = new List<InputItems>();
                    }

                    categoryPages.Add(pageItems);
                }
            }

            // register all explicit categories
            if (pageItems.TryGetValue("categories", out var categoriesObject) && categoriesObject is string[] pageCategories)
            {
                foreach (var pageCategory in pageCategories)
                {
                    if (!categories.TryGetValue(pageCategory, out var categoryPages))
                    {
                        categoryPages = categories[pageCategory] = new List<InputItems>();
                    }

                    categoryPages.Add(pageItems);
                }
            }
        }

        private Task TransformInputSource(IInputSource inputSource, List<IPipeline> pipelines)
        {
            return Task.WhenAll(
                inputSource
                .Get(this)
                .Select(i =>
                {
                    foreach (var pipeline in pipelines)
                    {
                        if (pipeline.Accept(this, i))
                        {
                            return pipeline.TransformAsync(this, i);
                        }
                    }

                    return Task.CompletedTask;
                }));
        }
    }
}
