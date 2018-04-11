using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SynopticS
{
    public class DelegateStartup : StartupBase<IServiceCollection>
    {
        private Action<IApplicationBuilder> ConfigureApp { get; }

        public DelegateStartup(IServiceProviderFactory<IServiceCollection> factory, Action<IApplicationBuilder> configureApp)
            : base(factory)
        {
            ConfigureApp = configureApp;
        }

        public override void Configure(IApplicationBuilder app) => ConfigureApp(app);
    }
}