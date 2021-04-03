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
            newEggBlackList.Add("/p/");

            List<string> amazonBlackList = new List<string>();
            amazonBlackList.Add("picassoRedirect");

            WebCrawlerService wcs = new WebCrawlerService("Server = localhost; Database = DB; Trusted_Connection = True;");
            wcs.testService();
            //new egg
            wcs.getAllInformationFromPage("https://www.newegg.com/amd-ryzen-7-3700x/p/N82E16819113567", ".product-title", ".product-pane .price-current strong",
                ".product-buy-box #ProductBuy", ".table-horizontal tr th", ".table-horizontal tr td", ".comments-name", ".comments-title .comments-text",
                ".comments-content", ".rating-views", ".rating-views-count", "plusminusadd to cart");
        }
    }
}
