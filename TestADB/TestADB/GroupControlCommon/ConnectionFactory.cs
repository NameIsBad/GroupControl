using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Common
{
   public  class ConnectionFactory
    {
        public static readonly string connectionName = ConfigurationManager.AppSettings["ConnectionName"];
        public static readonly string connString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        private static readonly object _obj = new object();

        private static IDbConnection connection;

        public IDbConnection CreateConnection()
        {
            switch (connectionName)
            {
                case "SQLServer":
                    GetConnection();
                    break;
                case "MySQL":
                    //   conn = new MySqlConnection(connString);
                    break;
                default:
                    GetConnection();
                    break;
            }

            return connection;
        }

        public IDbConnection GetConnection()
        {

            if (null == connection)
            {
                connection = new SqlConnection(connString);

                connection.Open();
            }

            else if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            else if (connection.State == ConnectionState.Broken)
            {
                connection.Close();

                connection.Open();
            }

            return connection;
        }
    }
}
