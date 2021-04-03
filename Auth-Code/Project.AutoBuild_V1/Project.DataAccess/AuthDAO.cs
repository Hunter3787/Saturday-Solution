using Microsoft.Data.SqlClient;
using Project.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Project.DataAccess
{
    public class AuthDAO
    {
        // private commonResponseDAL CrAuth;
        private string _connectionString;
        private UserPermissionsEntitiy _userPermissions;

        /// this is the common respone object for authentication
        private CommonReponseAuth _CRAuth = new CommonReponseAuth();
        public AuthDAO(string connection)
        {
            this._connectionString = connection;

        }

        /// <summary>
        /// we want to gaurd against an exception being thrown 
        /// if the connection is closed or if db is not connected
        /// </summary>
        /// <returns></returns>
        public object tryConnection()
        {
            SqlConnection con = new SqlConnection(this._connectionString);
            con.Open();
            if (this._connectionString != null && con.State != ConnectionState.Closed)
            {
                _CRAuth.connectionState = true;
                _CRAuth.Success = $"The Connection state to DB is true";
            }
            else
            {
                _CRAuth.connectionState = false;
                _CRAuth.Failure = $"The connection state is false, try again later.";
                con.Close();
            }
            return _CRAuth;
        }

        public object CheckConnection(SqlConnection con)
        {
            if (this._connectionString != null && con.State != ConnectionState.Closed)
            {
                _CRAuth.connectionState = true;
                _CRAuth.Success = $"The Connection state to DB is true";
            }
            else
            {
                _CRAuth.connectionState = false;
                _CRAuth.Failure = $"The connection state is false, try again later.";
                con.Close();
            }
            return _CRAuth;

        }



        public bool AuthenticateUserCred(UserCredentials userCredentials)
        {
            using (var conn = new SqlConnection(_connectionString))
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
        public object RetrieveUserPermissions(UserCredentials userCredentials)
        {
            Console.WriteLine("Inside RetrieveUserPermissions");

           

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string SQL = "retrievePermissions";
                    using (SqlCommand command = new SqlCommand(SQL, conn))
                    {

                    // before we continue let us check the connection state.
                    _CRAuth = (CommonReponseAuth)CheckConnection(conn);
                    Console.WriteLine($"checking connection in authDAO RetrieveUserPermission {_CRAuth.FailureBool} ");

                    if (_CRAuth.connectionState ==false)
                    {
                        Console.WriteLine($"checking connection in authDAO:RetrieveUserPermission output: {_CRAuth.FailureBool} ");
                        return _CRAuth;
                    }

                    command.Transaction = conn.BeginTransaction();
                        // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                        command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = SQL;
                        //Add any required parameters to the Command.Parameters collection.
                        var param = new SqlParameter[2];
                        param[0] = new SqlParameter("@username", userCredentials.Username);
                        param[0].Value = userCredentials.Username;
                        param[1] = new SqlParameter("@passhash", userCredentials.Password);
                        param[1].Value = userCredentials.Password;
                        // add the commands the parameters for the stored procedure
                        command.Parameters.AddRange(param);

                        var _reader = command.ExecuteReader();
                        while (_reader.Read())
                        {
                            _userPermissions = new UserPermissionsEntitiy();
                            // ret = $"user ID: {_reader.GetInt64(0)} Permissions: {_reader.GetString(1)} scopeOfPermission { _reader.GetString(2) }";
                            _userPermissions.UserAccountID = _reader.GetInt64(0);
                            _userPermissions.Permission = _reader.GetString(1);
                            _userPermissions.scopeOfPermissions = _reader.GetString(2);
                            _CRAuth.AuthUserDTO.Claims.Add(_userPermissions);
                        }
                        if (_reader.HasRows)
                        {
                            _CRAuth.Success = "User Exists";
                            _CRAuth.SuccessBool = true;
                        }
                        else
                        {
                            _CRAuth.Failure = "User not found";
                            _CRAuth.FailureBool = true;

                        }
                        return _CRAuth;
                    }
            }
            
        }
    }
}
