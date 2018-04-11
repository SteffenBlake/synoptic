using System;
using System.ComponentModel;

namespace SynopticS.Demo
{
    public class StructureMapCommandDependencyResolver : ICommandDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapCommandDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object Resolve(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }
    }
}