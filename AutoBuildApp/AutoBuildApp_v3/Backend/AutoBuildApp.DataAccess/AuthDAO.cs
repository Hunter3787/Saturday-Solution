using Microsoft.Data.SqlClient;
using AutoBuildApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Security;

namespace AutoBuildApp.DataAccess
{
    public class AuthDAO
    {
        private Claims _userClaims;

        public AuthDAO(string connection)
        {
            try
            {
                // set the claims needed for this method call
                ConnectionString = connection;
                Console.WriteLine($"in auth DAO connection string  { ConnectionString}");
            }
            catch (ArgumentNullException)
            {
                if (connection == null)
                {
                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);
                }
            }


        }
        public AuthDAO()
        {
            ConnectionString = " ";
        }

        public string ConnectionString { get; set; }


        /// <summary>
        /// we want to gaurd against an exception being thrown 
        /// if the connection is closed or if db is not connected
        /// </summary>
        /// <returns></returns>
        public object TryConnection()
        { //REINSTANTIATE IT 
            CommonReponseAuth _CRAuth = new CommonReponseAuth();
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();
                if (this.ConnectionString != null && con.State != ConnectionState.Closed)
                {
                    _CRAuth.connectionState = true;
                    _CRAuth.ResponseString = $"The Connection state to DB is true";
                }
                else
                {
                    _CRAuth.connectionState = false;
                    _CRAuth.ResponseString = $"The connection state is false, try again later.";
                    con.Close();
                }
            }
            return _CRAuth;
        }

        public object CheckConnection(SqlConnection con)
        {
            //REINSTANTIATE IT 
            CommonReponseAuth _CRAuth = new CommonReponseAuth();
            Console.WriteLine($" in CHECK CONNECTION");
            if (con == null)
            {
                _CRAuth.ResponseBool = false;
                _CRAuth.ResponseString = "NULL EXCEPTION";
                //Console.WriteLine($" Common response expected for NULL: {_CRAuth.ToString() }" );
                return _CRAuth;
            }
            using (con)
            {
                con.Open();
                if (this.ConnectionString != null &&
                    con.State != ConnectionState.Closed)
                {
                    _CRAuth.connectionState = true;
                    return _CRAuth;
                }
                else
                {
                    _CRAuth.connectionState = false;
                    return _CRAuth;

                }
            }

        }
        public bool AuthenticateUserCred(UserCredentials userCredentials)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {

                conn.OpenAsync();
                using (var command = new SqlCommand())
                {

                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        $"SELECT * FROM userCredentials " +
                        $"WHERE username =@username AND passwordHash = @passwordHash ";
                    var parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@username", userCredentials.Username);
                    parameters[1] = new SqlParameter("@password", userCredentials.Password);


                    command.Parameters.AddRange(parameters);

                    var rowsAdded = command.ExecuteNonQuery();

                    if (rowsAdded == 1)
                    {
                        return true;
                    }

                }

            }
            return false;

        }


        /// <summary>
        /// do a single query that takes the users credentials and spits 
        /// back 
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        public CommonReponseAuth RetrieveUserInformation(UserCredentials userCredentials)
        {
            CommonReponseAuth _CRAuth = new CommonReponseAuth(); // EMPHAREL  ( FOR THE MONENT )VALEUE NO NEED TO STORE

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                Console.WriteLine($"\tRetrieveUserPermissions METHOD \n");
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_retrievePermissions = "retrievePermissions";
                using (SqlCommand command = new SqlCommand(SP_retrievePermissions, conn))
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();
                        #region SQL related

                        // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                        command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = SP_retrievePermissions;
                        /// command.Parameters.AddWithValue   -> fix itttttttt!!!!!
                        //Add any required parameters to the Command.Parameters collection.
                        // command.Parameters.AddWithValue("@username", userCredentials.Username);
                        var param = new SqlParameter[2];
                        param[0] = new SqlParameter("@username", userCredentials.Username);
                        param[0].Value = userCredentials.Username;
                        param[1] = new SqlParameter("@passhash", userCredentials.Password);
                        param[1].Value = userCredentials.Password;
                        // add the commands the parameters for the stored procedure
                        command.Parameters.AddRange(param);
                        #endregion


                        using (var reader = command.ExecuteReader())
                        {
                            Console.WriteLine($" reader rows: {reader.HasRows}");
                            if (!reader.HasRows) // use the bang!!!!!!! 
                            {
                                _CRAuth.ResponseString = "User not found";
                                _CRAuth.IsUserExists = false;
                                return _CRAuth; //return  
                            }

                            //READ AND STORE ALL THE ORDINALS YOU NEED
                            int username = reader.GetOrdinal("username");
                            int permissions = reader.GetOrdinal("permission");
                            int scope = reader.GetOrdinal("scopeOfPermission");



                            while (reader.Read())
                            {
                                _userClaims = new Claims();
                                /// ret = $"user ID: {reader.GetInt64(0)} Permissions: {reader.GetString(1)} scopeOfPermission { reader.GetString(2) }";
                                ///_userPermissions.UserAccountID = reader.GetInt64(0);
                                /// magic values -> will the collumns alwats be the same
                                /// better to use ordinal names -> no matter where the column just specifiy thr column 
                                _CRAuth.AuthUserDTO.UserEmail = (string)reader[username];
                                _userClaims.Permission = (string)reader[permissions];
                                _userClaims.scopeOfPermissions = (string)reader[scope];
                                _CRAuth.AuthUserDTO.Claims.Add(_userClaims);
                            }
                            // auto reader close
                        }

                    }
                    catch (SqlException) 
                    {
                        command.Transaction.Rollback();
                        _CRAuth.ResponseBool = false;

                    }

                    command.Transaction.Commit();
                    _CRAuth.connectionState = true;
                    _CRAuth.ResponseString = "User Exists";
                    _CRAuth.IsUserExists = true;

                    return _CRAuth;
                }

            }
        }
    }
}
