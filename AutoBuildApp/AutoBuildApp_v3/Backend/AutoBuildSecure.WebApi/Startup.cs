using AutoBuildApp.Security.Models;
using AutoBuildSecure.WebApi.HelperFunctions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;

namespace AutoBuildSecure.WebApi
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
                opts.AddPolicy(name: "CorsPolicy", builder =>
                {
                    builder.WithMethods("GET", "POST", "OPTIONS", "PUT")
                    .WithOrigins("http://127.0.0.1:5500")
                    .AllowAnyHeader();
                });
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });


            /// my custome middleware for jwt
            app.UseMiddleware<DemoMiddleware>();
        }
    }
}
