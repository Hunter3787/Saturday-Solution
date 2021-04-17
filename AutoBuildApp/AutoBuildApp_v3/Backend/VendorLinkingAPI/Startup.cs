using AutoBuildSecure.WebApi.HelperFunctions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VendorLinkingAPI
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
                opts.AddPolicy(name: "CorsPolicy", builder =>
                {
                    builder.WithMethods("GET", "POST", "OPTIONS", "PUT")
                    .WithOrigins("http://127.0.0.1:5501")
                    .AllowAnyHeader();
                });
            });
            services.AddTransient<ClaimsPrincipal>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseMiddleware<JWT_Middleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });


        }
    }
}
