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
    public class ImagesExtend
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
                    throw new Exception("需要图片网站基地址");
                str =BaseImageUrl+"/"+str;
            }
            return str;
        }
        /**/
        /// <summary>
        /// 根椐Html内空自动识别图像文件,并下载到服务器指定目录
        /// </summary>
        /// <param name="strHTML"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public int SaveUrlPics(ref string strHTML, string path)
        {
            string[] imgurlAry = GetImgTag(strHTML);
            string basePath = string.Empty;
            if (HttpContext.Current != null)
            {
               basePath= HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                basePath=Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase,path);
            }
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            try
            {
                string imageUrl = string.Empty;
                for (int i = 0; i < imgurlAry.Length; i++)
                {
                    imageUrl=DateTime.Now.ToString("yyyyMMddHHMM")+"_"+imgurlAry[i].Substring(imgurlAry[i].LastIndexOf("/") + 1);
                    WebClient wc = new WebClient();
                    wc.DownloadFile(imgurlAry[i], Path.Combine(basePath,imageUrl));
                    strHTML = strHTML.Replace(imgurlAry[i], Path.Combine(path, imageUrl));
                }

            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
            return imgurlAry.Length;
        }

    }
}