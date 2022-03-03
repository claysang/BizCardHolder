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
            // var connStr = appConfiguration.GetConnectionString(ConfigKeyNpgsqlConnectionString);
            var dbUser = appConfiguration.GetConnectionString("user");
            var dbPwd = appConfiguration.GetConnectionString("password");
            var connStr = "User ID =" + dbUser + ";Password=" + dbPwd + ";Server=database;Port=5432;Database=leichao;Pooling=true;";

            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
                options.UseInternalServiceProvider(serviceProvider).UseNpgsql(connStr)
            );

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }

        public static void EnsureDatabase(this IApplicationBuilder app, Action<string> databaseInitializer)
        {
            var services = app.ApplicationServices;
            var appConfiguration = services.GetService<IConfiguration>();
            var dbUser = appConfiguration.GetConnectionString("user");
            var dbPwd = appConfiguration.GetConnectionString("password");
            var connStr = "User ID =" + dbUser + ";Password=" + dbPwd + ";Server=database;Port=5432;Database=leichao;Pooling=true;";

            services.GetService<IApplicationLifetime>()
                .ApplicationStarted
                .Register(() => databaseInitializer(connStr));
        }
    }
}
