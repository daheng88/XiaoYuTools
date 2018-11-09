using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace XiaoYuI.Utility
{
    public class HttpHelper
    {
        /// <summary>
        /// 作者：Ark
        /// 时间：2014.08.11
        /// 描述：Http request(请求)获取返回值
        ///       编码：UTF8
        ///       请求方式：post
        ///       contentType：json
        /// </summary>
        /// <param name="url">请求的网址</param>
        /// <param name="requestData">post数据</param>
        /// <returns></returns>
        public static string OpenRead(string url, string requestData = "", string method = "post", string contentType = "application/json")
        {
            Encoding encoding = Encoding.UTF8;
            string result = OpenRead(url, requestData, encoding, method, contentType);
            return result;
        }


        /// <summary>
        /// 作者：Ark
        /// 时间：2014.08.11
        /// 描述：Http request(请求)获取返回值
        /// </summary>
        /// <param name="url">请求的网址</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="encoding">编码</param>
        /// <param name="method">请求方式</param>
        /// <param name="contentType">http标头</param>
        /// <param name="timeoutSec">请求超时时间(秒)</param>
        /// <returns></returns>
        public static string OpenRead(string url, string requestData, Encoding encoding, string method = "post", string contentType = "application/json", int timeoutSec = 0)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.ContentType = contentType;

            if (!string.IsNullOrWhiteSpace(requestData))
            {
                byte[] buffer = encoding.GetBytes(requestData);
                request.ContentLength = buffer.Length;
                if (timeoutSec > 0)
                {
                    request.Timeout = timeoutSec * 1000;
                }

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static string Html2Text(string htmlStr)
        {

            if (String.IsNullOrEmpty(htmlStr))
            {
                return "";
            }

            string regEx_style = "<style[^>]*?>[\\s\\S]*?<\\/style>"; //定义style的正则表达式 
            string regEx_script = "<script[^>]*?>[\\s\\S]*?<\\/script>"; //定义script的正则表达式   
            string regEx_html = "<[^>]+>"; //定义HTML标签的正则表达式   
            htmlStr = Regex.Replace(htmlStr, regEx_style, "");//删除css
            htmlStr = Regex.Replace(htmlStr, regEx_script, "");//删除js
            htmlStr = Regex.Replace(htmlStr, regEx_html, "");//删除html标记
            htmlStr = Regex.Replace(htmlStr, "\\s*|\t|\r|\n", "");//去除tab、空格、空行
            htmlStr = htmlStr.Replace(" ", "");

            // htmlStr = htmlStr.Replace(""", "");//去除异常的引号" " "

            //htmlStr = htmlStr.Replace(""", "");

            return htmlStr.Trim();

        }
    }
}
