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
        private HtmlDocument htmlDocument = new HtmlDocument();
        private List<Proxy> allProxies;
        private Proxy currentProxy;
        private const string PROXY_WEBSITE = "https://free-proxy-list.net/";
        private const string PROXY_WEBSITE_XPATH = "//*[@id=\"proxylisttable\"]/tbody/tr";
        private const int NUMBER_OF_PAGES_TO_CRAWL = 5;


        public WebCrawlerService(string connectionString)
        {
            this.webCrawlerDAO = new WebCrawlerDAO(connectionString);
            allProxies = getAllProxies(PROXY_WEBSITE, PROXY_WEBSITE_XPATH);
            currentProxy = allProxies[0];
        }
        public void testService(Product product)
        {
            webCrawlerDAO.PostProductToDatabase(product);
        }

        public WebCrawlerService(string connectionString, List<String> listOfLinks)
        {
            this.webCrawlerDAO = new WebCrawlerDAO(connectionString);
        }

        public List<string> grabHrefLinksFromPage(string url, string replaceDelimiter, string querySelectorString, string companyUrl, List<string> blacklistLinks)
        {
            List<string> hrefLinks = new List<string>();
            int pageNumber = 1;
            for (int i = 0; i < NUMBER_OF_PAGES_TO_CRAWL; i++)
            {
                htmlDocument.LoadHtml(getHtml(url));
                var links = htmlDocument.DocumentNode.QuerySelectorAll(querySelectorString);
                if (!links.Any())
                {
                    Console.WriteLine("OUt");
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
                            hrefLinks.Add(fullLink);
                        }
                    }
                }
                url = url.Replace(replaceDelimiter + pageNumber, replaceDelimiter + ++pageNumber);
            }
            return hrefLinks;
        }

        public void getAllInformationFromPage(string url, string companyName, string productType, string titleQuerySelector, string priceQuerySelector, string specsKeysQuerySelector,
                                                string specsValuesQuerySelector, string reviewerNameQuerySelector, string reviewDateQuerySelector,
                                                string ratingsContentQuerySelector)
        {
            bool validIP = false;
            while (!validIP)
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
                Product product = new Product(price != null, companyName, url, specsValues.ElementAt(modelNumberIndex).InnerText.Trim(), title.InnerText.Trim(), productType, specsValues.ElementAt(brandIndex).InnerText.Trim(), specsDictionary);
                webCrawlerDAO.PostProductToDatabase(product);
                break;
                //add attributes


            }
        }

        public string getHtml(string url)
        {
            bool successfulProxy = false;
            while (!successfulProxy)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 2000;
                    request.Proxy = (currentProxy == null) ? null : new WebProxy(currentProxy.IPAddress, currentProxy.Port);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    if (currentProxy != null) Console.WriteLine(currentProxy.Country + " - " + currentProxy.IPAddress + " - " + currentProxy.Port);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                Console.WriteLine("Successful proxy");
                                url = reader.ReadToEnd();
                                successfulProxy = true;
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    Console.WriteLine("Bad Proxy. Rotating...");
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
