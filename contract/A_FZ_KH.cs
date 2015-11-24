using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace contract
{
    public partial class A_FZ_KH : Form
    {
        private ToolStripItem[] bts = null;
        private IsetText form;
        public A_FZ_KH()
        {
            InitializeComponent();
        }

        public A_FZ_KH(IsetText form)
        {
            InitializeComponent();
            this.form = form;
        }

        #region IChildForm 成员
        /// <summary>
        /// FORM 激活时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Button btn1;
        public void FormActivated(object sender, EventArgs e)
        {
            (this.MdiParent as MForm1).AddButtons(this.bts);
            btn1 = new Button();
            btn1.Location = new System.Drawing.Point(3, 3);
            btn1.Name = this.Name;
            btn1.Size = new System.Drawing.Size(150, 23);
            btn1.Text = this.Text;
            btn1.UseVisualStyleBackColor = true;
            btn1.Margin = new Padding(0, 0, 0, 0);
            btn1.Tag = this;
            btn1.Click += new EventHandler(btn1_Click);
            (this.MdiParent as MForm1).AddStatus(btn1);

        }
        void btn1_Click(object sender, EventArgs e)
        {
            this.Activate();
        }
        /// <summary>
        /// FORM 停用时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FormDeactivate(object sender, EventArgs e)
        {
            //MAINFROM工具栏按钮清空
            (this.MdiParent as MForm1).ClearButtons();

        }

        public void Form_Closing(object sender, EventArgs e)
        {
            (this.MdiParent as MForm1).DelStatus(btn1);
        }

        #endregion

        private void Reg()
        {

            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("选择客户", "选择客户",ClassCustom.getImage("t1.png"), this.btn_xz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("添加客户", "添加客户",ClassCustom.getImage("tjkh.png"), this.btn_add,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        #region 按钮事件

        private void btn_xz(object sender, EventArgs e)
        {
            try
            {
                this.treeView1_DoubleClick(null, null);
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void btn_add(object sender, EventArgs e)
        {
            try
            {
                A_CLIENT sp = new A_CLIENT();
                sp.MdiParent = this.MdiParent;
                sp.Show();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void btn_close(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void A_FZ_KH_Load(object sender, EventArgs e)
        {
            try
            {
                Reg();
                this.comboBox1.SelectedIndex = 0;
                this.textBox1.Focus();
                this.treeView1.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE LEN(CCODE)=" + ClassConstant.GetLeveLChar("LEVEL_KH", 0).Length + "  ORDER BY CCODE");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE (SUBSTRING(CCODE,3,2) LIKE '" + (ClassConstant.USER_ID == "0101999999" ? "__" : ClassConstant.DW_ID.Substring(2)) + "' OR CCODE LIKE '01%'  OR CCODE LIKE '05%' ) AND  CCODE LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 1) + "'  ORDER BY CCODE");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.treeView1.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Tag = dr1[0].ToString();
                        tn1.Name = dr1[0].ToString();
                        this.treeView1.Nodes[tn.Name].Nodes.Add(tn1);
                        DataTable dt3 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE CCODE LIKE'" + dr1[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 2) + "'  ORDER BY CNAME");
                        foreach (DataRow dr2 in dt3.Rows)
                        {
                            TreeNode tn2 = new TreeNode(dr2[1].ToString());
                            tn2.Tag = dr2[0].ToString();
                            tn2.Name = dr2[0].ToString();
                            this.treeView1.Nodes[tn.Name].Nodes[tn1.Name].Nodes.Add(tn2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (!(form is A_CX_HT))
                {
                    if (this.treeView1.SelectedNode.Nodes.Count != 0)
                        return;
                    if (this.treeView1.SelectedNode.Tag.ToString().Substring(0, 2) == "01")
                    {
                        if (this.treeView1.SelectedNode.Tag.ToString().Length != 4)
                            return;
                    }
                    else
                    {
                        if (this.treeView1.SelectedNode.Tag.ToString().Length != 8)
                            return;
                    }
                }

                form.SetTextKH(this.treeView1.SelectedNode.Tag.ToString(), this.treeView1.SelectedNode.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                    return;
                this.treeView1.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE LEN(CCODE)=" + ClassConstant.GetLeveLChar("LEVEL_KH", 0).Length + "  ORDER BY CCODE");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE (SUBSTRING(CCODE,3,2)='" + ClassConstant.DW_ID.Substring(2) + "' OR CCODE NOT LIKE '02%') AND  CCODE LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 1) + "'  ORDER BY CCODE");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.treeView1.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Tag = dr1[0].ToString();
                        tn1.Name = dr1[0].ToString();
                        this.treeView1.Nodes[tn.Name].Nodes.Add(tn1);
                        DataTable dt3;
                        if (this.radioButton1.Checked)
                        {
                            dt3 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE " + (this.comboBox1.Text == "客户名" ? "CNAME" : "CSHORTCODE") + " LIKE '" + this.textBox1.Text + "%' AND CCODE LIKE'" + dr1[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 2) + "'  ORDER BY CCODE");
                        }
                        else
                        {
                            dt3 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE " + (this.comboBox1.Text == "客户名" ? "CNAME" : "CSHORTCODE") + " LIKE '%" + this.textBox1.Text + "%' AND CCODE LIKE'" + dr1[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 2) + "'  ORDER BY CCODE");
                        }

                        foreach (DataRow dr2 in dt3.Rows)
                        {
                            TreeNode tn2 = new TreeNode(dr2[1].ToString());
                            tn2.Tag = dr2[0].ToString();
                            tn2.Name = dr2[0].ToString();
                            this.treeView1.Nodes[tn.Name].Nodes[tn1.Name].Nodes.Add(tn2);
                        } this.treeView1.ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void 合同展开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeView1.ExpandAll();
        }

        private void 全部折叠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeView1.CollapseAll();
        }
    }
}
