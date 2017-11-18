using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace Faa
{
    public class Action
    {
        /// <summary>
        /// SQL Connection
        /// </summary>
        /// <returns></returns>
        public SqlConnection getConnection()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\App_Data\faaDB.mdf;
                                        Initial Catalog=faaDB;Integrated Security=True;Pooling=False";
            string newConString = ConfigurationManager.ConnectionStrings["ConnectionStringFaaDB"].ToString();
            SqlConnection sqlConnection = new SqlConnection(newConString);
            return sqlConnection;
        }
    }
}