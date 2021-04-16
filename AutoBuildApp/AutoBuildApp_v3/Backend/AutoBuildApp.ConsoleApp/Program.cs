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


            //var queryBy1 = "AscendingLikes";
            //var queryBy2 = "BuildType_GraphicArtist";
            //var queryBy3 = "BuildType_Gaming";
            //var queryBy4 = "BuildType_WordProcessing";
            //var queryBy5 = "RandomSHIIIIIIZZZ";

            //var list = mostPopularBuildsManager.GetBuildPosts(queryBy5, queryBy2);

            //foreach (var item in list)
            //{
            //    //Console.WriteLine(item.EntityId);
            //    //Console.WriteLine(item.Username);
            //    //Console.WriteLine(item.Title);
            //    //Console.WriteLine(item.Description);
            //    Console.WriteLine("Likes: "+item.LikeIncrementor + " Build Type:" + item.BuildType);
            //    //Console.WriteLine(item.BuildImagePath);
            //    //Console.WriteLine(item.DateTime);
            //    Console.WriteLine();
            //}

            for(var i = 2; i < 10; i++)
            {
                var like = new Like()
                {
                    PostId = "30008",
                    UserId = i.ToString()
                };
                var result = mostPopularBuildsManager.AddLike(like);
                Console.WriteLine(result);
            }

            var rut = mostPopularBuildsManager.GetBuildPost("30002");

            Console.WriteLine(rut.EntityId);

            //var result = mostPopularBuildsManager.addLike(like);

            Console.WriteLine("Hello World");

            Console.Read();
        }
    }
}
