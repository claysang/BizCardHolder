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

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appRoot = GetAppRootPath();
            var configPath = Path.Combine(appRoot, ClientAppProjectName);

            var configuration = Configuration(configPath, environmentName).Build();

            // var connectionString = configuration.GetConnectionString(ConfigKeyNpgsqlConnectionString);
            var dbUrl = configuration.GetConnectionString("url");
            var dbPort = configuration.GetConnectionString("port");
            var dbUser = configuration.GetConnectionString("user");
            var dbPwd = configuration.GetConnectionString("password");
            var connectionString = "User ID =" + dbUser + ";Password=" + dbPwd + ";Server=" + dbUrl + ";Port=" + dbPort + ";Database=leichao;Pooling=true;";

            return CreateDbContext(connectionString);
        }

        protected virtual IConfigurationBuilder Configuration(string basePath, string environmentName)
        {
            var jsonFile = environmentName == null ? "appsettings.json" : $"appsettings.{environmentName}.json";

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(jsonFile, true)
                .AddEnvironmentVariables();

            return builder;
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

            var relativePath = AppContext.BaseDirectory.Substring(0,
                AppContext.BaseDirectory.LastIndexOf("/bin", StringComparison.InvariantCultureIgnoreCase));
            var appRoot = relativePath.Substring(0,
                relativePath.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);

            return appRoot;
        }
    }
}
