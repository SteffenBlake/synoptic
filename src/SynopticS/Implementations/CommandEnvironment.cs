using Microsoft.Extensions.FileProviders;

namespace SynopticS
{
    public class CommandEnvironment : ICommandEnvironment
    {
        public string EnvironmentName { get; set; } = Resources.EnvironmentName.Production;
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}