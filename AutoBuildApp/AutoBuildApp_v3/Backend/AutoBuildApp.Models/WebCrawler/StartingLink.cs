using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.WebCrawler
{
    public class StartingLink
    {
        public string Link { get; private set; }
        public string ComponentType { get; private set; }

        public StartingLink(string link, string componentType)
        {
            Link = link;
            ComponentType = componentType;
        }
    }
}
