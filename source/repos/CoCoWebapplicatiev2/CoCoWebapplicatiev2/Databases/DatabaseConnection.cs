using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using CoCoWebapplicatiev2.Classes;
using FirebirdSql.Data.FirebirdClient;


namespace CoCoWebapplicatiev2.Databases
{
    public class DatabaseConnectie
    {
        //Connect to the database
        private string connString =
            "Data Source=10.0.20.150;" +
            @"Initial Catalog=C:\CoCo\Data\MAINBASE.GDB;" +
            "User id=SYSDBA;" +
            "Password=masterkey;";

        FbConnection connection = new FbConnection();


        public DatabaseConnectie()
        {
        }

        /**
         * @Kyle van Raaij
         * make a connection with the database.
         * 
         * @return firebird database connection
         **/
        public FbConnection GetFbConnection()
        {
            FbConnection connection = new FbConnection();
            connection.ConnectionString = connString;
            return connection;
        }

    }
}