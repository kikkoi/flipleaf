using System.Threading.Tasks;

namespace FlipLeaf.Core
{
    public interface IPipeline
    {
        Task ExecuteAsync(IStaticSite ctx);
    }
}
