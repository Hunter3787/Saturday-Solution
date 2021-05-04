using AutoBuildApp.Models.Entities;
using AutoBuildApp.Security.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace AutoBuildApp.Models 
{ 
    public class LoginDAO
    {
        private String _connection;
        private SqlDataAdapter adapter = new SqlDataAdapter();
        private Claims _userClaims;
        public LoginDAO(String connectionString)
        {
            // instantiation of the connections string via a constructor to avoid any hardcoding
            this._connection = connectionString;
        }

        //public String MatchData(String userName, String password)
        //{
        //    using (SqlConnection connection = new SqlConnection(this._connection))
        //    {
        //        connection.Open();
        //        using (SqlTransaction transaction = connection.BeginTransaction())
        //        {
        //            String sequal = "SELECT COUNT (*) FROM userAccounts WHERE UserName = @USERNAME AND passwordHash = @PASSWORDHASH;";
        //            adapter.InsertCommand = new SqlCommand(sequal, connection, transaction);
        //            adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userName;
        //            adapter.InsertCommand.Parameters.Add("@PASSWORDHASH", SqlDbType.VarChar).Value = password;

        //            try
        //            {
        //                int rows = 0;
        //                rows = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
        //                transaction.Commit();
        //                if (rows == 1)
        //                {
        //                    return "Logged in";
        //                }
        //                else return "Failed to login";
        //            }
        //            catch (SqlException ex)
        //            {
        //                transaction.Rollback();
        //                return ex.Message;
        //            }
        //        }
        //    }
        //}



        /// <summary>
        /// do a single query that takes the users credentials and spits 
        /// back 
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        public CommonReponseAuth LoginInformation(UserCredentials userCredentials)
        {
            CommonReponseAuth _CRAuth = new CommonReponseAuth(); 
            // EMPHAREL  ( FOR THE MONENT )VALEUE NO NEED TO STORE

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_retrievePermissions = "RetrievePermissions";
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
                            if (!reader.HasRows) // use the bang!!!!!!! 
                            {
                                _CRAuth.ResponseString = "User not found";
                                _CRAuth.IsUserExists = false;
                            }
                           
                            //READ AND STORE All THE ORDINALS YOU NEED
                            int username = reader.GetOrdinal("username");
                            int permissions = reader.GetOrdinal("permission");
                            int scope = reader.GetOrdinal("scopeOfPermission");
                            int locked = reader.GetOrdinal("locked");



                            while (reader.Read())
                            {
                                if ((bool)reader[locked])
                                {
                                    _CRAuth.ResponseString = "Account is locked";
                                    _CRAuth.ResponseBool = false;
                                    reader.Close();
                                    return _CRAuth;
                                }
                                _userClaims = new Claims();
                                _CRAuth.AuthUserDTO.UserName = (string)reader[username];
                                _userClaims.Permission = (string)reader[permissions];
                                _userClaims.ScopeOfPermissions = (string)reader[scope];
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
                    //Console.WriteLine($"Auth DAO Common response check:: {_CRAuth.ToString()}");
                    _CRAuth.connectionState = true;
                    _CRAuth.ResponseString = "User Exists";
                    _CRAuth.IsUserExists = true;

                    return _CRAuth;
                }
            }
        }
    }
}
