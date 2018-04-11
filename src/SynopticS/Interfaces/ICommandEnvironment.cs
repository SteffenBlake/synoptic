using Microsoft.Extensions.FileProviders;

namespace SynopticS
{
    public interface ICommandEnvironment
    {
        /// <summary>
        /// Gets or sets the name of the environment. This property is automatically set by the host to the value
        /// of the "ASPNETCORE_ENVIRONMENT" environment variable.
        /// </summary>
        string EnvironmentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the application. This property is automatically set by the host to the assembly containing
        /// the application entry point.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the application content files.
        /// </summary>
        string ContentRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="T:Microsoft.Extensions.FileProviders.IFileProvider" /> pointing at <see cref="P:SynopticS.ICommandEnvironment.ContentRootPath" />.
        /// </summary>
        IFileProvider ContentRootFileProvider { get; set; }
    }
}