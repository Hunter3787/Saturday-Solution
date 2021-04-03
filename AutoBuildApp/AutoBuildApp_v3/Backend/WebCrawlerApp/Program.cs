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
            wcs.getAllInformationFromPage("https://www.newegg.com/pny-geforce-rtx-3090-vcg309024tfxppb/p/N82E16814133807", ".product-title", ".product-pane .price-current strong",
                ".product-buy-box #ProductBuy", ".table-horizontal tr th", ".table-horizontal tr td", ".comments-name", ".comments-title .comments-text",
                ".comments-content", ".rating-views", ".rating-views-count", "plusminusadd to cart");
            //wcs.getAllInformationFromPage("https://www.amazon.com/MSI-GT-710-1GD3H-LP/dp/B01AZHOWL0/ref=sr_1_5?_encoding=UTF8&c=ts&dchild=1&keywords=Computer%2BGraphics%2BCards&qid=1617407423&s=pc&sr=1-5&ts_id=284822&th=1",
            //    "#productTitle", "#priceblock_ourprice", "availability", ".a-expander-content .prodDetSectionEntry", ".a-expander-content .prodDetAttrValue",
            //    ".a-profile-name", "[data-hook=review-date]", "[data-hook=review-collapsed]", "[data-hook=rating-out-of-text]", "[data-hook=total-review-count]", "add");
        }
    }
}
