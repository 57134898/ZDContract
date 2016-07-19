using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace contract
{
    public partial class A_CLIENT : Form, IChildForm
    {
        private DataTable dt;//DGV DATASOUCE
        private DataView dv;
        private ToolStripItem[] bts = null;
        private bool ctype;//类型 客户 TRUE 供应商 false 
        public A_CLIENT()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 类型 客户 TRUE 供应商 false 
        /// </summary>
        /// <param name="ctype">客户 TRUE 供应商 false </param>
        public A_CLIENT(bool ctype)
        {
            InitializeComponent();
            this.ctype = ctype;
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

        private void A_CLIENT_Load(object sender, EventArgs e)
        {
            //this.comboBox1.Items.AddRange(new object[] { "编码", "客户名", "简码", "地址" });
            if (ctype)
            {
                this.Text = "客户";
            }
            else
            {
                this.Text = "供应商";
            }
            Reg();
            DataLoad();

            #region 查询列COMBOBOX的值
            DataTable dtse = new DataTable();//查询条件 se = new DataTable();
            dtse.Columns.Add("key");
            dtse.Columns.Add("value");
            dtse.Rows.Add(new object[] { "编码", "CCODE" });
            dtse.Rows.Add(new object[] { "客户名", "CNAME" });
            dtse.Rows.Add(new object[] { "简码", "CSHORTCODE" });
            dtse.Rows.Add(new object[] { "地址", "CADDRESS" });
            dtse.Rows.Add(new object[] { "电话", "CTEL" });
            dtse.Rows.Add(new object[] { "联系人", "CLINKMAN" });
            dtse.Rows.Add(new object[] { "手机", "CPHONE" });
            dtse.Rows.Add(new object[] { "传真", "CFAXNO" });
            dtse.Rows.Add(new object[] { "开户行", "CBANKNAME" });
            dtse.Rows.Add(new object[] { "账号", "CACCOUNT" });
            dtse.Rows.Add(new object[] { "邮编", "CPOSNO" });
            dtse.Rows.Add(new object[] { "E-mail", "CEMAIL" });
            dtse.Rows.Add(new object[] { "法人", "CFR" });
            dtse.Rows.Add(new object[] { "税号", "CSH" });
            this.comboBox1.DataSource = dtse;
            this.comboBox1.DisplayMember = "key";
            this.comboBox1.ValueMember = "value";
            #endregion
        }

        private void DgvCssSet()
        {
            //SELECT [CID], [CCODE], [CNAME], [CSHORTCODE], [CADDRESS], [CTEL], [CLINKMAN], [CPHONE], [CFAXNO], [CPOSNO], [CBANKNAME], [CACCOUNT], [CEMAIL], [FLAG] FROM [JXC2011].[dbo].[ACLIENTS]
            this.dataGridView1.Columns["CID"].Visible = false;
            this.dataGridView1.Columns["FLAG"].Visible = false;
            this.dataGridView1.Columns["CCODE"].HeaderText = "编码";
            this.dataGridView1.Columns["CNAME"].HeaderText = "客户名";
            this.dataGridView1.Columns["CSHORTCODE"].HeaderText = "简码";
            this.dataGridView1.Columns["CADDRESS"].HeaderText = "地址";
            this.dataGridView1.Columns["CTEL"].HeaderText = "电话";
            this.dataGridView1.Columns["CLINKMAN"].HeaderText = "联系人";
            this.dataGridView1.Columns["CPHONE"].HeaderText = "手机";
            this.dataGridView1.Columns["CFAXNO"].HeaderText = "传真";
            this.dataGridView1.Columns["CBANKNAME"].HeaderText = "开户行";
            this.dataGridView1.Columns["CACCOUNT"].HeaderText = "账号";
            this.dataGridView1.Columns["CPOSNO"].HeaderText = "邮编";
            this.dataGridView1.Columns["CEMAIL"].HeaderText = "E-mail";
            this.dataGridView1.Columns["CAREA"].HeaderText = "地区";
            this.dataGridView1.Columns["CMEMO"].HeaderText = "备注";
            this.dataGridView1.Columns["CID"].HeaderText = "编号";
            this.dataGridView1.Columns["CFR"].HeaderText = "法人";
            this.dataGridView1.Columns["CSH"].HeaderText = "税号";
            this.dataGridView1.Columns["CFPTEL"].HeaderText = "开发票电话";

            this.dataGridView1.Columns["CFPBAND"].HeaderText = "开发票银行";
            this.dataGridView1.Columns["CFPPHONE"].HeaderText = "开发票账号";
            this.dataGridView1.AutoResizeColumns();
        }
        public void DataLoad()
        {
            try
            {
                dt = DBAdo.DtFillSql("SELECT [CID], [CCODE], [CNAME], [CSHORTCODE], [CADDRESS], [CTEL], [CLINKMAN], [CPHONE], [CFAXNO], [CPOSNO], [CBANKNAME], [CACCOUNT], [CEMAIL], A.ANAME [CAREA],[CFR], [CSH],[CFPTEL], [CFPBAND],[CFPPHONE],[CMEMO], [ACODE],  [FLAG] FROM [ACLIENTS] C LEFT JOIN AREAS A ON C.CAREA=A.ACODE  WHERE FLAG = 1 AND (CCODE like '01%'  OR CCODE like '11%'  OR CCODE LIKE '05%' OR CCODE LIKE '02" + ClassConstant.DW_ID.Substring(2) + "%' OR CCODE LIKE '03" + ClassConstant.DW_ID.Substring(2) + "%') ORDER BY CCODE");
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
            this.splitContainer1.Panel1Collapsed = true;

            bts = new ToolStripItem[]{
                    new Factory_ToolBtn("计算器","计算器",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("添加客户","添加客户",ClassCustom.getImage("add.png"),this.btn_Add,null,true).TBtnProduce(),
                    new Factory_ToolBtn("删除客户", "删除客户",ClassCustom.getImage("del.png"), this.btn_Del,null,true).TBtnProduce(),
                    new Factory_ToolBtn("修改客户","修改客户",ClassCustom.getImage("upd.png"),this.btn_Update,null,true).TBtnProduce(),
                    new Factory_ToolBtn("查询客户", "查询客户",ClassCustom.getImage("sel.png"), this.btn_Sel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("刷新", "刷新",ClassCustom.getImage("sx.png"), this.A_CLIENT_Load,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("打印", "打印",ClassCustom.getImage("print.png"), this.PrintF,null,true).TBtnProduce()
                    };



            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        #region 按钮事件
        private void btn_Add(object sender, EventArgs e)
        {
            if (ctype)
            {
                if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_CLIENT_OP)))
                    return;
                A_CLIENT_OP cm = new A_CLIENT_OP(1, this);
                cm.MdiParent = this.MdiParent;
                cm.Show();
            }
            else
            {
                if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_CLIENT_OP)))
                    return;
                A_CLIENT_OP cm = new A_CLIENT_OP(1, this);
                cm.MdiParent = this.MdiParent;
                cm.Show();
            }
        }

        private void btn_Del(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
            {
                return;
            }
            if (this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Substring(0, 2) == "01" || this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Substring(0, 2) == "05")
            {
                return;
            }
            if (!(DialogResult.Yes == MessageView.MessageYesNoShow("是否删除选中" + (ctype ? "客户" : "供应商") + "信息?")))
                return;
            try
            {
                DBAdo.ExecuteNonQuerySql("DELETE FROM ACLIENTS WHERE CID= '" + this.dataGridView1.SelectedRows[0].Cells["CID"].Value.ToString() + "'");
                this.DataLoad();

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }

        }

        private void btn_Update(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                if (this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Substring(0, 2) == "01" || this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Substring(0, 2) == "05")
                {
                    return;
                }
                if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_CLIENT_OP)))
                    return;
                A_CLIENT_OP cm = new A_CLIENT_OP(3, this, this.dataGridView1.SelectedRows[0]);
                cm.MdiParent = this.MdiParent;
                cm.Show();
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
                ClassCustom.PrintE(ClassCustom.ExportDataGridview1(this.dataGridView1, (ctype ? "客户明细" : "供应商明细")), this.dataGridView1, Excel.XlPaperSize.xlPaperA4, Excel.XlPageOrientation.xlLandscape);
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
            ClassCustom.ExportDataGridview1(this.dataGridView1, (ctype ? "客户明细" : "供应商明细"));
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.dv.RowFilter = ClassCustom.Tj(this.comboBox1, this.comboBox2, this.textBox1);
            //MessageBox.Show(dv.RowFilter);
            this.dataGridView1.DataSource = dv;
            this.splitContainer1.Panel1Collapsed = true;
            this.comboBox1.Text = "";
            this.comboBox2.Text = "";
            this.textBox1.Text = "";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.btn_Update(null, null);
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




    }
}
