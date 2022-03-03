using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Mcrio.Configuration.Provider.Docker.Secrets;

namespace BizCard.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configBuilder =>
                    {
                      configBuilder.AddDockerSecrets(
                              allowedPrefixesEnvVariableName: "MY_SECRETS_PREFIXES"
                          );
                      // allow command line arguments to override docker secrets
                      //   if (args != null)
                      //   {
                      //     configBuilder.AddCommandLine(args);
                      //   }
                    })
                .UseStartup<Startup>();
        }
    }
}
