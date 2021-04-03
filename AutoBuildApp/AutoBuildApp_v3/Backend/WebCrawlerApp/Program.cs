using AutoBuildApp.Services.WebCrawlerServices;
using System;
using System.Collections.Generic;

namespace WebCrawlerApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            string x = "hey";
            //List<string> newEggBlackList = new List<string>();
            //newEggBlackList.Add("ComboDealDetails");
            //newEggBlackList.Add("/p/");

            //List<string> amazonBlackList = new List<string>();
            //amazonBlackList.Add("picassoRedirect");

            WebCrawlerService wcs = new WebCrawlerService("Server = localhost; Database = DB; Trusted_Connection = True;");
            //wcs.testService();
            ////new egg
            //wcs.getAllInformationFromPage("https://www.newegg.com/amd-ryzen-7-3800xt-ryzen-7-3rd-gen/p/N82E16819113652", ".product-title", ".product-pane .price-current strong",
            //    ".product-buy-box #ProductBuy", ".table-horizontal tr th", ".table-horizontal tr td", ".comments-name", ".comments-title .comments-text",
            //    ".comments-content", "plusminusadd to cart");
            wcs.getAllInformationFromPage("https://www.amazon.com/XFX-Thicc-2025MHz-Graphics-Rx-57XT8TBD8/dp/B07ZP5QZX2/ref=sr_1_115?dchild=1&keywords=nvidia+rtx+2060+super&qid=1617371450&sr=8-115",
                "#productTitle", "#priceblock_ourprice", "availability", ".a-expander-content .prodDetSectionEntry", ".a-expander-content .prodDetAttrValue",
                ".a-profile-name", "[data-hook=review-date]", "[data-hook=review-collapsed]", "add");
        }
    }
}
