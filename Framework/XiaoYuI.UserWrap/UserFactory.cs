using System;
using System.Collections.Generic;
using System.Text;

namespace XiaoYuI.UserWrap
{
  public  class UserFactory
    {
      private static  IUserController currentUserController = null;
      public static void SetUserController(IUserController controller)
      {
          currentUserController = controller;
      }

      public static IUserController GetUserController()
      {
          if (currentUserController == null)
              throw new ArgumentNullException("currentUserController");
          return currentUserController;
      }
    }
}
