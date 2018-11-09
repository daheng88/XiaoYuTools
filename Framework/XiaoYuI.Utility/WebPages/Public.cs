using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace XiaoYuI.Utility.WebPages
{
    public class Public
    {
         //分页
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag)
        {
            if (pagetag == "")
                pagetag = "page";
            if (url.Contains(pagetag))
            {
                string t = url.Substring(url.IndexOf(pagetag) - 1, 1);
                t = t + pagetag + "=" + HttpContext.Current.Request.QueryString[pagetag].ToString();
                url = url.Replace(t, "");

            }
            int startPage = 1;
            int endPage = 1;

            if (url.IndexOf("?") > 0)
            {
                url = url + "&";
            }
            else
            {
                url = url + "?";
            }

            string t1 = "<a href=\"" + url + "&" + pagetag + "=1";
            string t2 = "<a href=\"" + url + "&" + pagetag + "=" + countPage;

            t1 += "\">&laquo;</a>";
            t2 += "\">&raquo;</a>";

            if (countPage < 1)
                countPage = 1;
            if (extendPage < 3)
                extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    s.Append(pagetag);
                    s.Append("=");
                    s.Append(i);

                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }

        //发送邮件
        public static bool SendMail(string SmtpServer, string PassWord, string SendMail, string SendName, string ReceiverMail, string ReceiverName, string MailSubject, string MailBody, string Attachments)
        {
            System.Net.Mail.SmtpClient smtp;
            smtp = new System.Net.Mail.SmtpClient(SmtpServer);
            smtp.Timeout = 60000;
            smtp.UseDefaultCredentials = true;
            //设置发件人用户密码 
            smtp.Credentials = new System.Net.NetworkCredential(SendMail.Split('@')[0], PassWord);
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            //设置发件人地址姓名 
            message.From = new System.Net.Mail.MailAddress(SendMail, SendName, System.Text.Encoding.UTF8);
            //设置收件人地址姓名 
            message.To.Add(new System.Net.Mail.MailAddress(ReceiverMail, ReceiverName, System.Text.Encoding.UTF8));
            message.IsBodyHtml = true;
            message.Subject = MailSubject;
            message.Body = MailBody;
            if (Attachments != "" && Attachments != null)
                message.Attachments.Add(new System.Net.Mail.Attachment(Attachments));
            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                return false;
            }
        } 

        //来自那个搜索引擎
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }
            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// 获得当前页面客户端的IP
        public static string GetIP()
        {


            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(result) || !Validator.IsIP(result))
            {
                return "127.0.0.1";
            }

            return result;

        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return "";
        }
        /// 过滤非法sql字符串
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput.Trim() == string.Empty)
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }
        /// 生成随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        public static int getRandom(int minValue, int maxValue)
        {
            Random ri = new Random(unchecked((int)DateTime.Now.Ticks));
            int k = ri.Next(minValue, maxValue);
            return k;
        }
        /// <summary>
        /// 生成GUID
        /// </summary>
        /// <returns></returns>
        public static string CreateGUID()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + getRandom(100, 999).ToString();
        }

        public static string sub(string str, int n)
        {
            if (str.Length > n) str = str.Substring(0, n);
            return str;
        }

        public static string sub3Dot(string str, int n)
        {
            if (str.Length > n) str = str.Substring(0, n)+"...";
            return str;
        }

        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
