using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

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
            //    string constring = @"Data Source=DESKTOP-OL65NEI\SQLEXPRESS;Initial Catalog=faaDB;Integrated Security=True;Pooling=False";
            //    string testConstring = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename="
            //        + Path.GetDirectoryName(Application.ExecutablePath)
            //        + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename="
                + Path.GetDirectoryName(Application.ExecutablePath)
                + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False";

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }
    }
}