using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
namespace XiaoYuI.FileUtils
{
   public  class UploadHandle
    {

        /// <summary>
        /// 文件保存根目录
        /// </summary>
        public static string FilesRootDir
        {

            get
            {
                string rootDir = ConfigurationManager.AppSettings["FilesRootDir"] ?? "/";
                if (rootDir == "" || rootDir == "/")
                    rootDir = AppDomain.CurrentDomain.BaseDirectory;
                else if (rootDir.StartsWith("file://") || rootDir.StartsWith("\\"))
                    rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootDir);

                return rootDir;
            }
        }

        /// <summary>
        /// 获取一个新的文件名(时间+升级数字)
        /// </summary>
        /// <param name="extension">扩展名(如：jpg)</param>
        /// <returns>新文件名称</returns>
        public static string GetNewFileName(string extension)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            return string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), random.Next(0, 99).ToString("00"), extension.Replace(".", string.Empty).Trim());
        }

        /// <summary>
        /// 上传文件 字节数据至文件中。
        /// </summary>
        /// <param name="mStream">文件字节数组</param>
        /// <param name="sRelativePath">上传文件相对路径</param>
        /// <param name="sUpFileName">保存文件名</param>
        /// <returns>true：上传成功、false：上传失败</returns>
        public static bool UpLoadFile(byte[] arrByte, string savePath)
        {
            string path = savePath.Replace("\\", "/");
            string dir = path.Substring(0, path.LastIndexOf("/"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return WriteBytesToDiskFile(arrByte, path);
        }

        /// <summary>
        /// 将字节数组数据写入到指定的文件中。会直接覆盖已存在的文件。
        /// </summary>
        /// <param name="arrByte">文件字节数组</param>
        /// <param name="sFullFileName">上传文件完整路径</param>
        /// <returns>true：文件保存成功、false：文件保持失败</returns>
        private static bool WriteBytesToDiskFile(byte[] arrByte, string sFullFileName)
        {
            bool b = true;
            FileStream fsFile = new FileStream(sFullFileName, FileMode.Create, FileAccess.ReadWrite);
            try
            {
                fsFile.Write(arrByte, 0, arrByte.Length);
            }
            catch
            {
                b = false;
            }
            finally
            {
                fsFile.Flush();
                fsFile.Close();
            }
            return b;
        }
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
