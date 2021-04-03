using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Abstractions
{
    public interface ICommonResponse
    {

        string Success { get; set; }
        string Failure { get; set; }




    }
}