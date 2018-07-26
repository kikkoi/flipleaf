using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{

    public class YamlHeaderPrepareMiddleware : ITextMiddleware
    {
        private readonly YamlParser _yaml;

        public YamlHeaderPrepareMiddleware(YamlParser yaml) => _yaml = yaml;

        public async Task InvokeAsync(TextInputContext ctx, TextMiddlewareAsyncDelegate next)
        {
            // 1) yaml header
            var newContent = ctx.Content;
            bool parsed;
            Dictionary<string, object> pageContext;
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

                // apply header keys
                foreach (var key in pageContext.Keys)
                {
                    ctx.Items[key] = pageContext[key];
                }
            }


            await next().ConfigureAwait(false);
        }
    }
}
