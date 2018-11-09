
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using XiaoYu.PlugInBase;

using System.Threading;
using System.Threading.Tasks;
namespace XiaoYuTools
{
    public partial class Form1 : Form
    {
        const string PLUGIN_TITLE = "实用小工具";

        Dictionary<string, List<UserControlBase>> dicLoadedUCs = new Dictionary<string, List<UserControlBase>>();
        List<UserControlBase> lPlugIn = new List<UserControlBase>();
        Panel pnlUC
        {
            get
            {
                return splitContainer1.Panel2;
            }
        }

        public Form1()
        {
            InitializeComponent();

            this.Text = PLUGIN_TITLE;

            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Dock = DockStyle.Fill;

            tvPlugins.Dock = DockStyle.Fill;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoadPlugIns();
        }

        private void TSMI_ReloadPlugin_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (MessageBox.Show("确定重新加载吗？", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                pnlUC.Controls.Clear();
            }

           
        }

        private void tvPlugins_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.Text = PLUGIN_TITLE;

            if (tvPlugins.SelectedNode == null
                && tvPlugins.Nodes.Count > 0)
            {
                tvPlugins.SelectedNode = tvPlugins.Nodes[0];
            }
            if (tvPlugins.SelectedNode == null) return;

            if (tvPlugins.SelectedNode.Parent == null
                && tvPlugins.SelectedNode.Nodes.Count > 0)
            {
                tvPlugins.SelectedNode = tvPlugins.SelectedNode.Nodes[0];
            }
            if(tvPlugins.SelectedNode.Tag!=null)
            {
                var uc = (UserControlBase)tvPlugins.SelectedNode.Tag;
                pnlUC.Controls.Clear();
                pnlUC.Controls.Add(uc);
                if (uc.UcIcon != null)
                { 
                
                }
            }
        }


        /// <summary>
        /// 加载插件
        /// </summary>
        void LoadPlugIns()
        {
            tvPlugins.Nodes.Clear();
            lPlugIn.Clear();
            dicLoadedUCs.Clear();
            var plugIns = LoadPlugInManager.GetPlugIns();
            if (plugIns != null && plugIns.Any())
            {
                plugIns.ForEach(m =>
                {

                    TreeNode _tn_ = null;
                    foreach (TreeNode n in tvPlugins.Nodes)
                    {
                        if (n.Text == m.UCTpye)
                        {
                            _tn_ = n;
                            break;
                        }
                    }
                    if (_tn_ == null)
                    {
                        _tn_ = new TreeNode(m.UCTpye);
                        tvPlugins.Nodes.Add(_tn_);
                    }
                    TreeNode _n_ = new TreeNode(m.UCName);
                    _n_.ToolTipText = m.Recommend;
                    _n_.Tag = m;
                    _tn_.Nodes.Add(_n_);



                });
                tvPlugins.ExpandAll();
            }


        }



    }


  

}
