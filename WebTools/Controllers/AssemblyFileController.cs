using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using XiaoYuI.Interfaces;

namespace WebTools.Controllers
{
    public class AssemblyFileController : Controller
    {
        // GET: Default1
        public ActionResult PublicKey()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnPublicKey(string data)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string path = Server.MapPath("~/Upload");
                byte[] token = Assembly.LoadFile(data)
                    .GetName()
                    .GetPublicKeyToken();
                string content = BitConverter.ToString(token).Replace("-", "").ToLower();
                result.data = content;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = ex.Message;
            }
            return Json(result);
        }
    }
}