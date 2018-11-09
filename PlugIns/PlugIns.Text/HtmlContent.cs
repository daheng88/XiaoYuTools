using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XiaoYu.PlugInBase;
using XiaoYuI.Utility;

namespace PlugIns.Text
{
    public partial class HtmlContent : UserControlBase
    {
        public HtmlContent()
        {
            InitializeComponent();
            this.ucName = "html清除标签";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string content = this.textBox1.Text;


            this.textBox2.Text = HttpHelper.Html2Text(content);
        }
    }
}
