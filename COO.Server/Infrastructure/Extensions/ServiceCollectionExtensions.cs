using COO.Server.Controllers.Identity;
using COO.Server.Controllers.MMO;

namespace COO.Server.Infrastructure.Extensions
{
    using Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.OpenApi.Models;
    using Services;
    using FluentValidation.AspNetCore;
    using COO.DataAccess.Contexts;
    using COO.Infrastructure.Services.DataHash;

    public static class ServiceCollectionExtensions
    {
        public static AppSettings GetApplicationSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
            services.Configure<AppSettings>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppSettings>();
        }

        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDbContext<COODbContext>(options => options
                .UseNpgsql(
                    configuration.GetDefaultConnectionString()));

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            => services
                .AddTransient<IDataHashService, DataHashService>()
                .AddTransient<IEmailService, EmailService>();

        public static IServiceCollection AddSwagger(this IServiceCollection services)
            => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1", 
                    new OpenApiInfo
                    {
                        Title = "My COO API", 
                        Version = "v1"
                    });
            });

        public static void AddApiControllers(this IServiceCollection services)
            => services
                .AddControllers(options => options
                    .Filters
                    .Add<ModelOrNotFoundActionFilter>())
                    .AddFluentValidation(fv => {
                        fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    });
    }
}
