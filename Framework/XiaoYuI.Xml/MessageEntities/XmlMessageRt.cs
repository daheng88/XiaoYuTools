
using System.Xml.Serialization;
namespace XiaoYuI.Xml.MessageEntities
{
    /// <summary>
    /// 返回消息类
    /// </summary>
    public class XmlMessageRt
    {
        /// <summary>
        ///执行结果
        /// </summary>
        public XmlReturningStatus Status { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
    }
}
