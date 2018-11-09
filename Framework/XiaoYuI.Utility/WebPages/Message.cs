using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
namespace XiaoYuI.Utility.WebPages
{
    /// <summary>
    /// 消息类

    /// </summary>
    public class Message
    {
        /// <summary>
        /// 弹出提示消息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="strMessage"></param>
        public static void ShowMessage(Page page, string strMessage)
        {
            StringBuilder _strScript = new StringBuilder();
            _strScript.Append(@"<script type='text/javascript' defer>");
            _strScript.Append("alert('" + strMessage + "');");
            _strScript.Append(@"</script>");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "message", _strScript.ToString());
        }

        /// <summary>
        /// 输出脚本（页头）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="strScript"></param>
        public static void ResponseScript(Page page, string strScript)
        {
            StringBuilder _strScript = new StringBuilder();
            _strScript.Append(@"<script type='text/javascript' defer>");
            _strScript.Append(strScript);
            _strScript.Append(@"</script>");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "script", _strScript.ToString());
        }

        /// <summary>
        /// 向页尾注册脚本

        /// </summary>
        /// <param name="page"></param>
        /// <param name="strScript"></param>
        public static void ResponseScriptStart(Page page, string strScript)
        {
            StringBuilder _strScript = new StringBuilder();
            _strScript.Append(@"<script type='text/javascript' defer>");
            _strScript.Append(strScript);
            _strScript.Append(@"</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "script", _strScript.ToString());
        }

        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(System.Web.UI.Page page, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
        }

        /// <summary>
        /// 控件点击 消息确认提示框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl Control, string msg)
        {
            //Control.Attributes.Add("onClick","if (!window.confirm('"+msg+"')){return false;}");
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(System.Web.UI.Page page, string msg, string url)
        {
            //Response.Write("<script>alert('帐户审核通过！现在去为企业充值。');window.location=\"" + pageurl + "\"</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + msg + "');window.location=\"" + url + "\"</script>");


        }
        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirects(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("top.location.href='{0}'", url);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }
    }
}