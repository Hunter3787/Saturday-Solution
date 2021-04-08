using AutoBuildApp.DomainModels;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
using System;

namespace AutoBuildApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var mostPopularBuildsManager = new MostPopularBuildsManager();

            var buildPost = new BuildPost()
            {
                Username = "Billy",
                Title = "Gharam",
                Description = "Greatest build ever",
                LikeIncrementor  = 0,
                BuildType = BuildType.Gaming,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\3.jpg",
                DateTime = "2019"
            };

            var result = mostPopularBuildsManager.PublishBuild(buildPost);

            Console.WriteLine(result);

            Console.WriteLine("Hello World");

            Console.Read();
        }
    }
}
