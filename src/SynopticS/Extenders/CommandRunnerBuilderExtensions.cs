using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SynopticS.Resources;

namespace SynopticS
{
    public static class CommandRunnerBuilderExtensions
    {
        public static ICommandRunnerBuilder Configure(this ICommandRunnerBuilder builder, Action<IApplicationBuilder> configureApp)
        {
            if (configureApp == null)
                throw new ArgumentNullException(nameof(configureApp));

            var name = configureApp.GetMethodInfo().DeclaringType.GetTypeInfo().Assembly.GetName().Name;
            return builder.UseSetting(CommandRunnerDefaults.ApplicationKey, name).ConfigureServices(services => services.AddSingleton(sp => new DelegateStartup(sp.GetRequiredService<IServiceProviderFactory<IServiceCollection>>(), configureApp) as IStartup));
        }

        /// <summary>Specify the startup type to be used by the web host.</summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> to configure.</param>
        /// <param name="startupType">The <see cref="T:System.Type" /> to be used.</param>
        /// <returns>The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</returns>
        public static ICommandRunnerBuilder UseStartup(this ICommandRunnerBuilder hostBuilder, Type startupType)
        {
            var name = startupType.GetTypeInfo().Assembly.GetName().Name;
            return hostBuilder.UseSetting(CommandRunnerDefaults.ApplicationKey, name).ConfigureServices(services =>
            {
                if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                {
                    services.AddSingleton(typeof(IStartup), startupType);
                }
                else
                {
                    services.AddSingleton(typeof(IStartup), sp =>
                    {
                        var requiredService = sp.GetRequiredService<ICommandEnvironment>();
                        return new ConventionBasedStartup(StartupLoader.LoadMethods(sp, startupType,
                            requiredService.EnvironmentName)) as object;
                    });
                }
            });
        }

        /// <summary>Specify the startup type to be used by the web host.</summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> to configure.</param>
        /// <typeparam name="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <returns>The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</returns>
        public static ICommandRunnerBuilder UseStartup<TStartup>(this ICommandRunnerBuilder hostBuilder) where TStartup : class
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }

        public static ICommandRunnerBuilder UseDefaultServiceProvider(this ICommandRunnerBuilder hostBuilder, Action<ServiceProviderOptions> configure)
        {
            return hostBuilder.UseDefaultServiceProvider((context, options) => configure(options));
        }

        public static ICommandRunnerBuilder UseDefaultServiceProvider(this ICommandRunnerBuilder hostBuilder, Action<CommandRunnerBuilderContext, ServiceProviderOptions> configure)
        {
            return hostBuilder.ConfigureServices((context, services) =>
            {
                var options = new ServiceProviderOptions();
                configure(context, options);
                services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(new DefaultServiceProviderFactory(options)));
            });
        }

        public static ICommandRunnerBuilder ConfigureAppConfiguration(this ICommandRunnerBuilder hostBuilder, Action<IConfigurationBuilder> configureDelegate)
        {
            return hostBuilder.ConfigureAppConfiguration((context, builder) => configureDelegate(builder));
        }

        public static ICommandRunnerBuilder ConfigureLogging(this ICommandRunnerBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices(collection => collection.AddLogging(configureLogging));
        }

        public static ICommandRunnerBuilder ConfigureLogging(this ICommandRunnerBuilder hostBuilder, Action<CommandRunnerBuilderContext, ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));
        }

        public static ICommandRunnerBuilder ConfigureServices(this ICommandRunnerBuilder hostBuilder, Action<IServiceCollection> configureServices)
        {
            if (hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));
            if (configureServices == null)
                throw new ArgumentNullException(nameof(configureServices));

            return hostBuilder.ConfigureServices((_, services) => configureServices(services));
        }
    }
}