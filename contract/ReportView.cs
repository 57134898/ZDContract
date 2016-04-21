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
    public partial class ReportView : Form
    {
        private DataTable dt_dw;
        private DataTable dt_lx;
        private DataTable dt_bm;
        private string reportName;
        private bool isJT;

        public ReportView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">报表名</param>
        /// <param name="isJT">是集团否</param>
        public ReportView(string reportName, bool isJT)
        {
            InitializeComponent();
            this.reportName = reportName;
            this.isJT = isJT;
            if (reportName == "Report_JT_合同类型汇总表.rdlc")
            {
                this.panel1.Visible = false;
                this.panel3.Visible = false;
                this.panel7.Visible = true;
            }
            if (reportName == "Report_JT_各单位收付款汇总表")
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = true;
            }
            if (reportName == "Report_JT_各单位货款同期对比表")
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = true;
            }

            if (reportName == "各单位签订合同情况表")
            {
                this.panel1.Visible = false;
                this.panel2.Visible = true;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = false;
                this.panel7.Visible = true;
                this.panel8.Visible = false;
            }
            if (reportName == "集团合同类型汇总表")
            {
                this.panel1.Visible = false;
                this.panel2.Visible = true;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
            }
            if (reportName == "销售毛利表")
            {
                this.panel1.Visible = true;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
            }
            if (reportName == "合同收付款明细表全部")
            {
                this.panel9.Visible = true;
            }
            if (reportName == "铸锻公司全部采购外协合同汇总表")
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
            }
            if (reportName == "毛利新")
            {
                this.panel1.Visible = true;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = true;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
                this.panel9.Visible = true;
            }
            if (reportName == "集团毛利新")
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = true;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
                this.panel9.Visible = true;
                //this.radioButton2.Visible = false;
            }
            if (reportName == "毛利新1")
            {
                this.panel1.Visible = true;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = true;
                this.panel6.Visible = true;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
                this.panel9.Visible = true;
            }
        }

        private void ReportView_Load(object sender, EventArgs e)
        {
            try
            {

                //this.reportViewer1.RefreshReport();
                Reg();
                this.YEAR.Text = DateTime.Now.Year.ToString();
                this.MONTH.Text = DateTime.Now.Month.ToString();
                dt_dw = DBAdo.DtFillSql(string.Format("select bcode ,bname from bcode where LEN(Bcode)<=4 AND Bcode LIKE '01%' OR  Bcode LIKE '02%'"));
                foreach (DataRow r in dt_dw.Rows)
                {
                    this.HDW.Items.Add(r[0].ToString() + ":" + r[1].ToString());
                }
                if (ClassConstant.USER_ID == "0101999999"
                    || ClassConstant.USER_ID == "0101010001"
                    || ClassConstant.USER_ID == "0201999999"
                    || ClassConstant.USER_ID == "0201010001")
                {
                    this.HDW.Enabled = true;
                }
                else
                {
                    foreach (var item in this.HDW.Items)
                    {
                        if (ClassCustom.codeSub(item.ToString()) == ClassConstant.DW_ID)
                        {
                            this.HDW.SelectedItem = item;
                            break;
                        }
                    }
                    this.HDW.Enabled = false;
                }
                dt_lx = DBAdo.DtFillSql(string.Format("select LID ,LNAME from ALX where 1=1 and len(lid) = 2"));
                foreach (DataRow r in dt_lx.Rows)
                {
                    this.HLX.Items.Add(r[0].ToString() + ":" + r[1].ToString());
                }
                this.Text = "报表浏览器 - [" + reportName + "]";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 合同收付款明细表
        /// </summary>
        private void getReport_合同收付款明细表()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report合同收付款明细表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
                }
                else
                {
                    containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
                }


                string sql = string.Format(@"SELECT 合同号,客户名,结算金额,HKH 客户码,(SELECT  YEAR(MAX(DATE)) FROM AFKXX F0 WHERE H.合同号=F0.HTH) as 年,DBO.GetCustomerCate(HKH) as 客户类型 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F1 WHERE  F1.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)<{6}),0.00) AS A1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F2 WHERE  F2.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS A2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F3 WHERE  F3.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS A3 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F4 WHERE  F4.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)<{6}),0.00) AS B1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F5 WHERE  F5.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS B2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F6 WHERE  F6.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS B3 
                                            FROM VCONTRACTS H WHERE 1=1 and flag =1  AND HLX LIKE '{3}%' AND " + containTwo + " AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                     new object[] {
                         ClassConstant.NB, 
                         ClassConstant.WB, 
                         ClassConstant.NHI, 
                         ClassCustom.codeSub(HLX.Text), 
                         ClassCustom.codeSub(HDW.Text), 
                         ClassCustom.codeSub(HBM.Text),
                         YEAR.Text, MONTH.Text, 
                         ClassConstant.ZJ });
                this.progressBar1.Value = 50;
                this.progressBar1.Visible = true;
                DataTable dt = DBAdo.DtFillSql(sql);
                dt.Columns.Add("A4", typeof(decimal), "A1+A3");
                dt.Columns.Add("A5", typeof(decimal), "结算金额-A4");
                dt.Columns.Add("A6", typeof(decimal), "A5/结算金额");
                dt.Columns.Add("B4", typeof(decimal), "B1+B3");
                dt.Columns.Add("B5", typeof(decimal), "结算金额-B4");
                dt.Columns.Add("B6", typeof(decimal), "B5/结算金额");
                dt.Columns.Add("C", typeof(decimal), "B4-A4");
                this.progressBar1.Maximum = 100;
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("(C<>0 or B4 <> 结算金额 or A4 <> 结算金额) or 年 >= {0}", this.YEAR.Text);
                DataTable souce = dv.ToTable();
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R合同收付款明细表", souce);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }

        }

        public string IsTotal(string companyid)
        {
            if (companyid.Length <= 2)
            {
                if (this.checkBox1.Checked)
                {
                    return "";
                }
                else
                {
                    return " and hdw like '" + companyid + "%'";
                }
            }
            else
            {
                if (this.checkBox1.Checked)
                {
                    return "";
                }
                else
                {
                    return " and hdw like '" + companyid + "%'";
                }
            }
            return "";
        }

        private void getReport_合同收付款明细表_全部()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report合同收付款明细表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                //string sql = string.Format("SELECT 合同号,客户名,结算金额,HKH 客户码,year(签定日期) 年,CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{8}' THEN '在建工程' ELSE '鼓风' END as 客户类型 FROM VCONTRACTS WHERE 1=1 AND HLX LIKE '{3}%' AND HDW = '{4}' AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                //   new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });
                string sql = string.Format("SELECT 合同号,客户名,结算金额,HKH 客户码,0 年,DBO.GetCustomerCate(HKH) as 客户类型 FROM VCONTRACTS WHERE 1=1 AND HLX LIKE '{3}%' AND HDW = '{4}' AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                     new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });

                DataTable dt = DBAdo.DtFillSql(sql);
                dt.Columns.Add("A1", typeof(decimal));
                dt.Columns.Add("A2", typeof(decimal));
                dt.Columns.Add("A3", typeof(decimal));
                dt.Columns.Add("A4", typeof(decimal), "A1+A3");
                dt.Columns.Add("A5", typeof(decimal), "结算金额-A4");
                dt.Columns.Add("A6", typeof(decimal), "A5/结算金额");
                dt.Columns.Add("B1", typeof(decimal));
                dt.Columns.Add("B2", typeof(decimal));
                dt.Columns.Add("B3", typeof(decimal));
                dt.Columns.Add("B4", typeof(decimal), "B1+B3");
                dt.Columns.Add("B5", typeof(decimal), "结算金额-B4");
                dt.Columns.Add("B6", typeof(decimal), "B5/结算金额");
                dt.Columns.Add("C", typeof(decimal), "B4-A4");

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                this.progressBar1.Visible = true;

                foreach (DataRow r in dt.Rows)
                {
                    this.progressBar1.Value++;
                    Application.DoEvents();
                    r["年"] = DBAdo.ExecuteScalarSql(string.Format("SELECT YEAR(MAX(DATE)) FROM AFKXX WHERE 1=1 AND HTH = '{0}'", r[0].ToString()));
                    decimal a1 = 0;
                    decimal a2 = 0;
                    decimal a3 = 0;
                    decimal b1 = 0;
                    decimal b2 = 0;
                    decimal b3 = 0;
                    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                    object o3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));

                    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                    object oo3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));

                    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());
                    a3 = decimal.Parse(o3 == null || o3.ToString() == "" ? "0" : o3.ToString());

                    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());
                    b3 = decimal.Parse(oo3 == null || oo3.ToString() == "" ? "0" : oo3.ToString());

                    r["A1"] = a1;
                    r["A2"] = a2;
                    r["A3"] = a3;
                    r["B1"] = b1;
                    r["B2"] = b2;
                    r["B3"] = b3;

                }
                DataView dv = dt.DefaultView;

                if (this.comboBox2.Text == "已完成")
                {
                    dv.RowFilter = string.Format("C=0 and B4 =结算金额 and A4 =结算金额");
                }
                else if (this.comboBox2.Text == "未完成")
                {
                    dv.RowFilter = string.Format("C<>0 or B4 <> 结算金额 or A4 <> 结算金额");
                }
                else
                {
                    dv.RowFilter = "";
                }


                DataTable souce = dv.ToTable();
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R合同收付款明细表", souce);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }

        }

        private void getReport_合同收付款明细表_按客户()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report合同收付款明细表_按客户.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                //                string sql = string.Format(@"SELECT 合同号,客户名,结算金额,HKH 客户码,0 年,DBO.GetCustomerCate(HKH) as 客户类型
                //                                FROM VCONTRACTS WHERE 1=1 AND HLX LIKE '{3}%' AND HDW = '{4}' AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                //                     new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });

                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
                }
                else
                {
                    containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
                }
                string sql = string.Format(@"SELECT 合同号,客户名,结算金额,HKH 客户码,(SELECT  YEAR(MAX(DATE)) FROM AFKXX F0 WHERE H.合同号=F0.HTH) as 年,DBO.GetCustomerCate(HKH) as 客户类型 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F1 WHERE  F1.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)<{6}),0.00) AS A1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F2 WHERE  F2.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS A2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F3 WHERE  F3.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS A3 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F4 WHERE  F4.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)<{6}),0.00) AS B1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F5 WHERE  F5.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS B2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F6 WHERE  F6.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS B3 
                                            FROM VCONTRACTS H WHERE 1=1 and flag =1   AND HLX LIKE '{3}%' AND " + containTwo + " AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
     new object[] {
                         ClassConstant.NB, 
                         ClassConstant.WB, 
                         ClassConstant.NHI, 
                         ClassCustom.codeSub(HLX.Text), 
                         ClassCustom.codeSub(HDW.Text), 
                         ClassCustom.codeSub(HBM.Text),
                         YEAR.Text, MONTH.Text, 
                         ClassConstant.ZJ });

                DataTable dt = DBAdo.DtFillSql(sql);
                //dt.Columns.Add("A1", typeof(decimal));
                //dt.Columns.Add("A2", typeof(decimal));
                //dt.Columns.Add("A3", typeof(decimal));
                dt.Columns.Add("A4", typeof(decimal), "A1+A3");
                dt.Columns.Add("A5", typeof(decimal), "结算金额-A4");
                dt.Columns.Add("A6", typeof(decimal), "A5/结算金额");
                //dt.Columns.Add("B1", typeof(decimal));
                //dt.Columns.Add("B2", typeof(decimal));
                //dt.Columns.Add("B3", typeof(decimal));
                dt.Columns.Add("B4", typeof(decimal), "B1+B3");
                dt.Columns.Add("B5", typeof(decimal), "结算金额-B4");
                dt.Columns.Add("B6", typeof(decimal), "B5/结算金额");
                dt.Columns.Add("C", typeof(decimal), "B4-A4");

                this.progressBar1.Value = 0;
                this.progressBar1.Value = 50;
                this.progressBar1.Visible = true;
                DataView dv = dt.DefaultView;

                dv.RowFilter = string.Format("(C<>0 or B4 <> 结算金额 or A4 <> 结算金额) or 年 >= {0}", this.YEAR.Text);
                DataTable souce = dv.ToTable();
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R合同收付款明细表", souce);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }

        }

        private void getReport_回款汇总()
        {
            try
            {
                //this.button1.Enabled = false;
                string type = "and type='付款'";
                string title = "本年采购合同付款明细表";
                if (ClassCustom.codeSub(this.HLX.Text) == "02")
                {
                    type = "and type='回款'";
                    title = "本年销售合同回款明细表";
                }
                if (ClassCustom.codeSub(this.HLX.Text) == "03")
                {
                    type = "and type='付款'";
                    title = "本年外协合同付款明细表";
                }
                if (ClassCustom.codeSub(this.HLX.Text) == "04")
                {
                    type = "and type='付款'";
                    title = "本年在建工程一期合同付款明细表";
                }
                if (ClassCustom.codeSub(this.HLX.Text) == "05")
                {
                    type = "and type='付款'";
                    title = "本年在建工程二期合同付款明细表";
                }
                if (ClassCustom.codeSub(this.HLX.Text) == "06")
                {
                    type = "and type='付款'";
                    title = "本年购置固定资产合同付款明细表";
                }
                if (ClassCustom.codeSub(this.HLX.Text) == "07")
                {
                    type = "and type='付款'";
                    title = "本年技改合同付款明细表";
                }



                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report回款汇总.rdlc";

                this.reportViewer1.LocalReport.DataSources.Clear();

                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
                }
                else
                {
                    containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
                }

                DataTable dty = DBAdo.DtFillSql(string.Format(@"select DBO.GetCustomerCate(ca.ccode)  as 客户类型, cl.cname 客户名,
(SELECT sum(crmb.cash)FROM ACASH crmb WHERE crmb.hdw=ca.hdw AND crmb.ccode=ca.ccode AND month(crmb.ExchangeDate)='{0}' AND year(crmb.ExchangeDate) = '{1}' {2}) 现汇本月, sum(ca.cash) 现汇本年累计,
(SELECT sum(crmb.note)FROM ACASH crmb WHERE crmb.hdw=ca.hdw AND crmb.ccode=ca.ccode AND month(crmb.ExchangeDate)='{0}' AND year(crmb.ExchangeDate) = '{1}' {2}) 票据本月, sum(ca.note) 票据本年累计,
(SELECT sum(crmb.mz)FROM ACASH crmb WHERE crmb.hdw=ca.hdw AND crmb.ccode=ca.ccode AND month(crmb.ExchangeDate)='{0}' AND year(crmb.ExchangeDate) = '{1}'   {2}) 抹账本月, sum(ca.mz) 抹账本年累计 
from ACASH ca,ACLIENTS cl
where " + containTwo + " and year(ca.ExchangeDate)='" + this.YEAR.Text + "' and month(ca.ExchangeDate)<='" + this.MONTH.Text + "' and ca.ccode=cl.ccode " + type
             + "   AND CA.CID IN (SELECT DISTINCT CID FROM AFKXX T0 INNER JOIN ACONTRACT T1 ON T0.hth=T1.HCODE WHERE HLX LIKE '" + ClassCustom.codeSub(this.HLX.Text) + "__') group by ca.hdw,ca.ccode,cl.cname", this.MONTH.Text, this.YEAR.Text, type));


                progressBar1.Value = 0;

                progressBar1.Maximum = 100;
                progressBar1.Visible = true;

                //foreach (DataRow r in dty.Rows)
                //{
                //    Application.DoEvents();
                //    progressBar1.Value++;
                //    object s1 = DBAdo.ExecuteScalarSql("select  sum(ca.cash) from ACASH ca,ACLIENTS cl where hdw='" + ClassCustom.codeSub(this.HDW.Text) + "' and cl.cname='" + r["客户名"].ToString() + "' and month(ca.ExchangeDate)='" + this.MONTH.Text + "' and  year(ca.ExchangeDate) = '" + YEAR.Text + "' and ca.ccode=cl.ccode " + type + " group by cl.cname");
                //    object s2 = DBAdo.ExecuteScalarSql("select  sum(ca.note) from ACASH ca,ACLIENTS cl where hdw='" + ClassCustom.codeSub(this.HDW.Text) + "' and cl.cname='" + r["客户名"].ToString() + "' and month(ca.ExchangeDate)='" + this.MONTH.Text + "' and  year(ca.ExchangeDate) = '" + YEAR.Text + "'  and ca.ccode=cl.ccode " + type + " group by cl.cname");
                //    object s3 = DBAdo.ExecuteScalarSql("select  sum(ca.mz) from ACASH ca,ACLIENTS cl where hdw='" + ClassCustom.codeSub(this.HDW.Text) + "' and cl.cname='" + r["客户名"].ToString() + "' and month(ca.ExchangeDate)='" + this.MONTH.Text + "' and  year(ca.ExchangeDate) = '" + YEAR.Text + "'  and ca.ccode=cl.ccode " + type + " group by cl.cname");

                //    decimal a1 = decimal.Parse(s1 == null || s1.ToString() == "" ? "0" : s1.ToString());
                //    decimal a2 = decimal.Parse(s2 == null || s2.ToString() == "" ? "0" : s2.ToString());
                //    decimal a3 = decimal.Parse(s3 == null || s3.ToString() == "" ? "0" : s3.ToString());

                //    r["现汇本月"] = a1;
                //    r["票据本月"] = a2;
                //    r["抹账本月"] = a3;
                //}
                this.progressBar1.Visible = false;
                //this.button1.Enabled = true;
                ReportDataSource rds = new ReportDataSource("Contract1DataSet_R回款汇总", dty);
                this.reportViewer1.LocalReport.DataSources.Add(rds);
                ReportParameter rp1 = new ReportParameter("Report_DW", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp2 = new ReportParameter("Report_YEAR", this.YEAR.Text);
                ReportParameter rp3 = new ReportParameter("Report_MONTH", this.MONTH.Text);
                ReportParameter rp4 = new ReportParameter("Report_TITLE", title);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4 });
                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void getReport_合同汇总表()
        {
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Report1_ceshi.rdlc";
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report合同汇总.rdlc";
            this.reportViewer1.LocalReport.DataSources.Clear();
            string containTwo = string.Empty;
            if (this.checkBox1.Checked)
            {
                containTwo = string.Format(" SUBSTRING(AC.HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
            }
            else
            {
                containTwo = string.Format(" AC.HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
            }

            string sql = string.Format(@"select al.lname 合同类型,ac.hcode 合同号,DBO.GetCustomerCate(ac.HKH) as 客户类型 ,HKH 客户 ,
(SELECT  YEAR(MAX(DATE)) FROM AFKXX F0 WHERE ac.hcode=F0.HTH) as 年
,ISNULL((select hjsje from acontract h1 where ac.hcode=h1.hcode and year(h1.hdate)<'{0}'),0.00) AS 合同前
,ISNULL((select hjsje from acontract h2 where ac.hcode=h2.hcode and year(h2.hdate)='{0}' and month(h2.hdate)='{1}'),0.00) AS 合同本月
,ISNULL((select hjsje from acontract h3 where ac.hcode=h3.hcode and year(h3.hdate)='{0}' and month(h3.hdate)<='{1}'),0.00) AS 合同本年
,ISNULL((select sum(rmb) from afkxx f1 where f1.hth=ac.hcode and type= CASE WHEN SUBSTRING(ac.HLX,1,2)='02' THEN '回款' ELSE '付款' END  and year(f1.[date])<'{0}'),0.00) AS 收款前
,ISNULL((select sum(rmb) from afkxx f2 where f2.hth=ac.hcode and type= CASE WHEN SUBSTRING(ac.HLX,1,2)='02' THEN '回款' ELSE '付款' END  and year(f2.[date])='{0}' and month(f2.[date])='{1}'),0.00) AS 收款本月
,ISNULL((select sum(rmb) from afkxx f3 where f3.hth=ac.hcode and type= CASE WHEN SUBSTRING(ac.HLX,1,2)='02' THEN '回款' ELSE '付款' END  and year(f3.[date])='{0}' and month(f3.[date])<='{1}'),0.00) AS 收款本年
,ISNULL((select sum(rmb) from afkxx f4 where f4.hth=ac.hcode and type= CASE WHEN SUBSTRING(ac.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  and year(f4.[date])<'{0}'),0.00) AS 已开票前
,ISNULL((select sum(rmb) from afkxx f5 where f5.hth=ac.hcode and type= CASE WHEN SUBSTRING(ac.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  and year(f5.[date])='{0}' and month(f5.[date])='{1}'),0.00) AS 已开票本月
,ISNULL((select sum(rmb) from afkxx f6 where f6.hth=ac.hcode and type= CASE WHEN SUBSTRING(ac.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  and year(f6.[date])='{0}' and month(f6.[date])<='{1}'),0.00) AS 已开票本年
 from acontract ac,alx al where 1=1 and ac.hlx=al.lid and " + containTwo + " and ac.hlx like '" + ClassCustom.codeSub(this.HLX.Text)
                + "%' and (year(ac.hdate)<'"
                + this.YEAR.Text
                + "' or (year(ac.hdate)='"
                + this.YEAR.Text
                + "' and month(ac.hdate)<='"
                + this.MONTH.Text + "'))", this.YEAR.Text, this.MONTH.Text);
            DataTable dt = DBAdo.DtFillSql(sql);

            //dt.Columns.Add("合同前", typeof(decimal));
            //dt.Columns.Add("合同本月", typeof(decimal));
            //dt.Columns.Add("合同本年", typeof(decimal));
            dt.Columns.Add("合同总累计", typeof(decimal), "合同前+合同本年");
            //dt.Columns.Add("收款前", typeof(decimal));
            //dt.Columns.Add("收款本月", typeof(decimal));
            //dt.Columns.Add("收款本年", typeof(decimal));
            dt.Columns.Add("收款总累计", typeof(decimal), "收款前+收款本年");
            dt.Columns.Add("未收金额", typeof(decimal), "合同总累计-收款总累计");
            //dt.Columns.Add("已开票前", typeof(decimal));
            //dt.Columns.Add("已开票本月", typeof(decimal));
            //dt.Columns.Add("已开票本年", typeof(decimal));
            dt.Columns.Add("已开票总累计", typeof(decimal), "已开票前+已开票本年");
            dt.Columns.Add("未开票", typeof(decimal), "合同总累计-已开票总累计");

            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = 100;
            this.progressBar1.Visible = true;
            //this.button1.Enabled = false;


            //foreach (DataRow r in dt.Rows)
            //{
            //    Application.DoEvents();
            //    this.progressBar1.Value++;
            //    r["年"] = DBAdo.ExecuteScalarSql(string.Format("SELECT YEAR(MAX(DATE)) FROM AFKXX WHERE 1=1 AND HTH = '{0}'", r["合同号"].ToString()));
            //    object htq = DBAdo.ExecuteScalarSql("select hjsje from acontract where 1=1 and hcode='" + r["合同号"].ToString() + "' and hlx like '" + ClassCustom.codeSub(this.HLX.Text) + "%' and year(hdate)<'" + this.YEAR.Text + "' ");
            //    object htby = DBAdo.ExecuteScalarSql("select hjsje from acontract where 1=1 and hcode='" + r["合同号"].ToString() + "' and hlx like '" + ClassCustom.codeSub(this.HLX.Text) + "%' and year(hdate)='" + this.YEAR.Text + "' and month(hdate)='" + this.MONTH.Text + "'");
            //    object htbn = DBAdo.ExecuteScalarSql("select hjsje from acontract where 1=1 and hcode='" + r["合同号"].ToString() + "' and hlx like '" + ClassCustom.codeSub(this.HLX.Text) + "%' and year(hdate)='" + this.YEAR.Text + "' and month(hdate)<='" + this.MONTH.Text + "'");

            //    object skq = DBAdo.ExecuteScalarSql(string.Format("select sum(rmb) from afkxx where 1=1 and hth='{0}' and type='{1}' and year([date])<'{2}'", new string[] { r["合同号"].ToString(), (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款"), this.YEAR.Text }));
            //    object skby = DBAdo.ExecuteScalarSql(string.Format("select sum(rmb) from afkxx where 1=1 and hth='{0}' and type='{1}' and year([date])='{2}' and month([date])='{3}'", new string[] { r["合同号"].ToString(), (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款"), this.YEAR.Text, this.MONTH.Text }));
            //    object skbn = DBAdo.ExecuteScalarSql(string.Format("select sum(rmb) from afkxx where 1=1 and hth='{0}' and type='{1}' and year([date])='{2}' and month([date])<='{3}'", new string[] { r["合同号"].ToString(), (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款"), this.YEAR.Text, this.MONTH.Text }));

            //    object ykpq = DBAdo.ExecuteScalarSql(string.Format("select sum(rmb) from afkxx where 1=1 and hth='{0}' and type='{1}' and year([date])<'{2}'", new string[] { r["合同号"].ToString(), (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票"), this.YEAR.Text }));
            //    object ykpby = DBAdo.ExecuteScalarSql(string.Format("select sum(rmb) from afkxx where 1=1 and hth='{0}' and type='{1}' and year([date])='{2}' and month([date])='{3}'", new string[] { r["合同号"].ToString(), (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票"), this.YEAR.Text, this.MONTH.Text }));
            //    object ykpbn = DBAdo.ExecuteScalarSql(string.Format("select sum(rmb) from afkxx where 1=1 and hth='{0}' and type='{1}' and year([date])='{2}' and month([date])<='{3}'", new string[] { r["合同号"].ToString(), (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票"), this.YEAR.Text, this.MONTH.Text }));

            //    r["合同前"] = decimal.Parse(htq == null || htq.ToString() == "" ? "0" : htq.ToString());
            //    r["合同本月"] = decimal.Parse(htby == null || htby.ToString() == "" ? "0" : htby.ToString());
            //    r["合同本年"] = decimal.Parse(htbn == null || htbn.ToString() == "" ? "0" : htbn.ToString());
            //    r["收款前"] = decimal.Parse(skq == null || skq.ToString() == "" ? "0" : skq.ToString());
            //    r["收款本月"] = decimal.Parse(skby == null || skby.ToString() == "" ? "0" : skby.ToString());
            //    r["收款本年"] = decimal.Parse(skbn == null || skbn.ToString() == "" ? "0" : skbn.ToString());
            //    r["已开票前"] = decimal.Parse(ykpq == null || ykpq.ToString() == "" ? "0" : ykpq.ToString());
            //    r["已开票本月"] = decimal.Parse(ykpby == null || ykpby.ToString() == "" ? "0" : ykpby.ToString());
            //    r["已开票本年"] = decimal.Parse(ykpbn == null || ykpbn.ToString() == "" ? "0" : ykpbn.ToString());

            //}
            this.progressBar1.Visible = false;
            //this.button1.Enabled = true;
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.Format("合同总累计<>收款总累计 or 合同总累计<>已开票总累计 or 年 >='{0}'", this.YEAR.Text);
            DataTable ds = dv.ToTable();
            ReportDataSource rds = new ReportDataSource("Contract1DataSet_R合同汇总", ds);
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            ReportParameter rp1 = new ReportParameter("Report_DW", this.HDW.Text);
            ReportParameter rp2 = new ReportParameter("Report_YEAR", this.YEAR.Text);
            ReportParameter rp3 = new ReportParameter("Report_MONTH", this.MONTH.Text);
            ReportParameter rp4 = new ReportParameter("Report_LX", ClassCustom.codeSub(this.HLX.Text));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4 });
            this.reportViewer1.RefreshReport();
        }

        private void getReport_签订明细()
        {
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report签订明细.rdlc";
            this.reportViewer1.LocalReport.DataSources.Clear();
            string containTwo = string.Empty;
            if (this.checkBox1.Checked)
            {
                containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
            }
            else
            {
                containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
            }

            string s1 = string.Format(" CASE MONTH(co.hdate) WHEN {0} THEN co.hjsje ELSE 0.00 END ", this.MONTH.Text);
            // string s2 = string.Format(" CASE MONTH(co.hdate) WHEN {0} THEN 0.00 ELSE co.hjsje END ", this.MONTH.Text);
            //this.button1.Enabled = false;
            //string sql = string.Format("select case substring(co.hkh,1,2) when '01' then '内部' when '02' then '外部' when '05' then '北方重工' else '鼓风' end as 部门 ,co.HCODE 合同号,cl.cname 客户,co.HJSJE 合同结算金额1,0.00 合同结算金额2,co.hdate 签订时间,HJHDATE 交货时间,zbfs 合同中标方式 from acontract co,ACLIENTS cl where 1=1 and hdw='{0}' and hywy like '{1}%' and hlx like '{2}%' and co.hkh=cl.ccode and year(co.hdate)='{3}' and month(co.hdate)='{4}' union select case substring(co.hkh,1,2) when '01' then '内部' when '02' then '外部' when '05' then '北方重工' else '鼓风' end as 部门 ,co.HCODE 合同号,cl.cname 客户,0.00 合同结算金额1,co.HJSJE 合同结算金额2,co.hdate 签订时间,HJHDATE 交货时间,zbfs 合同中标方式 from acontract co,ACLIENTS cl where 1=1 and hdw='{0}' and hywy like '{1}%' and hlx like '{2}%' and co.hkh=cl.ccode and year(co.hdate)='{3}' and month(co.hdate)<='{4}'", new string[] { ClassCustom.codeSub(this.HDW.Text), ClassCustom.codeSub(this.HBM.Text), ClassCustom.codeSub(this.HLX.Text), this.YEAR.Text, this.MONTH.Text });
            string sql = string.Format(@"select DBO.GetCustomerCate(co.hkh) as 部门 ,co.HCODE 合同号,cl.cname 客户," + s1
                + @" 合同结算金额1,co.hjsje 合同结算金额2,co.hdate 签订时间,HJHDATE 交货时间,SUBSTRING([BIDCODE],CHARINDEX('-',[BIDCODE] ,CHARINDEX('-',[BIDCODE] ,0)+1)+1,100) 合同中标方式 ,
(select top 1 gname from asp a1 where a1.hth=co.HCODE)as 商品名称,
(select sum(gsl*gdz) from asp a2 where a2.hth=co.HCODE)as 总重量,
(select sum(gsl) from asp a3 where a3.hth=co.HCODE)as 数量,
(select avg(dj2)  gname from asp a4 where a4.hth=co.HCODE)as 价格吨 
                    from acontract co,ACLIENTS cl where 1=1 
                    and  " + containTwo + " and hywy like '{1}%' and hlx like '{2}%' and co.hkh=cl.ccode and year(co.hdate)='{3}' and month(co.hdate)<='{4}' ",
                      new string[] { ClassCustom.codeSub(this.HDW.Text), ClassCustom.codeSub(this.HBM.Text), ClassCustom.codeSub(this.HLX.Text), this.YEAR.Text, this.MONTH.Text });

            DataTable dt = DBAdo.DtFillSql(sql);
            //dt.Columns.Add("商品名称", typeof(string));
            //dt.Columns.Add("总重量", typeof(decimal));
            //dt.Columns.Add("数量", typeof(decimal));
            //dt.Columns.Add("价格吨", typeof(decimal));
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = 100;

            //foreach (DataRow r in dt.Rows)
            //{
            //    this.progressBar1.Value++;
            //    object sp = DBAdo.ExecuteScalarSql("select top 1 gname from asp where hth='" + r["合同号"] + "'");
            //    object zzl = DBAdo.ExecuteScalarSql("select sum(gsl*gdz) from asp where hth='" + r["合同号"] + "'");
            //    object sl = DBAdo.ExecuteScalarSql("select sum(gsl) from asp where hth='" + r["合同号"] + "'");
            //    object dj = DBAdo.ExecuteScalarSql("select avg(dj2)  from asp where  hth='" + r["合同号"] + "'");

            //    r["商品名称"] = sp.ToString();
            //    r["总重量"] = decimal.Parse(zzl == null || zzl.ToString() == "" ? "0" : zzl.ToString());
            //    r["数量"] = decimal.Parse(sl == null || sl.ToString() == "" ? "0" : sl.ToString());
            //    r["价格吨"] = decimal.Parse(dj == null || dj.ToString() == "" ? "0" : dj.ToString());
            //}
            //this.button1.Enabled=true;
            ReportDataSource rds = new ReportDataSource("Contract1DataSet_R签订明细", dt);
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            ReportParameter rp1 = new ReportParameter("Report_DW", this.HDW.Text);
            ReportParameter rp2 = new ReportParameter("Report_BM", this.HBM.Text);
            ReportParameter rp3 = new ReportParameter("Report_YEAR", this.YEAR.Text);
            ReportParameter rp4 = new ReportParameter("Report_MONTH", this.MONTH.Text);
            ReportParameter rp5 = new ReportParameter("Report_LX", ClassCustom.codeSub(this.HLX.Text));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5 });
            this.reportViewer1.RefreshReport();
        }

        /// <summary>
        /// 未知表
        /// </summary>
        private void getReport_合同总览表_按类型()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report合同总览表_按类型.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
                }
                else
                {
                    containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
                }
                string sql = string.Format("SELECT 0.00 J1,0.00 J2,0.00 J3,0.00 J4,合同号,客户名,结算金额,HKH 客户码,0年,DBO.GetCustomerCate(HKH) as 客户类型 FROM VCONTRACTS WHERE 1=1 AND HLX LIKE '{3}%' AND " + containTwo + "  AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                   new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });
                DataTable dt = DBAdo.DtFillSql(sql);
                dt.Columns.Add("A1", typeof(decimal));
                dt.Columns.Add("A2", typeof(decimal));
                dt.Columns.Add("A3", typeof(decimal));
                dt.Columns.Add("A4", typeof(decimal), "A1+A3");
                dt.Columns.Add("A5", typeof(decimal), "结算金额-A4");
                dt.Columns.Add("A6", typeof(decimal), "A5/结算金额");
                dt.Columns.Add("B1", typeof(decimal));
                dt.Columns.Add("B2", typeof(decimal));
                dt.Columns.Add("B3", typeof(decimal));
                dt.Columns.Add("B4", typeof(decimal), "B1+B3");
                dt.Columns.Add("B5", typeof(decimal), "结算金额-B4");
                dt.Columns.Add("B6", typeof(decimal), "B5/结算金额");
                dt.Columns.Add("C", typeof(decimal), "B4-A4");

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                this.progressBar1.Visible = true;

                foreach (DataRow r in dt.Rows)
                {
                    this.progressBar1.Value++;
                    Application.DoEvents();
                    r["年"] = DBAdo.ExecuteScalarSql(string.Format("SELECT YEAR(MAX(DATE)) FROM AFKXX WHERE 1=1 AND HTH = '{0}'", r["合同号"].ToString()));
                    decimal a1 = 0;
                    decimal a2 = 0;
                    decimal a3 = 0;
                    decimal b1 = 0;
                    decimal b2 = 0;
                    decimal b3 = 0;
                    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                    object o3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));

                    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                    object oo3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));

                    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());
                    a3 = decimal.Parse(o3 == null || o3.ToString() == "" ? "0" : o3.ToString());

                    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());
                    b3 = decimal.Parse(oo3 == null || oo3.ToString() == "" ? "0" : oo3.ToString());

                    r["A1"] = a1;
                    r["A2"] = a2;
                    r["A3"] = a3;
                    r["B1"] = b1;
                    r["B2"] = b2;
                    r["B3"] = b3;

                }



                DataView dv = dt.DefaultView;

                dv.RowFilter = string.Format("(C<>0 or B4 <> 结算金额 or A4 <> 结算金额) or 年 >= {0}", this.YEAR.Text);
                DataTable souce = dv.ToTable();
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R合同收付款明细表", souce);
                //this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                //ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                //ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", ClassCustom.codeSub1(this.HDW.Text));
                //ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                //ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                //ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }

        }
        /// <summary>
        /// 集团表
        /// </summary>
        private void getReport_JT_合同类型汇总表()
        {

            #region old
            //this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_合同类型汇总表.rdlc";
            //this.reportViewer1.LocalReport.DataSources.Clear();
            //string lx = (ClassCustom.codeSub(this.HLX.Text) == "02" ? " and (type = '回款' or type = '销项发票') " : " and (type = '付款' or type = '进项发票') ");
            //string sql = string.Format("select year(date) 年 ,month(date) 月,f.rmb 金额,hth 合同号,type 进度类型,case type when '回款' then '回款' when '付款' then '已付货款' when '进项发票' then '已开发票' when '销项发票' then '已收发票' end 类别,h.hdw 公司码,HKH 客户码,"
            //                        + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'以前年度' 时间类型,'' 公司名"
            //                        + " from afkxx f,aclients c,acontract h "
            //                        + " where f.hth=h.hcode and h.hkh = c.ccode"
            //                        + " AND h.HLX LIKE '{4}%'  AND (YEAR(date)<{5}) AND (YEAR(h.hdate)<{5} OR (YEAR(h.hdate)={5} AND MONTH(h.hdate)<= {6} ))" + lx,
            //                        new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //sql += string.Format("union select year(date) 年 ,month(date) 月,f.rmb 金额,hth 合同号,type 进度类型,case type when '回款' then '回款' when '付款' then '已付货款' when '进项发票' then '已开发票' when '销项发票' then '已收发票' end 类别,h.hdw 公司码,HKH 客户码,"
            //          + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'本月' 时间类型,'' 公司名"
            //          + " from afkxx f,aclients c,acontract h "
            //          + " where f.hth=h.hcode and h.hkh = c.ccode"
            //          + " AND h.HLX LIKE '{4}%'  AND ((YEAR(date)={5} AND MONTH(date)<= {6} )) AND (YEAR(h.hdate)<{5} OR (YEAR(h.hdate)={5} AND MONTH(h.hdate)<= {6} ))" + lx,
            //          new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //sql += string.Format("union select year(date) 年 ,month(date) 月,f.rmb 金额,hth 合同号,type 进度类型,case type when '回款' then '回款' when '付款' then '已付货款' when '进项发票' then '已开发票' when '销项发票' then '已收发票' end 类别,h.hdw 公司码,HKH 客户码,"
            //          + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'本年' 时间类型,'' 公司名"
            //          + " from afkxx f,aclients c,acontract h "
            //          + " where f.hth=h.hcode and h.hkh = c.ccode"
            //          + " AND h.HLX LIKE '{4}%'  AND ((YEAR(date)={5} AND MONTH(date)<= {6} )) AND (YEAR(h.hdate)<{5} OR (YEAR(h.hdate)={5} AND MONTH(h.hdate)<= {6} ))" + lx,
            //          new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //sql += string.Format("union select year(date) 年 ,month(date) 月,f.rmb 金额,hth 合同号,type 进度类型,case type when '回款' then '回款' when '付款' then '已付货款' when '进项发票' then '已开发票' when '销项发票' then '已收发票' end 类别,h.hdw 公司码,HKH 客户码,"
            //          + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'总累计' 时间类型,'' 公司名"
            //          + " from afkxx f,aclients c,acontract h "
            //          + " where f.hth=h.hcode and h.hkh = c.ccode"
            //          + " AND h.HLX LIKE '{4}%'  AND (YEAR(date)<{5} OR (YEAR(date)={5} AND MONTH(date)<= {6} )) AND (YEAR(h.hdate)<{5} OR (YEAR(h.hdate)={5} AND MONTH(h.hdate)<= {6} ))" + lx,
            //          new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //sql += string.Format("union select year(签定日期) 年 ,month(签定日期) 月,结算金额 金额,合同号,'' 进度类型,'结算金额' 类别,hdw 公司码,HKH 客户码,"
            //          + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'总累计' 时间类型,'' 公司名"
            //    //+ " from afkxx f,aclients c,acontract h "
            //          + " FROM VCONTRACTS "
            //          + " where 1=1"
            //          + " AND HLX LIKE '{4}%'  AND (YEAR(签定日期)<{5} OR (YEAR(签定日期)={5} AND MONTH(签定日期)<= {6} ))",
            //          new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //sql += string.Format("union select year(签定日期) 年 ,month(签定日期) 月,结算金额 金额,合同号,'' 进度类型,'结算金额' 类别,hdw 公司码,HKH 客户码,"
            //          + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'本年' 时间类型,'' 公司名"
            //    //+ " from afkxx f,aclients c,acontract h "
            //          + " FROM VCONTRACTS "
            //          + " where 1=1"
            //          + " AND HLX LIKE '{4}%'  AND ( (YEAR(签定日期)={5} AND MONTH(签定日期)<= {6} ))",
            //          new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //sql += string.Format("union select year(签定日期) 年 ,month(签定日期) 月,结算金额 金额,合同号,'' 进度类型,'结算金额' 类别,hdw 公司码,HKH 客户码,"
            //          + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'本月' 时间类型,'' 公司名"
            //    //+ " from afkxx f,aclients c,acontract h "
            //          + " FROM VCONTRACTS "
            //          + " where 1=1"
            //          + " AND HLX LIKE '{4}%'  AND ((YEAR(签定日期)={5} AND MONTH(签定日期)= {6} ))",
            //          new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //sql += string.Format("union select year(签定日期) 年 ,month(签定日期) 月,结算金额 金额,合同号,'' 进度类型,'结算金额' 类别,hdw 公司码,HKH 客户码,"
            //          + "CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END 客户类型,'以前年度' 时间类型,'' 公司名"
            //    //+ " from afkxx f,aclients c,acontract h "
            //          + " FROM VCONTRACTS "
            //          + " where 1=1"
            //          + " AND HLX LIKE '{4}%'  AND (YEAR(签定日期)<{5})",
            //          new object[]{
            //                        ClassConstant.NB,ClassConstant.WB,ClassConstant.NHI,ClassConstant.ZJ,ClassCustom.codeSub( HLX1.Text),YEAR.Text,MONTH.Text
            //                        });
            //DataTable dt = DBAdo.DtFillSql(sql);

            //this.progressBar1.Minimum = 0;
            //this.progressBar1.Maximum = dt.Rows.Count;
            ////DataTable hdws=DBAdo.DtFillSql(string.Format("select ccode,cname from aclients where 1=1 and ccode LIKE '01__'"));
            ////foreach (DataRow r in dt.Rows)
            ////{
            ////    this.progressBar1.Value++;
            ////    Application.DoEvents();
            ////    //r["公司名"] = 
            ////    //object sp = DBAdo.ExecuteScalarSql("select top 1 gname from asp where hth='" + r["合同号"] + "'");
            ////    //object zzl = DBAdo.ExecuteScalarSql("select sum(gsl*gdz) from asp where hth='" + r["合同号"] + "'");
            ////    //object sl = DBAdo.ExecuteScalarSql("select sum(gsl) from asp where hth='" + r["合同号"] + "'");
            ////    //object dj = DBAdo.ExecuteScalarSql("select avg(dj2)  from asp where  hth='" + r["合同号"] + "'");

            ////    //r["商品名称"] = sp.ToString();
            ////    //r["总重量"] = decimal.Parse(zzl == null || zzl.ToString() == "" ? "0" : zzl.ToString());
            ////    //r["数量"] = decimal.Parse(sl == null || sl.ToString() == "" ? "0" : sl.ToString());
            ////    //r["价格吨"] = decimal.Parse(dj == null || dj.ToString() == "" ? "0" : dj.ToString());

            ////}
            //ReportDataSource rds = new ReportDataSource("Contract1DataSet_Table_jt", dt);
            //this.reportViewer1.LocalReport.DataSources.Add(rds);
            ////ReportParameter rp1 = new ReportParameter("Report_DW", this.HDW.Text);
            ////ReportParameter rp2 = new ReportParameter("Report_BM", this.HBM.Text);
            ////ReportParameter rp3 = new ReportParameter("Report_YEAR", this.YEAR.Text);
            ////ReportParameter rp4 = new ReportParameter("Report_MONTH", this.MONTH.Text);
            ////ReportParameter rp5 = new ReportParameter("Report_LX", ClassCustom.codeSub(this.HLX.Text));
            ////this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5 });
            //this.reportViewer1.RefreshReport();
            #endregion
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_合同类型汇总表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = " 1= 1 ";
                }
                else
                {
                    containTwo = string.Format(" HDW LIKE '{0}%' ", ClassConstant.AccountingBook);
                }

                //string sql = string.Format("SELECT 合同号,客户名,结算金额,HKH 客户码,0 年,CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{8}' THEN '在建工程' ELSE '鼓风' END as 客户类型,hdw FROM VCONTRACTS WHERE 1=1 AND HLX LIKE '{3}%' AND HDW LIKE '%' AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                //   new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX1.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });
                string sql = string.Format(@"SELECT 合同号,客户名,结算金额,HKH 客户码,(SELECT  YEAR(MAX(DATE)) FROM AFKXX F0 WHERE H.合同号=F0.HTH) as 年,DBO.GetCustomerCate(HKH) as 客户类型 ,b.bname as hdw 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F1 WHERE  F1.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)<{6}),0.00) AS A1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F2 WHERE  F2.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS A2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F3 WHERE  F3.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS A3 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F4 WHERE  F4.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)<{6}),0.00) AS B1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F5 WHERE  F5.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS B2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F6 WHERE  F6.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS B3 
                                            FROM VCONTRACTS H inner join bcode b on h.hdw=b.bcode WHERE  " + containTwo + "  AND HLX LIKE '{3}%'   AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                        new object[] {
                         ClassConstant.NB, 
                         ClassConstant.WB, 
                         ClassConstant.NHI, 
                         ClassCustom.codeSub(HLX.Text), 
                         ClassCustom.codeSub(HDW.Text), 
                         ClassCustom.codeSub(HBM.Text),
                         YEAR.Text, MONTH.Text, 
                         ClassConstant.ZJ ,
                        ClassConstant.AccountingBook});
                DataTable dt = DBAdo.DtFillSql(sql);
                //dt.Columns.Add("A1", typeof(decimal));
                //dt.Columns.Add("A2", typeof(decimal));
                //dt.Columns.Add("A3", typeof(decimal));
                dt.Columns.Add("A4", typeof(decimal), "A1+A3");
                dt.Columns.Add("A5", typeof(decimal), "结算金额-A4");
                dt.Columns.Add("A6", typeof(decimal), "A5/结算金额");
                //dt.Columns.Add("B1", typeof(decimal));
                //dt.Columns.Add("B2", typeof(decimal));
                //dt.Columns.Add("B3", typeof(decimal));
                dt.Columns.Add("B4", typeof(decimal), "B1+B3");
                dt.Columns.Add("B5", typeof(decimal), "结算金额-B4");
                dt.Columns.Add("B6", typeof(decimal), "B5/结算金额");
                dt.Columns.Add("C", typeof(decimal), "B4-A4");

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Visible = true;

                //foreach (DataRow r in dt.Rows)
                //{
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    r["年"] = DBAdo.ExecuteScalarSql(string.Format("SELECT YEAR(MAX(DATE)) FROM AFKXX WHERE 1=1 AND HTH = '{0}'", r["合同号"].ToString()));
                //    decimal a1 = 0;
                //    decimal a2 = 0;
                //    decimal a3 = 0;
                //    decimal b1 = 0;
                //    decimal b2 = 0;
                //    decimal b3 = 0;
                //    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                //    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                //    object o3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));

                //    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                //    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                //    object oo3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));

                //    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                //    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());
                //    a3 = decimal.Parse(o3 == null || o3.ToString() == "" ? "0" : o3.ToString());

                //    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                //    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());
                //    b3 = decimal.Parse(oo3 == null || oo3.ToString() == "" ? "0" : oo3.ToString());

                //    r["A1"] = a1;
                //    r["A2"] = a2;
                //    r["A3"] = a3;
                //    r["B1"] = b1;
                //    r["B2"] = b2;
                //    r["B3"] = b3;
                //    r["hdw"] = DBAdo.ExecuteScalarSql(string.Format("select cname from aclients where ccode ='{0}'", r["hdw"].ToString())).ToString();

                //}


                DataView dv = dt.DefaultView;

                dv.RowFilter = string.Format("(C<>0 or B4 <> 结算金额 or A4 <> 结算金额) or 年 >= {0}", this.YEAR.Text);
                DataTable souce = dv.ToTable();
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R合同收付款明细表", souce);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                string reportName = "";
                //各公司产品销售合同汇总表
                //各公司产品销售合同汇总表（整体外委销售）
                //各公司材料采购合同汇总表
                //各公司外协合同汇总表（整体外委合同）
                //各公司外协合同汇总表（外协合同）
                if (ClassCustom.codeSub(HLX1.Text).Substring(0, 2) == "02")
                {
                    if (ClassCustom.codeSub(this.HLX1.Text) == "0202")
                    {
                        reportName = "各公司产品销售合同汇总表（整体外委销售）";
                    }
                    else
                    {
                        reportName = "各公司产品销售合同汇总表";
                    }
                }
                else if (ClassCustom.codeSub(HLX1.Text).Substring(0, 2) == "03")
                {
                    if (ClassCustom.codeSub(this.HLX1.Text) == "0301")
                    {
                        reportName = "各公司外协合同汇总表（整体外委合同）";
                    }
                    else
                    {
                        reportName = "各公司外协合同汇总表（外协合同）";
                    }
                }
                else
                {
                    reportName = "各公司材料采购合同汇总表";
                }

                ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", "沈阳铸锻工业有限公司");
                ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                ReportParameter rp6 = new ReportParameter("reportName", reportName);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }

        }

        private void getReport_JT_各单位收付款汇总表()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_各单位收付款汇总表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                //string sql1 = string.Format("select HDW 公司码,SUBSTRING(CCODE,1,2) 客户类型码, TYPE 进度类型 ,year(exchangedate) 年,month(exchangedate) 月, SUM(CASH) 现汇,SUM(NOTE) 票据,SUM(MZ) 抹账 from acash "
                //     + "where 1=1"
                //     + "GROUP BY HDW,SUBSTRING(CCODE,1,2) ,TYPE,year(exchangedate),month(exchangedate) "
                //     + "ORDER BY  HDW ,SUBSTRING(CCODE,1,2)  ,TYPE,year(exchangedate),month(exchangedate) "
                //     + "");

                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = " 1= 1 ";
                }
                else
                {
                    containTwo = string.Format(" A.HDW LIKE '{0}%' ", ClassConstant.AccountingBook);
                }
                string tj = (this.comboBox1.Text == "回款" ? " AND type = '回款' " : " AND type = '付款'");
                string s = string.Format("CASE substring(ccode,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END as 客户类型", new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassConstant.ZJ });
                string sql = "";
                string s1 = " SUM(CASH) 现汇本月,0.00 现汇本年,SUM(NOTE) 票据本月,0.00  票据本年,SUM(MZ) 抹账本月 ,0.00  抹账本年";
                string s2 = " 0.00  现汇本月,SUM(CASH) 现汇本年,0.00  票据本月,SUM(NOTE) 票据本年,0.00  抹账本月 ,SUM(MZ) 抹账本年";
                sql += string.Format("select b.bname 公司,{0}, TYPE 进度类型,{1} from acash a inner join bcode b on a.hdw=b.bcode where  " + containTwo + "  AND {2} GROUP BY b.bname,SUBSTRING(CCODE,1,2) ,TYPE "
                    , s, s1, string.Format(" year(exchangedate) = {0} and  month(exchangedate) = {1} " + tj, YEAR.Text, MONTH.Text));
                sql += string.Format(" union select b.bname 公司,{0}, TYPE 进度类型,{1} from acash a inner join bcode b on a.hdw=b.bcode where   " + containTwo + "   AND {2} GROUP BY b.bname,SUBSTRING(CCODE,1,2) ,TYPE "
                    , s, s2, string.Format(" year(exchangedate) = {0} and  month(exchangedate) <= {1} " + tj, YEAR.Text, MONTH.Text));
                DataTable dt = DBAdo.DtFillSql(sql);

                //dt.Columns.Add("客户类型", typeof(string));
                //dt.Columns.Add("现汇本月", typeof(decimal));
                //dt.Columns.Add("现汇本年", typeof(decimal));
                //dt.Columns.Add("票据本月", typeof(decimal));
                //dt.Columns.Add("票据本年", typeof(decimal));
                //dt.Columns.Add("抹账本月", typeof(decimal));
                //dt.Columns.Add("抹账本年", typeof(decimal));
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                //foreach (DataRow r in dt.Rows)
                //{
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    r["公司"] = DBAdo.ExecuteScalarSql(string.Format("select cname from aclients where ccode ='{0}'", r["公司"].ToString())).ToString();


                //}
                //各公司产品销售合同明细表
                //各公司产品采购合同明细表
                string reportName = "";
                if (ClassCustom.codeSub(this.comboBox1.Text) == "回款")
                {
                    reportName = "各公司产品销售合同回款明细表";
                }
                else
                {
                    reportName = "各公司材料采购合同付款明细表";
                }

                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_DataTable1", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("isFkOrHk", this.comboBox1.Text == "付款" ? true.ToString() : false.ToString());
                ReportParameter rp2 = new ReportParameter("year", this.YEAR.Text);
                ReportParameter rp3 = new ReportParameter("month", this.MONTH.Text);
                ReportParameter rp4 = new ReportParameter("reportName", reportName);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4 });

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
        }

        private void getReport_JT_各单位货款同期对比表()
        {

            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_各单位货款同期对比表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = " 1= 1 ";
                }
                else
                {
                    containTwo = string.Format(" A.HDW LIKE '{0}%' ", ClassConstant.AccountingBook);
                }
                string tj = (this.comboBox1.Text == "回款" ? " AND type = '回款' " : " AND type = '付款'");
                string s = string.Format("CASE substring(ccode,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END as 客户类型", new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassConstant.ZJ });
                string sql = "";
                string s1 = " SUM(CASH+note+mz) 本年本月,0.00 本年累计,0.00 同期本月,0.00  同期本年 ";
                string s2 = " 0.00 本年本月,SUM(CASH+note+mz) 本年累计,0.00 同期本月,0.00  同期本年 ";
                string s3 = " 0.00 本年本月,0.00 本年累计,SUM(CASH+note+mz) 同期本月,0.00  同期本年 ";
                string s4 = " 0.00 本年本月,0.00 本年累计,0.00 同期本月,SUM(CASH+note+mz)  同期本年 ";
                sql += string.Format("select b.bname 公司,{0},{1} from acash  a inner join bcode b on a.hdw=b.bcode  where  " + containTwo + "   AND {2} GROUP BY b.bname,SUBSTRING(CCODE,1,2)  "
                    , s, s1, string.Format(" year(exchangedate) = {0} and  month(exchangedate) = {1} " + tj, YEAR.Text, MONTH.Text));
                sql += string.Format(" union select b.bname 公司,{0},{1} from acash a inner join bcode b on a.hdw=b.bcode  where  " + containTwo + "  AND {2} GROUP BY b.bname,SUBSTRING(CCODE,1,2) "
                    , s, s2, string.Format(" year(exchangedate) = {0} and  month(exchangedate) <= {1} " + tj, YEAR.Text, MONTH.Text));
                sql += string.Format(" union select b.bname 公司,{0},{1} from acash a inner join bcode b on a.hdw=b.bcode  where  " + containTwo + "    AND {2} GROUP BY b.bname,SUBSTRING(CCODE,1,2) "
                    , s, s3, string.Format(" year(exchangedate) = {0} and  month(exchangedate) <= {1} " + tj, (int.Parse(YEAR.Text) - 1).ToString(), MONTH.Text));
                sql += string.Format(" union select b.bname 公司,{0},{1} from acash  a inner join bcode b on a.hdw=b.bcode where " + containTwo + "    AND {2} GROUP BY b.bname,SUBSTRING(CCODE,1,2)  "
                    , s, s4, string.Format(" year(exchangedate) = {0} and  month(exchangedate) <= {1} " + tj, (int.Parse(YEAR.Text) - 1).ToString(), MONTH.Text));
                DataTable dt = DBAdo.DtFillSql(sql);

                //dt.Columns.Add("客户类型", typeof(string));
                //dt.Columns.Add("现汇本月", typeof(decimal));
                //dt.Columns.Add("现汇本年", typeof(decimal));
                //dt.Columns.Add("票据本月", typeof(decimal));
                //dt.Columns.Add("票据本年", typeof(decimal));
                //dt.Columns.Add("抹账本月", typeof(decimal));
                //dt.Columns.Add("抹账本年", typeof(decimal));
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                //foreach (DataRow r in dt.Rows)
                //{
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    r["公司"] = DBAdo.ExecuteScalarSql(string.Format("select cname from aclients where ccode ='{0}'", r["公司"].ToString())).ToString();


                //}


                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_DataTable11", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);

                //各公司本年销售合同回款汇总表
                //
                string reportName = "";
                if (ClassCustom.codeSub(this.comboBox1.Text) == "回款")
                {
                    reportName = "各公司本年销售合同回款汇总表";
                }
                else
                {
                    reportName = "各公司本年采购合同付款汇总表";
                }

                ReportParameter rp1 = new ReportParameter("isFkOrHk", this.comboBox1.Text == "付款" ? true.ToString() : false.ToString());
                ReportParameter rp2 = new ReportParameter("year", this.YEAR.Text);
                ReportParameter rp3 = new ReportParameter("month", this.MONTH.Text);
                ReportParameter rp4 = new ReportParameter("reportName", reportName);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4 });

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

        }

        private void getReport_JT_各单位签订合同情况表()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_各单位签订合同情况表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = " 1= 1 ";
                }
                else
                {
                    containTwo = string.Format(" HDW LIKE '{0}%' ", ClassConstant.AccountingBook);
                }
                string khtype = string.Format(" CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{3}' THEN '在建工程' ELSE '鼓风' END ", ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassConstant.ZJ);
                string sql = string.Format(" select H.HCODE 合同号,{0} as 客户类型,H.HDATE 签订日期,H.ZBFS 中标方式  ,HKH 客户码,b.bname 公司,HLX 合同类型,(select sum(zz) from asp where hth=h.hcode) 总重,(select avg(dj2)  from asp where  hth=h.hcode) 平均价格,hjsje 金额本月,0.00 金额本年 from ACONTRACT H inner join bcode b on h.hdw=b.bcode  where  " + containTwo + "  and hlx like '{3}%' and (year(h.hdate)= {1} and month(h.hdate)= {2}) ", new object[] { khtype, this.YEAR.Text, this.MONTH.Text, ClassCustom.codeSub(this.HLX1.Text), ClassConstant.AccountingBook });
                sql += string.Format(" union select H.HCODE 合同号,{0} as 客户类型,H.HDATE 签订日期,H.ZBFS 中标方式  ,HKH 客户码,b.bname 公司,HLX 合同类型,(select sum(zz) from asp where hth=h.hcode) 总重,(select avg(dj2)  from asp where  hth=h.hcode) 平均价格,0.00 金额本月,hjsje 金额本年 from ACONTRACT H inner join bcode b on h.hdw=b.bcode  where  " + containTwo + " and hlx like '{3}%' and (year(h.hdate)= {1} and month(h.hdate)<= {2}) ", new object[] { khtype, this.YEAR.Text, this.MONTH.Text, ClassCustom.codeSub(this.HLX1.Text), ClassConstant.AccountingBook });

                DataTable dt = DBAdo.DtFillSql(sql);
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R_JT_签订合同", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;

                //foreach (DataRow r in dt.Rows)
                //{
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    object zz = DBAdo.ExecuteScalarSql("select sum(zz) from asp where hth='" + r["合同号"] + "'");
                //    object pjjg = DBAdo.ExecuteScalarSql("select avg(dj2)  from asp where  hth='" + r["合同号"] + "'");


                //    r["总重"] = decimal.Parse(zz == null || zz.ToString() == "" ? "0" : zz.ToString());
                //    r["平均价格"] = decimal.Parse(pjjg == null || pjjg.ToString() == "" ? "0" : pjjg.ToString());
                //    r["公司"] = DBAdo.ExecuteScalarSql(string.Format("select cname from aclients where ccode ='{0}'", r["公司"].ToString())).ToString();
                //}

                ReportParameter rp1 = new ReportParameter("YEAR", this.YEAR.Text);
                ReportParameter rp2 = new ReportParameter("MONTH", this.MONTH.Text);
                ReportParameter rp3 = new ReportParameter("HLX", ClassCustom.codeSub1(this.HLX.Text));
                string reportName = "";
                if (ClassCustom.codeSub(this.HLX.Text).Substring(0, 2) == "02")
                {
                    if (ClassCustom.codeSub(this.HLX1.Text) == "0201")
                    {
                        reportName = string.Format("各公司本年签订产品销售合同汇总表");
                    }
                    else if (ClassCustom.codeSub(this.HLX1.Text) == "0202")
                    {
                        reportName = string.Format("各公司本年签订产品销售合同汇总表（整体外委销售）");
                    }
                    else
                    {
                        reportName = string.Format("各公司本年签订产品销售合同汇总表");
                    }
                }
                else if (ClassCustom.codeSub(this.HLX.Text).Substring(0, 2) == "03")
                {
                    if (ClassCustom.codeSub(this.HLX1.Text) == "0301")
                    {
                        //各公司本年签订产品销售合同汇总表	销售
                        //各公司本年签订产品销售合同汇总表（整体外委销售）	
                        //各公司本年签订采购合同汇总表	采购
                        //各公司本年签订外协合同汇总表（整体外委合同）	外协
                        //各公司本年签订外协合同汇总表（外协合同）	
                        reportName = string.Format("各公司本年签订外协合同汇总表（整体外委合同");
                    }
                    else
                    {
                        reportName = string.Format("各公司本年签订外协合同汇总表（外协合同）");
                    }
                }
                else
                {
                    reportName = string.Format("各公司本年签订采购合同汇总表");
                }

                ReportParameter rp4 = new ReportParameter("reportName", reportName);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4 });

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
        }

        private void getReport_JT_集团合同类型汇总表()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_集团合同类型汇总表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = " 1= 1 ";
                }
                else
                {
                    containTwo = string.Format(" H.HDW LIKE '{0}%' ", ClassConstant.AccountingBook);
                }
                //string sql = string.Format("SELECT 合同号,客户名,结算金额,HKH 客户码,year(签定日期) 年,CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{8}' THEN '在建工程' ELSE '鼓风' END as 客户类型 FROM VCONTRACTS WHERE 1=1 AND HLX LIKE '{3}%' AND HDW = '{4}' AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                //   new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });
                //string sql = string.Format("SELECT 合同号,客户名,结算金额,HKH 客户码,0 年,CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{8}' THEN '在建工程' ELSE '鼓风' END as 客户类型,HLX,HDW FROM VCONTRACTS WHERE 1=1 AND HLX LIKE '{3}%'  AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                //     new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });
                string sql = string.Format(@"SELECT 合同号,客户名,结算金额,HKH 客户码,(SELECT  YEAR(MAX(DATE)) FROM AFKXX F0 WHERE H.合同号=F0.HTH) as 年,DBO.GetCustomerCate(HKH) as 客户类型 ,b.bname as hdw ,h.合同类型 hlx

                                            ,case when YEAR(签定日期)<{6} THEN 结算金额 ELSE 0.00 END AS 合同前
                                            ,case when YEAR(签定日期)={6} AND  MONTH(签定日期)= {7} THEN 结算金额 ELSE 0.00 END AS 合同本月
                                            ,case when YEAR(签定日期)={6} AND  MONTH(签定日期)<= {7} THEN 结算金额 ELSE 0.00 END  AS 合同本年

                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F1 WHERE  F1.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)<{6}),0.00) AS A1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F2 WHERE  F2.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS A2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F3 WHERE  F3.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS A3 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F4 WHERE  F4.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)<{6}),0.00) AS B1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F5 WHERE  F5.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS B2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F6 WHERE  F6.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS B3 
                                            FROM VCONTRACTS H inner join bcode b on h.hdw=b.bcode WHERE  " + containTwo + "  AND HLX LIKE '{3}%'   AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
        new object[] {
                         ClassConstant.NB, 
                         ClassConstant.WB, 
                         ClassConstant.NHI, 
                         ClassCustom.codeSub(HLX.Text), 
                         ClassCustom.codeSub(HDW.Text), 
                         ClassCustom.codeSub(HBM.Text),
                         YEAR.Text, MONTH.Text, 
                         ClassConstant.ZJ ,
                        ClassConstant.AccountingBook});

                DataTable dt = DBAdo.DtFillSql(sql);
                //dt.Columns.Add("A1", typeof(decimal));
                //dt.Columns.Add("A2", typeof(decimal));
                //dt.Columns.Add("A3", typeof(decimal));
                dt.Columns.Add("A4", typeof(decimal), "A1+A3");
                dt.Columns.Add("A5", typeof(decimal), "结算金额-A4");
                dt.Columns.Add("A6", typeof(decimal), "A5/结算金额");
                //dt.Columns.Add("B1", typeof(decimal));
                //dt.Columns.Add("B2", typeof(decimal));
                //dt.Columns.Add("B3", typeof(decimal));
                dt.Columns.Add("B4", typeof(decimal), "B1+B3");
                dt.Columns.Add("B5", typeof(decimal), "结算金额-B4");
                dt.Columns.Add("B6", typeof(decimal), "B5/结算金额");
                dt.Columns.Add("C", typeof(decimal), "B4-A4");
                //dt.Columns.Add("合同前", typeof(decimal));
                //dt.Columns.Add("合同本月", typeof(decimal));
                //dt.Columns.Add("合同本年", typeof(decimal));
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                this.progressBar1.Visible = true;

                //foreach (DataRow r in dt.Rows)
                //{
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    r["年"] = DBAdo.ExecuteScalarSql(string.Format("SELECT YEAR(MAX(DATE)) FROM AFKXX WHERE 1=1 AND HTH = '{0}'", r[0].ToString()));
                //    decimal a1 = 0;
                //    decimal a2 = 0;
                //    decimal a3 = 0;
                //    decimal b1 = 0;
                //    decimal b2 = 0;
                //    decimal b3 = 0;
                //    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                //    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                //    object o3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));

                //    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                //    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                //    object oo3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));

                //    object htq = DBAdo.ExecuteScalarSql("select hjsje from acontract where 1=1 and hcode='" + r[0].ToString() + "' and hlx like '" + ClassCustom.codeSub(this.HLX.Text) + "%' and year(hdate)<'" + this.YEAR.Text + "' ");
                //    object htby = DBAdo.ExecuteScalarSql("select hjsje from acontract where 1=1 and hcode='" + r[0].ToString() + "' and hlx like '" + ClassCustom.codeSub(this.HLX.Text) + "%' and year(hdate)='" + this.YEAR.Text + "' and month(hdate)='" + this.MONTH.Text + "'");
                //    object htbn = DBAdo.ExecuteScalarSql("select hjsje from acontract where 1=1 and hcode='" + r[0].ToString() + "' and hlx like '" + ClassCustom.codeSub(this.HLX.Text) + "%' and year(hdate)='" + this.YEAR.Text + "' and month(hdate)<='" + this.MONTH.Text + "'");


                //    r["合同前"] = decimal.Parse(htq == null || htq.ToString() == "" ? "0" : htq.ToString());
                //    r["合同本月"] = decimal.Parse(htby == null || htby.ToString() == "" ? "0" : htby.ToString());
                //    r["合同本年"] = decimal.Parse(htbn == null || htbn.ToString() == "" ? "0" : htbn.ToString());

                //    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                //    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());
                //    a3 = decimal.Parse(o3 == null || o3.ToString() == "" ? "0" : o3.ToString());

                //    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                //    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());
                //    b3 = decimal.Parse(oo3 == null || oo3.ToString() == "" ? "0" : oo3.ToString());

                //    r["A1"] = a1;
                //    r["A2"] = a2;
                //    r["A3"] = a3;
                //    r["B1"] = b1;
                //    r["B2"] = b2;
                //    r["B3"] = b3;
                //    r["HDW"] = DBAdo.ExecuteScalarSql(string.Format("select cname from aclients where ccode ='{0}'", r["HDW"].ToString())).ToString();
                //    r["hlx"] = DBAdo.ExecuteScalarSql(string.Format("select lname from alx where lid ='{0}'", r["hlx"].ToString())).ToString();
                //}
                DataView dv = dt.DefaultView;

                dv.RowFilter = string.Format("(C<>0 or B4 <> 结算金额 or A4 <> 结算金额) or 年 >= {0}", this.YEAR.Text);
                DataTable souce = dv.ToTable();
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R合同收付款明细表", souce);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                //铸锻公司销售合同汇总表
                //铸锻公司采购合同汇总表
                //铸锻公司外协合同汇总表
                string reportName = "";
                if (ClassCustom.codeSub(this.HLX.Text).Substring(0, 2) == "02")
                {
                    reportName = "铸锻公司销售合同汇总表";
                }
                else if (ClassCustom.codeSub(this.HLX.Text).Substring(0, 2) == "03")
                {
                    reportName = "铸锻公司外协合同汇总表";
                }
                else
                {
                    reportName = "铸锻公司采购合同汇总表";
                }


                ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                //ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", ClassCustom.codeSub1(this.HLX.Text));
                ReportParameter rp6 = new ReportParameter("reportName", reportName);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp3, rp4, rp5, rp6 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }
        }

        private void getReprot_JT_毛利表()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_毛利表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
                }
                else
                {
                    containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
                }
                string ccodetype = string.Format(" CASE SUBSTRING(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' when  '{3}' then '在建工程' ELSE '鼓风'  END as 客户类别 ", new string[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassConstant.ZJ });
                string dateFilter = string.Format(" AND (YEAR(签定日期)<{0} OR (YEAR(签定日期)={0} AND MONTH(签定日期)<= {1} ", YEAR.Text, MONTH.Text);
                string sql = "";
                sql += " DECLARE @HLX NVARCHAR(10), @HDW NVARCHAR(10) ";
                sql += " SET @HLX='02__' SET @HDW='" + ClassCustom.codeSub(this.HDW.Text) + "' ";
                sql += @" SELECT *
,ISNULL(( SELECT SUM(hjsje/(CASE SUBSTRING(HKH,1,2) WHEN '01' THEN 1 ELSE 1.17 END)) hjsje FROM ACONTRACT WHERE HCODE IN (SELECT WXHTH FROM AWX WHERE XSHTH =T0.合同号)),0.00) 成本1
,ISNULL(( SELECT SUM(hjsje/(CASE SUBSTRING(HKH,1,2) WHEN '01' THEN 1 ELSE 1.17 END)) hjsje FROM ACONTRACT WHERE HCODE IN (SELECT WXHTH FROM AWX WHERE XSHTH =T0.合同号))*0.17,0.00) 税额1
,ISNULL(( SELECT SUM(HJSJE) FROM ACONTRACT WHERE HCODE IN (SELECT WXHTH FROM AWX WHERE XSHTH =T0.合同号)),0.00) 小计1 
FROM (SELECT 合同号," + ccodetype + ",客户名, CASE SUBSTRING(HKH,1,2) WHEN '01' THEN 结算金额 ELSE 结算金额/1.17 END  成本,结算金额- CASE SUBSTRING(HKH,1,2) WHEN '01' THEN 结算金额 ELSE 结算金额/1.17 END 税额,结算金额 小计,签定日期,SUBSTRING(HYWY,1,6) 部门  ";
                sql += " FROM VCONTRACTS WHERE HLX LIKE @HLX AND  " + containTwo + dateFilter + "))) T0,aywy t1 ";
                sql += " WHERE T0.部门=T1.YCODE " + dateFilter + "))";

                DataTable dt = DBAdo.DtFillSql(sql);


                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                this.progressBar1.Visible = true;

                //foreach (DataRow r in dt.Rows)
                //{

                //    //bool hs = DBAdo.ExecuteScalarSql(string.Format("SELECT {0}"));
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    decimal a1 = 0;
                //    string tname = string.Format("  (select hjsje/(CASE SUBSTRING(HKH,1,2) WHEN '01' then 1 ELSE 1.17 END) hjsje  from ACONTRACT WHERE HCODE IN (SELECT WXHTH FROM AWX WHERE XSHTH ='{0}'))", r["合同号"].ToString(), ccodetype);
                //    //原毛利表
                //    //object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(HJSJE)/(" + (r["客户类别"].ToString() == "内部" ? "1" : "1.17") + ") FROM {0} T0", tname));
                //    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(HJSJE) FROM {0} T0", tname));
                //    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                //    decimal a2 = 0;
                //    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(HJSJE) FROM ACONTRACT WHERE HCODE IN (SELECT WXHTH FROM AWX WHERE XSHTH ='{0}')", r["合同号"].ToString()));
                //    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());
                //    decimal a3 = 0;
                //    object o3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(HJSJE) FROM ACONTRACT WHERE HCODE IN (SELECT WXHTH FROM AWX WHERE XSHTH ='{0}')", r["合同号"].ToString()));
                //    a3 = decimal.Parse(o3 == null || o3.ToString() == "" ? "0" : o3.ToString());
                //    r["成本1"] = a1;
                //    r["税额1"] = a3 - a1;
                //    r["小计1"] = a3;
                //}

                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_DataTable3", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("HDW", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp2 = new ReportParameter("YEAR", this.YEAR.Text);
                ReportParameter rp3 = new ReportParameter("MONTH", this.MONTH.Text);

                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }
        }

        private void getReprot_毛利新1()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_ML_NEW1.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                /*                    */
                string state = "";
                if (this.comboBox2.Text == "" || this.comboBox2.Text == "全部")
                {

                }
                else
                {
                    state = " AND WXState='" + this.comboBox2.Text + "' ";
                }
                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
                }
                else
                {
                    containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
                }

                string s = @"SELECT [合同号1]
      ,[客户1]
      ,[签订日期1]
      ,[结算金额1]
      ,[已收货款]
      ,[已开发票]
      ,[合同号2]
      ,[客户2]
      ,[签订日期2]
      ,[结算金额2]
      ,[已付货款]
      ,[已收发票]
      ,[客户类别]
      ,[公司名]
      ,[毛利],[已收货款]-[已付货款] AS '已到毛利' ,WXState,
      CASE WHEN [毛利] -[已收货款]+[已付货款]<0 THEN 0 ELSE [毛利] -[已收货款]+[已付货款] END AS '未到毛利' ,
      CASE WHEN [毛利] -[已收货款]+[已付货款]<0 THEN [已收货款]-[已付货款] -[毛利]  ELSE 0 END AS '未付外协款' 
      FROM V_GrossProfit
        where  1=1  AND " + containTwo + "  ";
                if (this.radioButton1.Checked)
                {
                    s += "   AND 签订日期1 BETWEEN '{1}' AND '{2}'   ";
                }
                else
                {
                    s += "  AND [合同号1] IN (SELECT DISTINCT hth FROM AFKXX WHERE [DATE] BETWEEN  '{1}' AND '{2}' )";
                }
                string sql = string.Format(s + state, new string[] { this.HDW.Text==""?"%":ClassCustom.codeSub(this.HDW.Text), 
      this.dateTimePicker1.Value.ToShortDateString(),
      this.dateTimePicker2.Value.ToShortDateString()});
                DataTable dt = DBAdo.DtFillSql(sql);
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                this.progressBar1.Visible = true;

                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_ML_NEW", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("dept", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp2 = new ReportParameter("year", this.YEAR.Text);
                ReportParameter rp3 = new ReportParameter("month", this.MONTH.Text);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });
                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetReport_JT_铸锻公司全部采购外协合同汇总表()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_铸锻公司全部采购外协合同汇总表.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                //                string sql = string.Format(@"SELECT top 50 合同号,客户名,结算金额,HKH 客户码,0 年,CASE substring(HKH,1,2) WHEN '{0}' THEN '内部' WHEN '{1}' THEN '外部' WHEN '{2}' THEN '北方重工' WHEN '{8}' THEN '在建工程' ELSE '鼓风' END as 客户类型,合同类型 HLX,hdw 
                //                                        
                //,case when YEAR(签定日期)<{6} THEN 结算金额 ELSE 0.00 END AS 合同前
                //,case when YEAR(签定日期)={6} AND  MONTH(签定日期)= {7} THEN 结算金额 ELSE 0.00 END AS 合同本月
                //,case when YEAR(签定日期)={6} AND  MONTH(签定日期)<= {7} THEN 结算金额 ELSE 0.00 END  AS 合同本年

                //    FROM VCONTRACTS WHERE 1=1 AND (HLX LIKE '01%' or HLX LIKE '03%' )AND HDW LIKE '%' AND HYWY LIKE '{5}%' AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7}))",
                //                   new object[] { ClassConstant.NB, ClassConstant.WB, ClassConstant.NHI, ClassCustom.codeSub(HLX1.Text), ClassCustom.codeSub(HDW.Text), ClassCustom.codeSub(HBM.Text), YEAR.Text, MONTH.Text, ClassConstant.ZJ });


                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = " 1= 1 ";
                }
                else
                {
                    containTwo = string.Format(" H.HDW LIKE '{0}%' ", ClassConstant.AccountingBook);
                }


                string sql = string.Format(@"SELECT 合同号,客户名,结算金额,HKH 客户码,(SELECT  YEAR(MAX(DATE)) FROM AFKXX F0 WHERE H.合同号=F0.HTH) as 年,DBO.GetCustomerCate(HKH) as 客户类型 ,b.bname as hdw ,h.合同类型 hlx

                                            ,case when YEAR(签定日期)<{6} THEN 结算金额 ELSE 0.00 END AS 合同前
                                            ,case when YEAR(签定日期)={6} AND  MONTH(签定日期)= {7} THEN 结算金额 ELSE 0.00 END AS 合同本月
                                            ,case when YEAR(签定日期)={6} AND  MONTH(签定日期)<= {7} THEN 结算金额 ELSE 0.00 END  AS 合同本年

                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F1 WHERE  F1.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)<{6}),0.00) AS A1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F2 WHERE  F2.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS A2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F3 WHERE  F3.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '回款' ELSE '付款' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS A3 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F4 WHERE  F4.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)<{6}),0.00) AS B1 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F5 WHERE  F5.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)={7}),0.00) AS B2 
                                            ,ISNULL((SELECT SUM(RMB) FROM AFKXX F6 WHERE  F6.HTH =H.合同号 AND TYPE = CASE WHEN SUBSTRING(H.HLX,1,2)='02' THEN '销项发票' ELSE '进项发票' END  AND YEAR(DATE)={6} AND MONTH(DATE)<={7}),0.00) AS B3 
                                            FROM VCONTRACTS H inner join bcode b on h.hdw=b.bcode WHERE  " + containTwo + "  AND HLX LIKE '{3}%'   AND (YEAR(签定日期)<{6} OR (YEAR(签定日期)={6} AND MONTH(签定日期)<= {7})) and h.结算金额<>0",
                                                                                                                                                                                                                       new object[] {
                         ClassConstant.NB, 
                         ClassConstant.WB, 
                         ClassConstant.NHI, 
                         ClassCustom.codeSub(HLX.Text), 
                         ClassCustom.codeSub(HDW.Text), 
                         ClassCustom.codeSub(HBM.Text),
                         YEAR.Text, MONTH.Text, 
                         ClassConstant.ZJ ,
                        ClassConstant.AccountingBook});
                DataTable dt = DBAdo.DtFillSql(sql);
                //dt.Columns.Add("A1", typeof(decimal));
                //dt.Columns.Add("A2", typeof(decimal));
                //dt.Columns.Add("A3", typeof(decimal));
                dt.Columns.Add("A4", typeof(decimal), "A1+A3");
                dt.Columns.Add("A5", typeof(decimal), "结算金额-A4");
                dt.Columns.Add("A6", typeof(decimal), "A5/结算金额");
                //dt.Columns.Add("B1", typeof(decimal));
                //dt.Columns.Add("B2", typeof(decimal));
                //dt.Columns.Add("B3", typeof(decimal));
                dt.Columns.Add("B4", typeof(decimal), "B1+B3");
                dt.Columns.Add("B5", typeof(decimal), "结算金额-B4");
                dt.Columns.Add("B6", typeof(decimal), "B5/结算金额");
                dt.Columns.Add("C", typeof(decimal), "B4-A4");

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Visible = true;

                //foreach (DataRow r in dt.Rows)
                //{
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    r["年"] = DBAdo.ExecuteScalarSql(string.Format("SELECT YEAR(MAX(DATE)) FROM AFKXX WHERE 1=1 AND HTH = '{0}'", r["合同号"].ToString()));
                //    decimal a1 = 0;
                //    decimal a2 = 0;
                //    decimal a3 = 0;
                //    decimal b1 = 0;
                //    decimal b2 = 0;
                //    decimal b3 = 0;
                //    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                //    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));
                //    object o3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "回款" : "付款") }));

                //    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)<{1}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                //    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)={2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));
                //    object oo3 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND YEAR(DATE)={1} AND MONTH(DATE)<= {2}", new string[] { r[0].ToString(), YEAR.Text, MONTH.Text, (ClassCustom.codeSub(this.HLX.Text) == "02" ? "销项发票" : "进项发票") }));

                //    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                //    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());
                //    a3 = decimal.Parse(o3 == null || o3.ToString() == "" ? "0" : o3.ToString());

                //    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                //    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());
                //    b3 = decimal.Parse(oo3 == null || oo3.ToString() == "" ? "0" : oo3.ToString());

                //    r["A1"] = a1;
                //    r["A2"] = a2;
                //    r["A3"] = a3;
                //    r["B1"] = b1;
                //    r["B2"] = b2;
                //    r["B3"] = b3;
                //    r["hdw"] = DBAdo.ExecuteScalarSql(string.Format("select cname from aclients where ccode ='{0}'", r["hdw"].ToString())).ToString();

                //}


                DataView dv = dt.DefaultView;

                dv.RowFilter = string.Format("(C<>0 or B4 <> 结算金额 or A4 <> 结算金额) or 年 >= {0}", this.YEAR.Text);
                DataTable souce = dv.ToTable();
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_R合同收付款明细表", souce);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                string reportName = "";
                //各公司产品销售合同汇总表
                //各公司产品销售合同汇总表（整体外委销售）
                //各公司材料采购合同汇总表
                //各公司外协合同汇总表（整体外委合同）
                //各公司外协合同汇总表（外协合同）

                reportName = "铸锻公司全部采购外协合同汇总表";


                ReportParameter rp1 = new ReportParameter("Report_Parameter_isXS", ClassCustom.codeSub(this.HLX.Text) == "02" ? true.ToString() : false.ToString());
                //ReportParameter rp2 = new ReportParameter("Report_Parameter_HDW", "沈阳铸锻工业有限公司");
                ReportParameter rp3 = new ReportParameter("Report_Parameter_YEAR", this.YEAR.Text);
                ReportParameter rp4 = new ReportParameter("Report_Parameter_MONTH", this.MONTH.Text);
                ReportParameter rp5 = new ReportParameter("Report_Parameter_HLX", "11");
                ReportParameter rp6 = new ReportParameter("reportName", reportName);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp3, rp4, rp5, rp6 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }
        }

        private void getReprot_毛利新()
        {
            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_ML_NEW.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                /*                    */
                string state = "";
                if (this.comboBox2.Text == "" || this.comboBox2.Text == "全部")
                {

                }
                else
                {
                    state = " AND WXState='" + this.comboBox2.Text + "' ";
                }

                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = string.Format(" SUBSTRING(HDW,3,2)='{0}' ", ClassCustom.codeSub(HDW.Text).Substring(2));
                }
                else
                {
                    containTwo = string.Format(" HDW = '{0}' ", ClassCustom.codeSub(HDW.Text));
                }

                string sql = string.Format(@"SELECT [合同号1]
      ,[客户1]
      ,[签订日期1]
      ,[结算金额1]
      ,[已收货款]
      ,[已开发票]
      ,[合同号2]
      ,[客户2]
      ,[签订日期2]
      ,[结算金额2]
      ,[已付货款]
      ,[已收发票]
      ,[客户类别]
      ,[公司名]
      ,[毛利],[已收货款]-[已付货款] AS '已到毛利' ,WXState,
      CASE WHEN [毛利] -[已收货款]+[已付货款]<0 THEN 0 ELSE [毛利] -[已收货款]+[已付货款] END AS '未到毛利' ,
      CASE WHEN [毛利] -[已收货款]+[已付货款]<0 THEN [已收货款]-[已付货款] -[毛利]  ELSE 0 END AS '未付外协款' 
      FROM V_GrossProfit
        where  签订日期1 BETWEEN '{1}' AND '{2}' AND  " + containTwo + " " + state, ClassCustom.codeSub(this.HDW.Text), this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());
                DataTable dt = DBAdo.DtFillSql(sql);
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                this.progressBar1.Visible = true;
                //foreach (DataRow r in dt.Rows)
                //{
                //    //bool hs = DBAdo.ExecuteScalarSql(string.Format("SELECT {0}"));
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    decimal a1 = 0;
                //    decimal a2 = 0;

                //    decimal b1 = 0;
                //    decimal b2 = 0;

                //    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号1"].ToString(), YEAR.Text, MONTH.Text, "回款" }));
                //    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号1"].ToString(), YEAR.Text, MONTH.Text, "销项发票" }));

                //    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号2"].ToString(), YEAR.Text, MONTH.Text, "付款" }));
                //    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号2"].ToString(), YEAR.Text, MONTH.Text, "进项发票" }));

                //    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                //    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());

                //    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                //    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());

                //    r["已收货款"] = a1;
                //    r["已开发票"] = a2;
                //    r["已付货款"] = b1;
                //    r["已收发票"] = b2;
                //}
                ReportDataSource reportDataSource = new ReportDataSource("Contract1DataSet_ML_NEW", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("dept", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp2 = new ReportParameter("year", this.YEAR.Text);
                ReportParameter rp3 = new ReportParameter("month", this.MONTH.Text);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });
                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void getReprot_集团毛利新()
        {

            try
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.Reports.Report_JT_毛利.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                string containTwo = string.Empty;
                if (this.checkBox1.Checked)
                {
                    containTwo = " 1= 1 ";
                }
                else
                {
                    containTwo = string.Format(" HDW LIKE '{0}%' ", ClassConstant.AccountingBook);
                }
                /*                    */
                string state = "";
                if (this.comboBox2.Text == "" || this.comboBox2.Text == "全部")
                {

                }
                else
                {
                    state = " AND WXState='" + this.comboBox2.Text + "' ";
                }
                string s = @"SELECT [合同号1]
      ,[客户1]
      ,[签订日期1]
      ,[结算金额1]
      ,[已收货款]
      ,[已开发票]
      ,[合同号2]
      ,[客户2]
      ,[签订日期2]
      ,[结算金额2]
      ,[已付货款]
      ,[已收发票]
      ,[客户类别]
      ,[公司名]
      ,[毛利],[已收货款]-[已付货款] AS '已到毛利' ,WXState,
      CASE WHEN [毛利] -[已收货款]+[已付货款]<0 THEN 0 ELSE [毛利] -[已收货款]+[已付货款] END AS '未到毛利' ,
      CASE WHEN [毛利] -[已收货款]+[已付货款]<0 THEN [已收货款]-[已付货款] -[毛利]  ELSE 0 END AS '未付外协款' 
      FROM V_GrossProfit
        where  " + containTwo + "  ";
                if (this.radioButton1.Checked)
                {
                    s += "   AND 签订日期1 BETWEEN '{1}' AND '{2}'   ";
                }
                else
                {
                    s += "  AND [合同号1] IN (SELECT DISTINCT hth FROM AFKXX WHERE [DATE] BETWEEN  '{1}' AND '{2}' )";
                }
                string sql = string.Format(s + state, new string[] { this.HDW.Text==""?"%":ClassCustom.codeSub(this.HDW.Text), 
      this.dateTimePicker1.Value.ToShortDateString(),
      this.dateTimePicker2.Value.ToShortDateString()}
     );
                DataTable dt = DBAdo.DtFillSql(sql);
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                this.progressBar1.Visible = true;
                //foreach (DataRow r in dt.Rows)
                //{
                //    //bool hs = DBAdo.ExecuteScalarSql(string.Format("SELECT {0}"));
                //    this.progressBar1.Value++;
                //    Application.DoEvents();
                //    decimal a1 = 0;
                //    decimal a2 = 0;

                //    decimal b1 = 0;
                //    decimal b2 = 0;

                //    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号1"].ToString(), YEAR.Text, MONTH.Text, "回款" }));
                //    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号1"].ToString(), YEAR.Text, MONTH.Text, "销项发票" }));

                //    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号2"].ToString(), YEAR.Text, MONTH.Text, "付款" }));
                //    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号2"].ToString(), YEAR.Text, MONTH.Text, "进项发票" }));

                //    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                //    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());

                //    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                //    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());

                //    r["已收货款"] = a1;
                //    r["已开发票"] = a2;
                //    r["已付货款"] = b1;
                //    r["已收发票"] = b2;
                //}
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1_ML_NEW", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                //ReportParameter rp1 = new ReportParameter("dept", ClassCustom.codeSub1(this.HDW.Text));
                ReportParameter rp2 = new ReportParameter("year", this.YEAR.Text);
                ReportParameter rp3 = new ReportParameter("month", this.MONTH.Text);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp2, rp3 });
                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (reportName)
                {
                    case "合同收付款明细表全部":
                        getReport_合同收付款明细表_全部();
                        break;
                    case "合同收付款明细表":
                        getReport_合同收付款明细表();
                        break;
                    case "合同收付款明细表_按客户":
                        getReport_合同收付款明细表_按客户();
                        break;
                    case "回款总汇":
                        getReport_回款汇总();
                        break;
                    case "合同汇总":
                        getReport_合同汇总表();
                        break;
                    case "签订明细":
                        getReport_签订明细();
                        break;
                    case "Report合同总览表_按类型":
                        getReport_合同总览表_按类型();
                        break;
                    case "Report_JT_合同类型汇总表.rdlc":
                        getReport_JT_合同类型汇总表();
                        break;
                    case "Report_JT_各单位收付款汇总表":
                        getReport_JT_各单位收付款汇总表();
                        break;
                    case "Report_JT_各单位货款同期对比表":
                        getReport_JT_各单位货款同期对比表();
                        break;
                    case "各单位签订合同情况表":
                        this.getReport_JT_各单位签订合同情况表();
                        break;
                    case "集团合同类型汇总表":
                        this.getReport_JT_集团合同类型汇总表();
                        break;
                    case "销售毛利表":
                        this.getReprot_JT_毛利表();
                        break;
                    case "铸锻公司全部采购外协合同汇总表":
                        this.GetReport_JT_铸锻公司全部采购外协合同汇总表();
                        break;
                    case "毛利新":
                        this.getReprot_毛利新();
                        break;
                    case "集团毛利新":
                        getReprot_集团毛利新();
                        break;
                    case "毛利新1":
                        this.getReprot_毛利新1();
                        break;
                    default:
                        throw new Exception("未知报表！");
                }
                this.splitContainer1.Panel1Collapsed = true;
                this.progressBar1.Value = 0;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }



        private void HDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.HBM.Items.Clear();
                dt_bm = DBAdo.DtFillSql(string.Format("select YCODE,YNAME from AYWY where 1=1 and YCODE  like '{0}__'", ClassCustom.codeSub((sender as ComboBox).Text)));

                foreach (DataRow r in dt_bm.Rows)
                {
                    this.HBM.Items.Add(r[0].ToString() + ":" + r[1].ToString());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void reportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            try
            {
                LocalReport localReport = (LocalReport)e.Report;

                //if (this.reportViewer1.LocalReport.ReportEmbeddedResource == "contract.Reports.Report合同收付款明细表.rdlc")
                //{
                //    localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_AFKXX", DBAdo.DtFillSql("SELECT * FROM AFKXX")));
                //    localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_ASP", DBAdo.DtFillSql("SELECT * FROM ASP")));
                //}
                //localReport.OriginalParametersToDrillthrough
                //Console.WriteLine(localReport.DisplayName);
                //Console.WriteLine(int.Parse(localReport.OriginalParametersToDrillthrough[0].Values[0].ToString()));
                switch (this.reportViewer1.LocalReport.ReportEmbeddedResource)
                {
                    case "contract.Reports.Report合同收付款明细表.rdlc":
                        if (int.Parse(localReport.OriginalParametersToDrillthrough[0].Values[0].ToString()) == 0)
                        {
                            DataTable dt = DBAdo.DtFillSql(string.Format("SELECT * FROM Vcx1 WHERE 合同号 ='{0}'", localReport.OriginalParametersToDrillthrough[1].Values[0].ToString()));
                            localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_ASP", DBAdo.DtFillSql("SELECT * FROM ASP")));
                            if (dt.Rows.Count > 0)
                            {
                                ReportParameter rp1 = new ReportParameter("hkh", dt.Rows[0]["客户名"].ToString());
                                ReportParameter rp2 = new ReportParameter("qdrq", dt.Rows[0]["QDRQ"].ToString());
                                ReportParameter rp3 = new ReportParameter("jhrq", dt.Rows[0]["交货日期"].ToString());
                                ReportParameter rp4 = new ReportParameter("hlx", dt.Rows[0]["hlx"].ToString());
                                ReportParameter rp5 = new ReportParameter("sp", dt.Rows[0]["商品"].ToString());
                                ReportParameter rp6 = new ReportParameter("rmb", dt.Rows[0]["金额"].ToString());
                                ReportParameter rp7 = new ReportParameter("fp", dt.Rows[0]["发票"].ToString());
                                localReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7 });
                                DataTable _dtsp = DBAdo.DtFillSql(string.Format("SELECT HTH 合同号, YEAR(date) 年 ,month(date) 月,TYPE 类型, SUM(RMB) 金额 FROM AFKXX WHERE HTH ='{0}'  GROUP BY HTH , YEAR(date)  ,month(date) ,TYPE ", localReport.OriginalParametersToDrillthrough[1].Values[0].ToString()));
                                for (int i = 0; i < 12; i++)
                                {
                                    _dtsp.Rows.Add(new object[] { localReport.OriginalParametersToDrillthrough[1].Values[0].ToString() ,this.YEAR.Text,i+1,
                                    (dt.Rows[0]["hlx"].ToString().Substring(0,2)=="02"?"回款":"付款")
                                    ,0});
                                    _dtsp.Rows.Add(new object[] { localReport.OriginalParametersToDrillthrough[1].Values[0].ToString() ,this.YEAR.Text,i+1,
                                    (dt.Rows[0]["hlx"].ToString().Substring(0,2)=="02"?"销项发票":"进项发票")
                                    ,0});
                                }
                                localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_SP", _dtsp));
                            }
                        }
                        break;
                    case "contract.Reports.Report合同导航.rdlc":
                        localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_ASP", DBAdo.DtFillSql("SELECT * FROM ASP")));
                        break;

                    case "contract.Reports.Report签订明细.rdlc":
                        if (int.Parse(localReport.OriginalParametersToDrillthrough[0].Values[0].ToString()) == 0)
                        {
                            DataTable dt = DBAdo.DtFillSql(string.Format("SELECT * FROM Vcx1 WHERE 合同号 ='{0}'", localReport.OriginalParametersToDrillthrough[1].Values[0].ToString()));
                            localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_ASP", DBAdo.DtFillSql("SELECT * FROM ASP")));
                            if (dt.Rows.Count > 0)
                            {
                                ReportParameter rp1 = new ReportParameter("hkh", dt.Rows[0]["客户名"].ToString());
                                ReportParameter rp2 = new ReportParameter("qdrq", dt.Rows[0]["QDRQ"].ToString());
                                ReportParameter rp3 = new ReportParameter("jhrq", dt.Rows[0]["交货日期"].ToString());
                                ReportParameter rp4 = new ReportParameter("hlx", dt.Rows[0]["hlx"].ToString());
                                ReportParameter rp5 = new ReportParameter("sp", dt.Rows[0]["商品"].ToString());
                                ReportParameter rp6 = new ReportParameter("rmb", dt.Rows[0]["金额"].ToString());
                                ReportParameter rp7 = new ReportParameter("fp", dt.Rows[0]["发票"].ToString());
                                localReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7 });
                                DataTable _dtsp = DBAdo.DtFillSql(string.Format("SELECT HTH 合同号, YEAR(date) 年 ,month(date) 月,TYPE 类型, SUM(RMB) 金额 FROM AFKXX WHERE HTH ='{0}'  GROUP BY HTH , YEAR(date)  ,month(date) ,TYPE ", localReport.OriginalParametersToDrillthrough[1].Values[0].ToString()));
                                for (int i = 0; i < 12; i++)
                                {
                                    _dtsp.Rows.Add(new object[] { localReport.OriginalParametersToDrillthrough[1].Values[0].ToString() ,this.YEAR.Text,i+1,
                                    (dt.Rows[0]["hlx"].ToString().Substring(0,2)=="02"?"回款":"付款")
                                    ,0});
                                    _dtsp.Rows.Add(new object[] { localReport.OriginalParametersToDrillthrough[1].Values[0].ToString() ,this.YEAR.Text,i+1,
                                    (dt.Rows[0]["hlx"].ToString().Substring(0,2)=="02"?"销项发票":"进项发票")
                                    ,0});
                                }
                                localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_SP", _dtsp));
                            }
                        }
                        break;
                    case "contract.Reports.Report合同汇总.rdlc":
                        if (int.Parse(localReport.OriginalParametersToDrillthrough[0].Values[0].ToString()) == 0)
                        {
                            DataTable dt = DBAdo.DtFillSql(string.Format("SELECT * FROM Vcx1 WHERE 合同号 ='{0}'", localReport.OriginalParametersToDrillthrough[1].Values[0].ToString()));
                            localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_ASP", DBAdo.DtFillSql("SELECT * FROM ASP")));
                            if (dt.Rows.Count > 0)
                            {
                                ReportParameter rp1 = new ReportParameter("hkh", dt.Rows[0]["客户名"].ToString());
                                ReportParameter rp2 = new ReportParameter("qdrq", dt.Rows[0]["QDRQ"].ToString());
                                ReportParameter rp3 = new ReportParameter("jhrq", dt.Rows[0]["交货日期"].ToString());
                                ReportParameter rp4 = new ReportParameter("hlx", dt.Rows[0]["hlx"].ToString());
                                ReportParameter rp5 = new ReportParameter("sp", dt.Rows[0]["商品"].ToString());
                                ReportParameter rp6 = new ReportParameter("rmb", dt.Rows[0]["金额"].ToString());
                                ReportParameter rp7 = new ReportParameter("fp", dt.Rows[0]["发票"].ToString());
                                localReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7 });
                                DataTable _dtsp = DBAdo.DtFillSql(string.Format("SELECT HTH 合同号, YEAR(date) 年 ,month(date) 月,TYPE 类型, SUM(RMB) 金额 FROM AFKXX WHERE HTH ='{0}'  GROUP BY HTH , YEAR(date)  ,month(date) ,TYPE ", localReport.OriginalParametersToDrillthrough[1].Values[0].ToString()));
                                for (int i = 0; i < 12; i++)
                                {
                                    _dtsp.Rows.Add(new object[] { localReport.OriginalParametersToDrillthrough[1].Values[0].ToString() ,this.YEAR.Text,i+1,
                                    (dt.Rows[0]["hlx"].ToString().Substring(0,2)=="02"?"回款":"付款")
                                    ,0});
                                    _dtsp.Rows.Add(new object[] { localReport.OriginalParametersToDrillthrough[1].Values[0].ToString() ,this.YEAR.Text,i+1,
                                    (dt.Rows[0]["hlx"].ToString().Substring(0,2)=="02"?"销项发票":"进项发票")
                                    ,0});
                                }
                                localReport.DataSources.Add(new ReportDataSource("Contract1DataSet_SP", _dtsp));
                            }
                        }
                        break;
                    //Report合同汇总.rdlc
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                // MessageBox.Show(ex.Message);
                return;
            }



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
                    new Factory_ToolBtn("  查询  ", "  查询  ",ClassCustom.getImage("sel.png"), this.toolStripButton1_Click,null,true).TBtnProduce(),
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

        private void HLX_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.HLX1.DataSource = DBAdo.DtFillSql(string.Format("SELECT LID,LID+':'+LNAME lname FROM aLX WHERE 1=1 AND LID LIKE '{0}__'", ClassCustom.codeSub(this.HLX.Text)));
                this.HLX1.DisplayMember = "lname";
                this.HLX1.ValueMember = "lid";
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
