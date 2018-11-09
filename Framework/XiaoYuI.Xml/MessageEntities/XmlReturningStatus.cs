
using System.Xml.Serialization;
namespace XiaoYuI.Xml.MessageEntities
{
    /// <summary>
    /// 返回执行结果状态
    /// </summary>
    public enum XmlReturningStatus
    {
        /// <summary>
        /// 成功 (0)
        /// </summary>
        Success = 0,

        /// <summary>
        /// 异常 (-1)
        /// </summary>
        Error = -1,
    }
}
