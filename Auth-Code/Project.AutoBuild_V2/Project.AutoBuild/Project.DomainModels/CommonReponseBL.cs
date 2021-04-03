using Project.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DomainModels
{
    public class CommonReponseBL : ICommonResponse
    {
        public string SuccessString { get; set; }
        public string FailureString { get; set; }
        public bool SuccessBool { get; set; }
        public bool FailureBool { get; set; }

        // for the authentication it need to return 
        // an object for the jwt 




    }
}