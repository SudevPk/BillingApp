﻿using MetroFramework;
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
        private Action action = new Action();
        private CrudAction crudAction = new CrudAction();
        private UtilityAction utilityAction = new UtilityAction();

        public frmHome()
        {
            InitializeComponent();
            AutoCompleteProducts();
            AutoCompleteUsers();
            this.metroGrid5.Columns["Date"].DefaultCellStyle.NullValue = System.DateTime.Today.ToString();
            this.metroGrid5.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvUserDetails_RowPostPaint);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvUserDetails_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(metroGrid5.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
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

        private void metroButton5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(this, "\n\nData Will be Deleted ! Are You Sure ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                int id = 121;
                crudAction.deleteAllCustomers();
                MetroFramework.MetroMessageBox.Show(this, "\n\nData Deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                metroGrid3.DataSource = crudAction.selectAllCustomers();
                MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ///Do nothing
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            metroGrid3.DataSource = crudAction.selectAllCustomers();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            crudAction.AddDummyData();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            metroGrid4.DataSource = crudAction.selectAllBill();
            MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "Transaction Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            metroGrid3.DataSource = crudAction.SearchCustomerByName(metroTextBox5.Text);
            MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            metroGrid4.DataSource = crudAction.BillDetailsById(metroTextBox4.Text);
            MetroFramework.MetroMessageBox.Show(this, "\n\nSuccess.", "View All", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            metroTextBox5.AutoCompleteCustomSource = source;
            metroTextBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            metroTextBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPage4;
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabPage3;
        }

        private void metroTabPage1_Click(object sender, EventArgs e)
        {
        }

        private void metroGrid5_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string Item = "";
            string Quantity = "1";
            string RatePerItem = "1";
            string Total = "0";
            string GSTRate = "18";
            string Discount = "0";
            string ReceivedAmount = "0";
            string PendingAmount = "0";
            string Date = System.DateTime.Today.ToString();
            double grandTotal = 0, pendingTotal = 0;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.metroGrid5.Rows[e.RowIndex];
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
                Date = isNullorEmpy(row.Cells["Date"].Value) == true ? row.Cells["Date"].Value.ToString() : System.DateTime.Today.ToString();
                row.Cells["Total"].Value = int.Parse(Quantity) * int.Parse(RatePerItem);
            }
            for (int i = 0; i < this.metroGrid5.Rows.Count - 1; i++)
            {
                grandTotal += Convert.ToDouble(this.metroGrid5.Rows[i].Cells["TotalAmount"].Value);
            }
            metroTextBox6.Text = grandTotal.ToString();
            metroTextBox7.Text = grandTotal.ToString();
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
            var col = this.metroGrid5.CurrentCell.ColumnIndex;
            if (col == 1 || col == 2 || col == 3 || col == 5 || col == 6) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
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
            metroTextBox7.Text = (int.Parse(metroTextBox6.Text) - int.Parse(metroTextBox14.Text)).ToString();
        }
    }
}