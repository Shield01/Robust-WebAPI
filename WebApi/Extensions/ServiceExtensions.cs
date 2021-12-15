using AspNetCoreRateLimit;
using Infrastructure.Data_Transfer_Objects;
using Infrastructure.Database_Context;
using Infrastructure.Repository_Manager;
using LogService.Abstractions;
using LogService.Implementations;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });
        }

        public static void ConfigureIIS(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureNlog(this IServiceCollection services)
        {
            services.AddScoped<ILogImplementations, LogImplementations>();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InfrastructureDbContext>(options => options.UseSqlite(configuration.
                GetConnectionString("ProjectDatabase"), b => b.MigrationsAssembly("WebApi")));
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureCSVFormatter(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        public static void ConfigureCaching(this IServiceCollection services) => services.AddResponseCaching();

        public static void ConfigureHeaderCaching(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(
                (expirationOpt) =>
                {
                    expirationOpt.MaxAge = 65;
                    expirationOpt.CacheLocation = CacheLocation.Private;
                },
                (validationOpt) => 
                {
                    validationOpt.MustRevalidate = true;
                });
        }

        public static void ConfigureRateThrotlling(this IServiceCollection services)
        {
            var rateLimitRule = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 3,
                    Period = "5m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRule;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
