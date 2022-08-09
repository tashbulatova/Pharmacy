using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

/// <summary>
/// Параметры для подключения к БД
/// </summary>
namespace my1
{
    class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string datasource = @"(localdb)\MSSQLLocalDB";
            string database = "Pharmacy";
            string username = "";
            string password = "";
            return DBConn1.GetDBConnection(datasource, database, username, password);
        }
    }
}
