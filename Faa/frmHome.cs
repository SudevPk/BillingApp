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
using System.Drawing.Imaging;
using System.Drawing.Printing;
using MaterialSkin;

using System.Reflection;
using System.Runtime.InteropServices;

using Newtonsoft.Json;
using ClosedXML.Excel;

namespace Faa
{
    public partial class frmHome : Form
    {
        private Action action = new Action();
        private CrudAction crudAction = new CrudAction();
        private UtilityAction utilityAction = new UtilityAction();

        public frmHome()
        {
            InitializeComponent();
            AutoCompleteProducts();
            AutoCompleteUsers();
            AutoCompleteUserMobile();
            this.billGrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvUserDetails_RowPostPaint);
        }

        private void dgvUserDetails_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(billGrid.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //metroProgressSpinner1.Show();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            Billing.SelectedTab = metroTabPage2;
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(this, "\n\nData Will be Deleted ! Are You Sure ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                int id = 121;
                crudAction.deleteAllCustomers();
                MetroFramework.MetroMessageBox.Show(this, "\n\nData Deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                userGrid.DataSource = crudAction.selectAllCustomers();
                MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ///Do nothing
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            crudAction.AddDummyData();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            allBillGrid.DataSource = crudAction.selectAllBill();
            MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "Transaction Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            userGrid.DataSource = crudAction.SearchCustomerByName(metroTextBox5.Text);
            MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            allBillGrid.DataSource = crudAction.BillDetailsById(metroTextBox4.Text);
            MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //autocomplete function();
            //querry "select product_id,product_code+'-'+product_name as product_name from M_S_PRODUCT where product_name like '%pack%' or product_code like '%pack%'"
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            String searchTxt = txt_product_search.Text;
            if (searchTxt.Length <= 0)
            {
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

        public void AddProductTxtInitialize(String pName = "", String pCode = "", String pPer = "0", String pAmnt = "0", String pIncrAmnt = "0")
        {
            txt_add_incr_amnt.Text = pIncrAmnt;
            txt_add_product_code.Text = pCode;
            txt_add_product_name.Text = pName;
            txt_add_spcl_disc_amnt.Text = pAmnt;
            txt_add_spcl_disc_per.Text = pPer;
            txt_product_search.Text = "";
        }

        public void addProductBtnInitialize()
        {
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

        private void btn_product_save_Click(object sender, EventArgs e)
        {
            String prod_incr_amnt = txt_add_incr_amnt.Text;
            String prod_code = txt_add_product_code.Text;
            String prod_name = txt_add_product_name.Text;
            String prod_disc_amnt = txt_add_spcl_disc_amnt.Text;
            String prod_disc_perc = txt_add_spcl_disc_per.Text;

            if (prod_name.Length <= 0 || prod_code.Length <= 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Product Name/Product Code should not be Empty", "Cannot Be Empty", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }
            if (this.Text == "Save")
            {
                crudAction.InsertProduct(prod_incr_amnt, prod_code, prod_name, prod_disc_amnt, prod_disc_perc);
                //insert into table
                //insert into M_S_PRODUCT(product_name,product_code,special_discount_cash,special_discount_perc,increase_amnt_by_cash)
                //values(prod_name,prod_code,prod_disc_amnt,prod_disc_perc,prod_incr_amnt)
            }
            else if (this.Text == "Update")
            {
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

        private void AutoCompleteProducts()
        {
            string[] postSource = crudAction.AutoCompleteProducts();
            var source = new AutoCompleteStringCollection();
            source.AddRange(postSource);
            txt_product_search.AutoCompleteCustomSource = source;
            txt_product_search.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_product_search.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void AutoCompleteUsers()
        {
            string[] postSource = crudAction.AutoCompleteUsers();
            var source = new AutoCompleteStringCollection();
            source.AddRange(postSource);
            customerName.AutoCompleteCustomSource = source;
            customerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            customerName.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void AutoCompleteUserMobile()
        {
            string[] postSource = crudAction.AutoCompleteUserMobile();
            var source = new AutoCompleteStringCollection();
            source.AddRange(postSource);
            mobileNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
            mobileNumber.AutoCompleteCustomSource = source;
            mobileNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            mobileNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            Billing.SelectedTab = metroTabPage4;
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            Billing.SelectedTab = metroTabPage3;
        }

        private void metroGrid5_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string Item = "";
            string Quantity = "1";
            string RatePerItem = "1";
            string Total = "0";
            string GSTRate = "18";
            string Discount = "0";
            double grandTotal = 0;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.billGrid.Rows[e.RowIndex];
                Item = isNullorEmpy(row.Cells["Item"].Value) == true ? row.Cells["Item"].Value.ToString() : "1";
                if (Item == "Hard Tissue")
                    row.Cells["RatePerItem"].Value = 100;
                else
                    if (Item == "Soft Tissue")
                        row.Cells["RatePerItem"].Value = 50;
                Quantity = isNullorEmpy(row.Cells["Quantity"].Value) == true ? row.Cells["Quantity"].Value.ToString() : "1";
                RatePerItem = isNullorEmpy(row.Cells["RatePerItem"].Value) == true ? row.Cells["RatePerItem"].Value.ToString() : "1";
                Discount = isNullorEmpy(row.Cells["Discount"].Value) == true ? row.Cells["Discount"].Value.ToString() : "0";
                Total = (int.Parse(Quantity) * int.Parse(RatePerItem)).ToString();
                Total = (int.Parse(Total) + (int.Parse(Total) * int.Parse(GSTRate) / 100)).ToString();
                Total = Discount == "0" ? Total : (int.Parse(Total) - ((int.Parse(Total) * int.Parse(Discount) / 100))).ToString();
                row.Cells["GSTRate"].Value = GSTRate;
                row.Cells["TotalAmount"].Value = Total;
                row.Cells["Total"].Value = int.Parse(Quantity) * int.Parse(RatePerItem);
            }
            for (int i = 0; i < this.billGrid.Rows.Count - 1; i++)
            {
                grandTotal += Convert.ToDouble(this.billGrid.Rows[i].Cells["TotalAmount"].Value);
            }
            this.grandTotal.Text = grandTotal.ToString();
            this.pendingAmount.Text = grandTotal.ToString();
        }

        private bool isNullorEmpy(object value)
        {
            if (value != "" && value != null)
                return true;
            else
                return false;
        }

        private void metroGrid5_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            var col = this.billGrid.CurrentCell.ColumnIndex;
            if (col == 1 || col == 2 || col == 3 || col == 5 || col == 6) //Desired Column
            {
                System.Windows.Forms.TextBox tb = e.Control as System.Windows.Forms.TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void metroTextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }

        private void metroTextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }

        private void metroTextBox14_TextChanged_1(object sender, EventArgs e)
        {
            pendingAmount.Text = (int.Parse(grandTotal.Text) - int.Parse(receivedAmount.Text)).ToString();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            bool isVaid = validateBill();
            if (isVaid)
            {
                DataTable billDataTable = new DataTable();
                billDataTable.Clear();
                billDataTable.Columns.Add("Item");
                billDataTable.Columns.Add("Quantity");
                billDataTable.Columns.Add("RatePerItem");
                billDataTable.Columns.Add("Discount");
                billDataTable.Columns.Add("GSTRate");
                billDataTable.Columns.Add("Total");
                billDataTable.Columns.Add("TotalAmount");
                billDataTable.Columns.Add("Sale Date");
                billDataTable.Columns.Add("Grand Total");
                billDataTable.Columns.Add("Recieved");
                billDataTable.Columns.Add("Pending");
                if (billGrid.Rows.Count != 1)
                {
                    DataRow row = billDataTable.NewRow();
                    for (int item = 0; item < billGrid.Rows.Count - 1; item++)
                    {
                        row["Item"] = this.billGrid.Rows[item].Cells["Item"].Value.ToString();
                        row["Quantity"] = this.billGrid.Rows[item].Cells["Quantity"].Value == null ? "1" : this.billGrid.Rows[item].Cells["Quantity"].Value.ToString();
                        row["RatePerItem"] = this.billGrid.Rows[item].Cells["RatePerItem"].Value.ToString();
                        row["GSTRate"] = this.billGrid.Rows[item].Cells["GSTRate"].Value.ToString();
                        row["Discount"] = this.billGrid.Rows[item].Cells["Discount"].Value == null ? "0" : this.billGrid.Rows[item].Cells["Discount"].Value.ToString();
                        row["Total"] = this.billGrid.Rows[item].Cells["Total"].Value.ToString();
                        row["TotalAmount"] = this.billGrid.Rows[item].Cells["TotalAmount"].Value.ToString();
                        billDataTable.Rows.Add(row);
                    }
                    DataRow Lastrow = billDataTable.NewRow();
                    Lastrow["Sale Date"] = this.saleDate.Value;
                    Lastrow["Grand Total"] = this.grandTotal.Text;
                    Lastrow["Recieved"] = this.receivedAmount.Text;
                    Lastrow["Pending"] = this.pendingAmount.Text;
                    billDataTable.Rows.Add(Lastrow);
                    var path = @"C:\faaExcel\Bill\";
                    if (!File.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var currentDate = DateTime.Now.Day.ToString() + '_' + DateTime.Now.Month.ToString() + '_' + DateTime.Now.Year.ToString() + '_' +
                        DateTime.Now.TimeOfDay.ToString().Replace(":", "_").Replace(".", "_") + '_';
                    var filename = path + customerName.Text + currentDate + "_Bill.xlsx";
                    crudAction.exportExcel(billDataTable, filename);
                    MetroFramework.MetroMessageBox.Show(this, "Exported to \n" + filename, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Add a Product", "Cannot Be Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool validateBill()
        {
            if (companyName.Text == "")
            {
                MessageBox.Show("Enter Company Name !");
                companyName.Focus();
                return false;
            }
            else if (customerName.Text == "")
            {
                MessageBox.Show("Enter Customer Name !");
                customerName.Focus();
                return false;
            }
            else if (mobileNumber.Text == "")
            {
                MessageBox.Show("Enter Mobile Number !");
                mobileNumber.Focus();
                return false;
            }
            else if (mobileNumber.Text.Length < 10)
            {
                MessageBox.Show("Enter Valid Mobile Number(10 numbers) !");
                mobileNumber.Focus();
                return false;
            }
            else if (address.Text == "")
            {
                MessageBox.Show("Enter Address !");
                address.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            userGrid.DataSource = crudAction.selectAllCustomers();
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            if (this.userGrid.DataSource != null)
            {
                var dt = (DataTable)this.userGrid.DataSource;
                //To Get the Default View
                dt = dt.DefaultView.ToTable();
                var path = @"C:\faaExcel\User\";
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var currentDate = DateTime.Now.Day.ToString() + '_' + DateTime.Now.Month.ToString() + '_' + DateTime.Now.Year.ToString() + '_' +
                     DateTime.Now.TimeOfDay.ToString().Replace(":", "_").Replace(".", "_") + '_';
                var filename = path + currentDate + "_User.xlsx";
                crudAction.exportExcel(dt, filename);
                MetroFramework.MetroMessageBox.Show(this, "Exported to \n" + filename, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Add a Product", "Cannot Be Empty", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            if (this.allBillGrid.DataSource != null)
            {
                var dt = (DataTable)this.allBillGrid.DataSource;
                //To Get the Default View
                dt = dt.DefaultView.ToTable();
                var path = @"C:\faaExcel\AllBill\";
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var currentDate = DateTime.Now.Day.ToString() + '_' + DateTime.Now.Month.ToString() + '_' + DateTime.Now.Year.ToString() + '_' +
                     DateTime.Now.TimeOfDay.ToString().Replace(":", "_").Replace(".", "_") + '_';
                var filename = path + currentDate + "_AllBill.xlsx";
                crudAction.exportExcel(dt, filename);
                MetroFramework.MetroMessageBox.Show(this, "Exported to \n" + filename, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Add a Product", "Cannot Be Empty", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void metroTextBox10_TextChanged(object sender, EventArgs e)
        {
            System.Data.DataTable dt = crudAction.AutoCompleteBillDetails(mobileNumber.Text);
            if (dt.Rows.Count > 0)
            {
                customerName.Text = dt.Rows[0]["customer_name"].ToString();
                address.Text = dt.Rows[0]["customer_address"].ToString();
                email.Text = dt.Rows[0]["customer_email"].ToString();
            }
        }

        private void Billing_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void metroTabPage7_Click(object sender, EventArgs e)
        {
        }

        private void metroButton14_Click(object sender, EventArgs e)
        {

        }
    }
}