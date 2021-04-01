using COO.Server.Middleware;
using FluentValidation;

namespace COO.Server
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddDatabase(this.Configuration)
                .AddValidatorsFromAssembly(typeof(Startup).Assembly)
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
                })
                .ApplyMigrations();
        }
    }
}
