using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoYuI.Interfaces;
using XiaoYuI.Utility;
namespace WebTools.Controllers
{
    public class JsonOptController : Controller
    {
        // GET: JsonOpt
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Format()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnFormat(string data)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                result.data= JsonHelper.ConvertJsonString(data);
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnJsonMin(string data)
        {
            string strJson = System.Text.RegularExpressions.Regex.Replace(data, "(?<=[\\{\\}\\[\\]\\:\\,])[\\s\\n\\r]*|[\\s\\n\\r]*(?=[\\{\\}\\[\\]\\:\\,])", "");
            return Json(strJson);
        }
    }
}