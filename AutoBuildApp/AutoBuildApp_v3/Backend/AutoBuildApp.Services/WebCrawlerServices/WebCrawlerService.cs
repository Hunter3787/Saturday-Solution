﻿using AutoBuildApp.DataAccess;
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
using AutoBuildApp.Models.WebCrawler;
using PuppeteerSharp;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;

namespace AutoBuildApp.Services.WebCrawlerServices
{
    public class WebCrawlerService
    {
        private WebCrawlerDAO webCrawlerDAO;
        private HtmlDocument htmlDocument = new HtmlDocument();
        public List<Proxy> allProxies;
        private Proxy currentProxy;
        private const string PROXY_WEBSITE = "https://free-proxy-list.net/";
        private const string PROXY_WEBSITE_XPATH = "//*[@id=\"proxylisttable\"]/tbody/tr";
        private const string PROXY_WEBSITE_QUERYSELECTOR = "Array.from(document.querySelectorAll('#proxylisttable tbody [role=row]')).map(a => a.innerText)";
        private const int NUMBER_OF_PAGES_TO_CRAWL = 5;
        private LaunchOptions options;
        private NavigationOptions navigationOptions = new NavigationOptions
        {
            WaitUntil = new[]
            {
                WaitUntilNavigation.DOMContentLoaded
            }
        };
        private Dictionary<string, string> headers = new Dictionary<string, string>
        {
            {"referer", "https://www.google.com" },
            {"user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36"},
            {"accept", "text/html, application/xhtml+xml, application/xml;q=0.9" },
            {"accept-encoding", "gzip, deflate" },
            {"accept-language", "en-US,en;q=0.9" }
        };

        public WebCrawlerService(string connectionString)
        {
            this.webCrawlerDAO = new WebCrawlerDAO(connectionString);
            allProxies = AsyncContext.Run(() => getAllProxiesAsync());
            currentProxy = allProxies[0];
            options = new LaunchOptions()
            {
                Headless = true,
                IgnoreHTTPSErrors = true,
                Args = new[] {
                        $"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}",
                        //"--proxy-server=208.80.28.208:8080",
                        //"--proxy-server=183.88.226.50:8080",
                        "--no-sandbox",
                        "--disable-gpu",
                        "--ignore-certificate-errors",
                    }
            };
        }

        public WebCrawlerService(string connectionString, List<String> listOfLinks)
        {
            this.webCrawlerDAO = new WebCrawlerDAO(connectionString);
        }

        public async Task<bool> IsProxyValid()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://google.com");
            request.Proxy = new WebProxy(currentProxy.IPAddress, currentProxy.Port);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36";
            request.Timeout = 2000;            
            //LaunchOptions options = new LaunchOptions()

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }
            catch(Exception e)
            {
                Console.WriteLine("BAD - " + currentProxy.IPAddress + " - " + currentProxy.Port);
                allProxies.Remove(currentProxy);
                currentProxy = allProxies[0];
                return false;
            }
            Console.WriteLine("GOOD - " + currentProxy.IPAddress + " - " + currentProxy.Port);
            //{
            //    Headless = true,
            //    IgnoreHTTPSErrors = true,
            //    Args = new[] {
            //            $"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}",
            //            "--no-sandbox",
            //            "--disable-gpu",
            //            "--ignore-certificate-errors",
            //        }
            //};
            //using (var browser = await Puppeteer.LaunchAsync(options))
            //{
            //    using (Page page = await browser.NewPageAsync())
            //    {
            //        try
            //        {
            //            page.DefaultNavigationTimeout = 15000;
            //            await page.GoToAsync(url, navigationOptions);
            //        }
            //        catch(Exception e)
            //        {
            //            return false;
            //        }
            //    }
            //}
            return true;
        }

        public async Task<List<string>> grabHrefLinksFromPageAsync(string url, string replaceDelimiter, string querySelectorString, string companyUrl, List<string> blacklistLinks)
        {
            List<string> hrefLinks = new List<string>();
            //LaunchOptions options = new LaunchOptions()
            //{
            //    Headless = false,
            //    IgnoreHTTPSErrors = true,
            //    Args = new[] {
            //            $"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}",
            //            "--no-sandbox",
            //            "--disable-gpu",
            //            "--ignore-certificate-errors",
            //        }
            //};
            Browser browser = await Puppeteer.LaunchAsync(options);

            int repeat = 0;
            for (int i = 0; i < NUMBER_OF_PAGES_TO_CRAWL; i++)
            {
                try 
                {
                    await Task.Delay(new Random().Next(250));
                    using (Page page = await browser.NewPageAsync())
                    {
                        await page.SetExtraHttpHeadersAsync(headers);
                        //page.DefaultNavigationTimeout = 15000;
                        var watch = new System.Diagnostics.Stopwatch();
                        await page.GoToAsync(url, navigationOptions);

                        var links = await page.EvaluateExpressionAsync(querySelectorString);
                        // are you human check
                        if(links.Count() == 0)
                        {
                            rotateProxy();
                            options.Args[0] = $"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}";
                            await browser.CloseAsync();
                            browser = await Puppeteer.LaunchAsync(options);
                            continue;
                        }
                        Console.WriteLine("GOOD PROXY " + ": " + currentProxy.IPAddress + " - " + currentProxy.Port);

                        foreach (var link in links)
                        {
                            string fullLink = link.ToString();
                            bool cleanLink = true;
                            foreach (var blackListLink in blacklistLinks)
                            {
                                if (fullLink.Contains(blackListLink))
                                {
                                    cleanLink = false;
                                }
                            }
                            if (cleanLink)
                            {
                                hrefLinks.Add(fullLink);
                            }
                        }
                        repeat++;
                    } 
                }
                catch (Exception e)
                {
                    Console.WriteLine("BAD PROXY " + ": " + currentProxy.IPAddress + " - " + currentProxy.Port + "\t\t" + e.Message);
                    repeat = 0;
                    rotateProxy();
                    options.Args[0] = $"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}";
                    await browser.CloseAsync();
                    i--;
                    browser = await Puppeteer.LaunchAsync(options);
                }

                url = url.Replace(replaceDelimiter + (i + 1), replaceDelimiter + (i + 2));
            }
            await browser.CloseAsync();

            return hrefLinks;
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
                    Console.WriteLine(htmlDocument);
                    Console.WriteLine("OUt");
                    rotateProxy();
                    i--;
                    continue;
                }
                foreach (var link in links)
                {
                    string fullLink = companyUrl + link.Attributes["href"].Value;
                    bool cleanLink = true;
                    foreach (var blackListLink in blacklistLinks)
                    {
                        if (fullLink.Contains(blackListLink))
                        {
                            cleanLink = false;
                        }
                    }
                    if (cleanLink)
                    {
                        hrefLinks.Add(fullLink);
                    }
                }
                url = url.Replace(replaceDelimiter + pageNumber, replaceDelimiter + ++pageNumber);
            }
            return hrefLinks;
        }

        //public async void getAllInformationFromPageAsync(IWebCrawler webCrawler)
        //{
        //    Product product = webCrawler.getAllInfo();
        //    webCrawlerDAO.PostProductToDatabase(product);

        //}

        public async void getAllInformationFromPageAsync(string url, string companyName, string productType, string titleQuerySelector, string priceQuerySelector, string specsKeysQuerySelector,
                                                string specsValuesQuerySelector, string reviewerNameQuerySelector, string reviewDateQuerySelector,
                                                string ratingsContentQuerySelector)
        {

            Browser browser = await Puppeteer.LaunchAsync(options);

            bool validIP = false;
            while (!validIP)
            {
                try
                {
                    using (Page page = await browser.NewPageAsync())
                    {
                        await page.SetExtraHttpHeadersAsync(headers);
                        page.DefaultNavigationTimeout = 15000;
                        await page.GoToAsync(url, navigationOptions);
                        Console.WriteLine("GOOD PROXY " + ": " + currentProxy.IPAddress + " - " + currentProxy.Port);

                        Console.WriteLine(url);
                        var specsKeys = await page.EvaluateExpressionAsync(specsKeysQuerySelector);
                        var specsVals = await page.EvaluateExpressionAsync(specsValuesQuerySelector);
                        if(specsKeys.Count() == 0)
                        {
                            rotateProxy();
                            options.Args[0] = $"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}";
                            await browser.CloseAsync();
                            browser = await Puppeteer.LaunchAsync(options);
                            continue;
                        }
                        var title = await page.EvaluateExpressionAsync(titleQuerySelector);
                        var price = await page.EvaluateExpressionAsync(priceQuerySelector);

                        Dictionary<string, string> specsDictionary = new Dictionary<string, string>();
                        int keyCount = specsKeys.Count();
                        int modelNumberIndex = 0;
                        int brandIndex = 0;
                        for (int i = 0; i < keyCount; i++)
                        {
                            string key = specsKeys.ElementAt(i).ToString();
                            string value = specsVals.ElementAt(i).ToString();
                            if (key.ToLower().Contains("model"))
                            {
                                modelNumberIndex = i;
                            }
                            if (key.ToLower().Contains("brand"))
                            {
                                brandIndex = i;
                            }
                            if (!specsDictionary.ContainsKey(key))
                            {
                                specsDictionary.Add(key, value);
                            }
                        }

                        // click ratings
                        //new egg
                        var numberOfReviewsBeforeReload = await page.EvaluateExpressionAsync(reviewerNameQuerySelector);

                        if (numberOfReviewsBeforeReload.Count() == 0)
                        {
                    //        Models.WebCrawler.Product product = new Models.WebCrawler.Product(price != null, companyName, url, specsVals.ElementAt(modelNumberIndex).ToString(), title.ToString(), productType,
                    //specsVals.ElementAt(brandIndex).ToString(), totalStarRating.ToString()[0].ToString(), totalNumberOfReviews.ToString().Split(' ')[0], price == null ? "N/A" : price.ToString(), specsDictionary, reviews);
                        }

                        await page.EvaluateExpressionAsync("var x = document.querySelectorAll('.tab-nav'); " +
                                                            "for(let f of x) {" +
                                                            "   if(f.innerText == 'Reviews') {" +
                                                            "       f.click();" +
                                                            "   }" +
                                                            "}");

                        await page.WaitForSelectorAsync(".comments-content");

                        var reviewerNames = await page.EvaluateExpressionAsync(reviewerNameQuerySelector);
                        var reviewerDates = await page.EvaluateExpressionAsync(reviewDateQuerySelector);
                        var reviewContent = await page.EvaluateExpressionAsync(ratingsContentQuerySelector);
                        var individualRatings = await page.EvaluateExpressionAsync("Array.from(document.querySelectorAll('.comments-title .rating')).map(a=>a.classList.value)");

                        var totalStarRating = await page.EvaluateExpressionAsync("document.querySelector('.rating-views .rating-views-num').innerText");
                        var totalNumberOfReviews = await page.EvaluateExpressionAsync("document.querySelector('.rating-views-desc .rating-views-count').innerText");



                        List<Review> reviews = new List<Review>();
                        int reviewCount = reviewerNames.Count();
                        for (int i = 0; i < reviewCount; i++)
                        {
                            reviews.Add(new Review(reviewerNames.ElementAt(i).ToString(), getRatingFromString(individualRatings.ElementAt(i).ToString()), reviewContent.ElementAt(i).ToString(), reviewerDates.ElementAt(i).ToString()));
                        }
                        Models.WebCrawler.Product product = new Models.WebCrawler.Product(price != null, companyName, url, specsVals.ElementAt(modelNumberIndex).ToString(), title.ToString(), productType,
                            specsVals.ElementAt(brandIndex).ToString(), totalStarRating.ToString()[0].ToString(), totalNumberOfReviews.ToString().Split(' ')[0], price == null ? "N/A" : price.ToString(), specsDictionary, reviews);

                        // amazon
                        //var reviewsLink = await page.EvaluateExpressionAsync("document.querySelector('[data-hook=see-all-reviews-link-foot]').href");
                        //using (Page page2 = await browser.NewPageAsync())
                        //{
                        //    await page2.GoToAsync(reviewsLink.ToString());
                        //    var names = await page2.EvaluateExpressionAsync(reviewerNameQuerySelector);
                        //    var dates = await page2.EvaluateExpressionAsync(reviewDateQuerySelector);
                        //    var ratingsContent = await page2.EvaluateExpressionAsync(ratingsContentQuerySelector);
                        //}


                        bool validProduct = webCrawlerDAO.PostProductToDatabase(product);
                        if (validProduct)
                        {
                            webCrawlerDAO.PostSpecsOfProductsToDatabase(product);
                        }
                        webCrawlerDAO.PostToVendorProductsTable(product);
                        webCrawlerDAO.PostToVendorProductReviewsTable(product);
                    }
                    validIP = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("BAD PROXY " + ": " + currentProxy.IPAddress + " - " + currentProxy.Port + "\t\t" + e.Message);
                    rotateProxy();
                    options.Args[0] = $"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}";
                    await browser.CloseAsync();
                    browser = await Puppeteer.LaunchAsync(options);
                }
            }


            await browser.CloseAsync();

        }
        public string getRatingFromString(string ratingClass)
        {
            int length = ratingClass.Length;
            for (int i = 0; i < length; i++)
            {
                if (Char.IsDigit(ratingClass[i]))
                {
                    return ratingClass[i].ToString();
                }
            }
            return "";
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
                Console.WriteLine(url);
                var title = htmlDocument.DocumentNode.QuerySelector(titleQuerySelector);
                var price = htmlDocument.DocumentNode.QuerySelector(priceQuerySelector);
                var specsKeys = htmlDocument.DocumentNode.QuerySelectorAll(specsKeysQuerySelector);
                var specsValues = htmlDocument.DocumentNode.QuerySelectorAll(specsValuesQuerySelector);
                var ratingsReviewerName = htmlDocument.DocumentNode.QuerySelectorAll(reviewerNameQuerySelector);
                var ratingsDate = htmlDocument.DocumentNode.QuerySelectorAll(reviewDateQuerySelector);
                var ratingsContent = htmlDocument.DocumentNode.QuerySelectorAll(ratingsContentQuerySelector);

                Console.WriteLine("currently - " + url);
                // if any of these variables are empty, then we know something went wrong with the html read. Most likely, the proxy was flagged and thus the page was a catcha page.
                if (!specsKeys.Any() || !specsValues.Any())
                {
                    Console.Write("get all info??? NOPE");
                    Console.WriteLine(specsValues);
                    Console.WriteLine(htmlDocument);
                    rotateProxy();
                    continue;
                }

                // || !ratingsReviewerName.Any() || !ratingsDate.Any() || !ratingsContent.Any()
                Dictionary<string, string> specsDictionary = new Dictionary<string, string>();
                int keyCount = specsKeys.Count();
                int modelNumberIndex = 0;
                int brandIndex = 0;
                for (int i = 0; i < keyCount; i++)
                {
                    string key = specsKeys.ElementAt(i).InnerText;
                    string value = specsValues.ElementAt(i).InnerText;
                    if (key.ToLower().Contains("model"))
                    {
                        modelNumberIndex = i;
                    }
                    if (key.ToLower().Contains("brand"))
                    {
                        brandIndex = i;
                    }
                    if (!specsDictionary.ContainsKey(key))
                    {
                        specsDictionary.Add(key, value);
                    }
                }
                //create product
                //Models.WebCrawler.Product product = new Models.WebCrawler.Product(price != null, companyName, url, specsValues.ElementAt(modelNumberIndex).InnerText.Trim(), title.InnerText.Trim(), productType, specsValues.ElementAt(brandIndex).InnerText.Trim(), price.InnerText, specsDictionary);
                //bool validProduct = webCrawlerDAO.PostProductToDatabase(product);
                //if (validProduct)
                //{
                //    webCrawlerDAO.PostSpecsOfProductsToDatabase(product);
                //    webCrawlerDAO.PostToVendorProductsTable(product);
                //}
                break;
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
                    //request.Proxy = new WebProxy("121.148.250.59", 8080);
                    // 51.195.166.84 - 8181
                    request.Proxy = new WebProxy("208.80.28.208", 8080);
                    //request.Proxy = new WebProxy("35.234.46.196", 8080);
                    //request.Proxy = (currentProxy == null) ? null : new WebProxy(currentProxy.IPAddress, currentProxy.Port);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    request.Accept = "text/html, application/xhtml+xml, application/xml;q=0.9";
                    request.Method = "GET";
                    request.Referer = "https://www.google.com";
                    Thread.Sleep(new Random().Next(310));
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.9");

                    //mac
                    request.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A";
                    //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36";
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

        public async Task<List<Proxy>> getAllProxiesAsync()
        {
            List<Proxy> proxies = new List<Proxy>();
            LaunchOptions options = new LaunchOptions()
            {
                Headless = true,
                IgnoreHTTPSErrors = true,
                Args = new[] {
                        //$"--proxy-server={currentProxy.IPAddress}:{currentProxy.Port}",
                        "--proxy-server=208.80.28.208:8080",
                        "--no-sandbox",
                        "--disable-gpu",
                        "--ignore-certificate-errors",
                    }
            };
            using (var browser = await Puppeteer.LaunchAsync(options))
            {
                try
                {
                    using (Page page = await browser.NewPageAsync())
                    {

                        await page.SetExtraHttpHeadersAsync(headers);
                        page.DefaultNavigationTimeout = 15000;
                        await page.GoToAsync(PROXY_WEBSITE, navigationOptions);
                        JToken nextButtonClassList = null;
                        bool hasNextPage = true;
                        while(hasNextPage)
                        {
                            var links = await page.EvaluateExpressionAsync(PROXY_WEBSITE_QUERYSELECTOR);
                            var test = await page.EvaluateExpressionAsync("document.querySelector('.paginate_button.next')");
                            foreach (var link in links)
                            {
                                var wholeLink = link.ToString();
                                Console.WriteLine(wholeLink);
                                var stringArr = wholeLink.Split('\t');
                                proxies.Add(new Proxy(stringArr[0], Int32.Parse(stringArr[1])));
                            }
                            nextButtonClassList = await page.EvaluateExpressionAsync("Array.from(document.querySelector('.paginate_button.next').classList)");
                            if (await page.EvaluateExpressionAsync("document.querySelector('.paginate_button.next.disabled')") != null)
                            {
                                hasNextPage = false;
                            }
                            else
                            {
                                await page.EvaluateExpressionAsync("document.querySelector('.paginate_button.next').click()");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    rotateProxy();
                }

                await browser.CloseAsync();
            }

            return proxies;
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
                    proxies.Add(new Proxy(link.ChildNodes[0].InnerHtml, Int32.Parse(link.ChildNodes[1].InnerHtml)));
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
                //if (currentProxy.IPAddress == "208.80.28.208" && currentProxy.Port == 8080)
                //{
                //    rotateProxy();
                //}
                //if(currentProxy.IPAddress == "91.92.180.45" && currentProxy.Port == 8080)
                //{
                //    rotateProxy();
                //}
                //currentProxy = new Proxy("208.80.28.208", 8080);


            }

        }

        ~WebCrawlerService()
        {

        }
    }
}
