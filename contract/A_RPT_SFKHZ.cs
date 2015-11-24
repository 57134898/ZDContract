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
    public partial class A_RPT_SFKHZ : Form
    {
        private string type;
        private DataTable dt1;

        public A_RPT_SFKHZ()
        {
            InitializeComponent();
        }

        public A_RPT_SFKHZ(string name, string type)
        {
            InitializeComponent();
            this.Text = name;
            this.type = type;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                dt1 = new DataTable();
                DataTable temp = DBAdo.DtFillSql("SELECT CCODE+':'+CNAME 客户,'' 来源 FROM ACLIENTS WHERE CCODE LIKE '01%'");
                temp.Rows[0][0] = "01:沈阳铸锻工业有限公司";
                dt1.Columns.Add("A", typeof(string));
                dt1.Columns.Add("B", typeof(string));
                dt1.Columns.Add("C", typeof(string));

                dt1.Columns.Add("D", typeof(decimal));
                dt1.Columns.Add("E", typeof(decimal));
                dt1.Columns.Add("F", typeof(decimal));
                dt1.Columns.Add("G", typeof(decimal));
                dt1.Columns.Add("H", typeof(decimal));
                dt1.Columns.Add("I", typeof(decimal));
                dt1.Columns.Add("J", typeof(decimal), "D+F+H");
                dt1.Columns.Add("K", typeof(decimal), "E+G+I");
                int index = 0;
                foreach (DataRow r in temp.Rows)
                {
                   
                    for (int i = 0; i < 3; i++)
                    {
                        object[] o = new object[11];

                        o[0] = index.ToString();
                        o[1] = r[0].ToString();
                        if (i == 0)
                        {
                            o[2] = "内部";
                            string sql3 = string.Format("SELECT SUM(cash) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) = {3}",
                                new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[3] = DBAdo.ExecuteScalarSql(sql3).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql3).ToString();
                            string sql4 = string.Format("SELECT SUM(cash) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) <= {3}",
                                 new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[4] = DBAdo.ExecuteScalarSql(sql4).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql4).ToString();
                            string sql5 = string.Format("SELECT SUM(note) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) = {3}",
                                new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[5] = DBAdo.ExecuteScalarSql(sql5).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql5).ToString();
                            string sql6 = string.Format("SELECT SUM(note) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) <= {3}",
                                   new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[6] = DBAdo.ExecuteScalarSql(sql6).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql6).ToString();
                            string sql7 = string.Format("SELECT SUM(mz) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}  AND MONTH(ExchangeDate) = {3}",
                                   new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[7] = DBAdo.ExecuteScalarSql(sql7).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql7).ToString();
                            string sql8 = string.Format("SELECT SUM(mz) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND  HDW LIKE '{0}%' AND HKH LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}   AND MONTH(ExchangeDate) <= {3}",
                                  new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[8] = DBAdo.ExecuteScalarSql(sql8).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql8).ToString();
                        }
                        else if (i == 1)
                        {
                            o[2] = "外部";
                            string sql3 = string.Format("SELECT SUM(cash) FROM Acash WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH NOT LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) = {3}",
                              new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[3] = DBAdo.ExecuteScalarSql(sql3).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql3).ToString();
                            string sql4 = string.Format("SELECT SUM(cash) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%'  AND HKH NOT LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) <= {3}",
                                 new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[4] = DBAdo.ExecuteScalarSql(sql4).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql4).ToString();
                            string sql5 = string.Format("SELECT SUM(note) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH NOT LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}  AND MONTH(ExchangeDate) = {3}",
                                new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[5] = DBAdo.ExecuteScalarSql(sql5).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql5).ToString();
                            string sql6 = string.Format("SELECT SUM(note) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH NOT LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}  AND MONTH(ExchangeDate) <= {3}",
                                 new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[6] = DBAdo.ExecuteScalarSql(sql6).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql6).ToString();
                            string sql7 = string.Format("SELECT SUM(mz) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH NOT LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}  AND MONTH(ExchangeDate) = {3}",
                                   new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[7] = DBAdo.ExecuteScalarSql(sql7).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql7).ToString();
                            string sql8 = string.Format("SELECT SUM(mz) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' AND HKH NOT LIKE '01__') AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}  AND MONTH(ExchangeDate) <= {3}",
                                 new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[8] = DBAdo.ExecuteScalarSql(sql8).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql8).ToString();
                        }
                        else
                        {
                            o[2] = "小计";
                            string sql3 = string.Format("SELECT SUM(CASH) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' ) AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) = {3}",
                              new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[3] = DBAdo.ExecuteScalarSql(sql3).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql3).ToString();
                            string sql4 = string.Format("SELECT SUM(CASH) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' ) AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2} AND MONTH(ExchangeDate) <= {3}",
                                 new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[4] = DBAdo.ExecuteScalarSql(sql4).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql4).ToString();
                            string sql5 = string.Format("SELECT SUM(NOTE) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' ) AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}   AND MONTH(ExchangeDate) = {3}",
                                new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[5] = DBAdo.ExecuteScalarSql(sql5).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql5).ToString();
                            string sql6 = string.Format("SELECT SUM(NOTE) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' ) AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}   AND MONTH(ExchangeDate) <= {3}",
                                 new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[6] = DBAdo.ExecuteScalarSql(sql6).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql6).ToString();
                            string sql7 = string.Format("SELECT SUM(MZ) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' ) AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}   AND MONTH(ExchangeDate) = {3}",
                                  new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[7] = DBAdo.ExecuteScalarSql(sql7).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql7).ToString();
                            string sql8 = string.Format("SELECT SUM(MZ) FROM ACASH WHERE CID IN (SELECT DISTINCT CID FROM ACONTRACT H, AFKXX F WHERE H.HCODE =F.HTH AND HDW LIKE '{0}%' ) AND TYPE = '{1}' AND YEAR(ExchangeDate) = {2}   AND MONTH(ExchangeDate) <= {3}",
                                 new object[] { ClassCustom.codeSub(r[0].ToString()), this.toolStripComboBox3.Text, this.toolStripComboBox1.Text, this.toolStripComboBox2.Text });
                            o[8] = DBAdo.ExecuteScalarSql(sql8).ToString() == "" ? "0" : DBAdo.ExecuteScalarSql(sql8).ToString();
                        }
                        dt1.Rows.Add(o);
                    }
                    index++;
                }
                dt1.Rows[0][0] = "";
                this.dataGridView1.DataSource = dt1;
                this.dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                EXCEL_HZ(this.toolStripComboBox3.Text == "付款" ? "各单位付款汇总表" : "各单位货款回收汇总表", this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void EXCEL_HZ(string name, string date)
        {
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", "l2").Merge(false);
            excel.get_Range("A3", "l3").Merge(false);
            excel.get_Range("A4", "A5").Merge(false);
            excel.get_Range("B4", "B5").Merge(false);
            excel.get_Range("C4", "C5").Merge(false);
            excel.get_Range("L4", "L5").Merge(false);
            excel.get_Range("J4", "K4").Merge(false);
            excel.get_Range("D4", "E4").Merge(false);
            excel.get_Range("F4", "G4").Merge(false);
            excel.get_Range("H4", "I4").Merge(false);



            excel.get_Range("A1", "A1").Value = name;
            excel.get_Range("A3", "A3").Value = date;


            excel.get_Range("A4", "A4").Value = "序号";
            excel.get_Range("B4", "B4").Value = "公司";
            //excel.get_Range("J4", "J4").Value = "备注";
            excel.get_Range("C4", "C4").Value = "来源";
            excel.get_Range("D4", "D4").Value = "现汇";
            excel.get_Range("F4", "F4").Value = "票据";
            excel.get_Range("H4", "H4").Value = "抹账";
            excel.get_Range("J4", "J4").Value = "小计";
            excel.get_Range("D5", "D5").Value = "本月";
            excel.get_Range("E5", "E5").Value = "本年累计";
            excel.get_Range("F5", "F5").Value = "本月";
            excel.get_Range("G5", "G5").Value = "本年累计";
            excel.get_Range("H5", "H5").Value = "本月";
            excel.get_Range("I5", "I5").Value = "本年累计";
            excel.get_Range("J5", "J5").Value = "本月";
            excel.get_Range("K5", "K5").Value = "本年累计";

            excel.get_Range("L4", "L4").Value = "备注";

            for (int i = 0; i < 33 / 3; i++)
            {
                excel.get_Range("A" + (i * 3 + 6), "A" + (i * 3 + 8)).Merge(false);
                excel.get_Range("b" + (i * 3 + 6), "b" + (i * 3 + 8)).Merge(false);
            }

            // return;

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

            excel.get_Range("A1", "L5").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A1", "L5").Font.Bold = true;

            excel.get_Range("A6", "C38").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range("A6", "K8").Font.Bold = true;
            excel.get_Range("A1", "L38").EntireColumn.AutoFit();

            //excel.get_Range("C6", excel.Cells[dataGridView1.Rows.Count + 5, "J"]).NumberFormat = "#,##0.00";
            excel.get_Range("C6", excel.Cells[dataGridView1.Rows.Count + 5, "J"]).NumberFormat = "_ * #,##0.00_ ;_ * -#,##0.00_ ;_ * -??_ ;_ @_ ";

            excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count + 1]).EntireColumn.AutoFit();
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 5, dataGridView1.Columns.Count + 1]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$5";
            return;
            //excel.get_Range("A" + (this.dataGridView1.Rows.Count + 4).ToString(), "B" + (this.dataGridView1.Rows.Count + 6).ToString()).Merge(false);
            ////(excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ////(excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            //excel.get_Range("A1", "T6").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //excel.get_Range("A3", "A3").HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            ////(excel.Cells[5, "m"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ////(excel.Cells[5, "g"] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //excel.get_Range("A1", "t6").Font.Bold = true;
            //excel.Cells[4, "A"] = "序号";
            //excel.Cells[4, "B"] = "合同种类";
            //excel.get_Range("A4", "A6").Value = "序号";
            //excel.get_Range("B4", "c6").Value = "公司名称";
            //excel.get_Range("D4", "G4").Value = "合同总额";
            //excel.get_Range("H4", "K4").Value = (this.toolStripComboBox4.Text == "产品销售总览表" ? "已收货款" : "已付货款");
            //excel.get_Range("L4", "M4").Value = (this.toolStripComboBox4.Text == "产品销售总览表" ? "未收货款" : "未付货款");
            //excel.get_Range("N4", "Q4").Value = (this.toolStripComboBox4.Text == "产品销售总览表" ? "已开发票金额" : "已收发票金额");
            //excel.get_Range("R4", "S4").Value = (this.toolStripComboBox4.Text == "产品销售总览表" ? "未开票金额" : "未收票金额");
            //excel.get_Range("E5", "F5").Value = "本年";
            //excel.get_Range("I5", "J5").Value = "本年";
            //excel.get_Range("O5", "P5").Value = "本年";
            //excel.get_Range("D5", "D6").Value = "以前年度";
            //excel.get_Range("G5", "G6").Value = "总累计";
            //excel.get_Range("H5", "H6").Value = "以前年度";
            //excel.get_Range("K5", "K6").Value = "总累计";
            //excel.get_Range("L5", "L6").Value = "金额";
            //excel.get_Range("M5", "M6").Value = "比例";
            //excel.get_Range("N5", "N6").Value = "以前年度";
            //excel.get_Range("Q5", "Q6").Value = "总累计";
            //excel.get_Range("R5", "R6").Value = "金额";
            //excel.get_Range("S5", "S6").Value = "比例";
            //excel.get_Range("T4", "T6").Value = "备注";
            //excel.get_Range("E6", "E6").Value = "本月";
            //excel.get_Range("F6", "F6").Value = "本年";
            //excel.get_Range("I6", "I6").Value = "本月";
            //excel.get_Range("J6", "J6").Value = "本年";
            //excel.get_Range("O6", "O6").Value = "本月";
            //excel.get_Range("P6", "P6").Value = "本年";
            //excel.get_Range("A" + (this.dataGridView1.Rows.Count + 4).ToString(), "B" + (this.dataGridView1.Rows.Count + 4).ToString()).Value = "总计";
            //excel.get_Range("c" + (this.dataGridView1.Rows.Count + 4).ToString(), "c" + (this.dataGridView1.Rows.Count + 4).ToString()).Value = "外部";
            //excel.get_Range("c" + (this.dataGridView1.Rows.Count + 5).ToString(), "c" + (this.dataGridView1.Rows.Count + 5).ToString()).Value = "内部";
            //excel.get_Range("c" + (this.dataGridView1.Rows.Count + 6).ToString(), "c" + (this.dataGridView1.Rows.Count + 6).ToString()).Value = "合计";
            //excel.get_Range("a1", "t2").Value = name;
            //excel.get_Range("a3", "a3").Value = this.toolStripComboBox4.Text + "      " + this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月";


            ////DataTable dt = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LID LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "__'");
            //string str = ClassCustom.codeSub(this.toolStripComboBox4.Text);

            //DataTable dt = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE  CCODE LIKE '01__'");
            //int index = 0;

            //for (int i = 0; i < dt.Rows.Count * 2; i++)
            //{
            //    excel.get_Range("B" + (i + 7).ToString(), "B" + (i + 8).ToString()).Merge(false);
            //    excel.get_Range("B" + (i + 7).ToString(), "B" + (i + 8).ToString()).Value = dt.Rows[index][1].ToString();
            //    excel.get_Range("A" + (i + 7).ToString(), "A" + (i + 8).ToString()).Merge(false);
            //    excel.get_Range("A" + (i + 7).ToString(), "A" + (i + 8).ToString()).Value = (index + 1).ToString();
            //    excel.get_Range("C" + (i + 7).ToString(), "C" + (i + 7).ToString()).Value = "外部";
            //    excel.get_Range("C" + (i + 8).ToString(), "C" + (i + 8).ToString()).Value = "内部";
            //    index++;
            //    i++;
            //}



            //excel.get_Range("d7", excel.Cells[dataGridView1.Rows.Count + 6, "t"]).NumberFormat = "#,##0.00";
            //excel.get_Range("m7", excel.Cells[dataGridView1.Rows.Count + 6, "m"]).NumberFormat = "0%";
            //excel.get_Range("s7", excel.Cells[dataGridView1.Rows.Count + 6, "s"]).NumberFormat = "0%";
            ////填充数据
            //for (int i = 0; i < dataGridView1.RowCount; i++)
            //{
            //    for (int j = 3; j < dataGridView1.ColumnCount; j++)
            //    {
            //        if (dataGridView1[j, i].ValueType == typeof(string))
            //        {
            //            excel.Cells[i + 7, j + 1] = "'" + dataGridView1[j, i].Value.ToString();
            //        }
            //        else
            //        {
            //            excel.Cells[i + 7, j + 1] = dataGridView1[j, i].Value.ToString();
            //        }
            //    }
            //}


            //excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
            //ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]);
            //Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            //sheet1.PageSetup.PrintTitleRows = "$1:$6";
            //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;
        }

        private void A_RPT_SFKHZ_Load(object sender, EventArgs e)
        {
            this.toolStripComboBox1.Text = DateTime.Now.Year.ToString();
            this.toolStripComboBox2.Text = DateTime.Now.Month.ToString();
            this.toolStripComboBox3.Text = "回款";
        }

    }
}
