using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Faa
{
    public class CrudAction
    {
        private Action action = new Action();
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private Random gen = new Random();

        private DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public DataTable selectAllCustomers()
        {
            DataTable dataTable = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select * from M_S_CUSTOMERS order by created_date desc", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    cnn.Close();
                    da.Dispose();
                }
            }
            return dataTable;
        }

        public void deleteById(int id)
        {
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"DELETE FROM M_S_CUSTOMERS where cust_id=121", cnn))
                {
                    cmd.ExecuteNonQuery();
                    // this will query your database and return the result to your datatable
                }
            }
        }

        public void deleteAllCustomers()
        {
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"DELETE FROM M_S_CUSTOMERS", cnn))
                {
                    cmd.ExecuteNonQuery();
                    // this will query your database and return the result to your datatable
                }
            }
        }

        public string[] AutoCompleteUsers()
        {
            DataTable dtUsers = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select customer_name from M_S_CUSTOMERS", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dtUsers);
                    //use LINQ method syntax to pull the Title field from a DT into a string array...
                    string[] postSource = dtUsers
                                        .AsEnumerable()
                                        .Select<System.Data.DataRow, String>(x => x.Field<String>("customer_name"))
                                        .ToArray();
                    cnn.Close();
                    da.Dispose();
                    return postSource;
                }
            }
        }

        public DataTable selectAllBill()
        {
            DataTable dataTable = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select * from T_D_SALES order by sales_date desc", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    cnn.Close();
                    da.Dispose();
                    return dataTable;
                }
            }
        }

        public DataTable selectAllProducts()
        {
            DataTable dataTable = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select * from M_S_PRODUCT order by created_date desc", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    cnn.Close();
                    da.Dispose();
                    return dataTable;
                }
            }
        }

        public DataTable SearchCustomerByName(string customerName)
        {
            DataTable dataTable = new DataTable();
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select * from M_S_CUSTOMERS where customer_name='" + customerName + "'", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    cnn.Close();
                    da.Dispose();
                    return dataTable;
                }
            }
        }

        public DataTable BillDetailsById(string billId)
        {
            DataTable dataTable = new DataTable();
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select * from T_D_SALES T JOIN M_S_CUSTOMERS C ON T.customer_id=C.cust_id where customer_id='" + billId + "'", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    cnn.Close();
                    da.Dispose();
                    return dataTable;
                }
            }
        }

        public void AddDummyData()
        {
            /////Addind 100 Dummy Data
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("customer_name");
            dt.Columns.Add("customer_phone");
            dt.Columns.Add("customer_email");
            dt.Columns.Add("customer_address");
            dt.Columns.Add("special_discount_cash");
            dt.Columns.Add("special_discount_perc");
            dt.Columns.Add("created_date");
            dt.Columns.Add("is_delete");

            for (int i = 0; i < 100; i++)
            {
                dt.Rows.Add(new object[] { RandomString(6), RandomString(10), RandomString(4) + "@mail.com", RandomString(15), "100", "5", RandomDay().ToString(), 0 });
            }
            dt.Rows.Add(new object[] { "Sudev", RandomNumber(10, 1000), "sudev@mail.com", "Added by sudev", "100", "5", DateTime.Now.ToString(), 1 });

            //string constring = @"Data Source=DESKTOP-OL65NEI\SQLEXPRESS;Initial Catalog=faaDB;Integrated Security=True;Pooling=False";
            string constring = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + Path.GetDirectoryName(Application.ExecutablePath) + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False";
            ds.Tables.Add(dt);
            using (var bulkCopy = new SqlBulkCopy(constring, SqlBulkCopyOptions.KeepIdentity))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }

                bulkCopy.BulkCopyTimeout = 600;
                bulkCopy.DestinationTableName = "M_S_CUSTOMERS";
                bulkCopy.WriteToServer(dt);
            }
            ////Adding 1 Dummy Data
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"insert into M_S_CUSTOMERS values( 'Sandeep', " + RandomNumber(10, 1000) +
                    ", 'sandeep@mail.com', 'Added by sandeep', '100', '5', " + DateTime.Now.ToString() + ", 1)'", cnn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string[] AutoCompleteProducts()
        {
            DataTable dtUsers = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select product_name from M_S_PRODUCT", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dtUsers);
                    //use LINQ method syntax to pull the Title field from a DT into a string array...
                    string[] postSource = dtUsers
                                        .AsEnumerable()
                                        .Select<System.Data.DataRow, String>(x => x.Field<String>("product_name"))
                                        .ToArray();
                    cnn.Close();
                    da.Dispose();
                    return postSource;
                }
            }
        }

        public void InsertProduct(string prod_incr_amnt, string prod_code, string prod_name, string prod_disc_amnt, string prod_disc_perc)
        {
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"insert into M_S_PRODUCT(product_name,product_code,special_discount_cash,special_discount_perc,increase_amnt_by_cash)
                values(" + prod_name + "," + prod_code + ", " + prod_disc_amnt + "," + prod_disc_perc + ", " + prod_incr_amnt + ")", cnn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}