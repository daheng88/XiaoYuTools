using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XiaoYu.PlugInBase;

namespace PlugIns.Text
{
    public partial class Form1  : UserControlBase
    {
        public Form1()
        {
            InitializeComponent();
            this.ucName = "字数统计";
            this.label2.Text = "字数统计:";
        }

        private void button_Click(object sender, EventArgs e)
        {
             string content = this.textBox1.Text;
            int iAllChr = 0; //字符总数：不计字符'\n'和'\r'
            foreach (char ch in content)
            {
                if (ch != '\n' && ch != '\r')
                {
                    //除掉中文标点符号
                    if ("～！＠＃￥％…＆（）—＋－＝".IndexOf(ch) != -1 ||"｛｝【】：“”；‘'《》，。、？｜＼".IndexOf(ch) != -1) 
                        continue;
                    //除掉英文标点符号
                    if ("`~!@#$%^&*()_+-={}[]:\";'<>,.?/\\|".IndexOf(ch) != -1)
                    {
                        continue;
                    }
                    iAllChr++;
                }
                
            }

            this.label2.Text = string.Format("字数统计:{0}", iAllChr);
        }
    }
}
