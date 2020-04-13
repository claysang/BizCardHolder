using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace BizCard.Core.Data
{
    // https://stackoverflow.com/questions/47473277/entity-framework-core-migration-connection-string
    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public const string ConfigKeyNpgsqlConnectionString = "npgsqlConnectionString";

        private const string ClientAppProjectName = "BizCard.API";

        protected virtual IConfigurationBuilder Configuration(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environmentName}.json", true);
                
            return builder;
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appRoot = GetAppRootPath();
            var configPath = Path.Combine(appRoot, ClientAppProjectName);

            var configuration = Configuration(configPath, environmentName).Build();

            var connectionString = configuration.GetConnectionString(ConfigKeyNpgsqlConnectionString);

            return CreateDbContext(connectionString);
        }

        public ApplicationDbContext CreateDbContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty", nameof(connectionString));
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        private string GetAppRootPath()
        {
            var basePath = AppContext.BaseDirectory;

            var relativePath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("/bin", StringComparison.InvariantCultureIgnoreCase));
            var appRoot = relativePath.Substring(0, relativePath.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);

            return appRoot;
        }
    }
}
