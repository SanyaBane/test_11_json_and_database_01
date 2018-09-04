using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace test_11_json_and_database_01.Database
{
    class DBConnect
    {
        private MySqlConnection connection;

        private string server;
        private string database;
        private string uid;
        private string password;

        private static readonly DBConnect instance = new DBConnect();
        static DBConnect() { }

        private DBConnect()
        {
            Initialize();
        }

        public static DBConnect Instance
        {
            get
            {
                return instance;
            }
        }

        private void Initialize()
        {
            server = "localhost";
            database = "test_08_treeview_countries";
            uid = "root";
            password = "1113";

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" +
                "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);

        }

        private MySqlTransaction mySqlTransaction = null;

        public void TransactionBegin()
        {
            if (mySqlTransaction == null)
            {
                mySqlTransaction = connection.BeginTransaction();
            }
        }

        public void TransactionCommit()
        {
            if (mySqlTransaction != null)
            {
                mySqlTransaction.Commit();
                mySqlTransaction = null;
            }
        }

        public void TransactionRollback()
        {
            if(mySqlTransaction != null)
            {
                mySqlTransaction.Rollback();
                mySqlTransaction = null;
            }
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password.");
                        break;
                }

                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        

        public bool Insert(MySqlCommand cmd)
        {
            if (connection.State == ConnectionState.Open)
            {

                cmd.Connection = connection;

                try
                {
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch(MySqlException ex)
                {
                    throw ex;
                    return false;
                }
            }

            return false;
        }
    }
}
