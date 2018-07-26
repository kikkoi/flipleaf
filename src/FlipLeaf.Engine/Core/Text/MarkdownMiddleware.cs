using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{
    public class MarkdownMiddleware : ITextMiddleware
    {
        private readonly MarkdigSpecifics.MarkdigParser _md;

        public MarkdownMiddleware(MarkdigSpecifics.MarkdigParser md) => _md = md;

        public async Task InvokeAsync(TextInputContext ctx, TextMiddlewareAsyncDelegate next)
        {
            var newContent = ctx.Content;
            if (!_md.Parse(ref newContent))
            {
                // TODO Raise exception
            }

            ctx.Content = newContent;

            await next().ConfigureAwait(false);
        }
    }
}
