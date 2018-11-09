using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace XiaoYuTools
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //处理未捕获的异常
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                //处理UI线程异常
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                #region 程序入口点
                // 只允许运行一个实例的解决方案
                Process[] processes = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
                if (processes.Length > 1)
                {
                    MessageBox.Show("软件已经在运行中...");
                    System.Threading.Thread.Sleep(1000);
                    System.Environment.Exit(1);
                }
                else
                {
                    Application.Run(new Form1());
                }
                #endregion
            }
            catch (Exception ex)
            {
                InformException(ex);
            }
        }


        #region 捕捉全局异常代码
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            InformException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            InformException(e.ExceptionObject as Exception);
        }

        static void InformException(Exception ex)
        {
            // 用以弹出框显示
            string shortStr = "";
            // 日志记录详细异常信息
            string detailStr = "";

            string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";

            if (ex != null)
            {
                shortStr = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}",
                                    ex.GetType().Name, ex.Message);

                detailStr = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                                    ex.GetType().Name, ex.Message, ex.StackTrace);
            }
            else
            {
                shortStr = string.Format("应用程序线程错误:{0}", ex.Message);
                detailStr = string.Format("应用程序线程错误:{0}", ex);
            }

            MessageBox.Show(shortStr, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
          
        }
        #endregion

      
    }
}
