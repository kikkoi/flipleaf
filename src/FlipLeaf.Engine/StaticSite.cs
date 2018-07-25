using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FlipLeaf.Core;

namespace FlipLeaf
{
    public class StaticSite : IStaticSite
    {
        public StaticSite()
        {
            InputDirectory = Environment.CurrentDirectory;
        }

        public StaticSite(string directory)
        {
            InputDirectory = Path.GetFullPath(directory);
            OutputDirectory = Path.Combine(InputDirectory, SiteConfiguration.Default.OutputDir);
        }

        public string InputDirectory { get; }

        public SiteConfiguration Configuration { get; private set; }

        public string OutputDirectory { get; private set; }

        public IList<IPipeline> Pipelines { get; } = new List<IPipeline>();

        public void LoadConfiguration()
        {
            var config = SiteConfiguration.LoadFromDisk(Path.Combine(InputDirectory, SiteConfiguration.DefaultFileName)) ?? SiteConfiguration.Default;

            if (!string.IsNullOrEmpty(config.OutputDir))
            {
                OutputDirectory = Path.Combine(InputDirectory, config.OutputDir);
            }

            Configuration = config;
        }

        public async Task GenerateAsync()
        {
            foreach (var pipeline in Pipelines)
            {
                await pipeline
                    .ExecuteAsync(this)
                    .ConfigureAwait(false);
            }
        }

    }
}
