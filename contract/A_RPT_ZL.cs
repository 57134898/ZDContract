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
    public partial class A_RPT_ZL : Form
    {
        private string name;
        private DataTable souce;

        public A_RPT_ZL()
        {
            InitializeComponent();
        }

        public A_RPT_ZL(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        private void A_RPT_ZL_Load(object sender, EventArgs e)
        {
            this.Text = name;
            souce = new DataTable();
            souce.Columns.Add("A", typeof(int));//序号
            souce.Columns.Add("B", typeof(string));//合同各类
            souce.Columns.Add("C", typeof(string));//内外
            souce.Columns.Add("D", typeof(decimal));//上年
            souce.Columns.Add("E", typeof(decimal));//本月
            souce.Columns.Add("F", typeof(decimal));//本年
            souce.Columns.Add("G", typeof(decimal));//总累计
            souce.Columns.Add("H", typeof(decimal));//上年
            souce.Columns.Add("I", typeof(decimal));//本月
            souce.Columns.Add("J", typeof(decimal));//本年
            souce.Columns.Add("K", typeof(decimal));//总累计
            souce.Columns.Add("L", typeof(decimal));//金额
            souce.Columns.Add("M", typeof(decimal));//比例
            souce.Columns.Add("N", typeof(decimal));//上年
            souce.Columns.Add("O", typeof(decimal));//本月
            souce.Columns.Add("P", typeof(decimal));//本年
            souce.Columns.Add("Q", typeof(decimal));//总累计
            souce.Columns.Add("R", typeof(decimal));//金额
            souce.Columns.Add("S", typeof(decimal));//比例
            souce.Columns.Add("T", typeof(string));//备注

            this.dataGridView1.DataSource = souce;
            foreach (DataGridViewColumn c in this.dataGridView1.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.toolStripComboBox1.Text = DateTime.Now.Year.ToString();
            this.toolStripComboBox2.Text = DateTime.Now.Month.ToString();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            EXCEL_FKMX();
        }

        private void EXCEL_FKMX()
        {
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "c6").Merge(false);
            excel.get_Range("D4", "G4").Merge(false);
            excel.get_Range("H4", "K4").Merge(false);
            excel.get_Range("L4", "M4").Merge(false);
            excel.get_Range("N4", "Q4").Merge(false);
            excel.get_Range("R4", "S4").Merge(false);
            excel.get_Range("E5", "F5").Merge(false);
            excel.get_Range("I5", "J5").Merge(false);
            excel.get_Range("O5", "P5").Merge(false);
            excel.get_Range("D5", "D6").Merge(false);
            excel.get_Range("G5", "G6").Merge(false);
            excel.get_Range("H5", "H6").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("L5", "L6").Merge(false);
            excel.get_Range("M5", "M6").Merge(false);
            excel.get_Range("N5", "N6").Merge(false);
            excel.get_Range("Q5", "Q6").Merge(false);
            excel.get_Range("R5", "R6").Merge(false);
            excel.get_Range("S5", "S6").Merge(false);
            excel.get_Range("T4", "T6").Merge(false);
            excel.get_Range("A" + (this.dataGridView1.Rows.Count + 4).ToString(), "B" + (this.dataGridView1.Rows.Count + 6).ToString()).Merge(false);
            //(excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A1", "T6").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A3", "A3").HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            //(excel.Cells[5, "m"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //(excel.Cells[5, "g"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A1", "t6").Font.Bold = true;
            excel.Cells[4, "A"] = "序号";
            excel.Cells[4, "B"] = "合同种类";
            excel.get_Range("A4", "A6").Value = "序号";
            excel.get_Range("B4", "c6").Value = "公司名称";
            excel.get_Range("D4", "G4").Value = "合同总额";
            excel.get_Range("H4", "K4").Value = (this.toolStripComboBox4.Text == "0201:产品销售" ? "已收货款" : "已付货款");
            excel.get_Range("L4", "M4").Value = (this.toolStripComboBox4.Text == "0201:产品销售" ? "未收货款" : "未付货款");
            excel.get_Range("N4", "Q4").Value = (this.toolStripComboBox4.Text == "0201:产品销售" ? "已开发票金额" : "已收发票金额");
            excel.get_Range("R4", "S4").Value = (this.toolStripComboBox4.Text == "0201:产品销售" ? "未开票金额" : "未收票金额");
            excel.get_Range("E5", "F5").Value = "本年";
            excel.get_Range("I5", "J5").Value = "本年";
            excel.get_Range("O5", "P5").Value = "本年";
            excel.get_Range("D5", "D6").Value = "以前年度";
            excel.get_Range("G5", "G6").Value = "总累计";
            excel.get_Range("H5", "H6").Value = "以前年度";
            excel.get_Range("K5", "K6").Value = "总累计";
            excel.get_Range("L5", "L6").Value = "金额";
            excel.get_Range("M5", "M6").Value = "比例";
            excel.get_Range("N5", "N6").Value = "以前年度";
            excel.get_Range("Q5", "Q6").Value = "总累计";
            excel.get_Range("R5", "R6").Value = "金额";
            excel.get_Range("S5", "S6").Value = "比例";
            excel.get_Range("T4", "T6").Value = "备注";
            excel.get_Range("E6", "E6").Value = "本月";
            excel.get_Range("F6", "F6").Value = "本年";
            excel.get_Range("I6", "I6").Value = "本月";
            excel.get_Range("J6", "J6").Value = "本年";
            excel.get_Range("O6", "O6").Value = "本月";
            excel.get_Range("P6", "P6").Value = "本年";
            excel.get_Range("A" + (this.dataGridView1.Rows.Count + 4).ToString(), "B" + (this.dataGridView1.Rows.Count + 4).ToString()).Value = "总计";
            excel.get_Range("c" + (this.dataGridView1.Rows.Count + 4).ToString(), "c" + (this.dataGridView1.Rows.Count + 4).ToString()).Value = "外部";
            excel.get_Range("c" + (this.dataGridView1.Rows.Count + 5).ToString(), "c" + (this.dataGridView1.Rows.Count + 5).ToString()).Value = "内部";
            excel.get_Range("c" + (this.dataGridView1.Rows.Count + 6).ToString(), "c" + (this.dataGridView1.Rows.Count + 6).ToString()).Value = "合计";
            excel.get_Range("a1", "t2").Value = ClassCustom.codeSub1(this.toolStripComboBox4.Text) + "合同总览表";
            excel.get_Range("a3", "a3").Value = this.toolStripComboBox4.Text + "      " + this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月";


            //DataTable dt = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LID LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "__'");
            string str = ClassCustom.codeSub(this.toolStripComboBox4.Text);

            DataTable dt = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE  CCODE LIKE '01__'");
            int index = 0;

            for (int i = 0; i < dt.Rows.Count * 2; i++)
            {
                excel.get_Range("B" + (i + 7).ToString(), "B" + (i + 8).ToString()).Merge(false);
                excel.get_Range("B" + (i + 7).ToString(), "B" + (i + 8).ToString()).Value = dt.Rows[index][1].ToString();
                excel.get_Range("A" + (i + 7).ToString(), "A" + (i + 8).ToString()).Merge(false);
                excel.get_Range("A" + (i + 7).ToString(), "A" + (i + 8).ToString()).Value = (index + 1).ToString();
                excel.get_Range("C" + (i + 7).ToString(), "C" + (i + 7).ToString()).Value = "外部";
                excel.get_Range("C" + (i + 8).ToString(), "C" + (i + 8).ToString()).Value = "内部";
                index++;
                i++;
            }



            excel.get_Range("d7", excel.Cells[dataGridView1.Rows.Count + 6, "t"]).NumberFormat = "#,##0.00";
            excel.get_Range("m7", excel.Cells[dataGridView1.Rows.Count + 6, "m"]).NumberFormat = "0%";
            excel.get_Range("s7", excel.Cells[dataGridView1.Rows.Count + 6, "s"]).NumberFormat = "0%";
            //填充数据
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 3; j < dataGridView1.ColumnCount; j++)
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                souce.Rows.Clear();
                string str = ClassCustom.codeSub(this.toolStripComboBox4.Text);

                DataTable dt = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE  CCODE LIKE '01__'");
                int index = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {

                        souce.Rows.Add(null, dt.Rows[i][0].ToString() + ":" + dt.Rows[i][1].ToString(), j == 0 ? "外部" : "内部", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        souce.Rows[index][0] = index.ToString();
                        index++;
                    }
                }
                foreach (DataRow r in souce.Rows)
                {
                    string filter = " AND HDW = '" + ClassCustom.codeSub(r[1].ToString()) + "' AND HLX = '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "' AND " + (r[2].ToString() == "内部" ? "HKH LIKE '01__'" : "HKH NOT LIKE '01__'");
                    string lastYear = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE 1=1 AND year(签定日期) < " + this.toolStripComboBox1.Text + " " + filter).ToString();
                    string thisMonth = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE 1=1 AND year(签定日期) = " + this.toolStripComboBox1.Text + " AND MONTH(签定日期) = " + this.toolStripComboBox2.Text + " " + filter).ToString();
                    string thisYear = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE 1=1 AND  year(签定日期) = " + this.toolStripComboBox1.Text + " AND MONTH(签定日期) <= " + this.toolStripComboBox2.Text + " " + filter).ToString();
                    string all = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE 1=1 AND (year(签定日期) < " + this.toolStripComboBox1.Text + "OR ( year(签定日期) = " + this.toolStripComboBox1.Text + " AND MONTH(签定日期) <= " + this.toolStripComboBox2.Text + ") )" + filter).ToString();

                    string sql_fir = "select sum(f.rmb) rmb from afkxx f inner join acontract h  on f.hth=h.hcode ";

                    string lastYear1 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND year(date) < " + this.toolStripComboBox1.Text + " " + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "回款" : "付款") + "'").ToString();
                    string thisMonth1 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND year(date) = " + this.toolStripComboBox1.Text + "  AND MONTH(date) = " + this.toolStripComboBox2.Text + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "回款" : "付款") + "'").ToString();
                    string thisYear1 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND  year(date) = " + this.toolStripComboBox1.Text + " AND MONTH(date) <= " + this.toolStripComboBox2.Text + " " + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "回款" : "付款") + "'").ToString();
                    string all1 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND (year(date) < " + this.toolStripComboBox1.Text + "OR ( year(date) = " + this.toolStripComboBox1.Text + " AND MONTH(date) <= " + this.toolStripComboBox2.Text + ") )" + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "回款" : "付款") + "'").ToString();


                    string lastYear2 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND year(date) < " + this.toolStripComboBox1.Text + " " + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "销项发票" : "进项发票") + "'").ToString();
                    string thisMonth2 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND year(date) = " + this.toolStripComboBox1.Text + "  AND MONTH(date) = " + this.toolStripComboBox2.Text + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "销项发票" : "进项发票") + "'").ToString();
                    string thisYear2 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND  year(date) = " + this.toolStripComboBox1.Text + " AND MONTH(date) <= " + this.toolStripComboBox2.Text + " " + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "销项发票" : "进项发票") + "'").ToString();
                    string all2 = DBAdo.ExecuteScalarSql(sql_fir + " WHERE 1=1 AND (year(date) < " + this.toolStripComboBox1.Text + "OR ( year(date) = " + this.toolStripComboBox1.Text + " AND MONTH(date) <= " + this.toolStripComboBox2.Text + ") )" + filter + " AND TYPE = '" + (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "02" ? "销项发票" : "进项发票") + "'").ToString();

                    r[3] = decimal.Parse((lastYear.ToString() == "" ? "0" : lastYear.ToString()));
                    r[4] = decimal.Parse((thisMonth.ToString() == "" ? "0" : thisMonth.ToString()));
                    r[5] = decimal.Parse((thisYear.ToString() == "" ? "0" : thisYear.ToString()));
                    r[6] = decimal.Parse((all.ToString() == "" ? "0" : all.ToString()));
                    r[7] = decimal.Parse((lastYear1.ToString() == "" ? "0" : lastYear1.ToString()));
                    r[8] = decimal.Parse((thisMonth1.ToString() == "" ? "0" : thisMonth1.ToString()));
                    r[9] = decimal.Parse((thisYear1.ToString() == "" ? "0" : thisYear1.ToString()));
                    r[10] = decimal.Parse((all1.ToString() == "" ? "0" : all1.ToString()));
                    r[11] = decimal.Parse((all.ToString() == "" ? "0" : all.ToString())) - decimal.Parse((all1.ToString() == "" ? "0" : all1.ToString()));
                    r[12] = (decimal.Parse((all.ToString() == "" ? "0" : all.ToString())) - decimal.Parse((all1.ToString() == "" ? "0" : all1.ToString()))) / decimal.Parse((all.ToString() == "" ? "1" : all.ToString()));
                    r[13] = decimal.Parse((lastYear2.ToString() == "" ? "0" : lastYear2.ToString()));
                    r[14] = decimal.Parse((thisMonth2.ToString() == "" ? "0" : thisMonth2.ToString()));
                    r[15] = decimal.Parse((thisYear2.ToString() == "" ? "0" : thisYear2.ToString()));
                    r[16] = decimal.Parse((all2.ToString() == "" ? "0" : all2.ToString()));
                    r[17] = decimal.Parse((all.ToString() == "" ? "0" : all.ToString())) - decimal.Parse((all2.ToString() == "" ? "0" : all2.ToString()));
                    r[18] = (decimal.Parse((all.ToString() == "" ? "0" : all.ToString())) - decimal.Parse((all2.ToString() == "" ? "0" : all2.ToString()))) / decimal.Parse((all.ToString() == "" ? "1" : all.ToString()));

                }
                decimal D = 0;
                decimal E = 0;
                decimal F = 0;
                decimal G = 0;
                decimal H = 0;
                decimal I = 0;
                decimal J = 0;
                decimal K = 0;
                decimal L = 0;
                decimal M = 0;
                decimal N = 0;
                decimal O = 0;
                decimal P = 0;
                decimal Q = 0;
                decimal R = 0;
                decimal S = 0;
                decimal D1 = 0;
                decimal E1 = 0;
                decimal F1 = 0;
                decimal G1 = 0;
                decimal H1 = 0;
                decimal I1 = 0;
                decimal J1 = 0;
                decimal K1 = 0;
                decimal L1 = 0;
                decimal M1 = 0;
                decimal N1 = 0;
                decimal O1 = 0;
                decimal P1 = 0;
                decimal Q1 = 0;
                decimal R1 = 0;
                decimal S1 = 0;
                foreach (DataRow r in souce.Rows)
                {
                    if (r[2].ToString() == "外部")
                    {
                        D += decimal.Parse(r[3].ToString());
                        E += decimal.Parse(r[4].ToString());
                        F += decimal.Parse(r[5].ToString());
                        G += decimal.Parse(r[6].ToString());
                        H += decimal.Parse(r[7].ToString());
                        I += decimal.Parse(r[8].ToString());
                        J += decimal.Parse(r[9].ToString());
                        K += decimal.Parse(r[10].ToString());
                        L += decimal.Parse(r[11].ToString());
                        M += decimal.Parse(r[12].ToString());
                        N += decimal.Parse(r[13].ToString());
                        O += decimal.Parse(r[14].ToString());
                        P += decimal.Parse(r[15].ToString());
                        Q += decimal.Parse(r[16].ToString());
                        R += decimal.Parse(r[17].ToString());
                        S += decimal.Parse(r[18].ToString());
                    }
                    else
                    {
                        D1 += decimal.Parse(r[3].ToString());
                        E1 += decimal.Parse(r[4].ToString());
                        F1 += decimal.Parse(r[5].ToString());
                        G1 += decimal.Parse(r[6].ToString());
                        H1 += decimal.Parse(r[7].ToString());
                        I1 += decimal.Parse(r[8].ToString());
                        J1 += decimal.Parse(r[9].ToString());
                        K1 += decimal.Parse(r[10].ToString());
                        L1 += decimal.Parse(r[11].ToString());
                        M1 += decimal.Parse(r[12].ToString());
                        N1 += decimal.Parse(r[13].ToString());
                        O1 += decimal.Parse(r[14].ToString());
                        P1 += decimal.Parse(r[15].ToString());
                        Q1 += decimal.Parse(r[16].ToString());
                        R1 += decimal.Parse(r[17].ToString());
                        S1 += decimal.Parse(r[18].ToString());
                    }

                }
                souce.Rows.Add(null, null, "外部", D, E, F, G, H, I, J, K, L, (L / (G == 0 ? 1 : G)), N, O, P, Q, R, (R / (G == 0 ? 1 : G)), null);
                souce.Rows.Add(null, null, "内部", D1, E1, F1, G1, H1, I1, J1, K1, L1, (L1 / (G1 == 0 ? 1 : G1)), N1, O1, P1, Q1, R1, (R1 / (G1 == 0 ? 1 : G1)), null);
                souce.Rows.Add(null, null, "合计", D + D1, E + E1, F + F1, G + G1, H + H1, I + I1, J + J1, K + K1, L + L1, (L + L1) / ((G + G1) == 0 ? 1 : (G + G1)), N + N1, O + O1, P + P1, Q + Q1, R + R1, (R + R1) / ((G + G1) == 0 ? 1 : (G + G1)), null);
                foreach (DataRow r in souce.Rows)
                {
                    for (int i = 0; i < souce.Columns.Count; i++) if (souce.Columns[i].DataType == typeof(decimal)) if (decimal.Parse(r[i].ToString()) == 0)
                                r[i] = DBNull.Value;
                }
                this.dataGridView1.DataSource = souce;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
    }
}
