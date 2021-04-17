using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Services.WebCrawlerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nito.AsyncEx;
using PuppeteerSharp;
using System.Collections.Generic;
using System.Threading;

namespace AutoBuildApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Thread(() =>
            {
                AsyncContext.Run(async () => await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision));
                List<string> newEggBlackList = new List<string>();
                newEggBlackList.Add("ComboDealDetails");
                newEggBlackList.Add("newegg.com/p/");

                List<string> amazonBlackList = new List<string>();
                amazonBlackList.Add("picassoRedirect");

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    //new StartingLink("https://www.newegg.com/AMD-Motherboards/SubCategory/ID-22/Page-1?Tid=7625", "motherboard"),
                    //new StartingLink("https://www.newegg.com/Processors-Desktops/SubCategory/ID-343/Page-1?Tid=7671", "cpu")
                    new StartingLink("https://www.newegg.com/p/pl?cm_sp=Cat_Motherboards_2-_-Visnav-_-Intel-Motherboards_1&page=1&N=100007627", "motherboard"),
                    //new StartingLink("https://www.newegg.com/Desktop-Graphics-Cards/SubCategory/ID-48/Page-1?Tid=7709", "gpu"),
                    new StartingLink("https://www.newegg.com/Computer-Cases/SubCategory/ID-7/Page-1?Tid=7583", "case"),
                    new StartingLink("https://www.newegg.com/Power-Supplies/SubCategory/ID-58/Page-1?Tid=7657", "power supply"),
                    new StartingLink("https://www.newegg.com/Desktop-Memory/SubCategory/ID-147/Page-1?Tid=7611", "ram"),
                    new StartingLink("https://www.newegg.com/Internal-SSDs/SubCategory/ID-636/Page-1?Tid=11693", "ssd"),
                    new StartingLink("https://www.newegg.com/Desktop-Internal-Hard-Drives/SubCategory/ID-14/Page-1?Tid=167523", "hard drive")
                };
                WebCrawlerService wcs = new WebCrawlerService("Server = localhost; Database = DB; Trusted_Connection = True;");
                //List<string> allLinks = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync("https://www.amazon.com/s?k=Computer+CPU+Processors&i=computers&rh=n%3A229189&page=1&_encoding=UTF8&c=ts&qid=1617671746&ts_id=229189&ref=sr_pg_2",
                //    "page=", "Array.from(document.querySelectorAll('.a-size-mini.a-spacing-none.a-color-base.s-line-clamp-2 .a-link-normal.a-text-normal')).map(a=>a.href)", "https://www.amazon.com", amazonBlackList));

                //AsyncContext.Run(() => wcs.getAllInformationFromPageAsync("https://www.amazon.com/Intel-i7-9700K-Desktop-Processor-Unlocked/dp/B07HHN6KBZ/ref=sr_1_2?_encoding=UTF8&c=ts&dchild=1&keywords=Computer+CPU+Processors&qid=1618022193&s=pc&sr=1-2&ts_id=229189",
                //    "amazon", "cpu", "document.querySelector('#productTitle').innerText", "document.querySelector('#price_inside_buybox').innerText", "Array.from(document.querySelectorAll('.prodDetSectionEntry')).map(a=>a.innerText)",
                //    "Array.from(document.querySelectorAll('.prodDetSectionEntry')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('[data-hook=review] .a-profile-name')).map(a=>a.innerText)",
                //    "Array.from(document.querySelectorAll('[data-hook=review-date]')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('[data-hook=review-body]')).map(a=>a.innerText)"));
                //Console.ReadLine();
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                //Console.WriteLine(task.Count);
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
