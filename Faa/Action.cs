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
            string conString = "";
            conString = "Data Source=" + System.Environment.MachineName + @"\sqlexpress;Initial Catalog=faa;Integrated Security=True";
            return new SqlConnection(conString);
        }
    }
}