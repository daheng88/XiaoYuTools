using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace XiaoYuI.Xml
{
    /// <summary>
    /// 实体转换器
    /// </summary>
   public class XmlConverter
    {
       
       /// <summary>
       /// 把实体对象转换为Xml字符串,供同平台客户端使用
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="entity"></param>
       /// <returns></returns>
        public static string ObjToXml<T>(T entity)
        {
            return ObjToXml<T>(entity, false);
        }
       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="entity"></param>
       /// <returns></returns>
        public static string ObjToXmlNotNs<T>(T entity)
        {
            return ObjToXml<T>(entity,true);
        }
       /// <summary>
       /// 转换为xml字符串
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="entity"></param>
       /// <param name="NotNs">不包含命名空间</param>
       /// <returns></returns>
        private static string ObjToXml<T>(T entity,bool NotNs)
        {
           string result = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                XmlWriterSettings setting = new XmlWriterSettings();
                setting.Encoding = new UTF8Encoding(false);
                setting.Indent = true;
                XmlWriter writer = XmlWriter.Create(stream, setting);
                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                if (NotNs)
                {
                    ns.Add("", "");//不输出xmlns
                }
                serializer.Serialize(writer, entity, ns);
                stream.Position = 0;
                byte[] buf = new byte[stream.Length];
                stream.Read(buf, 0, buf.Length);
                //result = Convert.ToBase64String(buf);
                result = System.Text.Encoding.UTF8.GetString(buf);



            }
            return result;
        }

       /// <summary>
       /// 把xml转换为实体
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="Xml"></param>
       /// <returns></returns>
        public static T XmlToObj<T>(string Xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(Xml);
            return (T)xs.Deserialize(sr);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static T Load<T>(string filename) where T : class
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static bool Save<T>(T obj, string filename) where T : class
        {
            bool success = false;

            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return success;

        }

    }
}
