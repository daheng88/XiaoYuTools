using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XiaoYu.PlugInBase;
using XiaoYuI.Utility.Security;

namespace PlugIns.Security
{
    public partial class Form1 : UserControlBase
    {
        public Form1()
        {
            InitializeComponent();
            this.ucName = "MD5编码";
            this.radioButton16.Checked = true;
            this.comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = this.textBox1.Text;
            bool low = this.comboBox1.SelectedIndex == 0;
            string result = input;
            if (this.radioButton16.Checked)
            {
                result = MD5Extend.MD5Encrypt16(input);
            }
            else if (this.radioButton32.Checked)
            {
                result = MD5Extend.MD5Encrypt(input);
            }
            else
            {
                result = MD5Extend.MD5Encrypt64(input);
            }
            if (low)
            {
                result = result.ToLower();
            }
            else
            {
                result = result.ToUpper();
            }
            this.textBox2.Text = result;

        }

    }
}
