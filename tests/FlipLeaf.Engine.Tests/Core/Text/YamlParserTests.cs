using Xunit;

namespace FlipLeaf.Core.Text
{
    public class YamlParserTests
    {
        [Fact]
        public void Parse_Empty()
        {
            var content = string.Empty;
            var parser = new YamlParser();

            var p = parser.ParseHeader(ref content, out var pageContext);

            Assert.False(p);
        }

        [Fact]
        public void Parse_NewLine()
        {
            var content = "\n";
            var parser = new YamlParser();

            var p = parser.ParseHeader(ref content, out var pageContext);

            Assert.False(p);
        }

        [Fact]
        public void Parse_No_Header()
        {
            var content = "foobar\n";
            var parser = new YamlParser();

            var p = parser.ParseHeader(ref content, out var pageContext);

            Assert.False(p);
        }

        [Fact]
        public void Parse_Invalid_Header()
        {
            var content = "---\ncontent\n";
            var parser = new YamlParser();

            var p = parser.ParseHeader(ref content, out var pageContext);

            Assert.False(p);
        }

        [Fact]
        public void Parse_Empty_Header()
        {
            var content = "---\n---\ncontent\n";
            var parser = new YamlParser();

            var p = parser.ParseHeader(ref content, out var pageContext);

            Assert.False(p);
        }

        [Fact]
        public void Parse_Simple_Header()
        {
            var content = "---\nname: value\n---\n\n";
            var parser = new YamlParser();

            var p = parser.ParseHeader(ref content, out var pageContext);


            Assert.True(p);
            Assert.Equal("value", pageContext["name"]);
        }
    }
}
