﻿using Markdig;
using Markdig.Renderers;

namespace FlipLeaf.Core.Text.MarkdigSpecifics
{
    public class WikiLinkExtension : IMarkdownExtension
    {
        public WikiLinkExtension()
        {
        }

        public string Extension { get; set; } = ".html";

        public bool IncludeTrailingCharacters { get; set; } = false;

        public char WhiteSpaceUrlChar { get; set; } = '_';

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<WikiLinkParser>())
            {
                var parser = new WikiLinkParser()
                {
                    Extension = Extension,
                    IncludeTrailingCharacters = IncludeTrailingCharacters
                };

                pipeline.InlineParsers.Insert(0, parser);
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
        }
    }
}
