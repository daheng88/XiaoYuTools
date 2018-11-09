using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XiaoYu.PlugInBase;
using System.Web;
namespace PlugIns.UrlTools
{
    public partial class URLForm : UserControlBase
    {
        public URLForm()
        {
            InitializeComponent();
            this.ucName = "URL编码解码";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = HttpUtility.UrlEncode(this.textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = HttpUtility.UrlDecode(this.textBox1.Text);
        }
    }
}
