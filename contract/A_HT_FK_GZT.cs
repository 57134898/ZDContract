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
    public partial class A_HT_FK_GZT : Form, IChildForm
    {
        private DataTable dt;//DGV DATASOUCE
        private DataView dv;
        private ToolStripItem[] bts;

        private DataTable dt1;
        private DataTable dt2;

        private string htype;
        public A_HT_FK_GZT()
        {
            InitializeComponent();
        }

        private void A_HT_FK_GZT_Load(object sender, EventArgs e)
        {
            dt1 = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LEN(LID)=2 ");
            this.comboBoxLX.DataSource = dt1;
            this.comboBoxLX.DisplayMember = "LNAME";
            this.comboBoxLX.ValueMember = "LID";
            this.comboBoxLX_SelectedIndexChanged(null, null);
            Reg();

        }
        private void DgvCssSet()
        {
            if (htype.Substring(0, 2) == "02")
            {
                this.dataGridView1.Columns["发票"].HeaderText = "销项发票";
                this.dataGridView1.Columns["金额"].HeaderText = "已收货款";
                this.dataGridView1.Columns["发票1"].HeaderText = "未开销项发票";
                this.dataGridView1.Columns["金额1"].HeaderText = "尚欠账款";
                this.dataGridView1.Columns["财务余额"].HeaderText = "财务余额(已收货款-销项发票)";
            }
            else
            {
                this.dataGridView1.Columns["发票"].HeaderText = "进项发票";
                this.dataGridView1.Columns["金额"].HeaderText = "已付货款";
                this.dataGridView1.Columns["发票1"].HeaderText = "未收进项发票";
                this.dataGridView1.Columns["金额1"].HeaderText = "未付货款";
                this.dataGridView1.Columns["财务余额"].HeaderText = "财务余额(已付货款-进项发票)";
            }

            //if (htype == "02")
            //{
            //    this.dataGridView1.Columns["发票"].HeaderText = "销项发票";
            //    this.dataGridView1.Columns["金额"].HeaderText = "应收账款";
            //    this.dataGridView1.Columns["发票1"].HeaderText = "未开销项发票";
            //    this.dataGridView1.Columns["金额1"].HeaderText = "未收货款";
            //}
            //else
            //{
            //    this.dataGridView1.Columns["发票"].HeaderText = "进项发票";
            //    this.dataGridView1.Columns["金额"].HeaderText = "应付账款";
            //    this.dataGridView1.Columns["发票1"].HeaderText = "未收进项发票";
            //    this.dataGridView1.Columns["金额1"].HeaderText = "未付账款";
            //}

            this.dataGridView1.Columns[4].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[6].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[7].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[8].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[10].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[11].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[12].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[13].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[14].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[15].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.dataGridView1.AutoResizeColumns();
            for (int i = 36; i < this.dataGridView1.Columns.Count - 3; i++)
            {
                this.dataGridView1.Columns[i].Visible = false;
            }
            this.dataGridView1.AutoResizeColumns();
            this.dataGridView1.Columns[2].Frozen = true;
        }
        public void DataLoad()
        {
            try
            {
                if (this.comboBoxLX1.Text == "")
                {
                    htype = this.comboBoxLX.SelectedValue.ToString();
                }
                else
                {
                    htype = this.comboBoxLX1.SelectedValue.ToString();
                }
                //htype = this.comboBoxLX.SelectedValue.ToString() + this.comboBoxLX1.SelectedValue.ToString();
                //dt = DBAdo.DtFillSql("SELECT [HID], [HKH], [HDW], [HCODE], [HAREA], [HDATE], [HYWY], [HXM], [HZBJ], [HHS], [HHSBL], [HHTJE], [HJSJE], [HLX], [HMEMO], [HZT], [HJHDATE], [HUSER], [FLAG] FROM [ACONTRACT] WHERE FLAG = 1  AND HDW ='" + ClassConstant.DW_ID + "' AND HLX LIKE '" + htype + "%'");
                dt = DBAdo.DtFillSql("SELECT * FROM [VCONTRACTS] WHERE FLAG = 1  AND HDW ='" + ClassConstant.DW_ID + "' AND HLX = '" + htype + "'");

                dv = dt.DefaultView;
                this.dataGridView1.DataSource = dv;
                DgvCssSet();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void Reg()
        {
            //this.splitContainer1.Panel1Collapsed = true;
            //bts = new ToolStripItem[]{
            //        new Factory_ToolBtn("应收货款", "应收货款",ClassCustom.getImage("ys.png"),this.btn_ys,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("应付货款", "应付货款",ClassCustom.getImage("yf.png"), this.btn_yf,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("  估验  ", "  估验  ",ClassCustom.getImage("gy.png"),this.btn_gy,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("  抵消  ", "  抵消 ",ClassCustom.getImage("dx.png"),this.btn_dx,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        new Factory_ToolBtn(" 收发票 ", "收发票",ClassCustom.getImage("sfp.png"),this.btn_sfp,null,true).TBtnProduce(),
            //        new Factory_ToolBtn(" 开发票 ", "开发票",ClassCustom.getImage("kfp.png"),this.btn_kfp,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        new Factory_ToolBtn("生成金税数据", "生成金税数据",ClassCustom.getImage("js.png"),this.btn_js,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        //new Factory_ToolBtn("合同评审", "合同评审",ClassCustom.getImage("ps.png"),this.btn_ps,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("  查询  ", " 查询 ",ClassCustom.getImage("sel.png"), this.btn_Sel,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("进度明细","进度明细",ClassCustom.getImage("mx.png"),this.btn_mx,null,true).TBtnProduce(),
            //        //new Factory_ToolBtn("凭证信息","凭证信息",ClassCustom.getImage("pz.png"),this.btn_pz,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("  刷新  ", "  刷新  ",ClassCustom.getImage("sx.png"), this.button1_Click,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("  打印  ", "  打印  ",ClassCustom.getImage("print.png"), this.PrintF,null,true).TBtnProduce()
            //        };
            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),

                    new Factory_ToolBtn("  抵消  ", "  抵消 ",ClassCustom.getImage("dx.png"),this.btn_dx,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("生成金税数据", "生成金税数据",ClassCustom.getImage("js.png"),this.btn_js,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    //new Factory_ToolBtn("合同评审", "合同评审",ClassCustom.getImage("ps.png"),this.btn_ps,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  查询  ", " 查询 ",ClassCustom.getImage("sel.png"), this.btn_Sel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("进度明细","进度明细",ClassCustom.getImage("mx.png"),this.btn_mx,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("凭证信息","凭证信息",ClassCustom.getImage("pz.png"),this.btn_pz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  刷新  ", "  刷新  ",ClassCustom.getImage("sx.png"), this.button1_Click,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  打印  ", "  打印  ",ClassCustom.getImage("print.png"), this.PrintF,null,true).TBtnProduce()
                    };
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        public void ReLoad()
        {
            this.button1_Click(null, null);
        }

        #region 按钮事件

        private void btn_mx(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            A_HT_FK_MX cm = new A_HT_FK_MX(this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString(), this);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_pz(object sender, EventArgs e)
        {

        }

        private void btn_ps(object sender, EventArgs e)
        {
            this.splitContainer1.Panel1Collapsed = false;
        }

        private void btn_ys(object sender, EventArgs e)
        {
            //if (this.dataGridView1.Rows.Count == 0)
            //    return;
            //if (this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString().Substring(0, 2) != "02")
            //    return;
            //if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK)))
            //    return;
            //A_HT_FK cm = new A_HT_FK(this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString(), 1, this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString(), this, "回款", null, decimal.Parse(this.dataGridView1.SelectedRows[0].Cells["金额1"].Value.ToString()));
            //cm.MdiParent = this.MdiParent;
            //cm.Show();
            A_HT_FK_NEW fknew = new A_HT_FK_NEW();
            fknew.MdiParent = this.MdiParent;
            fknew.Show();
        }

        private void btn_yf(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            if (this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString().Substring(0, 2) == "02")
                return;
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK)))
                return;
            A_HT_FK cm = new A_HT_FK(this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString(), 1, this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString(), this, "付款", null, decimal.Parse(this.dataGridView1.SelectedRows[0].Cells["金额1"].Value.ToString()));
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_sfp(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            if (this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString().Substring(0, 2) == "02")
                return;
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK)))
                return;
            A_HT_FK cm = new A_HT_FK(this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString(), 1, this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString(), this, "进项发票", null, decimal.Parse(this.dataGridView1.SelectedRows[0].Cells["发票1"].Value.ToString()));
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_kfp(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            if (this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString().Substring(0, 2) != "02")
                return;
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK)))
                return;
            A_HT_FK cm = new A_HT_FK(this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString(), 1, this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString(), this, "销项发票", null, decimal.Parse(this.dataGridView1.SelectedRows[0].Cells["发票1"].Value.ToString()));
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_dx(object sender, EventArgs e)
        {

            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK_DX)))
                return;
            A_HT_FK_DX cm = new A_HT_FK_DX(this);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_gy(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            if (this.dataGridView1.SelectedRows[0].Cells["HLX"].Value.ToString().Substring(0, 2) == "02")
                return;
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_GY)))
                return;
            A_HT_GY cm = new A_HT_GY(1, this, this.dataGridView1.SelectedRows[0]);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_js(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;

            A_HT_JS ajs = new A_HT_JS(this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString());
            ajs.MdiParent = this.MdiParent;
            ajs.Show();
            return;
            DataTable js = DBAdo.DtFillSql("SELECT C.CNAME 客户名称,C.CSH 税号,C.CADDRESS + C.CTEL 地址电话,C.CACCOUNT 银行帐号,G.GNAME 货物名称,G.GXH 规格型号,G.GDW1 单位,G.GSL 数量,G.GDJ1 金额,G.GMEMO 备注,H.HCODE 合同号 "
                        + "FROM ACONTRACT H INNER JOIN ACLIENTS C ON H.HKH=C.CCODE "
                        + "INNER JOIN ASP G ON H.HCODE = G.HTH AND H.HCODE = '" + this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString() + "'");
            js.Columns.Add("分公司名称", typeof(string));
            foreach (DataRow r in js.Rows)
            {
                r["分公司名称"] = ClassConstant.DW_NAME;
            }
            this.dataGridView2.DataSource = js;
            if (this.dataGridView2.Rows.Count > 0)
            {
                ClassCustom.ExportDataGridview1(this.dataGridView2, this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString() + " 发票数据");
            }
            else
            {
                MessageBox.Show("没有数据");
            }

        }

        private void btn_Sel(object sender, EventArgs e)
        {
            this.splitContainer1.Panel1Collapsed = false;
        }

        private void PrintF(object sender, EventArgs e)
        {
            try
            {
                //调用EXCEL打印
                ClassCustom.PrintE(ClassCustom.ExportDataGridview1(this.dataGridView1, ""), this.dataGridView1, Excel.XlPaperSize.xlPaperA4, Excel.XlPageOrientation.xlLandscape);
                return;
                //DGV打印类
                if (this.dataGridView1.Rows.Count > 0)
                {
                    ZYPrinter printer = new ZYPrinter();
                    printer.Title = "客户信息";
                    //printer.SubTitle = "合同报表";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit |
                    StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.ShowTotalPageNumber = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    //printer.Footer = "页 脚";
                    //printer.Footer = "金额合计: " + summ.ToString();
                    printer.FooterSpacing = 15;
                    printer.PageSeparator = " / ";
                    printer.PageText = "页";
                    printer.PrintPreviewDataGridView(dataGridView1);
                }
                else
                {
                    MessageBox.Show("没有数据可以打印", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void ExportExcel(object sender, EventArgs e)
        {
            ClassCustom.ExportDataGridview1(this.dataGridView1, "");
        }

        #endregion



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

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBoxLX.Text == "")
            {
                return;
            }
            else
            {
                if (this.comboBoxLX1.Text == "")
                {
                    this.splitContainer1.Panel1Collapsed = true;
                    this.Text = "财务工作台-" + this.hLXTextBox.Text;
                    DataLoad();
                    DgvCssSet();
                }
                else
                {
                    this.splitContainer1.Panel1Collapsed = true;
                    this.Text = "财务工作台-" + this.hLXTextBox.Text;
                    DataLoad();
                    DgvCssSet();
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.treeView1.Visible)
            {
                this.treeView1.Visible = false;
            }
            else
            {
                this.treeView1.Visible = true;
                this.treeView1.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT * FROM ALX WHERE LEN(LID)=" + ClassConstant.GetLeveLChar("LEVEL_HTLX", 0).Length + " ORDER BY LID");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT * FROM ALX WHERE LID LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_HTLX", 1) + "'  ORDER BY LID");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Name = dr[0].ToString();
                    tn.Tag = dr[0].ToString();
                    this.treeView1.Nodes.Add(tn);
                    //foreach (DataRow dr1 in dt2.Rows)
                    //{
                    //    TreeNode tn1 = new TreeNode(dr1[1].ToString());
                    //    tn1.Name = dr1[0].ToString();
                    //    tn1.Tag = dr1[0].ToString();
                    //    this.treeView1.Nodes[tn.Name].Nodes.Add(tn1);
                    //}
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView1.SelectedNode.Nodes.Count != 0)
                    return;
                this.hLXTextBox.Text = this.treeView1.SelectedNode.Text;
                this.hLXTextBox.Tag = this.treeView1.SelectedNode.Tag;
                this.treeView1.Visible = false;

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void comboBoxLX_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dt2 = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LID LIKE '" + this.comboBoxLX.SelectedValue.ToString() + "__' ");
                this.comboBoxLX1.DataSource = dt2;
                this.comboBoxLX1.DisplayMember = "LNAME";
                this.comboBoxLX1.ValueMember = "LID";
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }
    }
}
