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
    public class CommonReponseAuth : ICommonResponse, IEquatable<CommonReponseAuth>
    {

        public string SuccessString { get; set; }
        public string FailureString { get; set; }
        public bool SuccessBool { get; set; }
        public bool connectionState { get; set; }

        public bool IsUserExists { get; set; }
        //do we factor in the object itself?

        // instantiate the objects I will be using:

        public bool isAuthenticated { get; set; }


        public string JWTString { get; set; }

        public AuthUserDTO AuthUserDTO;

        public CommonReponseAuth()
        {
            SuccessString = " ";
            FailureString = " ";
            JWTString = " ";
            SuccessBool = false;
            IsUserExists = false;
            isAuthenticated = false;
            connectionState = false;
            AuthUserDTO = new AuthUserDTO();
            AuthUserDTO.UserEmail = " ";

        }
        public override string ToString()
        {
            return $"\nSuccess string {SuccessString}\nFailure String {FailureString}\n" +
                $"Success Bool {SuccessBool}\nIsAuthenticated {isAuthenticated}\n" +
                $"Connection bool {connectionState}\n" +
                $"IsUser Exists {IsUserExists}" ;

        }

        public bool Equals(CommonReponseAuth other)
        {
           if(other == null)
            {
                return false;
            }
           if( this.SuccessString == other.SuccessString &&
                this.SuccessBool == other.SuccessBool &&
                this.FailureString == other.FailureString &&
                this.isAuthenticated == other.isAuthenticated &&
                this.IsUserExists == other.IsUserExists &&
                this.connectionState ==  other.connectionState)
            {
                return true;

            }
            return false;
        }
    }
}
