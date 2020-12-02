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


// -------


namespace AutoBuildApp.WindowsForm
{
    public partial class Form1 : Form
    {

        List<UserAccount> users = new List<UserAccount>;

        public Form1()
        {
            InitializeComponent();
            // we gatta connect this list box to the data
            UserAccountsListBox.DataSource =  users;
            UserAccountsListBox.DisplayMember = "UserAccountInfo";

        }

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
            // this is for the sack of demoeing !!!!
            // this is not for production
            // this may be seperated into a seperate class
            // not to much code should be here! remeber generic

            DataAccessModule database = new DataAccessModule();

           // this may return more then one thing so store it in a list.
           users = database.RetrieveAccounts(LastNameText.Text);


        }
    }
}
