using COO.Server.Middleware;
using FluentValidation;
using COO.Business.Logic.MMO.Write.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using COO.Server.Infrastructure.Extensions;
using MediatR;

namespace COO.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddMediatR(typeof(RegistrationCommandHandler).GetTypeInfo().Assembly)
                .AddValidatorsFromAssembly(typeof(RegistrationCommandValidator).Assembly)
                .AddDatabase(this.Configuration)
                .AddApplicationServices()
                .AddSwagger()
                .AddApiControllers();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwaggerUI()
                .UseRouting()
                .UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod())
                .UseMiddleware<ErrorHandlerMiddleware>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
