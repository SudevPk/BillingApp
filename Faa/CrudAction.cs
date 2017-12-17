using ClosedXML.Excel;
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
                using (SqlCommand cmd = new SqlCommand(@"select  customer_name as Name,customer_phone as Phone,
                                        customer_email as Email,customer_address as Address,created_date 'Created Date' from M_S_CUSTOMERS order by created_date desc", cnn))
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
                using (SqlCommand cmd = new SqlCommand(@"select customer_name+'('+customer_phone+')' as customer_name,cust_id from M_S_CUSTOMERS where isNull(is_delete,0) = 0", cnn))
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

        public string[] AutoCompleteUserMobile()
        {
            DataTable dtUsers = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select customer_phone from  M_S_CUSTOMERS", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dtUsers);
                    //use LINQ method syntax to pull the Title field from a DT into a string array...
                    string[] postSource = dtUsers
                                        .AsEnumerable()
                                        .Select<System.Data.DataRow, String>(x => x.Field<String>("customer_phone"))
                                        .ToArray();
                    cnn.Close();
                    da.Dispose();
                    return postSource;
                }
            }
        }

        public DataTable AutoCompleteBillDetails(string mobileNumber)
        {
            DataTable dtUsers = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select cust_id,customer_name,customer_phone,customer_email,customer_address
                                                from M_S_CUSTOMERS where customer_name+'(' +customer_phone+')' ='" + mobileNumber + "'", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dtUsers);
                    cnn.Close();
                    da.Dispose();
                    return dtUsers;
                }
            }
        }

        public DataTable selectAllBill()
        {
            DataTable dataTable = new DataTable();
            using (var cnn = action.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select customer_name as Name,customer_phone as Mobile,customer_email as Email,sales_date
                                             as 'Sale Date',total_amnt as 'Total Amount',amnt_paid as 'Amount paid',current_sales_balance as 'Balance' from T_D_SALES
                                             T JOIN M_S_CUSTOMERS C ON T.customer_id=C.cust_id order by sales_date desc", cnn))
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

        public DataTable SearchCustomerByName(string customerName, String phone)
        {
            DataTable dataTable = new DataTable();
            String sql = "";
            if (customerName == "ALL" && phone == "ALL")
            {
                sql = " select  ROW_NUMBER() over(order by C.cust_id asc) as SlNo, " +
                        " C.customer_name+'('+C.customer_phone+')' as Customer, " +
                        " count(S.sales_id) as NoOfInvoice, " +
                        " sum(S.total_amnt) as TotalAmount, " +
                        " sum(S.amnt_paid) as TotalPaid, " +
                        " sum(S.current_sales_balance) TotalBalance " +
                        " from M_S_CUSTOMERS C " +
                        " inner join T_D_SALES S on C.cust_id = S.customer_id and isnull(S.is_delete, 0) = 0 " +
                        " where isNull(C.is_delete,0) = 0 " +
                        " group by C.customer_name+'('+C.customer_phone+')',C.cust_id   " +
                        " order by TotalBalance desc";
            }
            else
            {
                sql = " select  ROW_NUMBER() over(order by S.sales_id asc) as SlNo, " +
                        " cast(S.sales_date as date) as Date,S.sales_id as InvoiceNo,total_amnt as BillAmount,S.amnt_paid as Paid, " +
                        " S.current_sales_balance as Pending " +
                        " from M_S_CUSTOMERS C " +
                        " inner join T_D_SALES S on C.cust_id = S.customer_id and isnull(S.is_delete, 0) = 0 " +
                        " where customer_name = '" + customerName + "' and customer_phone = '" + phone + "' and isNull(C.is_delete,0) = 0 " +
                        " order by S.sales_date ";
            }
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@sql, cnn))
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
            DataTable dataTable = new DataTable("billGrid");
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select item_name as Item,item_qty as Quantity,item_price_per_piece as RatePerItem,total as Total,
                                                        c_gst+s_gst as GSTRate,0 as Discount,0 as TotalAmount
                                                        from T_D_SALES_ITEMS SI INNER JOIN T_D_SALES S on S.sales_id=SI.sales_id where S.sales_id='" + billId + "'", cnn))
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

        public void exportExcel(DataTable dt, string filename)
        {
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Data");
                wb.SaveAs(filename, false);
            }
        }

        public void AddUser(string customerName, string mobileNumber, string email, string address)
        {
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [dbo].[M_S_CUSTOMERS]
                                                            ([customer_name]
                                                            ,[customer_phone]
                                                            ,[customer_email]
                                                            ,[customer_address]
                                                            ,[created_date]
                                                            ,[is_delete])
                                                        VALUES
                                                            ('" + customerName + "','" + mobileNumber + "','" + email + "','" + address + "','" + DateTime.Now + "',0)"
                                                            , cnn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int AddSales(string p1, string p2, string p3, string p4, string p5)
        {
            DataTable dtUsers = new DataTable();
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [dbo].[T_D_SALES]
                                                            ([sales_date]
                                                            ,[customer_id]
                                                            ,[total_amnt]
                                                            ,[amnt_paid]
                                                            ,[current_sales_balance]
                                                            ,[last_updated]
                                                            ,[is_delete])
                                                        VALUES
                                                            ('" + Convert.ToDateTime(p1) + "','" + p2 + "','" + p3 + "','" + p4 + "','" + p5 + "','" + DateTime.Now + "',0);SELECT sales_id from [dbo].[T_D_SALES] where sales_id=SCOPE_IDENTITY();", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dtUsers);
                    if (dtUsers.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dtUsers.Rows[0]["sales_id"]);
                    }
                    else
                    {
                        return 0;
                    }

                    //use LINQ method syntax to pull the Title field from a DT into a string array...
                }
            }
        }

        public void AddSaleitems(DataTable billDataTable)
        {
            string conString = "Data Source=" + System.Environment.MachineName + @"\sqlexpress;Initial Catalog=faa;Integrated Security=True";
            using (var bulkCopy = new SqlBulkCopy(conString, SqlBulkCopyOptions.KeepIdentity))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                foreach (DataColumn col in billDataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }

                bulkCopy.BulkCopyTimeout = 600;
                bulkCopy.DestinationTableName = "T_D_SALES_ITEMS";
                bulkCopy.WriteToServer(billDataTable);
            }
        }

        public string GetSalesId()
        {
            using (var cnn = action.getConnection())
            {
                DataTable dtUsers = new DataTable();
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select MAX(sales_id)+1 sales_id from T_D_SALES", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dtUsers);
                    if (dtUsers.Rows.Count > 0)
                    {
                        return dtUsers.Rows[0]["sales_id"].ToString();
                    }
                    else
                        return "";
                    //use LINQ method syntax to pull the Title field from a DT into a string array...
                }
            }
        }

        public DataTable UserDetailsByBillId(string p)
        {
            DataTable dtUsers = new DataTable();
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select  sales_date,cust_id,customer_name,customer_phone,customer_email,customer_address
                                                           from [T_D_SALES] INNER JOIN M_S_CUSTOMERS ON customer_id=cust_id
                                                            where sales_id=" + p + "", cnn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtUsers);
                    return dtUsers;
                }
            }
        }

        public DataTable TotalDetailsById(string p)
        {
            DataTable dtUsers = new DataTable();
            using (var cnn = action.getConnection())
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select total_amnt,amnt_paid,current_sales_balance from T_D_SALES where sales_id=" + p + "", cnn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtUsers);
                    return dtUsers;
                }
            }
        }
    }
}