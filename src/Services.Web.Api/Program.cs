using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Template.Services.Web.Api
{
    /// <summary>
    /// Startup program to run APIs.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main configuration.
        /// </summary>
        /// <param name="args">Settings parameters.</param>
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
