using AutoBuildApp.Security.Models;
using AutoBuildApp.Api.HelperFunctions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace AutoBuildApp.Api
{
    /// <summary>
    /// the start up class is the start of the application configurations
    /// Implemented here:
    ///     - middleware handling JWT token validation check 
    ///     - Cors Functionality 
    /// References
    /// 
    /// </summary>
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
            services.AddCors(opts =>
            {
                // Lecture: Get the configuration from an external source to make
                // development and deployment easier to manage.
                var env = "DEV";//Configuration.GetSection("Environment");

                if (env == "DEV")
                {
                    // Lecture: Only use these CORS setting for development. Never deploy to 
                    // production with these settings.
                    opts.AddPolicy(name: "CorsPolicy", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                }
                else
                {

                    // Lecture: Make sure to specify the exact origins that you want to allow
                    // cross origin communication with your code.
                    opts.AddPolicy(name: "CorsPolicy", builder =>
                    {
                        builder.WithMethods("GET", "POST", "OPTIONS")
                               .WithOrigins("http://localhost") // Change this
                               .AllowAnyHeader();
                    });
                }
            });
            services.AddTransient<ClaimsPrincipal>();
            services.AddControllers();
        }


        //. This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            


            app.UseAuthorization();

            /// my custome middleware for jwt
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });


        }
    }
}
