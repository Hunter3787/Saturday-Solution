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

            BuildPost buildPost = null;

            var result = mostPopularBuildsManager.PublishBuild(buildPost);

            //var result = mostPopularBuildsManager.addLike(like);

            Console.WriteLine(result);

            Console.Read();
        }
    }
}
