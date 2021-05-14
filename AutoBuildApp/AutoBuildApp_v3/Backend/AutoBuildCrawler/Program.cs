using AutoBuildApp.Api.Controllers;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Services.WebCrawlerServices;
using Microsoft.Extensions.Configuration;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoBuildApp.WebCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Connection String Setup
            string connStringName = ControllerGlobals.ADMIN_CREDENTIALS_CONNECTION;

            string connString = ConnectionManager.connectionManager.GetConnectionStringByName(connStringName);
            #endregion

            //AsyncContext.Run(async () => await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision));
            List<string> newEggBlackList = new List<string>();
            newEggBlackList.Add("ComboDealDetails");
            newEggBlackList.Add("newegg.com/p/");

            List<string> amazonBlackList = new List<string>();
            amazonBlackList.Add("picassoRedirect");

            const int STARTING_PAGE = 5;
            #region AMD Motherboard thread
            new Thread(() =>
            {

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/AMD-Motherboards/SubCategory/ID-22/Page-" + STARTING_PAGE + "?Tid=7625", ProductType.Motherboard.ToString()),
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region Intel Motherboard thread
            new Thread(() =>
            {

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/p/pl?cm_sp=Cat_Motherboards_2-_-Visnav-_-Intel-Motherboards_1&page=" + STARTING_PAGE + "&N=100007627", ProductType.Motherboard.ToString())
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region CPU thread
            new Thread(() =>
            {

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/Processors-Desktops/SubCategory/ID-343/Page-" + STARTING_PAGE + "?Tid=7671", ProductType.CPU.ToString())
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region GPU thread
            new Thread(() =>
            {
                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/Desktop-Graphics-Cards/SubCategory/ID-48/Page-" + STARTING_PAGE + "?Tid=7709", ProductType.GPU.ToString())
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region Case thread
            new Thread(() =>
            {
                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/Computer-Cases/SubCategory/ID-7/Page-" + STARTING_PAGE + "?Tid=7583", ProductType.Case.ToString())
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region PSU thread
            new Thread(() =>
            {

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/Power-Supplies/SubCategory/ID-58/Page-" + STARTING_PAGE + "?Tid=7657", ProductType.PSU.ToString()),
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region RAM thread
            new Thread(() =>
            {

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/Desktop-Memory/SubCategory/ID-147/Page-" + STARTING_PAGE + "?Tid=7611", ProductType.RAM.ToString()),
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region SSD thread
            new Thread(() =>
            {

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/Internal-SSDs/SubCategory/ID-636/Page-" + STARTING_PAGE + "?Tid=11693", ProductType.SSD.ToString()),
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion

            #region HDD thread
            new Thread(() =>
            {

                List<StartingLink> listOfLinks = new List<StartingLink>
                {
                    new StartingLink("https://www.newegg.com/Desktop-Internal-Hard-Drives/SubCategory/ID-14/Page-" + STARTING_PAGE + "?Tid=167523", ProductType.HDD.ToString())
                };

                WebCrawlerService wcs = new WebCrawlerService(connString);
                List<StartingLink> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync(listOfLinks, "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
                for (int i = 0; i < task.Count; i++)
                {
                    AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i].Link, "new egg", task[i].ComponentType, "document.querySelector('.product-view-img-original').src", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                        "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a => a.children.length != 0 && a.children[0].className == 'popover-question' ? a.children[0].children[1].innerText : a.innerText)", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-cell-side .comments-name')).map(a=>a.innerText)",
                        "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content:not(.comments-response .comments-content')).map(a=>a.innerText)"));
                }
            }).Start();
            #endregion    

        }
    }
}