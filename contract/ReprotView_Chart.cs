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
    public partial class ReprotView_Chart : Form
    {
        public ReprotView_Chart()
        {
            InitializeComponent();
        }

        private void ReprotView_Chart_Load(object sender, EventArgs e)
        {

            Reg();
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
                    this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                    this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_Chart1.rdlc";
                    this.reportViewer1.LocalReport.DataSources.Clear();

                    string sql = string.Format("SELECT H.HDW 公司,TYPE 类型,sum(F.rmb) 金额 FROM AFKXX F,ACONTRACT H WHERE F.HTH=H.HCODE AND DATE BETWEEN  '{0}' AND '{1}' GROUP BY H.HDW,TYPE", this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());
                    DataTable dt = DBAdo.DtFillSql(sql);

                    foreach (DataRow r in dt.Rows)
                    {

                        r[0] += ":" + DBAdo.ExecuteScalarSql(string.Format("SELECT cname from aclients where ccode ='{0}'", r[0].ToString())).ToString().Substring(10);
                        Application.DoEvents();

                    }



                    DataView dv = dt.DefaultView;
                    DataTable souce = dv.ToTable();
                    ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_DataTable_Chart1", souce);
                    this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    //ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                    //ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", ClassCustom.codeSub1(this.HDW.Text));
                    //ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                    //ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                    //ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                    //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5 });
                    this.splitContainer1.Panel1Collapsed = true;
                    this.reportViewer1.ZoomMode = ZoomMode.Percent;
                    this.reportViewer1.ZoomPercent = 100;
                    this.reportViewer1.RefreshReport();
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

        private void reportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            try
            {
                LocalReport localReport = (LocalReport)e.Report;
                Console.WriteLine(localReport.OriginalParametersToDrillthrough[0].Values[0].ToString());
                DataTable dt = DBAdo.DtFillSql(string.Format("SELECT EXCHANGEDATE 日期,c.cname 客户,TYPE 类型,cash 现汇,note 票据,mz 抹账, cash+note+mz 小计 FROM ACASH a,aclients c where 1=1 AND HDW ='{0}' and  c.ccode =a.ccode ORDER BY EXCHANGEDATE", ClassCustom.codeSub(localReport.OriginalParametersToDrillthrough[0].Values[0].ToString())));
                localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_DataTable2", dt));

                //ReportParameter rp1 = new ReportParameter("HDW", dt.Rows[0]["客户名"].ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                // MessageBox.Show(ex.Message);
                return;
            }
        }


    }
}
