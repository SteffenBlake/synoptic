using System;
using Microsoft.Extensions.Configuration;
using SynopticS.Resources;

namespace SynopticS
{
    public class CommandRunnerOptions
    {
        private IConfiguration Config { get; }

        public CommandRunnerOptions(IConfiguration config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private string _applicationName;
        public string ApplicationName => _applicationName ?? (_applicationName = Config[CommandRunnerDefaults.ApplicationKey]);

        private string _contentRootPath;
        public string ContentRootPath => _contentRootPath ?? (_contentRootPath = Config[CommandRunnerDefaults.ContentRootKey]);

        private string _environment;
        public string Environment => _environment ?? (_environment = Config[CommandRunnerDefaults.EnvironmentKey]);
    }
}