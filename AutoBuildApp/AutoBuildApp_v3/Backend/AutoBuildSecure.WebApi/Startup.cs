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
            services.AddTransient<UserPrinciple>();
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

            /// custom middle ware for JWT validation, first line of defense 
            app.Use(async (context, next) =>
            {
                if (!string.IsNullOrEmpty(context.Request.Headers[HeaderNames.Authorization]))
                {
                    var accessTokenReq = context.Request.Headers[HeaderNames.Authorization];
                    string accessToken = accessTokenReq;

                    Console.WriteLine($"The auth header: { accessTokenReq}");
                    string[] parse = accessToken.Split(' ');
                    accessToken = parse[1];
                    Console.WriteLine($"The auth header token: { accessToken}");
                    JWTValidator validateAuthorizationHeader = new JWTValidator(accessToken);

                    Console.WriteLine($"the access token: { accessTokenReq}");
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        if (validateAuthorizationHeader.IsValidJWT() == false)
                        {
                            // does not work but would like it to, had trouble invoking it to the page:
                            app.UseCustomErrors(env);
                            // alternative to the custom error page:
                            await context.Response.WriteAsync("JWT validation fail");
                        }
                    }
                }
                await next();
            });

            // middleware has an order of execution 

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });

        }
    }
}
