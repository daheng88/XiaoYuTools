using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
namespace XiaoYuI.Xml.MessageEntities
{
    /// <summary>
    /// C返回消息类
    /// </summary>
    public class XmlMessageObjectRt<T> : XmlMessageRt where T:class
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        public T Data { get; set; }
    }
}
