using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
namespace XiaoYuI.Xml
{
    /// <summary>
    /// 文件配置管理基类
    /// </summary>
    public class DefaultConfigFileManager<T> where T:class
    {
        /// <summary>
        /// 文件所在路径变量
        /// </summary>
        private string m_configfilepath;

        /// <summary>
        /// 配置对象变量
        /// </summary>
        private T m_configinfo = null;

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object m_lockHelper = new object();


        /// <summary>
        /// 文件所在路径
        /// </summary>
        public string ConfigFilePath
        {
            get { return m_configfilepath; }
            set { m_configfilepath = value; }
        }


        /// <summary>
        /// 配置对象
        /// </summary>
        public T Config
        {
            get { return m_configinfo; }
            set { m_configinfo = value; }
        }

        public DefaultConfigFileManager() { }
        public DefaultConfigFileManager(string filePath)
        {
            this.m_configfilepath = filePath;
        }
        /// <summary>
        /// 加载(反序列化)指定对象类型的配置对象
        /// </summary>
        /// <param name="fileoldchange">文件加载时间</param>
        /// <param name="checkTime">是否检查并更新传递进来的"文件加载时间"变量</param>
        /// <returns></returns>
        public T LoadConfig(DateTime fileoldchange, bool checkTime = true)
        {
            if (checkTime)
            {
                DateTime m_filenewchange = System.IO.File.GetLastWriteTime(this.ConfigFilePath);

                //当程序运行中config文件发生变化时则对config重新赋值
                if (fileoldchange != m_filenewchange)
                {
                    lock (m_lockHelper)
                    {
                        m_configinfo = LoadConfig();
                    }
                }
            }
            else
            {
                lock (m_lockHelper)
                {
                    m_configinfo = LoadConfig();
                }

            }
            return m_configinfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T LoadConfig()
        {
            return XmlConverter.Load<T>(this.ConfigFilePath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveConfig()
        {
            return XmlConverter.Save(this.Config, this.ConfigFilePath);
        }
    }
}
