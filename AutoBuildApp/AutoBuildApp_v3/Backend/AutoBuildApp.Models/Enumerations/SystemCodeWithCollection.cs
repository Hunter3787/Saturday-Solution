using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Enumerations
{
    public class SystemCodeWithCollection<T>
    {
        public AutoBuildSystemCodes Code { get; set; }
        public T GenericCollection { get; set; }
    }
}
