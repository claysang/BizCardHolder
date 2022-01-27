using AutoMapper;
using BizCard.API.Mapping;
using BizCard.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BizCard.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins",
                                        builder =>
                                    {
                                        builder.WithOrigins("*");
                                        builder.AllowAnyHeader();
                                    });
            });

            services.AddMvc();

            services.AddDataServices(Configuration);

            services.AddAutoMapper(typeof(CardProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("_myAllowSpecificOrigins");

            app.UseHttpsRedirection();
            app.UseRouting();

            app.EnsureDatabase(connStr =>
            {
                var contextFactory = new ApplicationContextFactory();

                var dbContext = contextFactory.CreateDbContext(connStr);

                dbContext.Database.Migrate();
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
