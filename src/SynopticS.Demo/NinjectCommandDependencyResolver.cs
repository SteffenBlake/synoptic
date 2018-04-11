using System;

namespace SynopticS.Demo
{
    public class NinjectCommandDependencyResolver : ICommandDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectCommandDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object Resolve(Type serviceType)
        {
            return _kernel.Get(serviceType);
        }
    }
}