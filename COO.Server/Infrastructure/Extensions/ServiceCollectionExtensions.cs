using COO.Business.Behaviors;
using COO.Server.Infrastructure.Services.Email;
using COO.Server.Infrastructure.Services.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
    using System.Text;

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
                .AddDbContextFactory<CooDbContext>(options => options
                .UseNpgsql(
                    configuration.GetDefaultConnectionString()));

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            => services
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IDataHashService, DataHashService>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

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

        [System.Obsolete]
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
