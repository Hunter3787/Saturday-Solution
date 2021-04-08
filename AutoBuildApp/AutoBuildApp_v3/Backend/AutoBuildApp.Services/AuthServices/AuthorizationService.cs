



//using System.Collections.Generic;
//using System.Threading;
//using AutoBuildApp.Security.Models;


//namespace AutoBuildApp.Services.Auth_Services
//{
//    class AuthorizationService
//    {

//        private UserPrinciple _threadPrinciple = (UserPrinciple)Thread.CurrentPrincipal;


//        //here there will be a check on the permissions passed to
//        // the authorization service

//        /// <summary>
//        /// AuthenticationService constructor that takes the set of permissions 
//        /// and does the necessary check 
//        /// </summary>
//        public AuthorizationService(IEnumerable<Claims> permissionsRequired)
//        {
//            PermissionsRequired = permissionsRequired;
//            IsAuthorized = false;
//        }


//        /// <summary>
//        /// constructor takes in already defined Claim Per Role
//        /// and implements check
//        /// goal: to make it easier for other features to do checks 
//        /// </summary>
//        /// <param name="claimsPerRoles"></param>
//        ////public AuthorizationService(ClaimsPerRoles claimsPerRoles)
//        ////{
//        ////    PermissionsRequired = claimsPerRoles.Permissions;
//        ////    IsAuthorized = false;
//        ////}



//        public IEnumerable<Claims> PermissionsRequired { get; set; }

//        /// <summary>
//        /// check ther permissions passed to that wihtin the principle object 
//        /// </summary>
//        /// <returns></returns>
//        public bool checkPermissions()
//        {
//            foreach (Claims claims in this._threadPrinciple.Permissions)
//            {
//                foreach (Claims claimNeeded in this.PermissionsRequired)
//                {
//                    if(claims.Equals(claimNeeded) == false)
//                    {
//                        return false;
//                    }
//                }
//            }
//            return true;
//        }

//        public bool IsAuthorized { get; private set; }
       

//        //public bool VerifyJWTSignature()
//        //{
//        //   string JWTToken =  _threadPrinciple.Identity.IssuedJWTToken;

//        //    _jwtValidator = new JWTValidator(JWTToken);
//        //    bool returnBool = _jwtValidator.IsValidJWT();
//        //    return returnBool;
//        //}

//        /// <summary>
//        /// this runs the check on the permissions 
//        /// and returns true or false and sets the property
//        /// is Authorized to be true if checks pass
//        /// </summary>
//        /// <returns></returns>
//        public bool verifyAuthorization()
//        {

//            // two checks needed the JWT
//            ////if ( VerifyJWTSignature() == false)
//            ////{
//            ////    return false;
//            ////}
//            if(checkPermissions() == false)
//            {
//                return false;
//            }

//            // then retun the bool 
//            IsAuthorized = true;
//            return true;

//        }

//    }
//}
