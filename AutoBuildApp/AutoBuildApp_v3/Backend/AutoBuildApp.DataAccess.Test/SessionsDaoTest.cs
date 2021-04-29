using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess.Test
{

    [TestClass]

    public class SessionsDaoTest
    {
       private static SessionsDAO _sessionsDAO = 
            new SessionsDAO("Data Source=localhost;Initial Catalog=DB;Integrated Security=True");


            private static IEnumerable<object[]> getcheckConnectionData()
            {
            return new List<object[]>(){
                    new object[]{"hoseho", DateTimeOffset.Now }}; }

            [TestMethod]
            [DataTestMethod]
            [DynamicData(nameof(getcheckConnectionData), DynamicDataSourceType.Method)]
            public void AuthDAO_checkConnection_CheckConnection
                (string Username, DateTimeOffset createDate) {

            Console.WriteLine($" created date: {createDate}");
            long actual = _sessionsDAO.CreateSession(Username, createDate);

            Console.WriteLine($" Sessions ID: {actual} \n" );
            //Assert:
            Assert.IsNotNull(actual);

            }

        }
    }
