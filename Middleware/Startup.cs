using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /*
                Middleware Demo
             */

            // Middleware is basically code that processes requests and responses. 

            /*
                app.Run(async (context) =>
            {
                // Response, in this case, print Hello World1. 
                // Terminating middleware as it does not allow other middleware to execute
                await context.Response.WriteAsync("Hello World1");
            });

                app.Run(async (context) =>
            {
                // Response, not run
                await context.Response.WriteAsync("Hello World2");
            });
             */

            // Case where the middleware can run other middleware. 
            app.Use(async (context, next) =>
            {
                // Response
                await context.Response.WriteAsync("Hello World1");
                // Runs next middleware. 
                await next();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World2");
            });

            // Figuring out how to implement it to logging and SQL database

        }
    }
}
