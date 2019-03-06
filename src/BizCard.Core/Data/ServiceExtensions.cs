using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BizCard.Core.Data
{
    public static class ServiceExtensions
    {
        public const string ConfigKeySqliteConnectionString = "sqliteConnectionString";

        public static void AddDataServices(this IServiceCollection services, IConfiguration appConfiguration)
        {
            var connStr = appConfiguration.GetConnectionString(ConfigKeySqliteConnectionString);

            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlite(connStr)
            );

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }

        public static void EnsureDatabase(this IApplicationBuilder app, Action<string> databaseInitializer)
        {
            var services = app.ApplicationServices;

            var appConfiguration = services.GetService<IConfiguration>();

            var connStr = appConfiguration.GetConnectionString(ConfigKeySqliteConnectionString);

            var dataSource = new SqliteConnection(connStr).DataSource;

            if (!File.Exists(dataSource))
            {
                services.GetService<IApplicationLifetime>()
                    .ApplicationStarted
                    .Register(() => databaseInitializer(connStr));
            }
        }
    }
}
