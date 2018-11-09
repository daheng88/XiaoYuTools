using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XiaoYu.PlugInBase.Properties;

namespace XiaoYu.PlugInBase
{
    public delegate void DelgLog(string info);
    public delegate void DelgUpdateStatusBarInfo(string info);
    public delegate bool DelgShareResources(Dictionary<string, object> dicObj);
    public delegate Dictionary<string, object> DelgGetResources(string sourceName);

    /// <summary>
    /// UC基类
    /// （*UC不要在设计时放置WebBrowser控件，不然会出错无法加载；采用代码生成添加在UC上）
    /// </summary>
    public partial class UserControlBase : UserControl
    {
        /// <summary>
        /// 日记记录
        /// </summary>
        public DelgLog Log;
        
        /// <summary>
        /// 更新状态栏信息
        /// </summary>
        public DelgUpdateStatusBarInfo UpdateStatusBarInfo;

        public DelgShareResources ShareResources;
        public DelgGetResources GetResources;

        public String UCName
        {
            get
            {
                return ucName;
            }
        }
        protected string ucName = "UCName";

        public String UCTpye
        {
            get
            {
                return ucType;
            }
        }
        protected string ucType = "我的应用";


        public String UCVersion
        {
            get
            {
                return ucVersion;
            }
        }
        protected string ucVersion = "1.0";

        public bool IsPrivateUc
        {
            get
            {
                return isPrivateUc;
            }
            protected set
            {
                isPrivateUc = value;
                if (isPrivateUc) authorized = false;
            }
        }
        private bool isPrivateUc = false;

        /// <summary>
        /// 是否已认证
        /// </summary>
        public bool Authorized
        {
            get
            {
                return authorized;
            }
            set
            {
                authorized = value;
            }
        }
        protected bool authorized = true;

        public string Recommend
        {
            get
            {
                return "【" + UCName + "】[" + UCVersion + "]\r\n" + recommend;
            }
        }
        protected string recommend = "";

        public Icon UcIcon
        {
            get
            {
                return ucIcon;
            }
        }
        protected Icon ucIcon = null;
            //System.Drawing.Icon.FromHandle(Resource.Control.GetHicon());

        public Dictionary<string,string> DicTags
        {
            get
            {
                return dicTags;
            }
        }
        private Dictionary<string, string> dicTags = new Dictionary<string, string>();

        /// <summary>
        /// 程序所在目录，以“\”结尾
        /// </summary>
        public string ProgramPath
        {
            get
            {
                return Environment.CurrentDirectory + "\\";
            }
        }

        public UserControlBase()
        {
            InitializeComponent();
        }

        public UserControlBase(string ucName, Icon icon = null)
        {
            InitializeComponent();
            this.ucName = ucName;
            if (icon != null) ucIcon = icon;
        }

        private void UserControlBase_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }
        
    }
}
