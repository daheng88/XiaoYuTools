using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
namespace XiaoYuI.FileUtils
{
    public class FileDownload
    {
        /// <summary>
        /// 微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite
        ///     下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。
        ///     将后台文件写入 HTTP 响应输出流（不在内存中进行缓冲）
        /// </summary>
        public void TransmitFile(string filename,string serverFilePath)
        {
            string attachmentFile = string.Format("attachment;filename={0}", filename);
            HttpContext.Current.Response.ContentType = "application/x-zip-compressed";
            HttpContext.Current.Response.AddHeader("Content-Disposition", attachmentFile);
            HttpContext.Current.Response.TransmitFile(serverFilePath);
        }
        /// <summary>
        /// 写文件方式下载
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="serverFilePath"></param>
        public void WriteFileDownload(string filename, string serverFilePath)
        {
            //添加这个编码可以防止在IE下文件名乱码的问题
           // fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);

            string filePath = serverFilePath;
            HttpResponse Response = HttpContext.Current.Response;

            FileInfo fileInfo = new FileInfo(filePath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();

            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary"); //Response.WriteFile(fileInfo.FullName);

            Response.ContentType = "application/octet-stream";
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            //Response.BinaryWrite();
            Response.Flush();
            Response.End();
        }
        
        /// <summary>
        /// WriteFile分块下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="serverFilePath"></param>
        public void FileStreamDownload(string fileName, string serverFilePath)
        {
            string filePath = serverFilePath;
            HttpResponse Response = HttpContext.Current.Response;
            
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            if (fileInfo.Exists)
            {

                const long ChunkSize = 102400;//100K 每次读取文件，只读取100K，这样可以缓解服务器的压力
                byte[] buffer = new byte[ChunkSize];

                Response.Clear();
                System.IO.FileStream iStream = System.IO.File.OpenRead(filePath);
                long dataLengthToRead = iStream.Length;//获取下载的文件总大小
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName));
                while (dataLengthToRead > 0 && Response.IsClientConnected)
                {

                    int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    Response.OutputStream.Write(buffer, 0, lengthRead);
                    Response.Flush();
                    dataLengthToRead = dataLengthToRead - lengthRead;

                }

                Response.Close();
            }
      }
        

        /// <summary>
        /// 流方式下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="serverFilePath"></param>
        public void BinaryWriteDownload(string fileName, string serverFilePath)
        {

            string filePath = serverFilePath;
            HttpResponse Response = HttpContext.Current.Response;
            //以字符流的形式下载文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        /// <summary>
        /// MVC3实现
        /// </summary>
        /// <returns></returns>
        //public ActionResult LoadFile2()
        //{
        //    string filePath = Server.MapPath("~/Download/xx.jpg");
        //    return File(filePath, "application/x-jpg", "demo.jpg");
        //}
    }
}
