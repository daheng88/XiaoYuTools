using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaoYuI.Interfaces
{
    /// <summary>
    /// API请求结果
    /// </summary>
    public class ReturnResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnResult()
        {
            this.code = 0;
            this.msg =string.Empty;
            this.data = "";
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_code">返回码</param>
        /// <param name="_msg">返回信息</param>
        public ReturnResult(int _code, string _msg)
        {
            this.code = _code;
            this.msg = _msg;
            this.data = "";
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_code">返回码</param>
        /// <param name="_msg">返回信息</param>
        /// <param name="_data">返回数据</param>
        public ReturnResult(int _code, string _msg, object _data)
        {
            this.code = _code;
            this.msg = _msg;
            this.data = _data;
        }
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }
    }

    /// <summary>
    /// API请求结果
    /// </summary>
    public class ReturnResult<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnResult()
        {
            this.code = 0;
            this.msg = string.Empty;
            this.data = default(T);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_code">返回码</param>
        /// <param name="_msg">返回信息</param>
        public ReturnResult(int _code, string _msg)
        {
            this.code = _code;
            this.msg = _msg;
            this.data = default(T);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_code">返回码</param>
        /// <param name="_msg">返回信息</param>
        /// <param name="_data">返回数据</param>
        public ReturnResult(int _code, string _msg, T _data)
        {
            this.code = _code;
            this.msg = _msg;
            this.data = _data;
        }
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T data { get; set; }
    }
}
