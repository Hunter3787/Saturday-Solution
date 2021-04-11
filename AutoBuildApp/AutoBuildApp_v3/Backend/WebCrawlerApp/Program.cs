﻿using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Services.WebCrawlerServices;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;

namespace WebCrawlerApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<string> newEggBlackList = new List<string>();
            newEggBlackList.Add("ComboDealDetails");
            newEggBlackList.Add("newegg.com/p/");

            List<string> amazonBlackList = new List<string>();
            amazonBlackList.Add("picassoRedirect");

            WebCrawlerService wcs = new WebCrawlerService("Server = localhost; Database = DB; Trusted_Connection = True;");
            //List<string> allLinks = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync("https://www.amazon.com/s?k=Computer+CPU+Processors&i=computers&rh=n%3A229189&page=1&_encoding=UTF8&c=ts&qid=1617671746&ts_id=229189&ref=sr_pg_2",
            //    "page=", "Array.from(document.querySelectorAll('.a-size-mini.a-spacing-none.a-color-base.s-line-clamp-2 .a-link-normal.a-text-normal')).map(a=>a.href)", "https://www.amazon.com", amazonBlackList));

            //AsyncContext.Run(() => wcs.getAllInformationFromPageAsync("https://www.amazon.com/Intel-i7-9700K-Desktop-Processor-Unlocked/dp/B07HHN6KBZ/ref=sr_1_2?_encoding=UTF8&c=ts&dchild=1&keywords=Computer+CPU+Processors&qid=1618022193&s=pc&sr=1-2&ts_id=229189",
            //    "amazon", "cpu", "document.querySelector('#productTitle').innerText", "document.querySelector('#price_inside_buybox').innerText", "Array.from(document.querySelectorAll('.prodDetSectionEntry')).map(a=>a.innerText)",
            //    "Array.from(document.querySelectorAll('.prodDetSectionEntry')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('[data-hook=review] .a-profile-name')).map(a=>a.innerText)",
            //    "Array.from(document.querySelectorAll('[data-hook=review-date]')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('[data-hook=review-body]')).map(a=>a.innerText)"));
            //Console.ReadLine();
            List<string> task = AsyncContext.Run(() => wcs.grabHrefLinksFromPageAsync("https://www.newegg.com/Processors-Desktops/SubCategory/ID-343/Page-1?Tid=7671", "Page-", "Array.from(document.querySelectorAll('.item-cell:not(.width-100) .item-img')).map(a=>a.href)", "", newEggBlackList));
            for (int i = 0; i < task.Count; i++)
            {
                AsyncContext.Run(() => wcs.getAllInformationFromPageAsync(task[i], "new egg", "cpu", "document.querySelector('.product-title').innerText;", "document.querySelector('.product-pane:not(.is-collapsed) :not(.product-raidobox) .price .price-current').innerText;",
                    "Array.from(document.querySelectorAll('.table-horizontal tr th')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.table-horizontal tr td')).map(a=>a.innerText);", "Array.from(document.querySelectorAll('.comments-name')).map(a=>a.innerText)",
                    "Array.from(document.querySelectorAll('.comments-title .comments-text')).map(a=>a.innerText)", "Array.from(document.querySelectorAll('.comments-content')).map(a=>a.innerText)"));
            }
            //double highest = 0;
            //double lowest = 500000;
            //double avg = 0;
            //foreach(var item in wcs.times)
            //{
            //    if(item.Value > highest)
            //    {
            //        highest = item.Value;
            //    }
            //    if(item.Value < lowest)
            //    {
            //        lowest = item.Value;
            //    }
            //    avg += item.Value;
            //}
            //Console.WriteLine("HIGHEST = " + highest);
            //Console.WriteLine("LOWEST = " + lowest);
            //Console.WriteLine("AVG = " + avg);
            //Console.WriteLine(task);
            //var task = AsyncContext.Run(() => wcs.getAllProxiesAsync());
            //Console.WriteLine(task.Count);
            //List<string> allLinks = wcs.grabHrefLinksFromPage("https://www.newegg.com/Desktop-Graphics-Cards/SubCategory/ID-48/Page-1?Tid=7709", "Page-", ".item-cell:not(.width-100) .item-img", "", newEggBlackList);
            ////List<string> allLinks = wcs.grabHrefLinksFromPage("https://www.amazon.com/s?k=Computer+CPU+Processors&i=computers&rh=n%3A229189&page=1&_encoding=UTF8&c=ts&qid=1617671746&ts_id=229189&ref=sr_pg_2", "page=", ".a-size-mini.a-spacing-none.a-color-base.s-line-clamp-2 .a-link-normal.a-text-normal", "https://www.amazon.com", amazonBlackList);

            //for (int i = 0; i < allLinks.Count; i++)
            //{
            //    wcs.getAllInformationFromPage(allLinks[i], "new egg", "cpu", ".product-title", ".product-pane .price-current strong",
            //        ".table-horizontal tr th", ".table-horizontal tr td", ".comments-name", ".comments-title .comments-text",
            //        ".comments-content");
            //    //wcs.getAllInformationFromPage(allLinks[i], "amazon", "cpu", "#productTitle", "#priceblock_ourprice", ".a-expander-content .prodDetSectionEntry", 
            //        //".a-expander-content .prodDetAttrValue", ".a-profile-name", "[data-hook=review-date]", "[data-hook=review-collapsed]");
            //}




            //Console.WriteLine("here");
            //Console.ReadLine();
            //wcs.grabHrefLinksFromPage("https://www.amazon.com/b?node=229189&ref=sr_nr_n_1", "page=", ".a-size-mini.a-spacing-none.a-color-base.s-line-clamp-2 .a-link-normal.a-text-normal", "https://www.amazon.com", amazonBlackList);

            //wcs.grabHrefLinksFromPage("https://www.newegg.com/Processors-Desktops/SubCategory/ID-343?Tid=7671", "Page-", ".item-cell .item-title", "", newEggBlackList);
            //wcs.getAllInformationFromPage("https://www.newegg.com/amd-ryzen-7-3800xt-ryzen-7-3rd-gen/p/N82E16819113652", "new egg", "cpu", ".product-title", ".product-pane .price-current strong",
            //    ".table-horizontal tr th", ".table-horizontal tr td", ".comments-name", ".comments-title .comments-text",
            //    ".comments-content");
            //wcs.getAllInformationFromPage("https://www.amazon.com/XFX-Thicc-2025MHz-Graphics-Rx-57XT8TBD8/dp/B07ZP5QZX2/ref=sr_1_115?dchild=1&keywords=nvidia+rtx+2060+super&qid=1617371450&sr=8-115",
            //"amazon", "cpu", "#productTitle", "#priceblock_ourprice", ".a-expander-content .prodDetSectionEntry", ".a-expander-content .prodDetAttrValue",
            //".a-profile-name", "[data-hook=review-date]", "[data-hook=review-collapsed]");
        }
    }
}
