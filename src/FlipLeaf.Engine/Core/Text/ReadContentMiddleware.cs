using System.IO;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{
    public class ReadContentMiddleware : ITextMiddleware
    {
        public async Task InvokeAsync(TextInputContext ctx, TextMiddlewareAsyncDelegate next)
        {
            using (var inputStream = ctx.Input.Open(ctx.Site))
            using (var reader = new StreamReader(inputStream))
            {
                ctx.Content = reader.ReadToEnd();
            }

            await next().ConfigureAwait(false);
        }
    }
}
