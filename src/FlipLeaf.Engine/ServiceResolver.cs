using System;

namespace FlipLeaf
{
    public interface IResolver
    {
        T Resolve<T>();

        object Resolve(Type type);
    }
}
