using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace contract
{
    public partial class ReprotView3 : Form, IsetText
    {
        private string reportName;
        private bool isJT;

        public ReprotView3()
        {
            InitializeComponent();
        }

        public ReprotView3(string reportName, bool isJT)
        {
            InitializeComponent();
            this.reportName = reportName;
            this.isJT = isJT;
        }

        #region Form基本方法

        private ToolStripItem[] bts = null;

        private void Reg()
        {
            OnButtonClick btn_tj = (object sender, EventArgs e) =>
            {
                this.splitContainer1.Panel1Collapsed = false;
                //this.splitContainer1.Panel2Collapsed = true;
            };

            OnButtonClick select = (object sender, EventArgs e) =>
            {
                try
                {
                    switch (reportName)
                    {
                        case "销售合同（对应）外协产品部件明细表":
                            Get_销售合同对应外协产品部件明细表();
                            break;
                        case "销售合同商品明细表":
                            this.Get_销售合同商品明细表();
                            break;
                        case "外协合同部件明细表":
                            this.Get_外协合同部件明细表();
                            break;
                        default:
                            break;
                    }


                    if (this.comboBox1.Text == "")
                    {
                        return;
                    }

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    MessageBox.Show(ex.Message);
                    return;
                }
            };
            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("编辑条件","  编辑条件",ClassCustom.getImage("upd.png"),btn_tj,null,true).TBtnProduce(),
                        //new Factory_ToolBtn("明细条款","明细条款",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
                        //new Factory_ToolBtn("商品明细", "商品明细",ClassCustom.getImage("sp.png"), this.btn_sp,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("  意见  ", "  意见  ",ClassCustom.getImage("yj.png"), this.btn_yj,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("客户信息", "客户信息",ClassCustom.getImage("khxx.png"), this.btn_kh,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  查询  ", "  查询  ",ClassCustom.getImage("sel.png"), select,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    //new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        private void Get_销售合同对应外协产品部件明细表()
        {
            if (this.comboBox1.Text == "")
            {
                return;
            }
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_销售合同对应外协合同明细表.rdlc";
            this.reportViewer1.LocalReport.DataSources.Clear();
            string sql = "";
            sql += "DECLARE @HCODE NVARCHAR(20);";
            sql += string.Format("SELECT @HCODE ='{0}';", this.comboBox1.Text);
            sql += "SELECT * FROM VCX1 WHERE 合同号 IN(SELECT WXHTH FROM AWX WHERE XSHTH = @HCODE);";
            DataTable dt = DBAdo.DtFillSql(sql);
            ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_DataSouce", dt);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            DataRow dr = DBAdo.DtFillSql(string.Format("SELECT   客户名, 合同号 ,AVG(结算金额) 结算金额,AVG(金额) 金额 , AVG(金额1)  金额1 ,AVG(发票)  发票 ,  AVG(发票1)发票1 ,AVG(财务余额) 财务余额    FROM VCX1 WHERE 合同号 = '{0}' group by  客户名,合同号", this.comboBox1.Text)).Rows[0];

            ReportParameter rp0 = new ReportParameter("rp0", this.comboBox1.Text);
            ReportParameter rp1 = new ReportParameter("rp1", dr["金额"].ToString());
            ReportParameter rp2 = new ReportParameter("rp2", dr["发票"].ToString());
            ReportParameter rp3 = new ReportParameter("rp3", dr["客户名"].ToString());
            ReportParameter rp4 = new ReportParameter("rp4", dr["结算金额"].ToString());
            ReportParameter rp5 = new ReportParameter("rp5", "");
            ReportParameter rp6 = new ReportParameter("rp6", "");
            ReportParameter rp7 = new ReportParameter("rp7", dr["金额1"].ToString());
            ReportParameter rp8 = new ReportParameter("rp8", dr["发票1"].ToString());
            ReportParameter rp9 = new ReportParameter("rp9", dr["财务余额"].ToString());
            //ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
            //ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp0, rp1, rp2, rp3, rp4, rp5, rp6, rp7, rp8, rp9 });
            this.splitContainer1.Panel1Collapsed = true;
            this.reportViewer1.ZoomMode = ZoomMode.Percent;
            this.reportViewer1.ZoomPercent = 100;
            this.reportViewer1.RefreshReport();
        }

        private void Get_销售合同商品明细表()
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    return;
                }
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_销售合同商品明细.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                string sql = "";
                sql += string.Format("SELECT * FROM VCX1 WHERE HLX LIKE '02%' AND  HKH = '{0}'", this.textBox1.Tag.ToString());
                DataTable dt = DBAdo.DtFillSql(sql);
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_DataSouce", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                DataRow dr = DBAdo.DtFillSql(string.Format("SELECT  客户名,SUM(结算金额) 结算金额,SUM(金额) 金额 , SUM(金额1)  金额1 ,SUM(发票)  发票 ,  SUM(发票1)发票1 ,SUM(财务余额) 财务余额   FROM VCONTRACTS WHERE HLX LIKE '02%' AND  HKH = '{0}' group by 客户名", this.textBox1.Tag.ToString())).Rows[0];

                ReportParameter rp1 = new ReportParameter("rp1", dr["客户名"].ToString());
                ReportParameter rp2 = new ReportParameter("rp2", dr["结算金额"].ToString());
                ReportParameter rp3 = new ReportParameter("rp3", dr["金额"].ToString());
                ReportParameter rp4 = new ReportParameter("rp4", dr["金额1"].ToString());
                ReportParameter rp5 = new ReportParameter("rp5", dr["发票"].ToString());
                ReportParameter rp6 = new ReportParameter("rp6", dr["发票1"].ToString());
                ReportParameter rp7 = new ReportParameter("rp7", dr["财务余额"].ToString());
                ReportParameter rp8 = new ReportParameter("rp8", "");
                //ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                //ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7, rp8 });
                this.splitContainer1.Panel1Collapsed = true;
                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Get_外协合同部件明细表()
        {
            if (this.textBox1.Text == "")
            {
                return;
            }
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_外协合同部件明细表.rdlc";
            this.reportViewer1.LocalReport.DataSources.Clear();
            string sql = "";
            sql += string.Format("SELECT * FROM VCX1 WHERE HLX LIKE '03%' AND  HKH = '{0}'", this.textBox1.Tag.ToString());
            DataTable dt = DBAdo.DtFillSql(sql);
            ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_DataSouce", dt);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            DataRow dr = DBAdo.DtFillSql(string.Format("SELECT  客户名,SUM(结算金额) 结算金额,SUM(金额) 金额 , SUM(金额1)  金额1 ,SUM(发票)  发票 ,  SUM(发票1)发票1 ,SUM(财务余额) 财务余额   FROM VCONTRACTS WHERE HLX LIKE '03%' AND HKH = '{0}' group by 客户名", this.textBox1.Tag.ToString())).Rows[0];

            ReportParameter rp1 = new ReportParameter("rp1", dr["客户名"].ToString());
            ReportParameter rp2 = new ReportParameter("rp2", dr["结算金额"].ToString());
            ReportParameter rp3 = new ReportParameter("rp3", dr["金额"].ToString());
            ReportParameter rp4 = new ReportParameter("rp4", dr["金额1"].ToString());
            ReportParameter rp5 = new ReportParameter("rp5", dr["发票"].ToString());
            ReportParameter rp6 = new ReportParameter("rp6", dr["发票1"].ToString());
            ReportParameter rp7 = new ReportParameter("rp7", dr["财务余额"].ToString());

            //ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
            //ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7 });
            this.splitContainer1.Panel1Collapsed = true;
            this.reportViewer1.ZoomMode = ZoomMode.Percent;
            this.reportViewer1.ZoomPercent = 100;
            this.reportViewer1.RefreshReport();
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


        private void btn_close(object sender, EventArgs e)
        {
            this.Close();
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

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = 200;
        }

        #endregion

        private void ReportView3_Load(object sender, EventArgs e)
        {
            this.Reg();

            this.reportViewer1.Dock = DockStyle.Fill;
            if (this.reportName == "销售合同（对应）外协产品部件明细表")
            {

            }
            else
            {
                this.panel2.Visible = false;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_FZ_KH)))
                return;
            A_FZ_KH cm = new A_FZ_KH(this);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }

        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            try
            {
                this.textBox1.Tag = key;
                this.textBox1.Text = value;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as TextBox).Text == "")
                    return;
                if (reportName == "销售合同（对应）外协产品部件明细表")
                {
                    DataTable cSouce = DBAdo.DtFillSql(string.Format("SELECT HCODE FROM ACONTRACT WHERE 1=1 AND HKH = '{0}' AND HDW = '{1}' AND HLX LIKE '02%'", this.textBox1.Tag.ToString(), ClassConstant.DW_ID));
                    this.comboBox1.DataSource = cSouce;
                    this.comboBox1.DisplayMember = "HCODE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}
