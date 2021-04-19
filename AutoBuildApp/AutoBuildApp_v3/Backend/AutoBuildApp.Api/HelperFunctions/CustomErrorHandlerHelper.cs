using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// References see /AuthReferences.txt.
/// </summary>
namespace AutoBuildApp.Api.HelperFunctions
{

    public class Problem
    {


        public int Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }

    }


    public static class CustomErrorHandlerHelper
    {
        public static void UseCustomErrors(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.Use(WriteDevelopmentResponse);
            }
            else
            {
                app.Use(WriteProductionResponse);
            }
        }

        private static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: true);

        private static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: false);


        private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
        {
            // there exists an exception handler middleware


            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();


            var ex = exceptionDetails?.Error;

            // Should always exist, but best to be safe!
            if (ex != null)
            {
                // ProblemDetails has it's own content type
                httpContext.Response.ContentType = "application/problem+json";

                // Get the details to display, depending on whether we want to expose the raw exception
                var title = includeDetails ? "An error occured: " + ex.Message : "An error occured";
                var details = includeDetails ? ex.ToString() : null;

                var problem = new Problem
                {
                    Status = 500,
                    Title = "this is   test",
                    Detail = details

                };
                //Serialize the problem details object to the Response as JSON (using System.Text.Json)

                // Send our modified content to the response body.
                await httpContext.Response.WriteAsync("this is a test");
                var stream = httpContext.Response.Body;
                await JsonSerializer.SerializeAsync(stream, problem);
            }



        }

    }



}