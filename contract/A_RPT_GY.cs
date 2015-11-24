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
    public partial class A_RPT_GY : Form
    {
        private string name;
        private DataTable souce = new DataTable();
        public A_RPT_GY()
        {
            InitializeComponent();
        }
        public A_RPT_GY(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        private void A_RPT_GY_Load(object sender, EventArgs e)
        {
            this.toolStripComboBox1.Text = DateTime.Now.Year.ToString();
            this.toolStripComboBox2.Text = DateTime.Now.Month.ToString();
            this.Text = name;
            foreach (DataRow r in DBAdo.DtFillSql("SELECT YCODE, YNAME FROM AYWY WHERE YCODE LIKE '" + ClassConstant.DW_ID + "__'").Rows)
            {
                this.toolStripComboBox3.Items.Add(r[0].ToString() + ":" + r[1].ToString());
            }
            souce.Columns.Add("A", typeof(int));//
            souce.Columns.Add("B", typeof(string));
            souce.Columns.Add("C", typeof(string));//
            souce.Columns.Add("D", typeof(string));//
            souce.Columns.Add("E", typeof(decimal));//
            souce.Columns.Add("F", typeof(decimal));
            souce.Columns.Add("G", typeof(decimal));
            souce.Columns.Add("H", typeof(decimal));
            souce.Columns.Add("I", typeof(string));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            souce.Rows.Clear();
            string sql = "SELECT 合同号,客户名 ,结算金额 FROM VCONTRACTS WHERE 1=1 "
                         + "AND HYWY LIKE '" + ClassCustom.codeSub(this.toolStripComboBox3.Text) + "%'"
                         + "AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' order by 客户名,合同号";
            DataTable dt = DBAdo.DtFillSql(sql);
            foreach (DataRow r in dt.Rows)
            {
                object EE1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH = '" + r[0].ToString() + "' AND TYPE = '估验' AND ((YEAR(DATE) = '" + this.toolStripComboBox1.Text + "' AND MONTH(DATE) < '" + this.toolStripComboBox2.Text + "') OR (YEAR(DATE) < '" + this.toolStripComboBox1.Text + "'))");
                //object EE2 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH = '" + r[0].ToString() + "' AND TYPE = '估验'  AND ((YEAR(DATE) = '" + this.toolStripComboBox1.Text + "' AND MONTH(DATE) < '" + this.toolStripComboBox2.Text + "') OR (YEAR(DATE) < '" + this.toolStripComboBox1.Text + "'))");

                object FF1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH = '" + r[0].ToString() + "' AND TYPE = '估验'  AND (YEAR(DATE) = '" + this.toolStripComboBox1.Text + "' AND MONTH(DATE) = '" + this.toolStripComboBox2.Text + "') AND RMB > 0");
                object GG1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH = '" + r[0].ToString() + "' AND TYPE = '估验'  AND (YEAR(DATE) = '" + this.toolStripComboBox1.Text + "' AND MONTH(DATE) = '" + this.toolStripComboBox2.Text + "' AND RMB < 0)");

                decimal EE = 0;
                decimal FF = 0;
                decimal GG = 0;
                decimal HH = 0;
                //EE = decimal.Parse(r[2].ToString() == "" ? "0" : r[2].ToString()) - decimal.Parse(EE1.ToString() == "" ? "0" : EE1.ToString()) - decimal.Parse(EE2.ToString() == "" ? "0" : EE2.ToString());
                EE = decimal.Parse(EE1.ToString() == "" ? "0" : EE1.ToString());
                FF = decimal.Parse(FF1.ToString() == "" ? "0" : FF1.ToString());
                GG = decimal.Parse(GG1.ToString() == "" ? "0" : GG1.ToString());
                HH = EE + FF + GG;
                souce.Rows.Add(new object[] { 0, r[0], r[1], "", EE, FF, GG, HH });
            }
            DataView dvv = souce.DefaultView;
            dvv.RowFilter = "E<>0 OR F <>0 OR  g<>0";
            souce = dvv.ToTable();

            #region 求合计
            int count = souce.Rows.Count;
            souce.Rows.Add(new object[] { null, null, "总计" });
            int xh = 1;
            for (int i = 0; i < souce.Rows.Count; i++)
            {
                if (souce.Rows[i][1].ToString() != "小计")
                {
                    souce.Rows[i][0] = (xh++).ToString();
                }
                if (i > 0)
                {

                    if (souce.Rows[i][2].ToString() != souce.Rows[i - 1][2].ToString())
                    {
                        souce.Rows.InsertAt(souce.NewRow(), i++);
                        souce.Rows[i - 1][1] = "小计";
                        souce.Rows[i - 1][2] = souce.Rows[i - 2][2];
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

                if (souce.Rows[i][1].ToString() == "小计")
                {
                    #region 求小计
                    decimal E1 = 0;
                    decimal F1 = 0;
                    decimal G1 = 0;
                    decimal H1 = 0;
                    for (int k = 0; k < souce.Rows.Count; k++)
                    {
                        if (souce.Rows[k]["C"].ToString() == souce.Rows[i]["C"].ToString() && souce.Rows[k]["B"].ToString() != "小计")
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

                if (souce.Rows[i][1].ToString() != "总计" && souce.Rows[i][1].ToString() != "小计")
                {
                    E += decimal.Parse(souce.Rows[i]["E"].ToString() == "" ? "0" : souce.Rows[i]["E"].ToString());
                    F += decimal.Parse(souce.Rows[i]["F"].ToString() == "" ? "0" : souce.Rows[i]["F"].ToString());
                    G += decimal.Parse(souce.Rows[i]["G"].ToString() == "" ? "0" : souce.Rows[i]["G"].ToString());
                    H += decimal.Parse(souce.Rows[i]["H"].ToString() == "" ? "0" : souce.Rows[i]["H"].ToString());
                }
            }
            souce.Rows.Add(new object[] { null, "总计", null, null, E, F, G, H });
            #endregion

            this.dataGridView1.DataSource = souce;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                EXCEL_MX(ClassCustom.codeSub1(this.toolStripComboBox4.Text) + "估验报表", ClassConstant.DW_NAME + "   " + ClassCustom.codeSub1(this.toolStripComboBox3.Text), this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");

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
            try
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
                excel.get_Range("I4", "I5").Merge(false);
                excel.get_Range("H4", "H5").Merge(false);
                excel.get_Range("F4", "G4").Merge(false);

                excel.Cells[1, 1] = name;
                excel.Cells[3, 1] = dw + "    " + date;
                (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                excel.get_Range("A4", "m5").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                excel.get_Range("A1", "S5").Font.Bold = true;
                excel.Cells[4, "A"] = "序号";
                excel.Cells[4, "B"] = "合同号";
                excel.Cells[4, "C"] = "客户";
                excel.Cells[4, "D"] = "产品名称";
                excel.Cells[4, "E"] = "上月余额";
                excel.Cells[4, "F"] = "本月";
                excel.Cells[4, "H"] = "结转下月余额";
                excel.Cells[5, "f"] = "估验金额";
                excel.Cells[5, "g"] = "冲估验金额";
                excel.Cells[4, "I"] = "备注";
                excel.get_Range("D6", excel.Cells[dataGridView1.Rows.Count + 5, "H"]).NumberFormat = "#,##0.00";
                //填充数据

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    //if (decimal.Parse(dataGridView1[5, i].Value.ToString() == "" ? "0" : dataGridView1[5, i].Value.ToString()) == 0 && decimal.Parse(dataGridView1[6, i].Value.ToString() == "" ? "0" : dataGridView1[6, i].Value.ToString()) == 0)
                    //{
                    //    continue;
                    //}
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
                //for (int i = 7; i < this.dataGridView1.Rows.Count; i++)
                //{
                //    if (excel.Cells[i, 2].ToString() == "System.__ComObject")
                //    {
                //        excel.get_Range("A" + i, "J" + i).Select();
                //        excel.get_Range("A" + i, "J" + i).Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                //    }
                //}
                excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
                ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count]);
                Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
                sheet1.PageSetup.PrintTitleRows = "$1:$5";
                //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
        }
    }
}
