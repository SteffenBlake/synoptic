using System;

namespace SynopticS
{
    public interface ICommandDependencyResolver
    {
        object Resolve(Type serviceType);
    }
}