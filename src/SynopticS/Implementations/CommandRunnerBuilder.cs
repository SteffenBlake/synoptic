using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.IO;

namespace SynopticS
{
    public class CommandRunnerBuilder : ICommandRunnerBuilder
    {
        private CommandRunnerBuilderContext Context { get; }
        private List<Action<CommandRunnerBuilderContext, IServiceCollection>> ConfigureServicesDelegates { get; }
        private List<Action<CommandRunnerBuilderContext, IConfigurationBuilder>> ConfigureAppConfigurationBuilderDelegates { get; }
        private IConfiguration Config { get; }
        private ICommandEnvironment CommandEnvironment { get; }

        private CommandRunnerOptions Options { get; set; }

        public CommandRunnerBuilder()
        {
            ConfigureServicesDelegates = new List<Action<CommandRunnerBuilderContext, IServiceCollection>>();
            ConfigureAppConfigurationBuilderDelegates = new List<Action<CommandRunnerBuilderContext, IConfigurationBuilder>>();

            Config = new ConfigurationBuilder().AddEnvironmentVariables("ASPNETCORE_").Build();
            CommandEnvironment = new CommandEnvironment();

            Context = new CommandRunnerBuilderContext
            {
                Configuration = Config,
                CommandEnvironment = CommandEnvironment
            };
        }

        public ICommandRunnerBuilder ConfigureAppConfiguration(Action<CommandRunnerBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            if (configureDelegate == null)
                throw new ArgumentNullException(nameof(configureDelegate));

            ConfigureAppConfigurationBuilderDelegates.Add(configureDelegate);

            return this;
        }

        public ICommandRunnerBuilder ConfigureServices(Action<CommandRunnerBuilderContext, IServiceCollection> configureServices)
        {
            if (configureServices == null)
                throw new ArgumentNullException(nameof(configureServices));

            ConfigureServicesDelegates.Add(configureServices);

            return this;
        }

        public string GetSetting(string key)
        {
            return Config[key];
        }

        public ICommandRunnerBuilder UseSetting(string key, string value)
        {
            Config[key] = value;
            return this;
        }

        private bool _runnerBuilt;

        public ICommandRunner Build()
        {
            if (_runnerBuilt)
                throw new InvalidOperationException("CommandRunnerBuilder can only build a single instance of CommandRunner");

            _runnerBuilt = true;

            var serviceProvider = BuildCommonServices().BuildServiceProvider();

            return new CommandRunner(serviceProvider);
        }

        private IServiceCollection BuildCommonServices()
        {
            Options = new CommandRunnerOptions(Config);
            CommandEnvironment.Initialize(Options.ApplicationName, ResolveContentRootPath(Options.ContentRootPath, AppContext.BaseDirectory), Options);

            var services = new ServiceCollection();
            services.AddSingleton(Context);
            services.AddSingleton(CommandEnvironment);

            var configurationBuilder = new ConfigurationBuilder();

            ConfigureAppConfigurationBuilderDelegates.ForEach(d => d(Context, configurationBuilder));

            var configurationRoot = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configurationRoot);
            Context.Configuration = configurationRoot;

            services.AddTransient<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            ConfigureServicesDelegates.ForEach(d => d(Context, services));

            return services;
        }

        private static string ResolveContentRootPath(string contentRootPath, string basePath)
        {
            if (string.IsNullOrEmpty(contentRootPath))
                return basePath;

            if (Path.IsPathRooted(contentRootPath))
                return contentRootPath;

            return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
        }
    }
}
