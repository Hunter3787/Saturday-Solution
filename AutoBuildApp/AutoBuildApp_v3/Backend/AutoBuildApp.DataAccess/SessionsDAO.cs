using AutoBuildApp.DataAccess;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class SessionsDAO
    {

        public SessionsDAO()
        {
            ConnectionString = " ";
        }

        public string ConnectionString { get; set; }

        public SessionsDAO(string connection)
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

        /// <summary>
        /// do a single query that takes the users credentials and spits 
        /// back 
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        public long CreateSession(string Username, DateTimeOffset createDate)
        {
            CommonReponseAuth _CRAuth = new CommonReponseAuth(); // EMPHAREL  ( FOR THE MONENT )VALEUE NO NEED TO STORE
            long SessionsID = 0;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_CreateSession = "SP_CreateSession";
                using (SqlCommand command = new SqlCommand(SP_CreateSession, conn))
                {
                    try
                    { 
                        command.Transaction = conn.BeginTransaction();
                        #region SQL related

                        // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_LONG).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = SP_CreateSession;

                        var param = new SqlParameter[2];
                        param[0] = new SqlParameter("@USERNAME", SqlDbType.VarChar);
                        param[0].Value = Username;
                        param[1] = new SqlParameter("@BEGINTIME", SqlDbType.DateTimeOffset);
                        param[1].Value = createDate;
                        // add the commands the parameters for the stored procedure
                        command.Parameters.AddRange(param);
                        #endregion

                        var _reader = command.ExecuteReader();
                        if (!_reader.HasRows) // use the bang!!!!!!! 
                        {
                            return -1; // nothing happened
                        }
                        //READ AND STORE All THE ORDINALS YOU NEED
                        int sessionsID = _reader.GetOrdinal("ID");
                        while (_reader.Read())
                        {
                            SessionsID = (long)_reader[sessionsID];
                        }
                        _reader.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        command.Transaction.Rollback();
                        return -999;
                    }

                    command.Transaction.Commit();
                }
                return SessionsID;
            }
        }
    }
}
