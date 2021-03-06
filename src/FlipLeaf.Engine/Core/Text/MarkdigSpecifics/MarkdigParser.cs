﻿using System.IO;
using Markdig;
using Markdig.Renderers;

namespace FlipLeaf.Core.Text.MarkdigSpecifics
{
    public class MarkdigParser
    {
        private readonly MarkdownPipelineBuilder _pipelineBuilder = new MarkdownPipelineBuilder();

        private MarkdownPipeline _pipeline;

        public MarkdigParser()
        {
        }

        /// <summary>
        /// Adds the specified extension to the extensions collection.
        /// </summary>
        public void Use<TExtension>() where TExtension : class, IMarkdownExtension, new()
        {
            _pipelineBuilder.Extensions.AddIfNotAlready<TExtension>();
        }

        /// <summary>
        /// Adds the specified extension instance to the extensions collection.
        /// </summary>
        public void Use<TExtension>(TExtension extension) where TExtension : class, IMarkdownExtension
        {
            _pipelineBuilder.Extensions.AddIfNotAlready(extension);
        }

        public bool Parse(ref string source)
        {
            if (_pipeline == null)
            {
                _pipeline = _pipelineBuilder.UseAdvancedExtensions().Build();
            }

            using (var writer = new StringWriter())
            {
                var renderer = new HtmlRenderer(writer);
                _pipeline.Setup(renderer);

                var doc = Markdown.Parse(source, _pipeline);

                renderer.Render(doc);

                writer.Flush();
                
                source = writer.ToString();
            }

            return true;
        }
    }
}
