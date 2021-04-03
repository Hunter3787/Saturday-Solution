using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Apis
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
                               .WithOrigins("http://localhost:8080") // Change this
                               .AllowAnyHeader();
                    });
                }
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(policyName: "CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "index.html");

                // Lecture: When building any web application, URL routing needs to account
                // for users going to invalid URLs within your application. It is best
                // practice to have a fallback route that the user will be redirected to if
                // they go to an invalid URL.
                endpoints.MapGet("/{*url}", async context =>
                {
                    if (!context.Response.HasStarted)
                    {
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = 200;
                    }

                    var file = new FileInfo(path);

                    await context.Response.SendFileAsync(new PhysicalFileInfo(file));
                    await context.Response.CompleteAsync();
                });

                endpoints.MapControllers();
            });
        }
    }
}
