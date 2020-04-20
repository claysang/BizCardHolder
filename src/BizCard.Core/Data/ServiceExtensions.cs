using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BizCard.Core.Data
{
    public static class ServiceExtensions
    {
        private const string ConfigKeyNpgsqlConnectionString = "npgsqlConnectionString";

        public static void AddDataServices(this IServiceCollection services, IConfiguration appConfiguration)
        {
            var connStr = appConfiguration.GetConnectionString(ConfigKeyNpgsqlConnectionString);

            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
                options.UseInternalServiceProvider(serviceProvider).UseNpgsql(connStr)
            );

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }

        public static void EnsureDatabase(this IApplicationBuilder app, Action<string> databaseInitializer)
        {
            var services = app.ApplicationServices;
            var appConfiguration = services.GetService<IConfiguration>();
            var connStr = appConfiguration.GetConnectionString(ConfigKeyNpgsqlConnectionString);

            services.GetService<IApplicationLifetime>()
                .ApplicationStarted
                .Register(() => databaseInitializer(connStr));
        }
    }
}
