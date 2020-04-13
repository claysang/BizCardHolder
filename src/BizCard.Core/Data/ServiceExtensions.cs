using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace BizCard.Core.Data
{
    public static class ServiceExtensions
    {
        public const string ConfigKeyNpgsqlConnectionString = "npgsqlConnectionString";

        public static void AddDataServices(this IServiceCollection services, IConfiguration appConfiguration)
        {
            var connStr = appConfiguration.GetConnectionString(ConfigKeyNpgsqlConnectionString);

            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connStr));

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }

        public static void EnsureDatabase(this IApplicationBuilder app, Action<string> databaseInitializer)
        {
            var services = app.ApplicationServices;

            var appConfiguration = services.GetService<IConfiguration>();

            var connStr = appConfiguration.GetConnectionString(ConfigKeyNpgsqlConnectionString);

            var dataSource = new NpgsqlConnection().DataSource;

            if (!File.Exists(dataSource))
            {
                services.GetService<IApplicationLifetime>()
                    .ApplicationStarted
                    .Register(() => databaseInitializer(connStr));
            }
        }
    }
}
