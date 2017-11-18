using MetroFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Faa
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //metroProgressSpinner1.Show();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPage2;
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
        }

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

        private void AddDummyData()
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
            using (SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + Path.GetDirectoryName(Application.ExecutablePath) + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False"))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"insert into M_S_CUSTOMERS values( 'Sandeep', " + RandomNumber(10, 1000) +
                    ", 'sandeep@mail.com', 'Added by sandeep', '100', '5', " + DateTime.Now.ToString() + ", 1)'", cnn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MetroFramework.MetroMessageBox.Show(this, "\n\nData Will be Deleted", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + Path.GetDirectoryName(Application.ExecutablePath) + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False"))
                {
                    using (SqlCommand cmd = new SqlCommand(@"DELETE FROM M_S_CUSTOMERS where cust_id=121", cnn))
                    {
                        // create data adapter
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        // this will query your database and return the result to your datatable
                        MetroFramework.MetroMessageBox.Show(this, "\n\nData Deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectAll();
                    }
                }
            }
            else
            {
                ///
            }
        }

        public void selectAll()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + Path.GetDirectoryName(Application.ExecutablePath) + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False"))
            {
                using (SqlCommand cmd = new SqlCommand(@"select * from M_S_CUSTOMERS", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    metroGrid3.DataSource = dataTable;
                    cnn.Close();
                    da.Dispose();
                    MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            selectAll();
        }

        private void metroTile2_Click_1(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPage4;
            AutoCompleteUsers();
        }

        private void AutoCompleteUsers()
        {
            DataTable dtUsers = new DataTable();
            using (SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + Path.GetDirectoryName(Application.ExecutablePath) + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False"))
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

                    var source = new AutoCompleteStringCollection();
                    source.AddRange(postSource);
                    metroTextBox5.AutoCompleteCustomSource = source;
                    metroTextBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    metroTextBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;

                    cnn.Close();
                    da.Dispose();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddDummyData();
        }

        
        private void metroButton9_Click(object sender, EventArgs e)
        {
            selectAllBill();
        }

        private void selectAllBill()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + Path.GetDirectoryName(Application.ExecutablePath) + "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False"))
            {
                using (SqlCommand cmd = new SqlCommand(@"select * from T_D_SALES", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    metroGrid4.DataSource = dataTable;
                    cnn.Close();
                    da.Dispose();
                    MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "Transaction Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            searchCustomer();
        }

        private void searchCustomer()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" +
                Path.GetDirectoryName(Application.ExecutablePath) +
                "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False"))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select * from M_S_CUSTOMERS where customer_name='" + metroTextBox5.Text + "'", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    metroGrid3.DataSource = dataTable;
                    cnn.Close();
                    da.Dispose();
                    MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection cnn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" +
                Path.GetDirectoryName(Application.ExecutablePath) +
                "\\App_Data\\faaDB.mdf;Initial Catalog=faaDB;Integrated Security=True;Pooling=False"))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(@"select * from T_D_SALES T JOIN M_S_CUSTOMERS C ON T.customer_id=C.cust_id where customer_id='" + metroTextBox4.Text + "'", cnn))
                {
                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    metroGrid3.DataSource = dataTable;
                    cnn.Close();
                    da.Dispose();
                    MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //autocomplete function();
            //querry "select product_id,product_code+'-'+product_name as product_name from M_S_PRODUCT where product_name like '%pack%' or product_code like '%pack%'"

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            String searchTxt = txt_product_search.Text;
            if (searchTxt.Length <= 0) {
                MetroFramework.MetroMessageBox.Show(this, "Search Text should not be empty", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //get the clicked product id from autocomplete dropdown
            //querry for getting product detail
            //select product_id,product_name,product_code,special_discount_cash,special_discount_perc,increase_amnt_by_cash from M_S_PRODUCT where product_id=''
            
            String product_name = "Packet 50";
            String product_code = "pckt50";
            String special_discount_cash = "15";
            String special_discount_perc = "10";
            String increase_amnt_by_cash = "50";
            addProductBtnInitialize();
            btn_product_delete.Enabled = true;
            btn_product_edit.Enabled = true;
            btn_product_save.Enabled = true;
            
            AddProductTxtInitialize(product_name, product_code, special_discount_perc, special_discount_cash, increase_amnt_by_cash);
            
        }

        private void btn_product_edit_Click(object sender, EventArgs e)
        {
            txt_add_incr_amnt.Enabled = true;
            txt_add_product_code.Enabled = true;
            txt_add_product_name.Enabled = true;
            txt_add_spcl_disc_amnt.Enabled = true;
            txt_add_spcl_disc_per.Enabled = true;
            btn_product_save.Text = "Update";
        }

        private void btn_product_delete_Click(object sender, EventArgs e)
        {
            var result = MetroFramework.MetroMessageBox.Show(this, "Are Your Want To Delete.?", "Item Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                //get the selected product id
                //delete querry
                //update M_D_PRODUCT set is_delete = 1 where product_id = '1'
                MetroFramework.MetroMessageBox.Show(this, "Item Deleted", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.None);
                addProductBtnInitialize();
                AddProductTxtInitialize();
            }
        }

        private void btn_product_cancel_Click(object sender, EventArgs e)
        {
            addProductBtnInitialize();
            AddProductTxtInitialize();
        }

        private void btn_product_new_Click(object sender, EventArgs e)
        {
            AddProductTxtInitialize();
            btn_product_save.Enabled = true;
            txt_add_incr_amnt.Enabled = true;
            txt_add_product_code.Enabled = true;
            txt_add_product_name.Enabled = true;
            txt_add_spcl_disc_amnt.Enabled = true;
            txt_add_spcl_disc_per.Enabled = true;
            btn_product_save.Text = "Save";
        }
        public void AddProductTxtInitialize(String pName="",String pCode="", String pPer ="0", String pAmnt = "0",String pIncrAmnt="0") {
            txt_add_incr_amnt.Text = pIncrAmnt;
            txt_add_product_code.Text = pCode;
            txt_add_product_name.Text = pName;
            txt_add_spcl_disc_amnt.Text = pAmnt;
            txt_add_spcl_disc_per.Text = pPer;
            txt_product_search.Text = "";

        }
        public void addProductBtnInitialize() {
            txt_add_incr_amnt.Enabled = false;
            txt_add_product_code.Enabled = false;
            txt_add_product_name.Enabled = false;
            txt_add_spcl_disc_amnt.Enabled = false;
            txt_add_spcl_disc_per.Enabled = false;
            btn_product_delete.Enabled = false;
            btn_product_edit.Enabled = false;
            btn_product_save.Enabled = false;
            btn_product_save.Text = "Save";
        }

        private void btn_product_save_Click(object sender, EventArgs e){
            String prod_incr_amnt = txt_add_incr_amnt.Text;
            String prod_code = txt_add_product_code.Text;
            String prod_name = txt_add_product_name.Text;
            String prod_disc_amnt = txt_add_spcl_disc_amnt.Text;
            String prod_disc_perc = txt_add_spcl_disc_per.Text;
            
            if (prod_name.Length<=0 || prod_code.Length<=0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Product Name/Product Code should not be Empty", "Cannot Be Empty", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }
            if (this.Text == "Save"){
                //insert into table
                //insert into M_S_PRODUCT(product_name,product_code,special_discount_cash,special_discount_perc,increase_amnt_by_cash) 
                //values(prod_name,prod_code,prod_disc_amnt,prod_disc_perc,prod_incr_amnt)
            }else if (this.Text == "Update"){
                //insert into table
                //get the selected product id
                //update M_S_PRODUCT set product_name = prod_name,
                //product_code = prod_code,
                //special_discount_cash = prod_disc_amnt,
                //special_discount_perc = prod_disc_perc,
                //increase_amnt_by_cash = prod_incr_amnt
                //where product_id = '1'
            }

        }
    }
}