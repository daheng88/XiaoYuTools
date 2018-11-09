using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic.Devices;
namespace XiaoYuI.FileUtils
{

    /// <summary>
    /// 
    /// </summary>
    public sealed class FileHelper
    {



 
        /// <summary>
        /// 文件单位转换，字节转兆
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FileSizeConvert(int size)
        {
            if (size == 0)
                return "0";
            var newSize = Convert.ToDouble(size);
            var units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            var mod = 1024.00;
            var i = 0;
            while (newSize >= mod)
            {
                newSize /= mod;
                i++;
            }
            return Math.Round(newSize, 2) + units[i];
        }



        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool FileIsExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="oldFile"></param>
        /// <param name="oldFile"></param>
        public static void RenameFile(string oldFile,string newFile)
        {
              Computer MyComputer = new Computer();
              MyComputer.FileSystem.RenameFile(oldFile, newFile);
        }

        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="oldFolder"></param>
        /// <param name="newFolder"></param>
        public static void RenameFolder(string oldFolder, string newFolder)
        {
            if (string.IsNullOrWhiteSpace(oldFolder))
                throw new ArgumentNullException("oldFolder");
            if (string.IsNullOrWhiteSpace(newFolder))
                throw new ArgumentNullException("newFolder");
            try
            {
        
                Directory.Move(oldFolder, newFolder);
            }
            catch (Exception ex)
            { 
              
            }
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="TargetDir"></param>
        public static void MoveFolder(string TargetDir)
        {
            if (string.IsNullOrEmpty(TargetDir))
                throw new ArgumentNullException("TargetDir");
            if (!Directory.Exists(TargetDir))
                Directory.CreateDirectory(TargetDir);
            try
            {
                IDataObject iData = Clipboard.GetDataObject();
                if (iData.GetDataPresent(DataFormats.FileDrop))
                {
                    System.Array fileArray = (System.Array)iData.GetData(DataFormats.FileDrop);
                    for (int i = 0; i < fileArray.Length; i++)
                    {
                        string filename = fileArray.GetValue(i).ToString();
                        FileInfo file = new FileInfo(filename);
                        file.MoveTo(Path.Combine(TargetDir, file.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("移动文件出错");
            }
        }

        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        public static void Delete(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        /// <summary>
        /// 删除文件到回收站
        /// </summary>
        /// <param name="FilePath"></param>
        public static void DeleteFileToRecycle(string FilePath)
        {
            FileSystem.DeleteFile(FilePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }
        /// <summary>
        /// 删除文件夹到回收站
        /// </summary>
        /// <param name="DirPath"></param>
        public static void DeleteFileDirectoryToRecycle(string DirPath)
        {

            FileSystem.DeleteDirectory(DirPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 生成上传文件名称
        /// </summary>
        /// <returns></returns>
        public static string GenerateFileName()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long timestamp = Convert.ToInt64(ts.TotalSeconds);
            return timestamp + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
        }
    }
}
