using System.Threading.Tasks;

namespace FlipLeaf.Core
{
    public interface IInputTransform
    {
        Task TransformAsync(IStaticSite ctx, IInput input);
    }
}
