using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Managers;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Services;
using AutoBuildApp.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Connor_s_Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ReviewRatingDAO _reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");

            ReviewRatingService reviewRatingService = new ReviewRatingService(_reviewRatingDAO);

            // This will start a manager and pass in the service.
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            //formData.append("buildId", sessionStorage.getItem('buildId').toString());
            //formData.append("username", addUserNameTextbox.value.trim());
            //formData.append("starRating", parseInt(starValue));
            //formData.append("message", addMessageTextbox.value.trim());
            //formData.append("image", photo);
            //post.BuildType = (BuildType)int.Parse(data["buildType"]);



            var reviewRating = new ReviewRating()
            {
                EntityId = "30004",
                StarRating = StarType.Five_Stars,
                Message = "MSG"
            };

            //Console.WriteLine(createResult);
        }
    }
}

