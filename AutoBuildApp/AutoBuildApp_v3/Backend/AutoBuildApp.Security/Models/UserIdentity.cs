

using System.Security.Principal;

namespace AutoBuildApp.Security.Models
{
    /// <summary>
    /// So lets take a look at that in the
    /// context of ASP.NET Core. Identities 
    /// in ASP.NET Core are a ClaimsIdentity.
    /// </summary>
    /// /https://andrewlock.net/introduction-to-authentication-with-asp-net-core/
    /// 
    //Identity Object An Identity object is basically a user account
    public class UserIdentity : IIdentity // IIdentity exposes three simple properties:                          // AuthenticationType, IsAuthenticated, and Name.
    {

        public UserIdentity()
        {
            this.AuthenticationType = "AutoBuild JWT";
            this.IsAuthenticated = false;
            this.Name = "AutoBuild User";
        }

        private string authenticationType = "AutoBuild JWT";

        /// <summary>
        /// Authentication type in ASP.NET is more
        /// like cookies and bearer or google...
        /// </summary>
        public string AuthenticationType {
            get { return this.authenticationType; }
            set { this.authenticationType = value; } 
        }
        /// <summary>
        /// the property IsAuthenticated indicates whether 
        /// an identity is authenticated or not.
        ///  
        /// This might seem redundant -
        ///  how could you have an identity with claims when it is not authenticated? 
        ///  One scenario may be where 
        ///  you allow guest users on your site
        ///  You still have an identity associated 
        ///  with the user, and that identity may still
        ///  have claims associated with it, but they will
        ///  not be authenticated. This is 
        ///  an important distinction to bear in mind. 
        ///  
        /// </summary>
        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }


        public override string ToString()
        {
            return $" " +
                $"\nAuthType: {this.AuthenticationType} " +
                $"\nIsAuth: {this.IsAuthenticated } " +
                $"\nName: {this.Name }";
        }

    }
}
