using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace contract
{
    public partial class A_HT_FX_XX : Form
    {

        public A_HT_FX_XX()
        {
            InitializeComponent();
        }
        #region Form基本方法
        private ToolStripItem[] bts = null;
        private DataTable dt;
        private DataView dv;

        public void reLoad()
        {
            Form_Load(null, null);
        }



        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Reg();
                DataLoad();
                DgvCssSet();
                dataGridView1_CellClick(null, new DataGridViewCellEventArgs(0, 0));
            }
            catch (Exception ex)
            {

                MessageView.MessageErrorShow(ex);
            }
        }

        private void DgvCssSet()
        {
            try
            {
                this.dataGridView1.Columns["CID"].Visible = false;


                this.dataGridView1.Columns[0].Frozen = true;
                this.dataGridView1.Columns[1].Frozen = true;
                this.dataGridView1.Columns[2].Frozen = true;
                this.dataGridView1.Columns[3].Frozen = true;
                this.dataGridView1.Columns[4].Frozen = true;
                this.dataGridView1.Columns["金额"].DefaultCellStyle.Format = "N2";
                this.dataGridView1.Columns["金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                this.dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {

                MessageView.MessageErrorShow(ex);
            }
        }

        private void DataLoad()
        {
            try
            {




                //dt = DBAdo.DtFillSql("SELECT [ID], C.CNAME,H.HCODE, [date], F.[rmb],F.[type], F.[flag], [vid], [vtype], [vyear], [vmonth] FROM AFKXX F INNER JOIN ACONTRACT H ON H.HCODE = F.HTH INNER JOIN ACLIENTS C ON H.HKH = C.CCODE AND H.HDW ='" + ClassConstant.DW_ID + "'");
                string columnsNames = "A.CID,ExchangeDate 日期,C.CNAME 客户,a.Type 类型,A.Cash+Note+Mz 金额,VoucherFlag 是否连接凭证,VoucherYear 凭证年,VoucherMonth 凭证月, VoucherType 凭证类型, VoucherId 凭证号, Cash 现汇,Note 票据,Mz 抹账,ZJBL1 开行,ZJBL2 原公司,ZJBL3 新公司 ,a.CCODE ";
                dt = DBAdo.DtFillSql("SELECT " + columnsNames + " FROM ACASH A ,ACLIENTS C,afkxx f WHERE A.CCODE=C.CCODE and a.cid= f.cid and  f.hth in (select hcode from acontract where hdw = '" + ClassConstant.DW_ID
                    + " ') GROUP BY A.CID,ExchangeDate,C.CNAME,a.Type,A.Cash+Note+Mz,VoucherFlag,VoucherYear,VoucherMonth,VoucherType,VoucherId,Cash,Note,Mz,ZJBL1,ZJBL2,ZJBL3,a.CCODE  ORDER BY C.CNAME");
                dv = dt.DefaultView;
                this.dataGridView1.DataSource = dv;



            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void Reg()
        {
            bool flag = false;
            if (ClassConstant.BNAME == "财务部" || ClassConstant.BNAME == "办公室")
            {
                flag = true;
            }

            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                     new ToolStripSeparator(),
                    new Factory_ToolBtn("凭证确认","凭证确认",ClassCustom.getImage("add.png"),this.btn_Add,null,flag).TBtnProduce(),
                    new Factory_ToolBtn("删除确认", "删除确认",ClassCustom.getImage("del.png"), this.btn_Del,null,flag).TBtnProduce(),
                    new Factory_ToolBtn("修改确认","修改确认",ClassCustom.getImage("upd.png"),this.btn_Update,null,flag).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  打印  ", "  打印  ",ClassCustom.getImage("print.png"), this.PrintF,null,true).TBtnProduce()
                    };

            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
            {
                e.Handled = true;
            }
        }

        private void QK()
        {

        }

        #region 按钮事件

        private void btn_sp(object sender, EventArgs e)
        {
            //if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_SK)))
            //    return;
            //A_HT_SK cm = new A_HT_SK();
            //cm.MdiParent = this.MdiParent;
            //cm.Show();
        }

        private void btn_Add(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_PZ)))
                return;
            A_HT_PZ cm = new A_HT_PZ(1, this, this.dataGridView1.SelectedRows[0]);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        private void btn_Del(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count == 0)
                    return;
                if (DialogResult.Yes != MessageBox.Show("是否删除连接凭证信息", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                string sql = "UPDATE ACASH SET VoucherFLAG =0,Voucheryear = null,Vouchermonth=null,Vouchertype=null,Voucherid=null WHERE CID=" + this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                DBAdo.ExecuteNonQuerySql(sql);
                this.Form_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void btn_Update(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_PZ)))
                return;
            A_HT_PZ cm = new A_HT_PZ(1, this, this.dataGridView1.SelectedRows[0]);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }




        private void btn_close(object sender, EventArgs e)
        {
            this.Close();
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
            ClassCustom.ExportDataGridview1(this.dataGridView1, "合同收付款与发票明细");
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


        #endregion

        string s = "";
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                s = "是否连接凭证 <>1";
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                s = "是否连接凭证 = 1";
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                s = " 1=1 ";
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                DataTable souce = DBAdo.DtFillSql("SELECT [ID], [date] 日期, [rmb] 金额, [hth] 合同号, [xshth] 销售合同号, [type] 类型, [fkfs] 付款方式, [fklx] 付款类型, [bl1] 开行, [bl2] 原公司, [bl3] 新公司, [CID] FROM [AFKXX] WHERE CID =" + this.dataGridView1["CID", e.RowIndex].Value.ToString());
                this.dataGridView2.DataSource = souce.DefaultView;
                this.dataGridView2.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dv.RowFilter = s + " and 日期 >= '" + this.dateTimePicker1.Value.ToShortDateString() + "' and  日期<= '" + this.dateTimePicker2.Value.ToShortDateString() + "'";
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
