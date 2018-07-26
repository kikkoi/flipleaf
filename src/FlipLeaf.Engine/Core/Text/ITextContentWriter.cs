using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{
    public interface ITextContentWriter
    {
        Task InvokeAsync(TextInputContext ctx);
    }
}
