using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
namespace XiaoYuI.Xml
{
    /// <summary>
    /// 网站基本设置管理类
    /// </summary>
    public class ConfigFileLoadManager 
    {
        public static string Dir { get; set; }
        public static T LoadGeneralCfg<T>() where T:class
        {
            string path = typeof(T).Name + ".config";
            if (!string.IsNullOrEmpty(Dir))
            {
                path = Path.Combine(Dir, path);
            }
            return LoadGeneralCfg<T>(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static T LoadGeneralCfg<T>(string path) where T : class
        {
            string rootPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (HttpContext.Current != null)
            {
                rootPath = HttpContext.Current.Server.MapPath("/");
            }
            DefaultConfigFileManager<T> defaultFile = new DefaultConfigFileManager<T>(Path.Combine(rootPath, path));
            return (T)defaultFile.LoadConfig();
        }


        public static bool Save<T>(T cfg) where T : class
        {
            string path = typeof(T).Name + ".config";
            if (!string.IsNullOrEmpty(Dir))
            {
                path = Path.Combine(Dir, path);
            }
            return Save(cfg, path);
        }

        public static bool Save<T>(T cfg, string path) where T : class
        {
            string rootPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (HttpContext.Current!=null)
            {
                rootPath = HttpContext.Current.Server.MapPath("/");
            }


            DefaultConfigFileManager<T> defaultFile = new DefaultConfigFileManager<T>(Path.Combine(rootPath, path));
            defaultFile.Config = cfg;
            return defaultFile.SaveConfig();
        }
    }
}
