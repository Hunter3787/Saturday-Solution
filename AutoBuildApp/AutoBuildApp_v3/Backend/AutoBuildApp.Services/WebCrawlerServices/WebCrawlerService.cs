using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using AutoBuildApp.Models.Web_Crawler;

namespace AutoBuildApp.Services.WebCrawlerServices
{
    public class WebCrawlerService
    {
        private WebCrawlerDAO webCrawlerDAO;
        private string vendorName;
        private List<String> listOfLinksToCrawl;
        private string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
        private HtmlDocument htmlDocument = new HtmlDocument();
        private List<Proxy> allProxies;
        private Proxy currentProxy;
        private const string PROXY_WEBSITE = "https://free-proxy-list.net/";
        private const string PROXY_WEBSITE_XPATH = "//*[@id=\"proxylisttable\"]/tbody/tr";
        private const int NUMBER_OF_PAGES_TO_CRAWL = 5;


        public WebCrawlerService(string connectionString)
        {
            this.webCrawlerDAO = new WebCrawlerDAO(connectionString);
            listOfLinksToCrawl = new List<String>();
            allProxies = getAllProxies(PROXY_WEBSITE, PROXY_WEBSITE_XPATH);
            currentProxy = allProxies[0];
        }
        public void testService()
        {
        }

        public WebCrawlerService(string connectionString, List<String> listOfLinks)
        {
            this.webCrawlerDAO = new WebCrawlerDAO(connectionString);
            listOfLinksToCrawl = new List<String>();
        }

        public List<string> grabHrefLinksFromPage(string url, string replaceDelimiter, string querySelectorString, string companyUrl, List<string> blacklistLinks)
        {
            HashSet<string> linksVisited = new HashSet<string>();
            List<string> allHrefLinks = new List<string>();
            int pageNumber = 1;
            for (int i = 0; i < NUMBER_OF_PAGES_TO_CRAWL; i++)
            {
                string html = getHtml(url);
                htmlDocument.LoadHtml(html);
                var links = htmlDocument.DocumentNode.QuerySelectorAll(querySelectorString);
                // Somet
                if (links == null || !links.Any())
                {
                    rotateProxy();
                    i--;
                    continue;
                }
                foreach (var link in links)
                {
                    string fullLink = companyUrl + link.Attributes["href"].Value;
                    foreach (var blackListLink in blacklistLinks)
                    {
                        if (!fullLink.Contains(blackListLink))
                        {
                            allHrefLinks.Add(fullLink);
                            Console.WriteLine(link.Attributes["href"].Value);
                        }
                    }
                }
                url = url.Replace(replaceDelimiter + pageNumber, replaceDelimiter + ++pageNumber);
            }

            return allHrefLinks;
        }

        public void getAllInformationFromPage(string url, string companyName, string productType, string titleQuerySelector, string priceQuerySelector, string availabilityQuerySelector, string specsKeysQuerySelector,
                                                string specsValuesQuerySelector, string reviewerNameQuerySelector, string reviewDateQuerySelector,
                                                string ratingsContentQuerySelector, string addToCartText)
        {
            bool x = true;
            while (x)
            {
                //TODO individual star rating
                htmlDocument.LoadHtml(getHtml(url));
                var title = htmlDocument.DocumentNode.QuerySelector(titleQuerySelector);
                var price = htmlDocument.DocumentNode.QuerySelector(priceQuerySelector);
                var specsKeys = htmlDocument.DocumentNode.QuerySelectorAll(specsKeysQuerySelector);
                var specsValues = htmlDocument.DocumentNode.QuerySelectorAll(specsValuesQuerySelector);
                var ratingsReviewerName = htmlDocument.DocumentNode.QuerySelectorAll(reviewerNameQuerySelector);
                var ratingsDate = htmlDocument.DocumentNode.QuerySelectorAll(reviewDateQuerySelector);
                var ratingsContent = htmlDocument.DocumentNode.QuerySelectorAll(ratingsContentQuerySelector);

                // if any of these variables are empty, then we know something went wrong with the html read. Most likely, the proxy was flagged and thus the page was a catcha page.
                if (!specsKeys.Any() || !specsValues.Any() || !ratingsReviewerName.Any() || !ratingsDate.Any() || !ratingsContent.Any())
                {
                    rotateProxy();
                    continue;
                }

                Dictionary<string, string> specsDictionary = new Dictionary<string, string>();
                int keyCount = specsKeys.Count();
                int modelNumberIndex = 0;
                int brandIndex = 0;
                for (int i = 0; i < keyCount; i++)
                {
                    string key = specsKeys.ElementAt(i).InnerText;
                    string value = specsKeys.ElementAt(i).InnerText;
                    if (key.ToLower().Contains("model"))
                    {
                        modelNumberIndex = i;
                    }
                    if (key.ToLower().Contains("brand"))
                    {
                        brandIndex = i;
                    }
                    specsDictionary.Add(specsKeys.ElementAt(i).InnerText, specsValues.ElementAt(i).InnerText);
                }


                //create product
                Product product = new Product(price != null, companyName, url, specsValues.ElementAt(modelNumberIndex).InnerText, title.InnerText.Trim(), productType, specsValues.ElementAt(brandIndex).InnerText, specsDictionary);
                webCrawlerDAO.PostProductToDatabase(product);
                //add attributes


                // new egg good
                //var availability = htmlDocument.DocumentNode.QuerySelectorAll(availabilityQuerySelector);

                //amazon 
                //var title = htmlDocument.DocumentNode.QuerySelectorAll("");
                //var specsKey = htmlDocument.DocumentNode.QuerySelectorAll(".a-expander-content .prodDetSectionEntry");
                //var specsValue = htmlDocument.DocumentNode.QuerySelectorAll(".a-expander-content .prodDetAttrValue");
                //var starRatings = htmlDocument.DocumentNode.QuerySelectorAll(".comments-title .rating");
                //var ratingsContent2 = htmlDocument.DocumentNode.QuerySelectorAll(".comments-content");

                //if (ratingsDate == null || !ratingsDate.Any())
                //{
                //    rotateProxy();
                //    Console.WriteLine("rotated");
                //    continue;
                //}
                //var availability = htmlDocument.DocumentNode.QuerySelectorAll("#buybox .a-box-inner .a-button-stack");
                //string available = "Unavailable";
                // if(availability == null || !availability.Any())
                // {
                //     rotateProxy();
                // }
                // else
                // {
                //     foreach(var potential in availability)
                //     {
                //         string text = potential.InnerText.ToLower().Trim();
                //         if(text.Equals(addToCartText))
                //         {
                //             available = "Available";
                //             x = false;
                //             break;
                //         }
                //     }
                //     Console.WriteLine(available);

                // }
                //foreach (var specs in specsKey)
                //{
                //    Console.WriteLine(specs.InnerText.Trim());
                //}
                //foreach (var specsV in specsValue)
                //{
                //    Console.WriteLine(specsV.InnerText.Trim()); ;
                //}
                List<int> allRatingStarAmount = new List<int>();

                //foreach (var rating in starRatings)
                //{
                //    string ratingValue = rating.Attributes["class"].Value;
                //    int num = getRating(ratingValue);
                //    //int num = ratingValue[ratingValue.Length - 1] - '0';
                //    Console.WriteLine(num);
                //    allRatingStarAmount.Add(num);
                //}

                //foreach (var ratings in ratingsDate)
                //{
                //    Console.WriteLine(ratings.InnerText);
                //}
            }




            //Console.WriteLine(specsKey2.Count());
            //Console.WriteLine(specsValue2.Count());


            //Console.WriteLine(allRatingStarAmount.Count);
            //Console.WriteLine(ratingsContent.Count());
            //foreach(var content in ratingsContent)
            //{
            //    Console.WriteLine(content.InnerText);
            //}
        }

        public int getRating(string rating)
        {
            int length = rating.Length;
            for (int i = 0; i < length; i++)
            {
                switch (rating[i] - '0')
                {
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                    case 3:
                        return 3;
                    case 4:
                        return 4;
                    case 5:
                        return 5;
                }
            }
            return 0;
        }

        public string getHtml(string url)
        {
            Console.WriteLine("getHtml");
            bool successfulProxy = false;
            while (!successfulProxy)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 2000;
                    request.Proxy = (currentProxy == null) ? null : new WebProxy(currentProxy.IPAddress, currentProxy.Port);
                    //request.Proxy = new WebProxy("168.119.137.56", 3128);
                    //request.Proxy = new WebProxy("122.49.77.175", 3128);
                    //request.Proxy = new WebProxy("208.80.28.208", 8080);
                    //52.35.63.184, 20033;

                    if (currentProxy != null) Console.WriteLine(currentProxy.Country + " - " + currentProxy.IPAddress + " - " + currentProxy.Port);
                    //request.Proxy = (currentProxy == null) ? null : new WebProxy("183.88.219.206", 41564);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                url = reader.ReadToEnd();
                                successfulProxy = true;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetType().ToString());
                    rotateProxy();
                }
            }
            return url;
        }

        public List<Proxy> getAllProxies(string url, string xPath)
        {
            List<Proxy> proxies = new List<Proxy>();
            string html = getHtml(url);
            htmlDocument.LoadHtml(html);
            var links = htmlDocument.DocumentNode.SelectNodes(xPath);

            foreach (var link in links)
            {
                //if (link.ChildNodes[3].InnerHtml == "United States")
                //{
                    proxies.Add(new Proxy(link.ChildNodes[0].InnerHtml, Int32.Parse(link.ChildNodes[1].InnerHtml), link.ChildNodes[3].InnerHtml));
                //}
            }
            return proxies;
        }

        public void rotateProxy()
        {
            if (allProxies != null)
            {
                Console.WriteLine("total proxies remaining = " + allProxies.Count);
                allProxies.Remove(currentProxy);

                if (allProxies.Count == 10)
                {
                    // use my proxy to give a better chance at successfully getting all proxies.
                    currentProxy = null;
                    allProxies.AddRange(getAllProxies(PROXY_WEBSITE, PROXY_WEBSITE_XPATH));
                }
                currentProxy = allProxies[0];


            }

        }

    }
}
