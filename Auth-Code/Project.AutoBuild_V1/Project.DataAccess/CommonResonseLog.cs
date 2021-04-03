using Project.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess
{
    public class CommonResonseLog : ICommonResponse
    {
        public bool successBool { get; set; }
        public bool failureBool { get; set; }

        public string Success { get; set; }
        public string Failure { get; set; }

        //my question is do I make a response object for the whole DAL and 
        // store all entities in here?
        //logger entitiy 

    }


}
