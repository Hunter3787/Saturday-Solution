using AutoBuildApp.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{
    public class CollectionCommonResponse<T> : CommonResponse
    {
        public T GenericCollection { get; set; }
    }
}
