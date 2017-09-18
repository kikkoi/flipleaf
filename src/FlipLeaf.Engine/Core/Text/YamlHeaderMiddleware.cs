using System.Data;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{

    public class YamlHeaderMiddleware : ITextMiddleware
    {
        private readonly YamlParser _yaml;

        public YamlHeaderMiddleware(YamlParser yaml) => _yaml = yaml;

        public async Task InvokeAsync(ITextInputContext ctx, TextMiddlewareAsyncDelegate next)
        {
            // 1) yaml header
            var newContent = ctx.Content;
            bool parsed;
            object pageContext;
            try
            {
                parsed = _yaml.ParseHeader(ref newContent, out pageContext);
            }
            catch (SyntaxErrorException see)
            {
                throw new ParseException($"The YAML header of the page is invalid", see);
            }

            if (parsed)
            {
                ctx.Content = newContent;
                ctx.PageContext = pageContext;
            }

            await next().ConfigureAwait(false);
        }
    }
}
