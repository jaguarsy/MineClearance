using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 扫雷
{
    public partial class Form2 : Form
    {
        Form1 frm = new Form1();
        public Form2()
        {
            InitializeComponent();
            label2.Text = Convert.ToString(frm.lNum);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
