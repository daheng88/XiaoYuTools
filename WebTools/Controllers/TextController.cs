using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebTools.Controllers
{
    public class TextController : Controller
    {
        #region Md5
        // GET: Text
        public ActionResult Md5()
        {
            return View();
        }

    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnMd5(int mode,string data)
        {
            string result = string.Empty;
            if (mode == 16)
            {
                result=MD5Encrypt16(data);
            }
            else if (mode == 32)
            {
                result = MD5Encrypt32(data);
            }
            else
            {
                result = MD5Encrypt64(data);
            }
            return Json(result.ToLower());
        }

        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
        #endregion

        #region base64

        // GET: Text
        public ActionResult Base64()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnBase64(int mode, string data)
        {
            if (mode == 0)
            {
                return Json(EncodeBase64(data));
            }
            else
                return Json(DecodeBase64(data));
        }
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <returns></returns>
        public static string EncodeBase64(string source)
        {
            string result = string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
               
            }
            return result;
        }
        // <summary>
        /// Base64解密
        /// </summary>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string decode)
        {
            string result = "";
            byte[] bytes = Convert.FromBase64String(decode);
            try
            {
                result = Encoding.UTF8.GetString(bytes);
            }
            catch
            {

            }
            return result;
        }
        #endregion

        public ActionResult UrlCode()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnUrlCode(int mode, string data)
        {
            if (mode == 0)
            {

                return Json(HttpUtility.UrlEncode(data));
            }
            else
                return Json(HttpUtility.UrlDecode(data));
        }

        public ActionResult HtmlCode()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnHtmlCode(int mode, string data)
        {
            if (mode == 0)
            {
                var s = HttpUtility.UrlDecode(data);
                return Json(HttpUtility.HtmlEncode(s));
            }
            else
                return Json(HttpUtility.HtmlDecode(data));
        }
    }
}