using AutoBuildApp.DataAccess.Entities;
using System;
using AutoBuildApp.Models.DataTransferObjects;


namespace AutoBuildApp.DataAccess
{
    // this is a responce to the Dashboard Bussiness object
    /// <summary>
    /// takes care of three scenerious 
    /// respose Successful
    /// response Failure
    /// response 
    /// </summary>
    public class CommonReponseAuth : CommonResponse, IEquatable<CommonReponseAuth>
    {

        public bool connectionState { get; set; }

        public bool IsUserExists { get; set; }
        //do we factor in the object itself?

        // instantiate the objects I will be using:

        public bool isAuthenticated { get; set; }


        public string JWTString { get; set; }

        public AuthUserDTO AuthUserDTO;

        public CommonReponseAuth()
        {
            ResponseString = " ";
            JWTString = " ";
            ResponseBool = false;
            IsUserExists = false;
            isAuthenticated = false;
            connectionState = false;
            AuthUserDTO = new AuthUserDTO();
            AuthUserDTO.UserEmail = " ";

        }
        public override string ToString()
        {
            return $"\nResponse String {ResponseString }\n" +
              $"Success Bool {ResponseBool}\nIsAuthenticated {isAuthenticated}\n" +
              $"Connection bool {connectionState}\n" +
              $"IsUser Exists {IsUserExists}";

        }

        public bool Equals(CommonReponseAuth other)
        {
            if (other == null)
            {
                return false;
            }
            if (this.ResponseString == other.ResponseString &&
               this.ResponseBool == other.ResponseBool &&
               this.isAuthenticated == other.isAuthenticated &&
               this.IsUserExists == other.IsUserExists &&
               this.connectionState == other.connectionState)
            {
                return true;

            }
            return false;
        }
    }
}
