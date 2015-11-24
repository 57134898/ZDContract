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
    public partial class A_RPT_KHHK : Form
    {
        public A_RPT_KHHK()
        {
            InitializeComponent();
        }

        private void A_RPT_KHHK_Load(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = DateTime.Now.Year;
            this.numericUpDown2.Value = DateTime.Now.Month;
            this.comboBox1.Text = "回款";
        }

        private DataTable dt;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count == 0)
                {
                    return;
                }
                EXCEL_FKMX(this.comboBox1.Text + "情况客户汇总表", ClassConstant.DW_NAME, this.numericUpDown1.Value + "年" + this.numericUpDown2.Value + "月");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void EXCEL_FKMX(string name, string dw, string date)
        {
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;

            //**********************************************************************************************
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("C4", "F4").Merge(false);
            excel.get_Range("C5", "C6").Merge(false);
            excel.get_Range("D5", "E5").Merge(false);
            excel.get_Range("F5", "F6").Merge(false);
            excel.get_Range("G4", "J4").Merge(false);
            excel.get_Range("G5", "G6").Merge(false);
            excel.get_Range("H5", "I5").Merge(false);
            excel.get_Range("J5", "J6").Merge(false);
            excel.get_Range("K4", "N4").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("L5", "M5").Merge(false);
            excel.get_Range("N5", "N6").Merge(false);
            excel.get_Range("O4", "R4").Merge(false);
            excel.get_Range("O5", "O6").Merge(false);
            excel.get_Range("P5", "Q5").Merge(false);
            excel.get_Range("R5", "R6").Merge(false);
            excel.get_Range("S4", "S6").Merge(false);


            excel.Cells[4, "A"] = "序号";
            excel.Cells[4, "B"] = "客户";
            excel.Cells[4, "C"] = "现汇";
            excel.Cells[4, "G"] = "票据";
            excel.Cells[4, "K"] = "抹帐";
            excel.Cells[4, "O"] = "小计";
            excel.Cells[4, "S"] = "备注";
            excel.Cells[5, "C"] = "前期";
            excel.Cells[5, "D"] = "本年";
            excel.Cells[5, "F"] = "总累计";
            excel.Cells[5, "G"] = "前期";
            excel.Cells[5, "H"] = "本年";
            excel.Cells[5, "J"] = "总累计";
            excel.Cells[5, "K"] = "前期";
            excel.Cells[5, "L"] = "本年";
            excel.Cells[5, "N"] = "总累计";
            excel.Cells[5, "O"] = "前期";
            excel.Cells[5, "P"] = "本年";
            excel.Cells[5, "R"] = "总累计";
            excel.Cells[6, "D"] = "本月";
            excel.Cells[6, "E"] = "本年累计";
            excel.Cells[6, "H"] = "本月";
            excel.Cells[6, "I"] = "本年累计";
            excel.Cells[6, "L"] = "本月";
            excel.Cells[6, "M"] = "本年累计";
            excel.Cells[6, "P"] = "本月";
            excel.Cells[6, "Q"] = "本年累计";
            //**********************************************************************************************
            excel.Cells[1, 1] = name;
            excel.Cells[3, 1] = dw + "    " + date;
            (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A4", "S4").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "m"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "g"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A1", "S6").Font.Bold = true;
            excel.get_Range("c7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            //excel.get_Range("k7", excel.Cells[dataGridView1.Rows.Count + 6, "k"]).NumberFormat = "0%";
            //excel.get_Range("q7", excel.Cells[dataGridView1.Rows.Count + 6, "q"]).NumberFormat = "0%";
            //填充数据
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (dataGridView1[j, i].ValueType == typeof(string))
                    {
                        excel.Cells[i + 7, j + 1] = "'" + dataGridView1[j, i].Value.ToString();
                    }
                    else
                    {
                        excel.Cells[i + 7, j + 1] = dataGridView1[j, i].Value.ToString();
                    }
                }
            }
            excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count + 1]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$6";
            //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //null 现汇前期,null 现汇本月,null 现汇本年累计,null 现汇总累计 ,"
                //     + "null 票据前期,null 票据本月,null 票据本年累计,null 票据总累计,"
                //     + "null 抹帐前期,null 抹帐本月,null 抹帐本年累计,null 抹帐总累计,"
                //     + "null 小计前期,null 小计本月,null 小计本年累计,null 小计总累计,null 备注 

                //string sql = "select distinct null 序号,hkh+':'+客户名 客户 from vcontracts where hlx like '02%' order by 客户";
                //string sql = "select distinct null 序号, C.CCODE+':'+C.CNAME 客户 from ACLIENTS C, ACASH S WHERE C.CCODE =S.CCODE and s.cash+s.note+s.mz<>0 and s.hdw='" + ClassConstant.DW_ID + "' order by 客户;";

                string sql = "select distinct null 序号, C.CCODE+':'+C.CNAME 客户 from ACLIENTS C, ACASH S WHERE s.type = '" + this.comboBox1.Text + "' and ((YEAR(ExchangeDate)='" + this.numericUpDown1.Value.ToString() + "'and MONTH(ExchangeDate)<='" + this.numericUpDown2.Value.ToString() + "') or YEAR(ExchangeDate)<'" + this.numericUpDown1.Value.ToString() + "') and C.CCODE =S.CCODE and s.hdw='" + ClassConstant.DW_ID + "' order by 客户;";
                dt = DBAdo.DtFillSql(sql);

                dt.Columns.Add("现汇前期", typeof(decimal));
                dt.Columns.Add("现汇本月", typeof(decimal));
                dt.Columns.Add("现汇本年", typeof(decimal));
                dt.Columns.Add("现汇总累计", typeof(decimal));

                dt.Columns.Add("票据前期", typeof(decimal));
                dt.Columns.Add("票据本月", typeof(decimal));
                dt.Columns.Add("票据本年", typeof(decimal));
                dt.Columns.Add("票据总累计", typeof(decimal));

                dt.Columns.Add("抹帐前期", typeof(decimal));
                dt.Columns.Add("抹帐本月", typeof(decimal));
                dt.Columns.Add("抹帐本年", typeof(decimal));
                dt.Columns.Add("抹帐总累计", typeof(decimal));

                dt.Columns.Add("小计前期", typeof(decimal));
                dt.Columns.Add("小计本月", typeof(decimal));
                dt.Columns.Add("小计本年", typeof(decimal));
                dt.Columns.Add("小计总累计", typeof(decimal));

                //DataTable temp = DBAdo.DtFillSql(string.Format("SELECT * FROM ACASH WHERE (YEAR(ExchangeDate)<{0}) OR (YEAR(ExchangeDate)={1} AND MONTH(ExchangeDate)<={2})",
                //    this.numericUpDown1.Value.ToString(), this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString()));

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dt.Rows[i]["序号"] = (i + 1).ToString();

                    string xhqq = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(CASH) FROM ACASH WHERE 1=1 AND type ='" + this.comboBox1.Text + "' AND (YEAR(ExchangeDate)<{0} and ccode = '{1}'and hdw='{2}') ", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["现汇前期"] = decimal.Parse(xhqq == "" ? "0" : xhqq);
                    string xhby = DBAdo.ExecuteScalarSql(string.Format("select sum(CASH) from acash where 1=1 and type = '" + this.comboBox1.Text + "' and (YEAR(ExchangeDate)={0} and MONTH(ExchangeDate)={1} and ccode = '{2}'and hdw='{3}')", this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["现汇本月"] = decimal.Parse(xhby == "" ? "0" : xhby);
                    string xhbn = DBAdo.ExecuteScalarSql(string.Format("select sum(CASH) from acash where 1=1 and type = '" + this.comboBox1.Text + "' and (YEAR(ExchangeDate)={0}  and ccode = '{1}' and MONTH(ExchangeDate)<={2} and hdw='{3}')", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), this.numericUpDown2.Value.ToString(), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["现汇本年"] = decimal.Parse(xhbn == "" ? "0" : xhbn);
                    string xhzj = DBAdo.ExecuteScalarSql(string.Format("select sum(CASH) from acash where 1=1 and type = '" + this.comboBox1.Text + "'   and (ccode = '{0}' and hdw='{1}' and ((YEAR(ExchangeDate)='{2}'and MONTH(ExchangeDate)<='{3}') or YEAR(ExchangeDate)<'{2}'))", ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID, this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString())).ToString();
                    dt.Rows[i]["现汇总累计"] = decimal.Parse(xhzj == "" ? "0" : xhzj);

                    string pjqq = DBAdo.ExecuteScalarSql(string.Format("select sum(note) from acash where 1=1 and type = '" + this.comboBox1.Text + "' and (year(exchangedate)<{0} and ccode = '{1}' and hdw='{2}')", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["票据前期"] = decimal.Parse(pjqq == "" ? "0" : pjqq);
                    string pjby = DBAdo.ExecuteScalarSql(string.Format("select sum(note) from acash where 1=1 and type = '" + this.comboBox1.Text + "' and (year(exchangedate)={0} and month(exchangedate)={1} and ccode = '{2}' and hdw='{3}')", this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["票据本月"] = decimal.Parse(pjby == "" ? "0" : pjby);
                    string pjbn = DBAdo.ExecuteScalarSql(string.Format("select sum(note) from acash where 1=1 and type = '" + this.comboBox1.Text + "' and (year(exchangedate)={0} and ccode = '{1}' and MONTH(ExchangeDate)<={2} and hdw='{3}')", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), this.numericUpDown2.Value.ToString(), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["票据本年"] = decimal.Parse(pjbn == "" ? "0" : pjbn);
                    string pjzj = DBAdo.ExecuteScalarSql(string.Format("select sum(note) from acash where 1=1 and type = '" + this.comboBox1.Text + "'   and (ccode = '{0}' and hdw='{1}' and ((YEAR(ExchangeDate)='{2}'and MONTH(ExchangeDate)<='{3}') or YEAR(ExchangeDate)<'{2}'))", ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID, this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString())).ToString();
                    dt.Rows[i]["票据总累计"] = decimal.Parse(pjzj == "" ? "0" : pjzj);

                    string mzqq = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(mz) FROM ACASH WHERE 1=1 AND type ='" + this.comboBox1.Text + "' AND (YEAR(ExchangeDate)<{0} and ccode = '{1}' and hdw='{2}') ", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["抹帐前期"] = decimal.Parse(mzqq == "" ? "0" : mzqq);
                    string mzby = DBAdo.ExecuteScalarSql(string.Format("select sum(mz) from acash where 1=1 and type = '" + this.comboBox1.Text + "' and (YEAR(ExchangeDate)={0} and MONTH(ExchangeDate)={1} and ccode = '{2}' and hdw='{3}')", this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["抹帐本月"] = decimal.Parse(mzby == "" ? "0" : mzby);
                    string mzbn = DBAdo.ExecuteScalarSql(string.Format("select sum(mz) from acash where 1=1 and type = '" + this.comboBox1.Text + "' and (YEAR(ExchangeDate)={0}  and ccode = '{1}' and MONTH(ExchangeDate)<={2} and hdw='{3}')", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), this.numericUpDown2.Value.ToString(), ClassConstant.DW_ID)).ToString();
                    dt.Rows[i]["抹帐本年"] = decimal.Parse(mzbn == "" ? "0" : mzbn);
                    string mzzj = DBAdo.ExecuteScalarSql(string.Format("select sum(mz) from acash where 1=1 and type = '" + this.comboBox1.Text + "'   and (ccode = '{0}' and hdw='{1}' and ((YEAR(ExchangeDate)='{2}'and MONTH(ExchangeDate)<='{3}') or YEAR(ExchangeDate)<'{2}'))", ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()), ClassConstant.DW_ID, this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString())).ToString();
                    dt.Rows[i]["抹帐总累计"] = decimal.Parse(mzzj == "" ? "0" : mzzj);

                    //string xjqq = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(mz) FROM ACASH WHERE 1=1 AND type ='回款' AND (YEAR(ExchangeDate)<{0} and ccode = '{1}') ", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()))).ToString();
                    dt.Rows[i]["小计前期"] = decimal.Parse(xhqq == "" ? "0" : xhqq) + decimal.Parse(pjqq == "" ? "0" : pjqq) + decimal.Parse(mzqq == "" ? "0" : mzqq);
                    //string xjby = DBAdo.ExecuteScalarSql(string.Format("select sum(mz) from acash where 1=1 and type = '回款' and (YEAR(ExchangeDate)={0} and MONTH(ExchangeDate)={1} and ccode = '{2}')", this.numericUpDown1.Value.ToString(), this.numericUpDown2.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()))).ToString();
                    dt.Rows[i]["小计本月"] = decimal.Parse(xhby == "" ? "0" : xhby) + decimal.Parse(pjby == "" ? "0" : pjby) + decimal.Parse(mzby == "" ? "0" : mzby);
                    //string xjbn = DBAdo.ExecuteScalarSql(string.Format("select sum(mz) from acash where 1=1 and type = '回款' and (YEAR(ExchangeDate)={0}  and ccode = '{1}')", this.numericUpDown1.Value.ToString(), ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()))).ToString();
                    dt.Rows[i]["小计本年"] = decimal.Parse(xhbn == "" ? "0" : xhbn) + decimal.Parse(pjbn == "" ? "0" : pjbn) + decimal.Parse(mzbn == "" ? "0" : mzbn);
                    //string xjzj = DBAdo.ExecuteScalarSql(string.Format("select sum(mz) from acash where 1=1 and type = '回款'   and (ccode = '{0}')", ClassCustom.codeSub(dt.Rows[i]["客户"].ToString()))).ToString();
                    dt.Rows[i]["小计总累计"] = decimal.Parse(xhzj == "" ? "0" : xhzj) + decimal.Parse(pjzj == "" ? "0" : pjzj) + decimal.Parse(mzzj == "" ? "0" : mzzj);
                    Application.DoEvents();
                    this.progressBar1.Value++;
                    //this.progressBar1.Refresh();

                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] == null)
                        {
                            continue;
                        }
                        if (dt.Rows[i][j].ToString() == "")
                        {
                            continue;
                        }
                        if (decimal.Parse(dt.Rows[i][j].ToString()) == 0)
                        {
                            dt.Rows[i][j] = DBNull.Value;
                        }
                    }
                }

                object[] o = new object[dt.Columns.Count];
                o[1] = "合计：";
                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    decimal sum = 0;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        sum += decimal.Parse(dt.Rows[j][i].ToString() == "" ? "0" : dt.Rows[j][i].ToString());
                    }
                    o[i] = sum;
                }
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.AutoResizeColumns();
                foreach (DataGridViewColumn c in this.dataGridView1.Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dt.Rows.Add(o);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
