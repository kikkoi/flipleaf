using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{
    public interface ITextMiddleware
    {
        Task InvokeAsync(ITextInputContext context, TextMiddlewareAsyncDelegate next);
    }

    public delegate Task TextMiddlewareAsyncDelegate();
}
