using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FeatureServices;
using System;

namespace AutoBuildApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();

            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

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

            var list = mostPopularBuildsManager.GetBuildPosts();

            foreach (var item in list)
            {
                Console.WriteLine(item.EntityId);
                Console.WriteLine(item.Username);
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Description);
                Console.WriteLine(item.LikeIncrementor);
                Console.WriteLine(item.BuildImagePath);
                Console.WriteLine(item.DateTime);
                Console.WriteLine();
            }

            Console.WriteLine(result);

            Console.WriteLine("Hello World");

            Console.Read();
        }
    }
}
