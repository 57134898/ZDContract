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
    public partial class A_HT : Form, IChildForm
    {
        private ToolStripItem[] bts;
        private DataTable dt;
        private DataView dv;
        private string htype;//01采购02销售03外协04在建
        private string lx;
        public A_HT()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 01采购02销售03外协04在建
        /// </summary>
        /// <param name="htype">01采购02销售03外协04在建</param>
        public A_HT(string htype)
        {
            InitializeComponent();
            this.htype = htype;
        }

        private void A_HT_Load(object sender, EventArgs e)
        {

            //this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            //string t = "";
            //if (htype == "01") { t = "采购合同"; }
            //if (htype == "02") { t = "销售合同"; }
            //if (htype == "03") { t = "外协合同"; }
            //if (htype == "04") { t = "在建合同"; }
            //this.Text = "合同-" + t;
            Reg();

            string sql = "SELECT LID,LNAME FROM ALX WHERE LID LIKE '" + this.htype + "__'";
            DataTable dtlx = DBAdo.DtFillSql(sql);
            this.comboBoxLX1.DataSource = dtlx;
            this.comboBoxLX1.DisplayMember = "LNAME";
            this.comboBoxLX1.ValueMember = "LID";
        }

        private void DgvCssSet()
        {
            if (htype.Substring(0, 2) == "02")
            {
                this.dataGridView1.Columns["发票"].HeaderText = "销项发票";
                this.dataGridView1.Columns["金额"].HeaderText = "已收货款";
                this.dataGridView1.Columns["发票1"].HeaderText = "未开销项发票";
                this.dataGridView1.Columns["金额1"].HeaderText = "尚欠货款";
            }
            else
            {
                this.dataGridView1.Columns["发票"].HeaderText = "进项发票";
                this.dataGridView1.Columns["金额"].HeaderText = "已付货款";
                this.dataGridView1.Columns["发票1"].HeaderText = "未收进项发票";
                this.dataGridView1.Columns["金额1"].HeaderText = "未付货款";
            }

            //this.dataGridView1.Columns[4].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[6].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[7].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[8].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[10].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[11].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[12].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[13].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[14].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[15].DefaultCellStyle.Format = "N2";
            //this.dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //this.dataGridView1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (DataGridViewColumn c in this.dataGridView1.Columns)
            {
                if (c.ValueType == typeof(decimal))
                {
                    c.DefaultCellStyle.Format = "N2";
                    c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            this.dataGridView1.AutoResizeColumns();

            this.dataGridView1.Columns[2].Frozen = true;
            //{
            //    this.dataGridView1.Columns["HAREA"].HeaderText = "地区";
            //    this.dataGridView1.Columns["HCODE"].HeaderText = "合同号";
            //    this.dataGridView1.Columns["HDATE"].HeaderText = "签定日期";
            //    this.dataGridView1.Columns["HDW"].HeaderText = (htype == "02" ? "出卖人" : "买受人");
            //    this.dataGridView1.Columns["HHS"].HeaderText = "含税";
            //    this.dataGridView1.Columns["HHTJE"].HeaderText = "合同金额";
            //    this.dataGridView1.Columns["HID"].Visible = false;
            //    this.dataGridView1.Columns["HJHDATE"].HeaderText = "交货日期";
            //    this.dataGridView1.Columns["HJSJE"].HeaderText = "结算金额";
            //    this.dataGridView1.Columns["HKH"].HeaderText = (htype == "02" ? "买受人" : "出卖人");
            //    this.dataGridView1.Columns["HLX"].HeaderText = "合同类型";
            //    this.dataGridView1.Columns["HMEMO"].HeaderText = "备注";
            //    this.dataGridView1.Columns["HHSBL"].HeaderText = "含税比例";
            //    this.dataGridView1.Columns["HUSER"].HeaderText = "操作员";
            //    this.dataGridView1.Columns["HXM"].HeaderText = "业务员";
            //    this.dataGridView1.Columns["HYWY"].HeaderText = "备注";
            //    this.dataGridView1.Columns["HZBJ"].HeaderText = "质保金";
            //    this.dataGridView1.Columns["HZT"].HeaderText = "状态";
        }

        public void DataLoad()
        {
            try
            {
                if (this.comboBoxLX1.Text == "")
                {
                    return;
                }
                string col = "FLAG 审批,HKH 客户码,客户名,合同号,签定日期,合同金额,结算金额,标号,质保金,运费,其它费用,合同类型,金额,发票,金额1,发票1,估验,财务余额,地区,签订日期,中标方式,";
                col += "合同备注,业务员,状态,交货日期,操作员,含税,比例,代理费,选型费,标书费,项目名称,";
                col += "FLAG,HAREA,HLX,HYWY,HDW,HID ";
                //dt = DBAdo.DtFillSql("SELECT [HID], [HKH], [HDW], [HCODE], [HAREA], [HDATE], [HYWY], [HXM], [HZBJ], [HHS], [HHSBL], [HHTJE], [HJSJE], [HLX], [HMEMO], [HZT], [HJHDATE], [HUSER], [FLAG] FROM [ACONTRACT] WHERE FLAG = 1  AND HDW ='" + ClassConstant.DW_ID + "' AND HLX LIKE '" + htype + "%'");
                dt = DBAdo.DtFillSql("SELECT " + col + " FROM [VCONTRACTS] WHERE 1 = 1  AND HDW ='" + ClassConstant.DW_ID + "' AND HLX LIKE '" + this.comboBoxLX1.SelectedValue.ToString() + "%'");

                dv = dt.DefaultView;

                this.dataGridView1.DataSource = dv;
                this.dataGridView1.Columns["签定日期"].HeaderText = "录入日期";
                DgvCssSet();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void Reg()
        {
            if (htype.Substring(0, 2) == "04")
            {
                OnButtonClick mx = (object sender, EventArgs e) =>
                {
                    if (this.dataGridView1.SelectedRows.Count > 0)
                    {
                        A_RPT_ZJGC_KHHZ mxx = new A_RPT_ZJGC_KHHZ();
                        mxx.MdiParent = this.MdiParent;
                        mxx.Show();
                    }
                };
                bts = new ToolStripItem[]{
                new Factory_ToolBtn("计算器","计算器",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("添加合同","添加合同",ClassCustom.getImage("add.png"),this.btn_Add,null,true).TBtnProduce(),
                    new Factory_ToolBtn("删除合同", "删除合同",ClassCustom.getImage("del.png"), this.btn_Del,null,true).TBtnProduce(),
                    new Factory_ToolBtn("修改合同","修改合同",ClassCustom.getImage("upd.png"),this.btn_Update,null,true).TBtnProduce(),
                    new Factory_ToolBtn("查询合同", "查询合同",ClassCustom.getImage("sel.png"), this.btn_Sel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("刷新", "刷新",ClassCustom.getImage("sx.png"), this.A_HT_Load,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("财务明细", "财务明细",ClassCustom.getImage("mx.png"), mx,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("打印", "打印",ClassCustom.getImage("print.png"), this.PrintF,null,true).TBtnProduce()
                    };
            }
            else
            {
                bts = new ToolStripItem[]{
                new Factory_ToolBtn("计算器","计算器",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("添加合同","添加合同",ClassCustom.getImage("add.png"),this.btn_Add,null,true).TBtnProduce(),
                    new Factory_ToolBtn("删除合同", "删除合同",ClassCustom.getImage("del.png"), this.btn_Del,null,true).TBtnProduce(),
                    new Factory_ToolBtn("修改合同","修改合同",ClassCustom.getImage("upd.png"),this.btn_Update,null,true).TBtnProduce(),
                    new Factory_ToolBtn("查询合同", "查询合同",ClassCustom.getImage("sel.png"), this.btn_Sel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("刷新", "刷新",ClassCustom.getImage("sx.png"), this.A_HT_Load,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("打印", "打印",ClassCustom.getImage("print.png"), this.PrintF,null,true).TBtnProduce()
                    };
            }
            //this.splitContainer1.Panel1Collapsed = true;
            //ToolStripItem    new ToolStripSeparator(),

            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        #region 按钮事件
        private void btn_Add(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_OP)))
                return;
            A_HT_OP cm = new A_HT_OP(1, this, this.htype);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_Del(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0 || bool.Parse(this.dataGridView1["FLAG", this.dataGridView1.SelectedRows[0].Index].Value.ToString().ToString()))
            {
                return;
            }
            if (!(DialogResult.Yes == MessageView.MessageYesNoShow("是否删除选中合同的信息?")))
                return;
            string hcode = this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString();
            DataTable dt = DBAdo.DtFillSql(string.Format(@"SELECT ID FROM AFKXX WHERE HTH='{0}'", hcode));
            string sql = string.Format(@"
DELETE FROM ACONTRACT WHERE hcode= '{0}';
DELETE FROM ASP WHERE HTH= '{0}';
DELETE FROM ACASH WHERE CID IN(SELECT DISTINCT CID FROM AFKXX WHERE HTH ='{0}');
DELETE FROM AWX WHERE XSHTH= '{0}' OR  WXHTH= '{0}';
", hcode);
            foreach (DataRow r in dt.Rows)
            {
                sql += string.Format(@" DELETE FROM AFKXX WHERE id ={0}", r[0].ToString());
            }
            DBAdo.ExecuteNonQuerySql(sql);
            this.DataLoad();
        }

        private void btn_Update(object sender, EventArgs e)
        {
            this.dataGridView1_CellDoubleClick(null, null);
            //if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_CLIENT_OP)))
            //    return;
            //A_CLIENT_OP cm = new A_CLIENT_OP(3, null, this.dataGridView1.SelectedRows[0], true);
            //cm.MdiParent = this.MdiParent;
            //cm.Show();
        }

        private void btn_Sel(object sender, EventArgs e)
        {
            //this.splitContainer1.Panel1Collapsed = false;
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_CX_HT)))
                return;
            A_CX_HT cx = new A_CX_HT();
            cx.MdiParent = this.MdiParent;
            cx.Show();
        }

        private void PrintF(object sender, EventArgs e)
        {
            try
            {
                //调用EXCEL打印
                ClassCustom.PrintE(ClassCustom.ExportDataGridview1(this.dataGridView1, "合同明细"), this.dataGridView1, Excel.XlPaperSize.xlPaperA4, Excel.XlPageOrientation.xlLandscape);
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
            ClassCustom.ExportDataGridview1(this.dataGridView1, "合同明细");
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_OP)))
                return;
            A_HT_OP cm = new A_HT_OP(3, this, this.dataGridView1.SelectedRows[0], this.htype);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
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
            btn1.Margin = new Padding(0, 0, 0, 0);
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
            try
            {
                (this.MdiParent as MForm1).DelStatus(btn1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
            DataLoad();
            this.splitContainer1.Panel1Collapsed = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                s = " 结算金额 = 金额 AND 结算金额 = 发票 ";
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                s = " (结算金额 <> 金额 OR 结算金额 <> 发票) ";
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                s = " 1=1 ";
            }
        }
        string s = "";
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dv.RowFilter = s + (this.checkBox1.Checked ? " and 签定日期 >= '" + this.dateTimePicker1.Value.ToShortDateString() + "' and  签定日期<= '" + this.dateTimePicker2.Value.ToShortDateString() + "'" : "");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

    }
}
