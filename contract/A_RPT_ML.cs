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
    public partial class A_RPT_ML : Form
    {
        private string name;
        private DataTable souce;
        public A_RPT_ML()
        {
            InitializeComponent();
        }
        public A_RPT_ML(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.name == "销售合同毛利明细表")
                {
                    if (this.toolStripComboBox3.Text.ToString() == null)
                        return;

                    //销售部门	签订日期	内外部合同	合同号	客户

                    string sql = "SELECT CASE SUBSTRING(HYWY,1,6) WHEN '011003' THEN '一部' ELSE '二部' END 部门, 签定日期 签订日期, CASE SUBSTRING(HKH,1,2) WHEN '01' THEN '内部' ELSE '外部' END 客户类型,合同号,客户名,结算金额   FROM vcontracts  WHERE 1=1 "
                        + "AND HLX LIKE '02%' And HYWY LIKE '" + ClassCustom.codeSub(this.toolStripComboBox3.Text) + "%'"
                        + "AND 签定日期 >= '" + this.dateTimePicker1.Value.ToShortDateString() + "'"
                        + "AND 签定日期 <= '" + this.dateTimePicker2.Value.ToShortDateString() + "' ORDER BY 客户类型,客户名,合同号";
                    DataTable dt = DBAdo.DtFillSql(sql);
                    souce = new DataTable();
                    souce.Columns.Add("A", typeof(string));//
                    souce.Columns.Add("B", typeof(string));
                    souce.Columns.Add("C", typeof(string));//
                    souce.Columns.Add("D", typeof(string));//
                    souce.Columns.Add("E", typeof(string));//
                    souce.Columns.Add("F", typeof(decimal));
                    souce.Columns.Add("G", typeof(decimal));
                    souce.Columns.Add("H", typeof(decimal));
                    souce.Columns.Add("I", typeof(decimal));
                    souce.Columns.Add("J", typeof(decimal));
                    souce.Columns.Add("K", typeof(decimal));
                    souce.Columns.Add("L", typeof(decimal));
                    souce.Columns.Add("M", typeof(decimal));
                    souce.Columns.Add("N", typeof(string));
                    foreach (DataRow r in dt.Rows)
                    {
                        string A = r[0].ToString();
                        string B = DateTime.Parse(r[1].ToString() == "" ? "2010-01-01" : r[1].ToString()).ToString("yyyy-MM-dd");
                        string C = r[2].ToString();
                        string D = r[3].ToString();
                        string E = r[4].ToString();
                        decimal H = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString());
                        decimal F;
                        decimal G;
                        if (r[2].ToString() == "外部")
                        {
                            G = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) / 1.17M;
                            F = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) / 1.17M;
                        }
                        else
                        {
                            F = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString());
                            G = 0;
                        }
                        DataTable temp = DBAdo.DtFillSql("SELECT CASE SUBSTRING(HKH,1,2) WHEN '01' THEN '内部' ELSE '外部' END 客户类型, 结算金额 FROM vcontracts WHERE 合同号 IN (SELECT wxhth FROM AWX WHERE XSHTH = '" + r[3].ToString() + "')");
                        decimal I = 0;
                        decimal J = 0;
                        decimal K = 0;
                        foreach (DataRow c in temp.Rows)
                        {
                            K += decimal.Parse(c[1].ToString() == "" ? "0" : c[1].ToString());
                            if (c[0].ToString() == "外部")
                            {
                                I += decimal.Parse(c[1].ToString() == "" ? "0" : c[1].ToString()) / 1.17M;
                                J += decimal.Parse(c[1].ToString() == "" ? "0" : c[1].ToString()) - decimal.Parse(c[1].ToString() == "" ? "0" : c[1].ToString()) / 1.17M;
                            }
                            else
                            {
                                I += decimal.Parse(c[1].ToString() == "" ? "0" : c[1].ToString());
                                //J = 0;
                            }

                        }
                        decimal L = F - I;
                        decimal M = I / F;
                        string N = "";
                        souce.Rows.Add(new object[] { A, B, C, D, E, F, G, H, I, J, K, L, M, N });
                    }
                    this.dataGridView1.DataSource = souce;
                    this.dataGridView1.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.name == "销售合同毛利明细表")
            {
                EXCEL_ML(this.dateTimePicker1.Value.ToShortDateString() + " 至 " + this.dateTimePicker2.Value.ToShortDateString(), ClassConstant.DW_NAME + ClassCustom.codeSub1(this.toolStripComboBox3.Text));
            }
        }

        private void EXCEL_ML(string date, string dw)
        {

            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A5").Merge(false);
            excel.get_Range("B4", "B5").Merge(false);
            excel.get_Range("C4", "C5").Merge(false);
            excel.get_Range("D4", "D5").Merge(false);
            excel.get_Range("E4", "E5").Merge(false);
            //excel.get_Range("F4", "I4").Merge(false);
            excel.get_Range("l4", "l5").Merge(false);
            excel.get_Range("m4", "m5").Merge(false);
            excel.get_Range("n4", "n5").Merge(false);
            excel.get_Range("f4", "h4").Merge(false);
            excel.get_Range("i4", "k4").Merge(false);

            excel.Cells[1, 1] = "销售合同毛利明细表";
            excel.Cells[3, 1] = dw + "    " + date;
            (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A4", "m5").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A1", "S5").Font.Bold = true;

            excel.Cells[4, "A"] = "部门";
            excel.Cells[4, "B"] = "签订日期";
            excel.Cells[4, "C"] = "客户类型";
            excel.Cells[4, "D"] = "合同号";
            excel.Cells[4, "E"] = "客户";
            excel.Cells[4, "F"] = "销售合同总额";
            excel.Cells[4, "i"] = "外协合同总额";
            excel.Cells[5, "f"] = "收入";
            excel.Cells[5, "g"] = "税额";
            excel.Cells[5, "h"] = "小计";
            excel.Cells[5, "i"] = "成本";
            excel.Cells[5, "j"] = "税额";
            excel.Cells[5, "k"] = "小计";
            excel.Cells[4, "l"] = "产品毛利";
            excel.Cells[4, "m"] = "比率";
            excel.Cells[4, "n"] = "备注";

            excel.get_Range("F6", excel.Cells[dataGridView1.Rows.Count + 5, "M"]).NumberFormat = "#,##0.00";
            excel.get_Range("M6", excel.Cells[dataGridView1.Rows.Count + 5, "M"]).NumberFormat = "0%";
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
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$5";
            //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;
        }

        private void A_RPT_QT_Load(object sender, EventArgs e)
        {
            this.Text = name;
            foreach (DataRow r in DBAdo.DtFillSql("SELECT YCODE, YNAME FROM AYWY WHERE YCODE LIKE '" + ClassConstant.DW_ID + "__'").Rows)
            {
                this.toolStripComboBox3.Items.Add(r[0].ToString() + ":" + r[1].ToString());
            }

        }
    }
}
