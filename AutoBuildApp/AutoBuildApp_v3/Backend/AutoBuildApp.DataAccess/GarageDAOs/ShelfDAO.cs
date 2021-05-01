using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AutoBuildApp.DataAccess
{
    public class ShelfDAO
    {
        private readonly string _connectionString;

        public ShelfDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool InsertShelf(string shelfID, string user)
        {
            bool success = false;
            string insertRequest = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;

                    

                    var rowsAdded = command.ExecuteNonQuery();
                    if(rowsAdded == 1)
                    {
                        command.Transaction.Commit();
                        success = true;
                    }
                }
            }

            return success;
        }

        public bool DeleteShelf(string shelfID)
        {
            return false;
        }

        public bool AddComponent(IComponent item)
        {
            return false;
        }

        public bool RemoveComponent(int id)
        {
            return false;
        }

        public bool UpdateShelf(int id)
        {
            return false;
        }

        public List<IComponent> GetComponentsByShelf(int id)
        {
            List<IComponent> outputList = new List<IComponent>();

            return outputList;
        }

        public IComponent GetComponentByID(int id)
        {
            return null;
        }
    }    
}
