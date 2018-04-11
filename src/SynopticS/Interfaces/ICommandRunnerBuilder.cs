using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SynopticS
{
    public interface ICommandRunnerBuilder
    {
        ICommandRunnerBuilder ConfigureAppConfiguration(Action<CommandRunnerBuilderContext, IConfigurationBuilder> configureDelegate);

        ICommandRunnerBuilder ConfigureServices(Action<CommandRunnerBuilderContext, IServiceCollection> configureServices);

        /// <summary>Get the setting value from the configuration.</summary>
        /// <param name="key">The key of the setting to look up.</param>
        /// <returns>The value the setting currently contains.</returns>
        string GetSetting(string key);

        /// <summary>Add or replace a setting in the configuration.</summary>
        /// <param name="key">The key of the setting to add or replace.</param>
        /// <param name="value">The value of the setting to add or replace.</param>
        ICommandRunnerBuilder UseSetting(string key, string value);

        /// <summary>
        /// Builds an <see cref="T:SynopticS.CommandRunner" /> which can process argument arrays./
        /// </summary>
        ICommandRunner Build();
    }
}
