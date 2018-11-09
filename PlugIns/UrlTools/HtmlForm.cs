using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XiaoYu.PlugInBase;

namespace PlugIns.UrlTools
{
    public partial class HtmlForm : UserControlBase
    {
        public HtmlForm()
        {
            InitializeComponent();
            this.ucName = "HTML处理类";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string regexstr = @"(&(#)?.+;)|(<[^>]*>)";
            this.textBox1.Text= Regex.Replace(this.textBox1.Text, regexstr, "", RegexOptions.IgnoreCase);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = TextToHTML(this.textBox1.Text);
        }

        /// <summary>
        /// 纯文本转HTML
        /// </summary>
        public static string TextToHTML(string value)
        {
            return TextToHTML(value, false);
        }
      
        /// <summary>
        /// 纯文本转HTML
        /// </summary>
        public static string TextToHTML(string value,  bool isOutBr)
        {
            StringBuilder sr = new StringBuilder();
            sr.Append(value);

            sr.Replace("&", "&amp;");
            sr.Replace(">", "&gt;");
            sr.Replace("<", "&lt;");
            sr.Replace(" ", "&nbsp;");
            sr.Replace("\"", "&quot;");
            sr.Replace("©", "&copy;");
            sr.Replace("®", "&reg;");
            sr.Replace("×", "&times;");
            sr.Replace("÷", "&divide;");
            if (isOutBr)
            {
                sr.Replace("\r\n", "<br>");
                sr.Replace("\r", "<br>");
                sr.Replace("\n", "<br>");
            }
            else
            {
                sr.Insert(0, "<p>");
                sr.Replace("\r\n", "\r");
                sr.Replace("\r", "</p>\r\n<p>");
                sr.Append("</p>");
            }

            return sr.ToString();
        }

        /// <summary>
        /// HTML输出为JS
        /// </summary>
        public static string HtmlToScript(string value)
        {
            StringBuilder sr = new StringBuilder();
            sr.Append(value);
            sr.Replace("\\", "\\\\");
            sr.Replace("/", "\\/");
            sr.Replace("'", "\\'");
            sr.Replace("\"", "\\\"");
            string[] strs = sr.ToString().Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            return String.Format("document.writeln(\"{0}\");",
                String.Join("\");\r\ndocument.writeln(\"", strs),
                StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// JS脚本输出字符串
        /// </summary>

        /// <returns></returns>
        public static string ScriptStringFormat(string value)
        {
            value = value.Replace("\\", "\\\\");
            value = value.Replace("'", "\\'");
            value = value.Replace("\"", "\\\"");
            return value;
        }


        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        public static string HtmlFilter(string value)
        {
            value = Regex.Replace(value, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"(select|textarea|input|link|iframe|frameset|frame|form|applet|embedlayer|ilayer|meta|object|script|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"javascript|eval", "", RegexOptions.IgnoreCase);
            return value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string content = this.textBox1.Text;
            StringBuilder sr = new StringBuilder();
            sr.Append(content);
            sr.Replace("&amp;", "&");
            sr.Replace("&gt;",">");
            sr.Replace( "&lt;","<");
            sr.Replace("&nbsp;"," ");
            sr.Replace("&quot;","\"");
            sr.Replace("&copy;","©");
            sr.Replace( "&reg;","®");
            sr.Replace("&times;","×");
            sr.Replace("&divide;","÷");
            this.textBox1.Text = sr.ToString();
        }
    }
}
