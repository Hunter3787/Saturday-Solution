using AutoBuildApp.Security.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;


namespace AutoBuildApp.Security.Models
{
    /// <summary>
    /// tThe class represent the JWT payload service 
    /// </summary>
    public class JWTPayload : IEquatable<JWTPayload>
    {
        private DateTimeOffset _dtoMin = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public JWTPayload()
        {
            this.iss = "Autobuild";
            this.sub = "Autobuild User";
            this.aud = "US";
            this.exp = ToUnixTimestamp(_dtoMin);
            this.nbf = ToUnixTimestamp(_dtoMin);
            this.iat = ToUnixTimestamp(_dtoMin);
            this.Username = string.Empty;
            this.UserCLaims = new List<Claims>
            {new Claims(PermissionEnumType.ReadOnly,ScopeEnumType.AutoBuild) };
        }

        
        //*Autubuild.com
        public JWTPayload(string Subject, string Issuer, string Audience,
            string Username, object Expiration, object NotBeforeTime, object DOBofJWTToken)
        {
            try
            {


                this.sub = Subject; // in our context "Autobuild User"
                this.iss = Issuer; // in our context "Autobuild"
                this.aud = Audience; // in our context "US"
                this.Username = Username;
                ///Console.WriteLine($"the type: {NotBeforeTime.GetType()}");
                ///Console.WriteLine($"the reference for datetime: {Object.ReferenceEquals(NotBeforeTime.GetType(), typeof(DateTimeOffset))}");
                ///Console.WriteLine($"the reference for long: {Object.ReferenceEquals(NotBeforeTime.GetType(), typeof(System.Int64))}");

                if (Object.ReferenceEquals(NotBeforeTime.GetType(), typeof(System.DateTimeOffset)) &&
                    Object.ReferenceEquals(DOBofJWTToken.GetType(), typeof(System.DateTimeOffset)) &&
                    Object.ReferenceEquals(Expiration.GetType(), typeof(System.DateTimeOffset)))
                {
                    this.iat = ToUnixTimestamp(DOBofJWTToken);
                    this.nbf = ToUnixTimestamp(NotBeforeTime);
                    this.exp = ToUnixTimestamp(Expiration);
                }
                else if (Object.ReferenceEquals(NotBeforeTime.GetType(), typeof(System.Int64)) &&
                        Object.ReferenceEquals(DOBofJWTToken.GetType(), typeof(System.Int64)) &&
                        Object.ReferenceEquals(Expiration.GetType(), typeof(System.Int64)))
                {
                    this.iat = (long)DOBofJWTToken;
                    this.nbf = (long)NotBeforeTime;
                    this.exp = (long)Expiration;

                }
            }
            catch (ArgumentNullException)
            {
                var expectedParamName = "NULL OBJECT PROVIDED";
                throw new ArgumentNullException(expectedParamName);
            }

        }

        /// <summary>
        /// transates the date time offset into the unix time in seconds 
        /// https://docs.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-5.0
        /// </summary>
        /// <param _name="datePassed"></param>
        /// <returns></returns>
        public long ToUnixTimestamp(object DatePassed)
        {
            DateTimeOffset datePass = (DateTimeOffset)DatePassed;
            //Console.WriteLine($"date passed: time: {datePassed}");
            var epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0,TimeSpan.Zero);
           ///Console.WriteLine("{0} --> Unix Seconds: {1}", epoch, epoch.ToUnixTimeSeconds());
            var time = datePass.ToUniversalTime().Subtract(epoch);
            //Console.WriteLine($"time: {time.Ticks / TimeSpan.TicksPerSecond}");
            return time.Ticks / TimeSpan.TicksPerSecond;
        }

        #region The JWT Reserved claims
        /// <summary>
        /// iss (Issuer): Issuer of the JWT
        /// in our case it is Senior project AutoBuild Application
        /// </summary>
        public string iss { get; set; }

        /// <summary>
        /// sub (Subject): Subject of the JWT (the user)
        /// </summary>
        public string sub { get; set; }

        /// <summary>
        /// aud (Audience): Recipient for which the JWT is intended
        /// users within the US
        /// </summary>
        public string aud { get; set; }

        /// <summary>
        ///    The "iat" (issued at) claim 
        ///    identifies the time at which 
        ///    the JWT was issued.
        /// </summary>
        public long iat { get;  set; }

        /// <summary>
        ///  (Expiration time): Time after which the JWT expires
        /// </summary>
        public long exp { get; set; }

        /// <summary>
        /// "NotBeforeTime" (Not Before) Claim
        /// The "NotBeforeTime" (not before) claim identifies the time before 
        /// which the JWT MUST NOT be accepted for processing.The
        /// processing of the "nb claim requires that the current 
        /// date/time MUST be after or equal to
        /// the not-before date/time listed in the "NotBeforeTime" claim.
        /// </summary>
        public long nbf { get; set; }

        #endregion



        #region JWT custom claims
        public string Username { get;  set; }
        /// <summary>
        /// JWS payload (set of claims): contains 
        /// security statements: the permissions 
        /// and the scope the user are allowed.
        /// </summary>
        public IList<Claims> UserCLaims { get; set; }
        #endregion

        #region IEquatable interface implementation
        public bool Equals(JWTPayload other)
        {
            if (other is null) { return false; }

            return this.aud == other.aud 
                && this.iss == other.iss
                && this.sub == other.sub;
        }

        #endregion

        public string GenerateClaims()
        {
            string ret = " ";
            foreach (Claims claim in UserCLaims)
            {
                ret += $"{ claim.Permission}, {claim.ScopeOfPermissions}\n";
            }
            return ret;
        }

        public override string ToString()
        {
            return $"\nThe JWT payload: " +
                $"iss: {iss}\n" +
                $"sub { sub},\n" +
                $" aud {aud}\n" +
                $" iat {iat}\n" +
                $"exp {exp} \n" +
                $"NotBeforeTime {nbf}\n" +
                $"Username {Username}\n" +
                $"UserCLaims {GenerateClaims()}\n";
        }
     


    }

    /*
     * references:
     * regarding the "seconds since epoch" 1970, Unix time
     * https://docs.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-5.0
     * 
     * the reserved claims:
     * https://auth0.com/docs/tokens/json-web-tokens/json-web-token-claims#reserved-claims
     * 
     * 
     * epoch converter 
     * https://www.epochconverter.com/
     */
}
