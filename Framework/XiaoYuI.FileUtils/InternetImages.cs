using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Web;

namespace XiaoYuI.FileUtils
{
    /// <summary>
    /// 针对互联网图片处理操作
    /// </summary>
    public class InternetImagesUtils
    {
        public string BaseImageUrl {
            get { return baseImageUrl; }
            set {
                string url = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Regex regObj = new Regex("http://([\\w-]+\\.)+[\\w-]+([\\w-\\.]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    url = regObj.Match(url).Value;
                }
                baseImageUrl = url;
            }
        }

        private string baseImageUrl = string.Empty;

        private string[] GetImgTag(string htmlStr)
        {
            Regex regObj = new Regex("<img.+?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string[] strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            foreach (Match matchItem in regObj.Matches(htmlStr))
            {
                strAry[i] = GetImgUrl(matchItem.Value);
                i++;
            }
            return strAry;
        }

        private string GetImgUrl(string imgTagStr)
        {
            string str = "";
            Regex reglink = new Regex("src=['\"](.*?)['\"]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
           // Regex regObj = new Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            str = reglink.Match(imgTagStr).Groups[1].Value;
            if (!str.Contains("http"))
            {
                if (string.IsNullOrEmpty(baseImageUrl))
                {
                    throw new Exception("需要图片网站基地址");
                }
                str =BaseImageUrl+"/"+str;
            }
            return str;
        }
      
        /// <summary>
        /// 根椐Html内空自动识别图像文件,并下载到服务器指定目录
        /// </summary>
        /// <param name="strHTML"></param>
        /// <param name="relativePath">相对地址</param>
        /// <param name="absolutePath">绝对地址</param>
        /// <returns></returns>
        public int SaveUrlPics(ref string strHTML, string relativePath, string absolutePath=null)
        {
            
            //如果基地址为空，不要去下载图片
            if (string.IsNullOrEmpty(baseImageUrl))
                return 0;
            string[] imgurlAry = GetImgTag(strHTML);
            string savePath = string.Empty;
            if (HttpContext.Current != null)
            {
                savePath = HttpContext.Current.Server.MapPath(relativePath);
            }
            else
            {
                if (!string.IsNullOrEmpty(absolutePath))
                {
                    savePath = absolutePath;
                   
                }
                else
                    savePath = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, relativePath);
            }
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            
                string imageUrl = string.Empty;
                for (int i = 0; i < imgurlAry.Length; i++)
                {
                    imageUrl = DateTime.Now.ToString("yyMMddHHmmss") + "_" + GetImagesName(imgurlAry[i].Substring(imgurlAry[i].LastIndexOf("/") + 1));
                    WebClient wc = new WebClient();
                    try
                    {
                      wc.DownloadFile(imgurlAry[i], Path.Combine(savePath, imageUrl));
                    }
                    catch (Exception ex)
                    {
                     //return ex.Message;
                    }
                    strHTML = strHTML.Replace(imgurlAry[i], Path.Combine(relativePath, imageUrl));
                }
            
            return imgurlAry.Length;
         }

        /// <summary>
        /// 判断图片合法性
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        private string GetImagesName(string imageUrl)
        {
            string ret = imageUrl;
            Regex reg=new Regex(".*?\\.(bmp|gif|jpg|png)",RegexOptions.IgnoreCase);
            if (!reg.IsMatch(imageUrl))
            {
                ret += ".jpg";
            }
            if (ret.Length > 20)
            {
                ret = ret.Substring(ret.Length-20);
            }
            return ret;
        }

    }
}