using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Security.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace AutoBuildApp.DataAccess 
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
            CommonReponseAuth _CRAuth = new CommonReponseAuth(); // EMPHAREL  ( FOR THE MONENT )VALEUE NO NEED TO STORE

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_retrievePermissions = "retrievePermissions";
                using (SqlCommand command = new SqlCommand(SP_retrievePermissions, conn))
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


                    var _reader = command.ExecuteReader();
                    if (!_reader.HasRows) // use the bang!!!!!!! 
                    {
                        _CRAuth.FailureString = "User not found";
                        _CRAuth.IsUserExists = false;
                    }
                    if (_reader.HasRows) // just else is enough 
                    {
                        _CRAuth.SuccessString = "User Exists";
                        _CRAuth.IsUserExists = true;
                    }

                    //READ AND STORE ALL THE ORDINALS YOU NEED
                    int username = _reader.GetOrdinal("username");
                    int permissions = _reader.GetOrdinal("permission");
                    int scope = _reader.GetOrdinal("scopeOfPermission");
                    int locked = _reader.GetOrdinal("locked");

                    

                    while (_reader.Read())
                    {
                        if ((bool)_reader[locked])
                        {
                            _CRAuth.FailureString = "Account is locked";
                            _CRAuth.SuccessBool = false;
                            _reader.Close();
                            return _CRAuth;
                        }
                        _userClaims = new Claims();
                        /// ret = $"user ID: {_reader.GetInt64(0)} Permissions: {_reader.GetString(1)} scopeOfPermission { _reader.GetString(2) }";
                        ///_userPermissions.UserAccountID = _reader.GetInt64(0);
                        /// magic values -> will the collumns alwats be the same
                        /// better to use ordinal names -> no matter where the column just specifiy thr column 
                        _CRAuth.AuthUserDTO.UserEmail = (string)_reader[username];
                        _userClaims.Permission = (string)_reader[permissions];
                        _userClaims.scopeOfPermissions = (string)_reader[scope];
                        _CRAuth.AuthUserDTO.Claims.Add(_userClaims);
                    }
                    //Console.WriteLine($"Auth DAO Common response check:: {_CRAuth.ToString()}");
                    _CRAuth.connectionState = true;
                    return _CRAuth;
                }
            }
        }
    }
}
