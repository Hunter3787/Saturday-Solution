using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    public class SaveBuild
    {
        public IList<string> ModelNumbers { get; set; }

        public string BuildName { get; set; }
    }
}
