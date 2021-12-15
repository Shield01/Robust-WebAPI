using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Extensions;
using AutoMapper;
using LogService.Abstractions;
using BusinessLogic.Exception_Handling;
using WebApi.Action_Filters;
using AspNetCoreRateLimit;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/LogConfiguration.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config => {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                config.CacheProfiles.Add("SetDurationLimit", new CacheProfile { Duration = 120 });
            }).AddNewtonsoftJson()
            .ConfigureCSVFormatter();

            services.ConfigureCors();

            services.ConfigureIIS();

            services.ConfigureNlog();

            services.ConfigureDatabase(Configuration);

            services.ConfigureRepositoryManager();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<ValidationFilterAttributes>();

            services.AddScoped<ValidateCompanyExistAttribute>();

            services.AddScoped<ValidateCompanyExistForEmployeeController>();

            services.AddScoped<ValidateCompanyExistForGetEmployeesAction>();

            services.ConfigureVersioning();

            services.ConfigureCaching();

            services.ConfigureHeaderCaching();

            services.AddMemoryCache();

            services.ConfigureRateThrotlling();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogImplementations logImplementations)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureExceptions(logImplementations);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
