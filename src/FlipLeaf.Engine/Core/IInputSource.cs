using System.Collections.Generic;

namespace FlipLeaf.Core
{
    public interface IInputSource
    {
        IEnumerable<IInput> Get(IStaticSite ctx);
    }
}
