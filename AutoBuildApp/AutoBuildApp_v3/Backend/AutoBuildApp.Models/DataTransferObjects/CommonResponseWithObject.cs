using AutoBuildApp.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{
    public class CommonResponseWithObject<T> : CommonResponse where T : new()
    {
        public T GenericObject { get; set; } = new T();
    }
}
