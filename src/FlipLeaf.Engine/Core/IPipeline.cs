using System.Threading.Tasks;

namespace FlipLeaf.Core
{
    public interface IPipeline
    {
        bool Accept(IStaticSite site, IInput input);

        Task<InputItems> PrepareAsync(IStaticSite site, IInput input);

        Task TransformAsync(IStaticSite site, IInput input);
    }
}
