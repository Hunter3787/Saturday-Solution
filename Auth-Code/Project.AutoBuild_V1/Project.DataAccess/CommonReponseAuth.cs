using Project.DataAccess.Abstractions;
using Project.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess
{
    // this is a responce to the Dashboard Bussiness object
    /// <summary>
    /// takes care of three scenerious 
    /// respose Successful
    /// response Failure
    /// response 
    /// </summary>
    public class CommonReponseAuth : ICommonResponse
    {
        public string Success { get; set; }
        public string Failure { get; set; }

        public bool SuccessBool { get; set; }
        public bool FailureBool { get; set; }


        public bool connectionState { get; set; }
        //do we factor in the object itself?

        // instantiate the objects I will be using:

        public AuthUserDTO AuthUserDTO;

        public CommonReponseAuth()
        {
            Success = " ";
            Failure = " ";
            AuthUserDTO = new AuthUserDTO();
            AuthUserDTO.UserEmail = " ";

        }
    }
}
