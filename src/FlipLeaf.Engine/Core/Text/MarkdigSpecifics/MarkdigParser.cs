using Markdig;

namespace FlipLeaf.Core.Text.MarkdigSpecifics
{
    public class MarkdigParser
    {
        private MarkdownPipeline _pipeline;

        public MarkdigParser()
        {
        }

        public MarkdownPipelineBuilder PipelineBuilder { get; } = new MarkdownPipelineBuilder();

        /// <summary>
        /// Adds the specified extension to the extensions collection.
        /// </summary>
        public MarkdownPipelineBuilder Use<TExtension>() where TExtension : class, IMarkdownExtension, new()
        {
            PipelineBuilder.Extensions.AddIfNotAlready<TExtension>();
            return PipelineBuilder;
        }

        /// <summary>
        /// Adds the specified extension instance to the extensions collection.
        /// </summary>
        public MarkdownPipelineBuilder Use<TExtension>(TExtension extension) where TExtension : class, IMarkdownExtension
        {
            PipelineBuilder.Extensions.AddIfNotAlready(extension);
            return PipelineBuilder;
        }

        public bool Parse(ref string source)
        {
            if(_pipeline == null)
            {
                _pipeline = PipelineBuilder.Build();
            }

            source = Markdown.ToHtml(source, _pipeline);

            return true;
        }
    }
}
