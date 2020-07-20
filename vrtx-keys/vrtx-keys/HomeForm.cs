using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vrtx_keys;

namespace vrtx_keys
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
        }

        private void HomeForm_KeyPress(object sender, KeyPressEventArgs e) {
            this.Focus();
            this.Hide();
            Console.Write(e.KeyChar);
   
        }
    }
}
