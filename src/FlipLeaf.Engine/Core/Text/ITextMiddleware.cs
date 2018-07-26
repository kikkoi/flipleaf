using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{
    public interface ITextMiddleware
    {
        Task InvokeAsync(TextInputContext context, TextMiddlewareAsyncDelegate next);
    }

    public delegate Task TextMiddlewareAsyncDelegate();
}
