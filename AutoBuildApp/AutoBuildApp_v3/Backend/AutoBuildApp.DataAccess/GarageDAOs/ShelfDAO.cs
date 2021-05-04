using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using AutoBuildApp.DataAccess.DAOGlobals;
using Microsoft.Data.SqlClient;

namespace AutoBuildApp.Models
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

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
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

        public bool UpdateShelf(string oldName, string newName, string user)
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
