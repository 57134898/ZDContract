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
    public partial class A_RTP_DZD : Form
    {
        private string ccode;
        private string cname;
        private DataTable dt;

        public A_RTP_DZD()
        {
            InitializeComponent();
        }

        public A_RTP_DZD(string ccode, string cname)
        {
            InitializeComponent();
            this.ccode = ccode;
            this.cname = cname;


        }

        private void A_RTP_DZD_Load(object sender, EventArgs e)
        {
            this.Text = "客户对账单  -  " + ccode + ":" + cname;
            this.numericUpDown1.Value = DateTime.Now.Year;
            this.numericUpDown2.Value = DateTime.Now.Month;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "回款")
            {
                DZD_CX_Q("回款");
            }
            if (this.comboBox1.Text == "付款")
            {
                DZD_CX_Q("付款");

            }
            if (this.comboBox1.Text == "进项发票")
            {
                DZD_CX_FP("进项发票");
            }
            if (this.comboBox1.Text == "销项发票")
            {
                DZD_CX_FP("销项发票");
            }
            this.progressBar1.Value = this.progressBar1.Maximum;
        }

        private void DZD_CX_FP(string type)
        {
            try
            {
                //Cash 现汇,Note 票据,Mz 抹账,,Type 类型
                string sql = "SELECT '' 日期, Cash+Note+Mz 合计金额,case VoucherFlag when 1 THEN '是' else '否' end as 连接凭证,VoucherYear 凭证年份,VoucherMonth 凭证月份,VoucherType 凭证类型,VoucherId 凭证号  FROM ACASH WHERE 1=1 and type ='" + type + "' and year(exchangedate)='" + this.numericUpDown1.Value.ToString() + "' and month(exchangedate)<='" + this.numericUpDown2.Value.ToString() + "' and Ccode = '" + ccode + "'order by exchangedate;";
                //string sql = "SELECT '' 日期,Cash+Note+Mz 合计金额,Type 类型,case VoucherFlag when 1 THEN '是' else '否' end as 连接凭证,VoucherYear 凭证年份,VoucherMonth 凭证月份,VoucherType 凭证类型,VoucherId 凭证号  FROM ACASH WHERE year(exchangedate)='" + this.numericUpDown1.Value.ToString() + "' and month(exchangedate)<='" + this.numericUpDown2.Value.ToString() + "' and Ccode = '" + ccode + "'order by exchangedate;";
                dt = DBAdo.DtFillSql(sql);
                DataTable rq;
                rq = DBAdo.DtFillSql("SELECT convert(nvarchar(30),exchangedate,102) 日期 FROM ACASH WHERE 1=1 and type='" + type + "' and year(exchangedate)='" + this.numericUpDown1.Value.ToString() + "' and month(exchangedate)<='" + this.numericUpDown2.Value.ToString() + "' and Ccode = '" + ccode + "'order by exchangedate;");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["日期"] = rq.Rows[i]["日期"];
                }
                decimal os1 = 0;
                //decimal os2 = 0;
                //decimal os3 = 0;
                int m = 0;
                object[] oj = new object[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    m = DateTime.Parse(dt.Rows[i]["日期"].ToString()).Month;
                    os1 += decimal.Parse(dt.Rows[i]["合计金额"].ToString());
                    //os2 += decimal.Parse(dt.Rows[i]["票据"].ToString());
                    //os3 += decimal.Parse(dt.Rows[i]["抹账"].ToString());
                }
                oj[0] = os1;
                //oj[1] = os2;
                //oj[2] = os3;
                //oj[3] = os1 + os2 + os3;
                DataTable ms = DBAdo.DtFillSql(string.Format("select sum(cash+note+mz) from acash where 1=1 and type='" + type + "' and  year(exchangedate)='" + this.numericUpDown1.Value.ToString() + "'  and Ccode = '" + ccode + "'and (month(exchangedate)={0})", m));
                decimal sum1 = 0;
                //decimal sum2 = 0;
                //decimal sum3 = 0;
                decimal sm1 = 0;
                //decimal sm2 = 0;
                //decimal sm3 = 0;

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    if (DateTime.Parse(dt.Rows[i - 1]["日期"].ToString()).Month == DateTime.Parse(dt.Rows[i]["日期"].ToString()).Month)
                    {

                        sum1 += decimal.Parse(dt.Rows[i - 1]["合计金额"].ToString());
                        //sum2 += decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        //sum3 += decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        sm1 += decimal.Parse(dt.Rows[i - 1]["合计金额"].ToString());
                        //sm2 += decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        //sm3 += decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        DataRow dr1 = dt.NewRow();
                        dr["日期"] = "本月累计";
                        //dr["现汇"] = sm1 + decimal.Parse(dt.Rows[i - 1]["现汇"].ToString());
                        //dr["票据"] = sm2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        //dr["抹账"] = sm3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        dr["合计金额"] = sm1 + decimal.Parse(dt.Rows[i - 1]["合计金额"].ToString());
                        dt.Rows.InsertAt(dr, i);
                        dr1["日期"] = "本年累计";
                        //dr1["现汇"] = sum1 + decimal.Parse(dt.Rows[i - 1]["现汇"].ToString());
                        //dr1["票据"] = sum2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        //dr1["抹账"] = sum3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        dr1["合计金额"] = sum1 + decimal.Parse(dt.Rows[i - 1]["合计金额"].ToString());
                        dt.Rows.InsertAt(dr1, i + 1);
                        sum1 = sum1 + decimal.Parse(dt.Rows[i - 1]["合计金额"].ToString());
                        //sum2 = sum2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        //sum3 = sum3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());

                        sm1 = 0;
                        //sm2 = 0;
                        //sm3 = 0;
                        i = i + 2;
                    }

                    Application.DoEvents();
                    this.progressBar1.Value++;
                }
                dt.Rows.Add("本月累计", ms.Rows[0][0]);
                dt.Rows.Add("本年累计", oj[0]);

                this.dataGridView1.DataSource = dt;
                this.dataGridView1.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DZD_CX_Q(string type)
        {
            try
            {

                //Type 类型,

                string sql = "SELECT '' 日期, Cash 现汇,Note 票据,Mz 抹账,Cash+Note+Mz 合计金额,case VoucherFlag when 1 THEN '是' else '否' end as 连接凭证,VoucherYear 凭证年份,VoucherMonth 凭证月份,VoucherType 凭证类型,VoucherId 凭证号  FROM ACASH WHERE 1=1 and type ='" + type + "' and year(exchangedate)='" + this.numericUpDown1.Value.ToString() + "' and month(exchangedate)<='" + this.numericUpDown2.Value.ToString() + "' and Ccode = '" + ccode + "'order by exchangedate;";
                dt = DBAdo.DtFillSql(sql);
                DataTable rq;
                rq = DBAdo.DtFillSql("SELECT convert(nvarchar(30),exchangedate,102) 日期 FROM ACASH WHERE  1=1 and type='" + type + "' and  year(exchangedate)='" + this.numericUpDown1.Value.ToString() + "' and month(exchangedate)<='" + this.numericUpDown2.Value.ToString() + "' and Ccode = '" + ccode + "'order by exchangedate;");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["日期"] = rq.Rows[i]["日期"];
                }
                decimal os1 = 0;
                decimal os2 = 0;
                decimal os3 = 0;
                int m = 0;
                object[] oj = new object[4];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    m = DateTime.Parse(dt.Rows[i]["日期"].ToString()).Month;
                    os1 += decimal.Parse(dt.Rows[i]["现汇"].ToString());
                    os2 += decimal.Parse(dt.Rows[i]["票据"].ToString());
                    os3 += decimal.Parse(dt.Rows[i]["抹账"].ToString());
                }
                oj[0] = os1;
                oj[1] = os2;
                oj[2] = os3;
                oj[3] = os1 + os2 + os3;
                DataTable ms;
                ms = DBAdo.DtFillSql(string.Format("select sum(cash) ,sum(note),sum(mz),sum(cash+note+mz) from acash where 1=1 and type='" + type + "' and  year(exchangedate)='" + this.numericUpDown1.Value.ToString() + "'  and Ccode = '" + ccode + "'and (month(exchangedate)={0})", m));
                decimal sum1 = 0;
                decimal sum2 = 0;
                decimal sum3 = 0;
                decimal sm1 = 0;
                decimal sm2 = 0;
                decimal sm3 = 0;

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    if (DateTime.Parse(dt.Rows[i - 1]["日期"].ToString()).Month == DateTime.Parse(dt.Rows[i]["日期"].ToString()).Month)
                    {

                        sum1 += decimal.Parse(dt.Rows[i - 1]["现汇"].ToString());
                        sum2 += decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        sum3 += decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        sm1 += decimal.Parse(dt.Rows[i - 1]["现汇"].ToString());
                        sm2 += decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        sm3 += decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        DataRow dr1 = dt.NewRow();
                        dr["日期"] = "本月累计";
                        dr["现汇"] = sm1 + decimal.Parse(dt.Rows[i - 1]["现汇"].ToString());
                        dr["票据"] = sm2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        dr["抹账"] = sm3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        dr["合计金额"] = sm1 + decimal.Parse(dt.Rows[i - 1]["现汇"].ToString()) + sm2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString()) + sm3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        dt.Rows.InsertAt(dr, i);
                        dr1["日期"] = "本年累计";
                        dr1["现汇"] = sum1 + decimal.Parse(dt.Rows[i - 1]["现汇"].ToString());
                        dr1["票据"] = sum2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        dr1["抹账"] = sum3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        dr1["合计金额"] = sum1 + decimal.Parse(dt.Rows[i - 1]["现汇"].ToString()) + sum2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString()) + sum3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());
                        dt.Rows.InsertAt(dr1, i + 1);
                        sum1 = sum1 + decimal.Parse(dt.Rows[i - 1]["现汇"].ToString());
                        sum2 = sum2 + decimal.Parse(dt.Rows[i - 1]["票据"].ToString());
                        sum3 = sum3 + decimal.Parse(dt.Rows[i - 1]["抹账"].ToString());

                        sm1 = 0;
                        sm2 = 0;
                        sm3 = 0;
                        i = i + 2;
                    }

                    Application.DoEvents();
                    this.progressBar1.Value++;
                }
                dt.Rows.Add("本月累计", ms.Rows[0][0], ms.Rows[0][1], ms.Rows[0][2], ms.Rows[0][3]);
                dt.Rows.Add("本年累计", oj[0], oj[1], oj[2], oj[3]);

                this.dataGridView1.DataSource = dt;
                this.dataGridView1.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "回款" || this.comboBox1.Text == "付款")
            {
                EXCEL_EXPORT_RMB("对账单(" + this.comboBox1.Text + ")", ClassConstant.DW_NAME, this.numericUpDown1.Value.ToString() + "年" + this.numericUpDown2.Value.ToString() + "月");
            }
            else
            {
                EXCEL_EXPORT_FP("对账单(" + this.comboBox1.Text + ")", ClassConstant.DW_NAME, this.numericUpDown1.Value.ToString() + "年" + this.numericUpDown2.Value.ToString() + "月");
            }

        }
        private void EXCEL_EXPORT_RMB(string tname, string cname, string date)
        {
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;

            //**********************************************************************************************
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A5").Merge(false);
            excel.get_Range("B4", "E4").Merge(false);
            excel.get_Range("F4", "J4").Merge(false);
            excel.get_Range("K4", "K5").Merge(false);


            excel.Cells[4, "A"] = "日期";
            excel.Cells[4, "B"] = "金额";
            excel.Cells[4, "F"] = "凭证信息";
            excel.Cells[4, "K"] = "备注";
            excel.Cells[5, "B"] = "现汇";
            excel.Cells[5, "C"] = "票据";
            excel.Cells[5, "D"] = "抹帐";
            excel.Cells[5, "E"] = "小计";
            excel.Cells[5, "F"] = "连接凭证";
            excel.Cells[5, "G"] = "年";
            excel.Cells[5, "H"] = "月";
            excel.Cells[5, "I"] = "凭证类型";
            excel.Cells[5, "J"] = "凭证号";
            //**********************************************************************************************
            excel.Cells[1, 1] = tname;
            excel.Cells[3, 1] = cname + "    " + date;
            (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A4", "K4").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "m"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "g"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A1", "K5").Font.Bold = true;
            //excel.get_Range("E7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            //excel.get_Range("k7", excel.Cells[dataGridView1.Rows.Count + 6, "k"]).NumberFormat = "0%";
            excel.get_Range("A6", excel.Cells[dataGridView1.Rows.Count + 6, "A"]).NumberFormat = "yyyy-MM-dd";
            //填充数据
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {

                    if (dataGridView1[j, i].ValueType == typeof(string))
                    {
                        excel.Cells[i + 6, j + 1] = "'" + dataGridView1[j, i].Value.ToString();
                    }
                    else
                    {
                        excel.Cells[i + 6, j + 1] = dataGridView1[j, i].Value.ToString();
                    }
                }

            }

            excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count + 1]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$6";
            //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;
        }
        private void EXCEL_EXPORT_FP(string tname, string cname, string date)
        {
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;

            //**********************************************************************************************
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A5").Merge(false);
            excel.get_Range("B4", "B5").Merge(false);
            excel.get_Range("C4", "G4").Merge(false);
            excel.get_Range("H4", "H5").Merge(false);


            excel.Cells[4, "A"] = "日期";
            excel.Cells[4, "B"] = "金额";
            excel.Cells[4, "C"] = "凭证信息";
            excel.Cells[4, "H"] = "备注";
            excel.Cells[5, "C"] = "连接凭证";
            excel.Cells[5, "D"] = "年";
            excel.Cells[5, "E"] = "月";
            excel.Cells[5, "F"] = "凭证类型";
            excel.Cells[5, "G"] = "凭证号";
            //**********************************************************************************************
            excel.Cells[1, 1] = tname;
            excel.Cells[3, 1] = cname + "    " + date;
            (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A4", "H5").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "m"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "g"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A1", "H5").Font.Bold = true;
            //excel.get_Range("E7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            //excel.get_Range("k7", excel.Cells[dataGridView1.Rows.Count + 6, "k"]).NumberFormat = "0%";
            //excel.get_Range("q7", excel.Cells[dataGridView1.Rows.Count + 6, "q"]).NumberFormat = "0%";
            //填充数据
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {

                    if (dataGridView1[j, i].ValueType == typeof(string))
                    {
                        excel.Cells[i + 6, j + 1] = "'" + dataGridView1[j, i].Value.ToString();
                    }
                    else
                    {
                        excel.Cells[i + 6, j + 1] = dataGridView1[j, i].Value.ToString();
                    }
                }

            }

            excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count + 1]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$6";
            //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;
        }

    }
}
