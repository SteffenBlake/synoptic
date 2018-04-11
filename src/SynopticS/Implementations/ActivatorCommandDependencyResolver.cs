using System;

namespace SynopticS
{
    internal class ActivatorCommandDependencyResolver : ICommandDependencyResolver
    {
        public object Resolve(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }
    }
}