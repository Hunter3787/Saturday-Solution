using Project.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess
{
    public class CommonResonseLog : ICommonResponse
    {
        public string SuccessString { get; set; }
        public string FailureString { get; set; }
        public bool SuccessBool { get; set; }

        //my question is do I make a response object for the whole DAL and 
        // store all entities in here?
        //logger entitiy 

    }


}
