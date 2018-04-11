using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace SynopticS
{
    public static class CommandEnvironmentExtensions
    {
        public static void Initialize(this ICommandEnvironment commandEnvironment, string applicationName, string contentRootPath, CommandRunnerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrEmpty(applicationName))
                throw new ArgumentException("A valid non-empty application name must be provided.", nameof(applicationName));
            if (string.IsNullOrEmpty(contentRootPath))
                throw new ArgumentException("A valid non-empty content root must be provided.", nameof(contentRootPath));
            if (!Directory.Exists(contentRootPath))
                throw new ArgumentException(string.Format("The content root '{0}' does not exist.", contentRootPath), nameof(contentRootPath));

            commandEnvironment.ApplicationName = applicationName;
            commandEnvironment.ContentRootPath = contentRootPath;
            commandEnvironment.ContentRootFileProvider = new PhysicalFileProvider(commandEnvironment.ContentRootPath);

            commandEnvironment.EnvironmentName = options.Environment ?? commandEnvironment.EnvironmentName;
        }
    }
}
