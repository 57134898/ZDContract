using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace contract
{
    public partial class A_CLIENT_OP : Form, IChildForm
    {
        private ToolStripButton[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_CLIENT ac;//客户表单
        private DataGridViewRow dgvr;//修改用
        private bool ctype = true;
        public A_CLIENT_OP()
        {
            InitializeComponent();
        }


        public A_CLIENT_OP(int op, A_CLIENT ac)//添加
        {
            InitializeComponent();
            this.op = op;
            this.ac = ac;

        }

        public A_CLIENT_OP(int op, A_CLIENT ac, DataGridViewRow dgvr)//修改
        {
            InitializeComponent();
            this.op = op;
            this.ac = ac;
            this.dgvr = dgvr;

        }

        private void A_CLIENT_M_Load(object sender, EventArgs e)
        {
            this.cNAMELabel.Text = (ctype ? "客户名" : "供应商");
            if (this.op == 1) { this.Text = (ctype ? "客户" : "供应商") + "-添加"; }
            if (this.op == 3)
            {
                this.Text = (ctype ? "客户" : "供应商") + "-修改";

                this.cCODETextBox.Text = this.dgvr.Cells["CCODE"].Value.ToString();
                this.cCODETextBox.Tag = this.dgvr.Cells["CCODE"].Value.ToString();
                this.cNAMETextBox.Text = this.dgvr.Cells["CNAME"].Value.ToString();
                this.cSHORTCODETextBox.Text = this.dgvr.Cells["CSHORTCODE"].Value.ToString();
                this.cADDRESSTextBox.Text = this.dgvr.Cells["CADDRESS"].Value.ToString();
                this.cTELTextBox.Text = this.dgvr.Cells["CTEL"].Value.ToString();
                this.cLINKMANTextBox.Text = this.dgvr.Cells["CLINKMAN"].Value.ToString();
                this.cPHONETextBox.Text = this.dgvr.Cells["CPHONE"].Value.ToString();
                this.cFAXNOTextBox.Text = this.dgvr.Cells["CFAXNO"].Value.ToString();
                this.cPOSNOTextBox.Text = this.dgvr.Cells["CPOSNO"].Value.ToString();
                this.cBANKNAMETextBox.Text = this.dgvr.Cells["CBANKNAME"].Value.ToString();
                this.cACCOUNTTextBox.Text = this.dgvr.Cells["CACCOUNT"].Value.ToString();
                this.cEMAILTextBox.Text = this.dgvr.Cells["CEMAIL"].Value.ToString();
                this.CareaTextbox.Text = this.dgvr.Cells["CAREA"].Value.ToString();
                this.CareaTextbox.Tag = this.dgvr.Cells["ACODE"].Value.ToString();
                this.CmemoRichTextBox.Text = this.dgvr.Cells["CMEMO"].Value.ToString();
                this.cfrTextBox.Text = this.dgvr.Cells["CFR"].Value.ToString();
                this.cshTextBox.Text = this.dgvr.Cells["CSH"].Value.ToString();
                this.fpdhtextBox.Text = this.dgvr.Cells["CFPTEL"].Value.ToString();
                this.textBox2.Text = this.dgvr.Cells["CFPBAND"].Value.ToString();
                this.textBox1.Text = this.dgvr.Cells["CFPPHONE"].Value.ToString();
            }

            bts = new ToolStripButton[]{
                new Factory_ToolBtn("计算器","计算器",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new Factory_ToolBtn("保存","保存",ClassCustom.getImage("sav.png"),btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭", "关闭",ClassCustom.getImage("tc.png"), btn_close,null,true).TBtnProduce()
                    };
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
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
            btn1.Margin = new Padding(0, 0, 0, 0);
            btn1.Text = this.Text;
            btn1.UseVisualStyleBackColor = true;
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

        #region 按钮事件
        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                if (this.cNAMETextBox.Text == "" || this.cCODETextBox.Text == "")
                    return;

                if (op == 1)
                {
                    if (DBAdo.ExecuteScalarSql("SELECT COUNT(*) FROM ACLIENTS WHERE ccode like '" + this.cCODETextBox.Tag.ToString() + "%' and SUBSTRING(CCODE,3,2)='" + ClassConstant.DW_ID.Substring(2) + "' and CNAME = '" + this.cNAMETextBox.Text + "'").ToString() == "0")
                    { }
                    else
                    {
                        MessageBox.Show((ctype ? "客户名已经存在！" : "供应商已经存在！"));
                        this.cNAMETextBox.Text = "";
                        return;
                    }

                }
                if (op == 3)
                {
                    if (DBAdo.ExecuteScalarSql("SELECT COUNT(*) FROM ACLIENTS WHERE  ccode like '" + this.cCODETextBox.Tag.ToString() + "%' and  SUBSTRING(CCODE,3,2)='" + ClassConstant.DW_ID.Substring(2) + "' and  CNAME = '" + this.cNAMETextBox.Text + "' AND CID != '" + dgvr.Cells[0].Value.ToString() + "'").ToString() == "0")
                    { }
                    else
                    {
                        MessageBox.Show((ctype ? "客户名已经存在！" : "供应商已经存在！"));
                        this.cNAMETextBox.Text = "";
                        return;
                    }
                }

                if (op == 1)
                {
                    string sql = "SELECT (max(CCODE)+1) FROM n7_铸锻公司..CCODE WHERE 1=1 AND CCODE <> '02019999' AND CCODE LIKE '" + this.cCODETextBox.Tag.ToString() + "____'";
                    string code = DBAdo.ExecuteScalarSql(sql).ToString();
                    if (string.IsNullOrEmpty(code))
                    {
                        code = this.cCODETextBox.Tag.ToString() + "0001";
                    }
                    code = code.PadLeft(8, '0');
                    OleDbParameter[] pars = new OleDbParameter[21];
                    pars[0] = new OleDbParameter("@CCODE", code);
                    pars[1] = new OleDbParameter("@CNAME", this.cNAMETextBox.Text);
                    pars[2] = new OleDbParameter("@CSHORTCODE", this.cSHORTCODETextBox.Text);
                    pars[3] = new OleDbParameter("@CADDRESS", this.cADDRESSTextBox.Text);
                    pars[4] = new OleDbParameter("@CTEL", this.cTELTextBox.Text);
                    pars[5] = new OleDbParameter("@CLINKMAN", this.cLINKMANTextBox.Text);
                    pars[6] = new OleDbParameter("@CPHONE", this.cPHONETextBox.Text);
                    pars[7] = new OleDbParameter("@CFAXNO", this.cFAXNOTextBox.Text);
                    pars[8] = new OleDbParameter("@CPOSNO", this.cPOSNOTextBox.Text);
                    pars[9] = new OleDbParameter("@CBANKNAME", this.cBANKNAMETextBox.Text);
                    pars[10] = new OleDbParameter("@CACCOUNT", this.cACCOUNTTextBox.Text);
                    pars[11] = new OleDbParameter("@CEMAIL", this.cEMAILTextBox.Text);
                    pars[12] = new OleDbParameter("@CAREA", (this.CareaTextbox.Tag == null ? "" : this.CareaTextbox.Tag.ToString()));
                    pars[13] = new OleDbParameter("@CMEMO", this.CmemoRichTextBox.Text);
                    pars[14] = new OleDbParameter("@CFR", this.cfrTextBox.Text);
                    pars[15] = new OleDbParameter("@CSH", this.cshTextBox.Text);
                    pars[16] = new OleDbParameter("@CTYPE", this.ctype);
                    pars[17] = new OleDbParameter("@CBCODE", ClassConstant.DW_ID);
                    pars[18] = new OleDbParameter("@FPDH", this.fpdhtextBox.Text);
                    pars[19] = new OleDbParameter("@CFPBAND", this.textBox2.Text);
                    pars[20] = new OleDbParameter("@CFPPHONE", this.textBox1.Text);
                    DBAdo.ExecuteScalarProcedure("ACLIENTS_INSERT", pars);
                    MessageBox.Show("添加成功");
                    this.cCODETextBox.Text = "";
                    this.cNAMETextBox.Text = "";
                    this.cSHORTCODETextBox.Text = "";
                    this.cADDRESSTextBox.Text = "";
                    this.cTELTextBox.Text = "";
                    this.cLINKMANTextBox.Text = "";
                    this.cPHONETextBox.Text = "";
                    this.cFAXNOTextBox.Text = "";
                    this.cPOSNOTextBox.Text = "";
                    this.cBANKNAMETextBox.Text = "";
                    this.cACCOUNTTextBox.Text = "";
                    this.cEMAILTextBox.Text = "";
                    this.CareaTextbox.Text = "";
                    this.CmemoRichTextBox.Text = "";
                    this.fpdhtextBox.Text = "";
                    this.textBox2.Text = "";
                    this.textBox1.Text = "";
                }
                if (op == 3)
                {
                    OleDbParameter[] pars1 = new OleDbParameter[21];
                    pars1[0] = new OleDbParameter("@CID", int.Parse(dgvr.Cells["CID"].Value.ToString()));
                    pars1[1] = new OleDbParameter("@CCODE", this.cCODETextBox.Text);
                    pars1[2] = new OleDbParameter("@CNAME", this.cNAMETextBox.Text);
                    pars1[3] = new OleDbParameter("@CSHORTCODE", this.cSHORTCODETextBox.Text);
                    pars1[4] = new OleDbParameter("@CADDRESS", this.cADDRESSTextBox.Text);
                    pars1[5] = new OleDbParameter("@CTEL", this.cTELTextBox.Text);
                    pars1[6] = new OleDbParameter("@CLINKMAN", this.cLINKMANTextBox.Text);
                    pars1[7] = new OleDbParameter("@CPHONE", this.cPHONETextBox.Text);
                    pars1[8] = new OleDbParameter("@CFAXNO", this.cFAXNOTextBox.Text);
                    pars1[9] = new OleDbParameter("@CPOSNO", this.cPOSNOTextBox.Text);
                    pars1[10] = new OleDbParameter("@CBANKNAME", this.cBANKNAMETextBox.Text);
                    pars1[11] = new OleDbParameter("@CACCOUNT", this.cACCOUNTTextBox.Text);
                    pars1[12] = new OleDbParameter("@CEMAIL", this.cEMAILTextBox.Text);
                    pars1[13] = new OleDbParameter("@CAREA", (this.CareaTextbox.Tag == null ? "" : this.CareaTextbox.Tag.ToString()));
                    pars1[14] = new OleDbParameter("@CMEMO", this.CmemoRichTextBox.Text);
                    pars1[15] = new OleDbParameter("@CFR", this.cfrTextBox.Text);
                    pars1[16] = new OleDbParameter("@CSH", this.cshTextBox.Text);
                    pars1[17] = new OleDbParameter("@CTYPE", this.ctype);
                    pars1[18] = new OleDbParameter("@FPDH", this.fpdhtextBox.Text);
                    pars1[19] = new OleDbParameter("@CFPBAND", this.textBox2.Text);
                    pars1[20] = new OleDbParameter("@CFPPHONE", this.textBox1.Text);
                    //MessageBox.Show(pars1[12].Value.ToString());
                    DBAdo.ExecuteNonQuerySqlProcedure("ACLIENTS_UPDATE", pars1);
                    MessageBox.Show("修改成功");
                    this.Close();
                }
                if (ac == null)
                    return;
                ac.DataLoad();
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

        private void cNAMETextBox_TextChanged(object sender, EventArgs e)
        {
            this.cSHORTCODETextBox.Text = ClassCustom.ChinesePY.GetPinYinIndex(this.cNAMETextBox.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView1.Visible)
                {
                    this.treeView1.Visible = false;
                    return;
                }
                this.treeView1.Visible = true;
                this.treeView1.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT * FROM AREAS WHERE LEN(ACODE)=1");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT * FROM AREAS WHERE ACODE LIKE'" + dr[0].ToString() + "__'");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.treeView1.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Name = dr1[0].ToString();
                        tn1.Tag = dr1[0].ToString();
                        this.treeView1.Nodes[tn.Name].Nodes.Add(tn1);
                        DataTable dt3 = DBAdo.DtFillSql("SELECT * FROM AREAS WHERE ACODE LIKE'" + dr1[0].ToString() + "___'");
                        foreach (DataRow dr2 in dt3.Rows)
                        {
                            TreeNode tn2 = new TreeNode(dr2[1].ToString());
                            tn2.Name = dr2[0].ToString();
                            tn2.Tag = dr2[0].ToString();
                            this.treeView1.Nodes[tn.Name].Nodes[tn1.Name].Nodes.Add(tn2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView1.SelectedNode.Level != 2)
                    return;
                this.CareaTextbox.Text = this.treeView1.SelectedNode.Text;
                this.CareaTextbox.Tag = this.treeView1.SelectedNode.Tag;
                this.treeView1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void cNAMETextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (op == 1)
                {
                    if (DBAdo.ExecuteScalarSql("SELECT COUNT(*) FROM ACLIENTS WHERE  ccode like '" + this.cCODETextBox.Tag.ToString() + "%' and SUBSTRING(CCODE,3,2)='" + ClassConstant.DW_ID.Substring(2) + "' and CNAME = '" + this.cNAMETextBox.Text + "'").ToString() == "0")
                        return;
                    MessageBox.Show((ctype ? "客户名已经存在！" : "供应商已经存在！"));
                    this.cNAMETextBox.Text = "";
                }
                if (op == 3)
                {
                    if (DBAdo.ExecuteScalarSql("SELECT COUNT(*) FROM ACLIENTS WHERE  ccode like '" + this.cCODETextBox.Tag.ToString() + "%' and  SUBSTRING(CCODE,3,2)='" + ClassConstant.DW_ID.Substring(2) + "' and  CNAME = '" + this.cNAMETextBox.Text + "' AND CID != '" + dgvr.Cells[0].Value.ToString() + "'").ToString() == "0")
                        return;
                    MessageBox.Show((ctype ? "客户名已经存在！" : "供应商已经存在！"));
                    this.cNAMETextBox.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView2.Visible)
                {
                    this.treeView2.Visible = false;
                    return;
                }
                this.treeView2.Visible = true;
                this.treeView2.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM n7_铸锻公司..CCODE WHERE LEN(CCODE)=2 AND CCODE NOT LIKE '01%' and ccode not like '99%' AND CCODE NOT LIKE '05%'");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT  CCODE,CNAME FROM n7_铸锻公司..CCODE WHERE CCODE LIKE'" + dr[0].ToString() + "" + ClassConstant.DW_ID.Substring(2) + "'");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.treeView2.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Name = dr1[0].ToString();
                        tn1.Tag = dr1[0].ToString();
                        this.treeView2.Nodes[tn.Name].Nodes.Add(tn1);
                        //DataTable dt3 = DBAdo.DtFillSql("SELECT  CCODE,CNAME FROM n7_铸锻公司..CCODE WHERE  CCODE LIKE'" + dr1[0].ToString() + "____'");
                        //foreach (DataRow dr2 in dt3.Rows)
                        //{
                        //    TreeNode tn2 = new TreeNode(dr2[1].ToString());
                        //    tn2.Name = dr2[0].ToString();
                        //    tn2.Tag = dr2[0].ToString();
                        //    this.treeView2.Nodes[tn.Name].Nodes[tn1.Name].Nodes.Add(tn2);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void treeView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView2.SelectedNode.Tag.ToString().Length != 4)
                    return;
                this.cCODETextBox.Text = this.treeView2.SelectedNode.Text;
                this.cCODETextBox.Tag = this.treeView2.SelectedNode.Tag;
                this.treeView2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

    }
}
