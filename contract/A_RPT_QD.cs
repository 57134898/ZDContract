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
    public partial class A_RPT_QD : Form
    {
        private string name;
        private DataTable souce = new DataTable();
        public A_RPT_QD()
        {
            InitializeComponent();
        }
        public A_RPT_QD(string name)
        {
            InitializeComponent();
            this.name = name;
        }
        private void A_RPT_QD_Load(object sender, EventArgs e)
        {
            foreach (DataRow r in DBAdo.DtFillSql("SELECT YCODE, YNAME FROM AYWY WHERE YCODE LIKE '" + ClassConstant.DW_ID + "__'").Rows)
            {
                this.toolStripComboBox1.Items.Add(r[0].ToString() + ":" + r[1].ToString());
            }

            foreach (DataRow r in DBAdo.DtFillSql("SELECT LID, LNAME FROM ALX where len(LID)=2").Rows)
            {
                this.toolStripComboBox2.Items.Add(r[0].ToString() + ":" + r[1].ToString());
            }
            souce.Columns.Add("A", typeof(int));//序号
            souce.Columns.Add("B", typeof(string));//内外
            souce.Columns.Add("C", typeof(string));//合同号
            souce.Columns.Add("D", typeof(string));//客户
            souce.Columns.Add("E", typeof(decimal));//结算
            souce.Columns.Add("F", typeof(decimal));//上年
            souce.Columns.Add("G", typeof(decimal));//本月
            souce.Columns.Add("H", typeof(decimal));//本年;
            souce.Columns.Add("I", typeof(string));//总累计
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            souce.Rows.Clear();
            string sql = "SELECT CASE SUBSTRING(HKH,1,2) WHEN '01' THEN '内部' ELSE '外部' END 客户类型,合同号,客户名 ,结算金额,金额,发票,签定日期 FROM VCONTRACTS WHERE 1=1 "
                         + "AND 签定日期 >= '" + this.dateTimePicker1.Value.ToShortDateString() + "'"
                         + "AND 签定日期 <= '" + this.dateTimePicker2.Value.ToShortDateString() + "'"
                         + "AND HYWY LIKE '" + ClassCustom.codeSub(this.toolStripComboBox1.Text) + "%'"
                         + "AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox2.Text) + "%' order by 客户类型,客户名,合同号";
            DataTable dt = DBAdo.DtFillSql(sql);

            foreach (DataRow r in dt.Rows)
            {//已完成 未完成 全部
                if (this.toolStripComboBox3.Text.ToString() == "已完成")
                {
                    if (r[3].ToString() == r[5].ToString() && r[3].ToString() == r[4].ToString())
                    {
                        decimal lastyear = 0;
                        decimal thismonth = 0;
                        decimal thisyear = 0;
                        if (DateTime.Parse(r[6].ToString()).Year == this.dateTimePicker2.Value.Year)
                        {
                            if (DateTime.Parse(r[6].ToString()).Month == this.dateTimePicker2.Value.Month)
                            {
                                thismonth = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                                thisyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                            }
                            else
                            {
                                thisyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                            }
                        }
                        else
                        {
                            lastyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                        }

                        decimal all = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                        souce.Rows.Add(new object[] { 0, r[0], r[1], r[2], lastyear, thismonth, thisyear, all, "         " });
                    }
                }
                if (this.toolStripComboBox3.Text.ToString() == "未完成")
                {
                    if (!(r[3].ToString() == r[5].ToString() && r[3].ToString() == r[4].ToString()))
                    {
                        decimal lastyear = 0;
                        decimal thismonth = 0;
                        decimal thisyear = 0;
                        if (DateTime.Parse(r[6].ToString()).Year == this.dateTimePicker2.Value.Year)
                        {
                            if (DateTime.Parse(r[6].ToString()).Month == this.dateTimePicker2.Value.Month)
                            {
                                thismonth = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                                thisyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                            }
                            else
                            {
                                thisyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                            }
                        }
                        else
                        {
                            lastyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                        }

                        decimal all = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                        souce.Rows.Add(new object[] { 0, r[0], r[1], r[2], lastyear, thismonth, thisyear, all, "         " });
                    }
                }
                if (this.toolStripComboBox3.Text.ToString() == "全部")
                {
                    decimal lastyear = 0;
                    decimal thismonth = 0;
                    decimal thisyear = 0;
                    if (DateTime.Parse(r[6].ToString()).Year == this.dateTimePicker2.Value.Year)
                    {
                        if (DateTime.Parse(r[6].ToString()).Month == this.dateTimePicker2.Value.Month)
                        {
                            thismonth = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                            thisyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                        }
                        else
                        {
                            thisyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                        }
                    }
                    else
                    {
                        lastyear = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                    }

                    decimal all = decimal.Parse(r[3].ToString() == "" ? "0" : r[3].ToString());
                    souce.Rows.Add(new object[] { 0, r[0], r[1], r[2], lastyear, thismonth, thisyear, all, "         " });
                }

            }
            #region 求合计
            int count = souce.Rows.Count;
            souce.Rows.Add(new object[] { null, null, "总计" });
            int xh = 1;
            for (int i = 0; i < souce.Rows.Count; i++)
            {
                if (souce.Rows[i][2].ToString() != "小计")
                {
                    souce.Rows[i][0] = (xh++).ToString();
                }
                if (i > 0)
                {

                    if (souce.Rows[i][3].ToString() != souce.Rows[i - 1][3].ToString())
                    {
                        souce.Rows.InsertAt(souce.NewRow(), i++);
                        souce.Rows[i - 1][2] = "小计";
                        souce.Rows[i - 1][3] = souce.Rows[i - 2][3];
                    }
                }

            }
            souce.Rows.Remove(souce.Rows[souce.Rows.Count - 1]);
            decimal E = 0;
            decimal F = 0;
            decimal G = 0;
            decimal H = 0;
            for (int i = 0; i < souce.Rows.Count; i++)
            {

                if (souce.Rows[i][2].ToString() == "小计")
                {
                    #region 求小计
                    decimal E1 = 0;
                    decimal F1 = 0;
                    decimal G1 = 0;
                    decimal H1 = 0;
                    for (int k = 0; k < souce.Rows.Count; k++)
                    {
                        if (souce.Rows[k]["D"].ToString() == souce.Rows[i]["D"].ToString() && souce.Rows[k]["C"].ToString() != "小计")
                        {
                            //MessageBox.Show("" + "    " + souce.Rows[i]["D"].ToString());
                            E1 += decimal.Parse(souce.Rows[k]["E"].ToString() == "" ? "0" : souce.Rows[k]["E"].ToString());
                            F1 += decimal.Parse(souce.Rows[k]["F"].ToString() == "" ? "0" : souce.Rows[k]["F"].ToString());
                            G1 += decimal.Parse(souce.Rows[k]["G"].ToString() == "" ? "0" : souce.Rows[k]["G"].ToString());
                            H1 += decimal.Parse(souce.Rows[k]["H"].ToString() == "" ? "0" : souce.Rows[k]["H"].ToString());

                        }
                    }
                    souce.Rows[i]["E"] = (E1 == 0 ? DBNull.Value as object : E1);
                    souce.Rows[i]["F"] = (F1 == 0 ? DBNull.Value as object : F1);
                    souce.Rows[i]["G"] = (G1 == 0 ? DBNull.Value as object : G1);
                    souce.Rows[i]["H"] = (H1 == 0 ? DBNull.Value as object : H1);
                    #endregion
                }

                if (souce.Rows[i][2].ToString() != "总计" && souce.Rows[i][2].ToString() != "小计")
                {
                    E += decimal.Parse(souce.Rows[i]["E"].ToString() == "" ? "0" : souce.Rows[i]["E"].ToString());
                    F += decimal.Parse(souce.Rows[i]["F"].ToString() == "" ? "0" : souce.Rows[i]["F"].ToString());
                    G += decimal.Parse(souce.Rows[i]["G"].ToString() == "" ? "0" : souce.Rows[i]["G"].ToString());
                    H += decimal.Parse(souce.Rows[i]["H"].ToString() == "" ? "0" : souce.Rows[i]["H"].ToString());
                }
            }
            souce.Rows.Add(new object[] { null, null, "总计", null, E, F, G, H });
            #endregion
            this.dataGridView1.DataSource = souce;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                EXCEL_MX("签订" + ClassCustom.codeSub1(this.toolStripComboBox2.Text) + "合同情况明细表", ClassConstant.DW_NAME + "    " + ClassCustom.codeSub1(this.toolStripComboBox1.Text), this.dateTimePicker1.Value.ToShortDateString() + "至" + this.dateTimePicker2.Value.ToShortDateString());
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }


        }

        private void EXCEL_MX(string name, string dw, string date)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("C4", "C6").Merge(false);
            excel.get_Range("D4", "D6").Merge(false);
            excel.get_Range("i4", "i6").Merge(false);

            excel.get_Range("e4", "h4").Merge(false);
            excel.get_Range("f5", "g5").Merge(false);
            excel.get_Range("e5", "e6").Merge(false);
            excel.get_Range("h5", "h6").Merge(false);



            

            excel.Cells[1, 1] = name;
            excel.Cells[3, 1] = dw + "    " + date;
            (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A4", "S4").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "m"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "g"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A1", "S6").Font.Bold = true;
            excel.Cells[4, "A"] = "序号";
            excel.Cells[4, "B"] = "内外";
            excel.Cells[4, "C"] = "合同号";
            excel.Cells[4, "D"] = "客户";
            excel.Cells[4, "e"] = "结算金额";
            excel.Cells[4, "i"] = "备注";
            excel.Cells[5, "e"] = "上年";
            excel.Cells[5, "h"] = "总累计";
            excel.Cells[5, "f"] = "本年";
            excel.Cells[6, "f"] = "本月";
            excel.Cells[6, "g"] = "本年";

            excel.get_Range("E7", excel.Cells[dataGridView1.Rows.Count + 6, "h"]).NumberFormat = "#,##0.00";

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
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$6";
            //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;
        }

    }
}
