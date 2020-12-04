using AutoBuildApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/ 

using Microsoft.Data.SqlClient;

// -------


namespace AutoBuildApp.WindowsForm
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();

        }

        DataSet ds = new DataSet();
        Microsoft.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter();

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {




        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            // we have created a new connection to the SQL database remember this template
            // this is essentially that new Microsoft.Data.SqlClient.Sqlconnection
            using (SqlConnection connection = new SqlConnection(ConnectionStringHelperClass.ConnectNow("AutoBuildDB")))
            {// using statement is used because it automatically closes when you reach the end curly brace

                // good reference: https://www.dotnetperls.com/sqlconnection



                // documentation for  Microsoft.Data.SqlClient;
                // https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient?view=sqlclient-dotnet-core-2.1

                adapter.InsertCommand = new SqlCommand("INSERT INTO userAccounts(firstName, lastName, roley)  VALUES( @FIRSTNAME, @LASTNAME, @ROLEY);", connection);
                adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = FirstNameText.Text;
                adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = LastNameText.Text;
                adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = RoleText.Text;

                connection.Open();
                adapter.InsertCommand.ExecuteNonQuery();
                //MessageBox.Show(connection.State.ToString());
                connection.Close();



            }// as sson as this curly brace is reached the connection is killed!


        }

        private void label1_Click(object sender, EventArgs e)
        {


        }

        private void serach_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionStringHelperClass.ConnectNow("AutoBuildDB")))
            {// using statement is used because it automatically closes when you reach the end curly brace

                // good reference: https://www.dotnetperls.com/sqlconnection

                adapter.SelectCommand = new SqlCommand("SELECT* FROM userAccounts;", connection);

                ds.Clear();
                adapter.Fill(ds);
                dg.DataSource = ds.Tables[0];


            }
        }
    }
}
