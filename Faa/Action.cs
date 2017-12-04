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
            string sanConection = @"Data Source=ANUGRAHAA\sqlexpress;Initial Catalog=faa;Integrated Security=True";
            string newConString = ConfigurationManager.ConnectionStrings["SudevConnectionString"].ToString();
            SqlConnection sqlConnection = new SqlConnection(sanConection);
            return sqlConnection;
        }
    }
}