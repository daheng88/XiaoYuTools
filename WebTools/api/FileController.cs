
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using XiaoYuI.Interfaces;
using XiaoYuI.FileUtils;
namespace DMP.Manager.api
{

    public class FileController : ApiController
    {

        [HttpPost]
        public async Task<ReturnResult> PostFormData()
        {
            ReturnResult result = new ReturnResult();
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                // throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                result.code = -1;
                result.msg = "提交的MIME类型不是multipart/form-data";
                return result;
            }
            string filePath = string.Empty;
            try
            {
                string root = HttpContext.Current.Server.MapPath("~/Upload");
                filePath = Path.Combine(root, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'));
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

            }
            catch (Exception ex)
            {
               // LogHelper.LogError("FileController->PostFormData:"+ex.ToString());
               // result.code = (int)ReturnResultEnum.permission_error;
                result.msg = "操作服务器文件目录权限不够!";
                return result;
            }
          
            var provider = new CustomMultipartFormDataStreamProvider(filePath);
            try
            {

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // var  provider.FormData.GetValues(key)
               

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    result.data = file.LocalFileName;

                }

                // return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (IOException ioEx)
            {
                result.code = -2;
                result.msg = "上传文件失败:" + ioEx.Message;
                return result;

            }
            catch (System.Exception e)
            {
               // LogHelper.LogError("FileController->PostFormData"+e.ToString());
                result.code = -4;
                result.msg = "上传文件失败:"+e.Message;
                // return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return result;
        }
    }

    /// <summary>
    /// 作者：Roger
    /// 日期：2017-03-08
    /// 描述：自定义文件保存名称
    /// </summary>
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return FileHelper.GenerateFileName() +"_"+ headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
       
    }



   

}
