using Markdig;

using Xunit;

namespace FlipLeaf.Core.Text.MarkdigSpecifics
{
    public class WikiLinkExtensionTests
    {
        [Fact]
        public void Integration_Test_01()
        {
            var p = new MarkdownPipelineBuilder()
                .Use<WikiLinkExtension>()
                .Build();

            var result = Markdown.ToHtml("[[Hello World]]", p);

            Assert.Equal("<p><a href=\"Hello_World.html\">Hello World</a></p>\n", result);
        }

        [Fact]
        public void Integration_Test_02()
        {
            var p = new MarkdownPipelineBuilder()
                .Use<WikiLinkExtension>()
                .Build();

            var result = Markdown.ToHtml("[[Url|Text]]", p);

            Assert.Equal("<p><a href=\"Url.html\">Text</a></p>\n", result);
        }

        [Fact]
        public void Integration_Test_03()
        {
            var p = new MarkdownPipelineBuilder()
                .Use(new WikiLinkExtension { IncludeTrailingCharacters = true })
                .Build();

            var result = Markdown.ToHtml("[[Text]]s", p);

            Assert.Equal("<p><a href=\"Text.html\">Texts</a></p>\n", result);
        }
    }

}
