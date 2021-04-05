using AutoBuildApp.Models.Web_Crawler;
using AutoBuildApp.Services.WebCrawlerServices;
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

            List<string> allLinks = wcs.grabHrefLinksFromPage("https://www.newegg.com/Processors-Desktops/SubCategory/ID-343?Tid=7671", "Page-", ".item-cell:not(.width-100) .item-img", "", newEggBlackList);
            for (int i = 0; i < allLinks.Count; i++)
            {
                wcs.getAllInformationFromPage(allLinks[i], "new egg", "cpu", ".product-title", ".product-pane .price-current strong",
                    ".table-horizontal tr th", ".table-horizontal tr td", ".comments-name", ".comments-title .comments-text",
                    ".comments-content");
            }


            Console.WriteLine("here");
            Console.ReadLine();
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
