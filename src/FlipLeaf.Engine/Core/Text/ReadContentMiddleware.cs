using System.IO;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{
    public class ReadContentMiddleware : ITextMiddleware
    {
        public async Task InvokeAsync(ITextInputContext ctx, TextMiddlewareAsyncDelegate next)
        {
            using (var inputStream = ctx.Input.Open(ctx.FlipContext))
            using (var reader = new StreamReader(inputStream))
            {
                ctx.Content = reader.ReadToEnd();
            }

            await next().ConfigureAwait(false);
        }
    }
}
