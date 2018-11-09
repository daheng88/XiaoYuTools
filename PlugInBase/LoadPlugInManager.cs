
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using XiaoYu.PlugInBase;

namespace XiaoYu.PlugInBase
{
    public class LoadPlugInManager
    {
        public static readonly string PlugInsDir = GetPathWithSlash(System.Environment.CurrentDirectory, true) + "PlugIns\\";

        static LoadPlugInManager()
        {
            if (!Directory.Exists(PlugInsDir))
            {
                Directory.CreateDirectory(PlugInsDir);
            }
        }

        public static string GetPathWithSlash(string dirPath, bool createIfNotExists = false)
        {
            if (!Directory.Exists(dirPath) & createIfNotExists)
            {
                Directory.CreateDirectory(dirPath);
            }
            if (dirPath.Trim().EndsWith(@"\"))
            {
                return dirPath.Trim();
            }
            return (dirPath.Trim() + @"\");
        }


        /// <summary>
        /// 加载PlugIns插件目录下的dll
        /// </summary>
        public static List<UserControlBase> GetPlugIns()
        {
            List<UserControlBase> lUc = new List<UserControlBase>();

            foreach (var dllFile in Directory.GetFiles(PlugInsDir))
            {
                FileInfo fi = new FileInfo(dllFile);
                if (!fi.Name.EndsWith(".dll")) continue;

                var _uc = CreateInstance(fi.FullName, new string[] { "PlugIns" });
                if (_uc != null)
                {
                    lUc.AddRange(_uc);
                }
            }

            return lUc;
        }

        /// <summary>
        /// 反射创建实例
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="typeFeature"></param>
        /// <param name="hostType"></param>
        /// <param name="dynamicLoad"></param>
        /// <returns></returns>
        public static List<UserControlBase> CreateInstance(string sFilePath, string[] typeFeature,  bool dynamicLoad = true)
        {
            var lUc = new List<UserControlBase>();
            Assembly assemblyObj = null;

            if (!dynamicLoad)
            {
                #region 方法一：直接从DLL路径加载
                assemblyObj = Assembly.LoadFrom(sFilePath);
                #endregion
            }
            else
            {
                #region 方法二：先把DLL加载到内存，再从内存中加载（可在程序运行时动态更新dll文件，比借助AppDomain方便多了！）
                using (FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bFile = br.ReadBytes((int)fs.Length);
                        br.Close();
                        fs.Close();
                        assemblyObj = Assembly.Load(bFile);
                    }
                }
                #endregion
            }

            if (assemblyObj != null)
            {
                #region 读取dll内的所有类，生成实例（这样可省去提供 命名空间 的步骤）
                // 程序集（命名空间）中的各种类
                foreach (Type type in assemblyObj.GetTypes())
                {
                    try
                    {
                        if (type.ToString().Contains("<>")) continue;
                        if (typeFeature != null)
                        {
                            bool invalidInstance = true;
                            foreach (var tf in typeFeature)
                            {
                                if (type.ToString().Contains(tf))
                                {
                                    invalidInstance = false;
                                    break;
                                }
                            }
                            if (invalidInstance) continue;
                        }

                        var uc = (UserControlBase)assemblyObj.CreateInstance(type.ToString()); //反射创建 
                        lUc.Add(uc);

                       
                    }
                    catch (InvalidCastException icex)
                    {
                        Console.WriteLine(icex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Create " + sFilePath + "(" + type.ToString() + ") occur " + ex.GetType().Name + ":\r\n" + ex.Message + (ex.InnerException != null ? "(" + ex.InnerException.Message + ")" : ""));
                    }
                }
                #endregion
            }

            return lUc;
        }
    }
}
