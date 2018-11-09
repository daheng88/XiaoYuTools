using System;
using System.Collections.Generic;
using System.Text;


namespace XiaoYuI.UserWrap
{
    public interface IUserController
    {
        UserBaseData GetUser(string UserId);

        bool Login(string Email,string Password,out UserBaseData user);
    }
}
