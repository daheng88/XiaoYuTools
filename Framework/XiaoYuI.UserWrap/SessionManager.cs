using System;
using System.Collections.Generic;
using System.Web;

namespace XiaoYuI.UserWrap
{
    public class SessionManager
    {
        public SessionManager()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //        
        }

        #region UserID的管理
        private const string SESSION_USERID_KEY = "{D7258F02-4AB2-49AE-A1E8-B947A603F7DE}";
        private const string SESSION_USER_KEY = "{88F19CFA-379B-4991-B5BB-FC39C3AB1009}";
        private const string SESSION_COMPANY_KEY = "{9641BC58-1F73-44CA-BB13-1476DA1B08FB}";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUserID()
        {
            HttpContext context = HttpContext.Current;
            string myUserID = context.Session[SESSION_USERID_KEY] as string;
            if (myUserID == null)
            {
                if (context.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(context.User.Identity.Name))
                {
                    myUserID = context.User.Identity.Name;
                    SetUserID(myUserID);
                }
                else
                    return string.Empty;
            }

            if (!myUserID.Equals(context.User.Identity.Name))
            {
                context.Session.Clear();
                SessionManager.SetUserID(myUserID);
            }
            return HttpContext.Current.Session[SESSION_USERID_KEY].ToString();
        }

        ///// <summary>
        ///// Created by：tanghua，2012-10-22，获取当前用户Url集合
        ///// </summary>
        //public static string GetUrlStrings()
        //{
        //    string urlStrings = string.Empty;
        //    if (HttpContext.Current.Session[PublicConsts.SessionKeyOfCurrerntUserUrlStrings] != null)
        //    {
        //        urlStrings = HttpContext.Current.Session[PublicConsts.SessionKeyOfCurrerntUserUrlStrings].ToString();
        //    }
        //    return urlStrings;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        public static void SetUserID(string UserID)
        {
            HttpContext.Current.Session[SESSION_USERID_KEY] = UserID;
        }

        #region >> 取用户详细信息
        public static UserBaseData CurrentUser
        {
            get
            {
                UserBaseData domain = HttpContext.Current.Session[SESSION_USER_KEY] as UserBaseData;
                if (domain == null)
                {
                    string userid = GetUserID();
                    domain=UserFactory.GetUserController().GetUser(userid);
                }
                return domain;
            }
        }
        #endregion

        //#region >> 取用户所在公司详细信息
        //public static CompanyModel TheCompany
        //{
        //    get
        //    {
        //        CompanyModel domain = HttpContext.Current.Session[SESSION_COMPANY_KEY] as CompanyModel;
        //        if (domain == null)
        //        {
        //            string userid = GetUserID(HttpContext.Current);
        //            domain = new CompanyModel();
        //            StringBuilder sqlCompany = new StringBuilder("select b.* from T_USER a, COMPANY b where a.company_seq=b.company_seq and a.USERID=:{USERID}");
        //            List<object> args = new List<object>();
        //            args.Add(userid.ToString());
        //            DataSet dsCompany = SqlHelper.ExecuteDataSet(sqlCompany.ToString(), args);
        //            if ((dsCompany != null) && (dsCompany.Tables[0].Rows.Count > 0))
        //            {
        //                domain = CompanyModel.DataRowToModel(dsCompany.Tables[0].Rows[0]);
        //                HttpContext.Current.Session[SESSION_COMPANY_KEY] = domain;
        //            }
        //        }
        //        return domain;
        //    }
        //}
        //#endregion

        /// <summary>
        /// 
        /// </summary>
        public static void ClearSession()
        { 
           HttpContext.Current.Session.Clear();
        }

       
        /// <summary>
        /// 
        /// </summary>
        public static void LogOut()
        {
            SessionManager.ClearSession();
            System.Web.Security.FormsAuthentication.SignOut();
        }

        #endregion
    }
}