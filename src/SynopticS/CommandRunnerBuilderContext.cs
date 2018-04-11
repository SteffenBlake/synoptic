using Microsoft.Extensions.Configuration;

namespace SynopticS
{
    public class CommandRunnerBuilderContext
    {    
        /// <summary>
         /// The <see cref="T:SynopticS.ICommandEnvironment" /> initialized by the <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" />.
         /// </summary>
        public ICommandEnvironment CommandEnvironment { get; set; }

        /// <summary>
        /// The <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> containing the merged configuration of the application and the <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; }
    }
}
