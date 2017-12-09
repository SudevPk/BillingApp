using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Faa
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Loader.Hide();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Loader.Show();
            frmHome frmHome = new frmHome();
            this.Hide();
            frmHome.Closed += (s, args) => this.Close();
            frmHome.Show();
        }
    }
}