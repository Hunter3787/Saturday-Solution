using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Enumerations
{
    public class SystemCodeWithObject<T>
    {
        public AutoBuildSystemCodes Code { get; set; }
        public T GenericObject { get; set; }
    }
}
