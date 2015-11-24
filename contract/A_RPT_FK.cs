using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Reporting.WinForms;


namespace contract
{
    public partial class A_RPT_FK : Form
    {
        private string name;
        private DataTable souce = new DataTable();
        public A_RPT_FK()
        {
            InitializeComponent();
            //string str = "user id=sa;password='';initial catalog=contract1;datasource=.;Provider=SQLOLEDB;connect Timeout=20";
            //DBAdo.setConStr(str);
        }

        public A_RPT_FK(string name)
        {
            InitializeComponent();
            this.name = name;

        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.toolStripComboBox5.Text == "" || this.toolStripComboBox1.Text == "" || this.toolStripComboBox2.Text == "" || (this.toolStripComboBox3.Visible == true && this.toolStripComboBox3.Text == "") || this.toolStripComboBox4.Text == "")
                return;
            try
            {
                if (this.name == "付款情况明细表")
                {
                    BB_FK();
                }
                if (this.name == "回款情况明细表")
                {
                    BB_SK();
                }
                if (this.name == "付款情况汇总表")
                {
                    BB_FK_HZ();
                }
                if (this.name == "回款情况汇总表")
                {
                    BB_SK_HZ();
                }


            


                foreach (DataGridViewColumn c in this.dataGridView1.Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
        }
        /// <summary>
        /// 回款情况明细表生成数据
        /// </summary>
        private void BB_SK()
        {
            //Thread.Sleep(1000);
            this.toolStripProgressBar1.Value = 0;

            string sql = "SELECT * FROM vcontracts WHERE 1=1 AND (YEAR(签定日期) <  '" + this.toolStripComboBox1.Text + "' OR (YEAR(签定日期) =  '" + this.toolStripComboBox1.Text + "' AND MONTH(签定日期) <=  '" + this.toolStripComboBox2.Text + "')) AND HDW = '" + ClassConstant.DW_ID + "' AND HLX  LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HYWY LIKE '" + ClassCustom.codeSub(this.toolStripComboBox3.Text) + "%'";
            DataTable dt = DBAdo.DtFillSql(sql);
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum / 10) { this.toolStripProgressBar1.Value++; }
            souce.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {//回款 销项发票
                object lastYear = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)< '" + this.toolStripComboBox1.Text + "' AND (type ='回款' OR type ='抵消') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisMonth = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)= '" + this.toolStripComboBox2.Text + "' AND (type ='回款' OR type ='抵消') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisYear = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)<='" + this.toolStripComboBox2.Text + "' AND (type ='回款' OR type ='抵消') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");

                object lastYear1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)< '" + this.toolStripComboBox1.Text + "' AND (type ='销项发票') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisMonth1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)= '" + this.toolStripComboBox2.Text + "' AND (type ='销项发票') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisYear1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)<='" + this.toolStripComboBox2.Text + "' AND (type ='销项发票') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");

                decimal zlj1 = decimal.Parse(lastYear1.ToString() == "" ? "0" : lastYear1.ToString()) + decimal.Parse(thisYear1.ToString() == "" ? "0" : thisYear1.ToString());
                decimal zlj = decimal.Parse(lastYear.ToString() == "" ? "0" : lastYear.ToString()) + decimal.Parse(thisYear.ToString() == "" ? "0" : thisYear.ToString());

                souce.Rows.Add(new object[] { i + 1, dt.Rows[i]["HKH"].ToString().Substring(0,2)=="01"?"内":"外",
                        dt.Rows[i]["合同号"], dt.Rows[i]["客户名"],
                        dt.Rows[i]["结算金额"], lastYear,thisMonth,thisYear,zlj, 
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        lastYear1,thisMonth1,thisYear1,zlj1,
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        zlj1-zlj, null});
            }

            DataView dv = souce.DefaultView;
            dv.Sort = " B,D ASC";
            souce = dv.ToTable();

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
            decimal I = 0;
            decimal J = 0;
            decimal L = 0;
            decimal M = 0;
            decimal N = 0;
            decimal O = 0;
            decimal P = 0;
            decimal R = 0;
            this.toolStripProgressBar1.Value = 0;
            this.toolStripProgressBar1.Maximum = souce.Rows.Count;
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum / 4 * 3)
            {
                this.toolStripProgressBar1.Value++;
            }
            for (int i = 0; i < souce.Rows.Count; i++)
            {

                if (souce.Rows[i][2].ToString() == "小计")
                {
                    #region 求小计
                    decimal E1 = 0;
                    decimal F1 = 0;
                    decimal G1 = 0;
                    decimal H1 = 0;
                    decimal I1 = 0;
                    decimal J1 = 0;
                    decimal L1 = 0;
                    decimal M1 = 0;
                    decimal N1 = 0;
                    decimal O1 = 0;
                    decimal P1 = 0;
                    decimal R1 = 0;
                    for (int k = 0; k < souce.Rows.Count; k++)
                    {
                        if (souce.Rows[k]["D"].ToString() == souce.Rows[i]["D"].ToString() && souce.Rows[k]["C"].ToString() != "小计")
                        {
                            //MessageBox.Show("" + "    " + souce.Rows[i]["D"].ToString());
                            E1 += decimal.Parse(souce.Rows[k]["E"].ToString() == "" ? "0" : souce.Rows[k]["E"].ToString());
                            F1 += decimal.Parse(souce.Rows[k]["F"].ToString() == "" ? "0" : souce.Rows[k]["F"].ToString());
                            G1 += decimal.Parse(souce.Rows[k]["G"].ToString() == "" ? "0" : souce.Rows[k]["G"].ToString());
                            H1 += decimal.Parse(souce.Rows[k]["H"].ToString() == "" ? "0" : souce.Rows[k]["H"].ToString());
                            I1 += decimal.Parse(souce.Rows[k]["I"].ToString() == "" ? "0" : souce.Rows[k]["I"].ToString());
                            J1 += decimal.Parse(souce.Rows[k]["J"].ToString() == "" ? "0" : souce.Rows[k]["J"].ToString());
                            L1 += decimal.Parse(souce.Rows[k]["L"].ToString() == "" ? "0" : souce.Rows[k]["L"].ToString());
                            M1 += decimal.Parse(souce.Rows[k]["M"].ToString() == "" ? "0" : souce.Rows[k]["M"].ToString());
                            N1 += decimal.Parse(souce.Rows[k]["N"].ToString() == "" ? "0" : souce.Rows[k]["N"].ToString());
                            O1 += decimal.Parse(souce.Rows[k]["O"].ToString() == "" ? "0" : souce.Rows[k]["O"].ToString());
                            P1 += decimal.Parse(souce.Rows[k]["P"].ToString() == "" ? "0" : souce.Rows[k]["P"].ToString());
                            R1 += decimal.Parse(souce.Rows[k]["R"].ToString() == "" ? "0" : souce.Rows[k]["R"].ToString());
                        }
                    }
                    souce.Rows[i]["E"] = (E1 == 0 ? DBNull.Value as object : E1);
                    souce.Rows[i]["F"] = (F1 == 0 ? DBNull.Value as object : F1);
                    souce.Rows[i]["G"] = (G1 == 0 ? DBNull.Value as object : G1);
                    souce.Rows[i]["H"] = (H1 == 0 ? DBNull.Value as object : H1);
                    souce.Rows[i]["I"] = (I1 == 0 ? DBNull.Value as object : I1);
                    souce.Rows[i]["J"] = (J1 == 0 ? DBNull.Value as object : J1);
                    souce.Rows[i]["L"] = (L1 == 0 ? DBNull.Value as object : L1);
                    souce.Rows[i]["M"] = (M1 == 0 ? DBNull.Value as object : M1);
                    souce.Rows[i]["N"] = (N1 == 0 ? DBNull.Value as object : N1);
                    souce.Rows[i]["O"] = (O1 == 0 ? DBNull.Value as object : O1);
                    souce.Rows[i]["P"] = (P1 == 0 ? DBNull.Value as object : P1);
                    souce.Rows[i]["R"] = (R1 == 0 ? DBNull.Value as object : R1);
                    souce.Rows[i]["K"] = (E1 == 0 ? 1 : decimal.Round(J1 / E1, 2));
                    souce.Rows[i]["Q"] = (E1 == 0 ? 1 : decimal.Round(P1 / E1, 2));
                    #endregion
                }

                if (souce.Rows[i][2].ToString() != "总计" && souce.Rows[i][2].ToString() != "小计")
                {
                    E += decimal.Parse(souce.Rows[i]["E"].ToString() == "" ? "0" : souce.Rows[i]["E"].ToString());
                    F += decimal.Parse(souce.Rows[i]["F"].ToString() == "" ? "0" : souce.Rows[i]["F"].ToString());
                    G += decimal.Parse(souce.Rows[i]["G"].ToString() == "" ? "0" : souce.Rows[i]["G"].ToString());
                    H += decimal.Parse(souce.Rows[i]["H"].ToString() == "" ? "0" : souce.Rows[i]["H"].ToString());
                    I += decimal.Parse(souce.Rows[i]["I"].ToString() == "" ? "0" : souce.Rows[i]["I"].ToString());
                    J += decimal.Parse(souce.Rows[i]["J"].ToString() == "" ? "0" : souce.Rows[i]["J"].ToString());
                    L += decimal.Parse(souce.Rows[i]["L"].ToString() == "" ? "0" : souce.Rows[i]["L"].ToString());
                    M += decimal.Parse(souce.Rows[i]["M"].ToString() == "" ? "0" : souce.Rows[i]["M"].ToString());
                    N += decimal.Parse(souce.Rows[i]["N"].ToString() == "" ? "0" : souce.Rows[i]["N"].ToString());
                    O += decimal.Parse(souce.Rows[i]["O"].ToString() == "" ? "0" : souce.Rows[i]["O"].ToString());
                    P += decimal.Parse(souce.Rows[i]["P"].ToString() == "" ? "0" : souce.Rows[i]["P"].ToString());
                    R += decimal.Parse(souce.Rows[i]["R"].ToString() == "" ? "0" : souce.Rows[i]["R"].ToString());
                }
            }
            souce.Rows.Add(new object[] { null, null, "总计", null, E, F, G, H, I, J, (E == 0 ? 1 : decimal.Round(J / E, 2)), L, M, N, O, P, (P == 0 ? 1 : decimal.Round(P / E, 2)), R });
            #endregion
            this.dataGridView1.DataSource = souce;
            this.dataGridView1.AutoResizeColumns();
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum) { this.toolStripProgressBar1.Value++; }
        }
        /// <summary>
        /// 付款情况明细表生成数据
        /// </summary>
        private void BB_FK()
        {
            //Thread.Sleep(1000);
            this.toolStripProgressBar1.Value = 0;
            string sql = "";
            if (ClassCustom.codeSub(this.toolStripComboBox4.Text).Substring(0, 2) == "04")
            {
                sql = "SELECT * FROM vcontracts WHERE 1=1  AND (YEAR(签定日期) <  '" + this.toolStripComboBox1.Text + "' OR (YEAR(签定日期) =  '" + this.toolStripComboBox1.Text + "' AND MONTH(签定日期) <=  '" + this.toolStripComboBox2.Text + "')) AND HDW = '" + ClassConstant.DW_ID + "' AND (HLX  LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' or hlx like '07%')AND HYWY LIKE '" + ClassCustom.codeSub(this.toolStripComboBox3.Text) + "%'";
            }
            else
            {
                sql = "SELECT * FROM vcontracts WHERE 1=1  AND (YEAR(签定日期) <  '" + this.toolStripComboBox1.Text + "' OR (YEAR(签定日期) =  '" + this.toolStripComboBox1.Text + "' AND MONTH(签定日期) <=  '" + this.toolStripComboBox2.Text + "')) AND HDW = '" + ClassConstant.DW_ID + "' AND HLX  LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HYWY LIKE '" + ClassCustom.codeSub(this.toolStripComboBox3.Text) + "%'";
            }

            DataTable dt = DBAdo.DtFillSql(sql);
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum / 10) { this.toolStripProgressBar1.Value++; }
            souce.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object lastYear = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)< '" + this.toolStripComboBox1.Text + "' AND (type ='付款' OR type ='抵消') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisMonth = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)= '" + this.toolStripComboBox2.Text + "' AND (type ='付款' OR type ='抵消') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisYear = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)<='" + this.toolStripComboBox2.Text + "' AND (type ='付款' OR type ='抵消') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");

                object lastYear1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)< '" + this.toolStripComboBox1.Text + "' AND (type ='进项发票') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisMonth1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)= '" + this.toolStripComboBox2.Text + "' AND (type ='进项发票') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");
                object thisYear1 = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE YEAR(date)= '" + this.toolStripComboBox1.Text + "' AND MONTH(date)<='" + this.toolStripComboBox2.Text + "' AND (type ='进项发票') AND hth='" + dt.Rows[i]["合同号"].ToString() + "'");

                decimal zlj1 = decimal.Parse(lastYear1.ToString() == "" ? "0" : lastYear1.ToString()) + decimal.Parse(thisYear1.ToString() == "" ? "0" : thisYear1.ToString());
                decimal zlj = decimal.Parse(lastYear.ToString() == "" ? "0" : lastYear.ToString()) + decimal.Parse(thisYear.ToString() == "" ? "0" : thisYear.ToString());

                if (this.toolStripComboBox5.Text == "已完成")
                {
                    if (decimal.Parse(dt.Rows[i]["结算金额"].ToString() == "" ? "0" : dt.Rows[i]["结算金额"].ToString()) == zlj && zlj == zlj1)
                    {
                        souce.Rows.Add(new object[] { i + 1, dt.Rows[i]["HKH"].ToString().Substring(0,2)=="01"?"内":"外",
                        dt.Rows[i]["合同号"], dt.Rows[i]["客户名"],
                        dt.Rows[i]["结算金额"], lastYear,thisMonth,thisYear,zlj, 
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        lastYear1,thisMonth1,thisYear1,zlj1,
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        zlj1-zlj, null});
                    }

                }
                else if (this.toolStripComboBox5.Text == "全部")
                {
                    souce.Rows.Add(new object[] { i + 1, dt.Rows[i]["HKH"].ToString().Substring(0,2)=="01"?"内":"外",
                        dt.Rows[i]["合同号"], dt.Rows[i]["客户名"],
                        dt.Rows[i]["结算金额"], lastYear,thisMonth,thisYear,zlj, 
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        lastYear1,thisMonth1,thisYear1,zlj1,
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        zlj1-zlj, null});
                }
                else
                {
                    if (!(decimal.Parse(dt.Rows[i]["结算金额"].ToString() == "" ? "0" : dt.Rows[i]["结算金额"].ToString()) == zlj && zlj == zlj1))
                    {
                        souce.Rows.Add(new object[] { i + 1, dt.Rows[i]["HKH"].ToString().Substring(0,2)=="01"?"内":"外",
                        dt.Rows[i]["合同号"], dt.Rows[i]["客户名"],
                        dt.Rows[i]["结算金额"], lastYear,thisMonth,thisYear,zlj, 
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        lastYear1,thisMonth1,thisYear1,zlj1,
                        decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1, 
                        decimal.Round( (decimal.Parse( dt.Rows[i]["结算金额"].ToString())-zlj1)/decimal.Parse(dt.Rows[i]["结算金额"].ToString()),2),
                        zlj1-zlj, null});
                    }
                }
            }

            DataView dv = souce.DefaultView;
            dv.Sort = " B,D ASC";
            souce = dv.ToTable();

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
            decimal I = 0;
            decimal J = 0;
            decimal L = 0;
            decimal M = 0;
            decimal N = 0;
            decimal O = 0;
            decimal P = 0;
            decimal R = 0;
            this.toolStripProgressBar1.Value = 0;
            this.toolStripProgressBar1.Maximum = souce.Rows.Count;
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum / 4 * 3)
            {
                this.toolStripProgressBar1.Value++;
            }
            for (int i = 0; i < souce.Rows.Count; i++)
            {

                if (souce.Rows[i][2].ToString() == "小计")
                {
                    #region 求小计
                    decimal E1 = 0;
                    decimal F1 = 0;
                    decimal G1 = 0;
                    decimal H1 = 0;
                    decimal I1 = 0;
                    decimal J1 = 0;
                    decimal L1 = 0;
                    decimal M1 = 0;
                    decimal N1 = 0;
                    decimal O1 = 0;
                    decimal P1 = 0;
                    decimal R1 = 0;
                    for (int k = 0; k < souce.Rows.Count; k++)
                    {
                        if (souce.Rows[k]["D"].ToString() == souce.Rows[i]["D"].ToString() && souce.Rows[k]["C"].ToString() != "小计")
                        {
                            //MessageBox.Show("" + "    " + souce.Rows[i]["D"].ToString());
                            E1 += decimal.Parse(souce.Rows[k]["E"].ToString() == "" ? "0" : souce.Rows[k]["E"].ToString());
                            F1 += decimal.Parse(souce.Rows[k]["F"].ToString() == "" ? "0" : souce.Rows[k]["F"].ToString());
                            G1 += decimal.Parse(souce.Rows[k]["G"].ToString() == "" ? "0" : souce.Rows[k]["G"].ToString());
                            H1 += decimal.Parse(souce.Rows[k]["H"].ToString() == "" ? "0" : souce.Rows[k]["H"].ToString());
                            I1 += decimal.Parse(souce.Rows[k]["I"].ToString() == "" ? "0" : souce.Rows[k]["I"].ToString());
                            J1 += decimal.Parse(souce.Rows[k]["J"].ToString() == "" ? "0" : souce.Rows[k]["J"].ToString());
                            L1 += decimal.Parse(souce.Rows[k]["L"].ToString() == "" ? "0" : souce.Rows[k]["L"].ToString());
                            M1 += decimal.Parse(souce.Rows[k]["M"].ToString() == "" ? "0" : souce.Rows[k]["M"].ToString());
                            N1 += decimal.Parse(souce.Rows[k]["N"].ToString() == "" ? "0" : souce.Rows[k]["N"].ToString());
                            O1 += decimal.Parse(souce.Rows[k]["O"].ToString() == "" ? "0" : souce.Rows[k]["O"].ToString());
                            P1 += decimal.Parse(souce.Rows[k]["P"].ToString() == "" ? "0" : souce.Rows[k]["P"].ToString());
                            R1 += decimal.Parse(souce.Rows[k]["R"].ToString() == "" ? "0" : souce.Rows[k]["R"].ToString());
                        }
                    }
                    souce.Rows[i]["E"] = (E1 == 0 ? DBNull.Value as object : E1);
                    souce.Rows[i]["F"] = (F1 == 0 ? DBNull.Value as object : F1);
                    souce.Rows[i]["G"] = (G1 == 0 ? DBNull.Value as object : G1);
                    souce.Rows[i]["H"] = (H1 == 0 ? DBNull.Value as object : H1);
                    souce.Rows[i]["I"] = (I1 == 0 ? DBNull.Value as object : I1);
                    souce.Rows[i]["J"] = (J1 == 0 ? DBNull.Value as object : J1);
                    souce.Rows[i]["L"] = (L1 == 0 ? DBNull.Value as object : L1);
                    souce.Rows[i]["M"] = (M1 == 0 ? DBNull.Value as object : M1);
                    souce.Rows[i]["N"] = (N1 == 0 ? DBNull.Value as object : N1);
                    souce.Rows[i]["O"] = (O1 == 0 ? DBNull.Value as object : O1);
                    souce.Rows[i]["P"] = (P1 == 0 ? DBNull.Value as object : P1);
                    souce.Rows[i]["R"] = (R1 == 0 ? DBNull.Value as object : R1);
                    souce.Rows[i]["K"] = (E1 == 0 ? 1 : decimal.Round(J1 / E1, 2));
                    souce.Rows[i]["Q"] = (E1 == 0 ? 1 : decimal.Round(P1 / E1, 2));
                    #endregion
                }

                if (souce.Rows[i][2].ToString() != "总计" && souce.Rows[i][2].ToString() != "小计")
                {
                    E += decimal.Parse(souce.Rows[i]["E"].ToString() == "" ? "0" : souce.Rows[i]["E"].ToString());
                    F += decimal.Parse(souce.Rows[i]["F"].ToString() == "" ? "0" : souce.Rows[i]["F"].ToString());
                    G += decimal.Parse(souce.Rows[i]["G"].ToString() == "" ? "0" : souce.Rows[i]["G"].ToString());
                    H += decimal.Parse(souce.Rows[i]["H"].ToString() == "" ? "0" : souce.Rows[i]["H"].ToString());
                    I += decimal.Parse(souce.Rows[i]["I"].ToString() == "" ? "0" : souce.Rows[i]["I"].ToString());
                    J += decimal.Parse(souce.Rows[i]["J"].ToString() == "" ? "0" : souce.Rows[i]["J"].ToString());
                    L += decimal.Parse(souce.Rows[i]["L"].ToString() == "" ? "0" : souce.Rows[i]["L"].ToString());
                    M += decimal.Parse(souce.Rows[i]["M"].ToString() == "" ? "0" : souce.Rows[i]["M"].ToString());
                    N += decimal.Parse(souce.Rows[i]["N"].ToString() == "" ? "0" : souce.Rows[i]["N"].ToString());
                    O += decimal.Parse(souce.Rows[i]["O"].ToString() == "" ? "0" : souce.Rows[i]["O"].ToString());
                    P += decimal.Parse(souce.Rows[i]["P"].ToString() == "" ? "0" : souce.Rows[i]["P"].ToString());
                    R += decimal.Parse(souce.Rows[i]["R"].ToString() == "" ? "0" : souce.Rows[i]["R"].ToString());
                }
            }
            souce.Rows.Add(new object[] { null, null, "总计", null, E, F, G, H, I, J, (E == 0 ? 1 : decimal.Round(J / E, 2)), L, M, N, O, P, (P == 0 ? 1 : decimal.Round(P / E, 2)), R });
            #endregion
            this.dataGridView1.DataSource = souce;
            this.dataGridView1.AutoResizeColumns();
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum) { this.toolStripProgressBar1.Value++; }
        }
        /// <summary>
        /// 付款情况汇总表生成数据
        /// </summary>
        private void BB_FK_HZ()//////////////////////////////////////////////////////////////////////////////
        {
            //Thread.Sleep(1000);
            this.toolStripProgressBar1.Value = 0;

            //string sql = "SELECT YCODE, YNAME FROM AYWY WHERE YCODE IN(SELECT DISTINCT SUBSTRING(YCODE,1,6) FROM ACONTRACT H,AYWY Y WHERE HDW = '" + ClassConstant.DW_ID + "' AND H.HYWY=Y.YCODE)";
            string sql = "SELECT YCODE, YNAME FROM AYWY WHERE YCODE IN(SELECT DISTINCT SUBSTRING(YCODE,1,6) FROM ACONTRACT H,AYWY Y WHERE HDW = '" + ClassConstant.DW_ID + "' AND H.HYWY=Y.YCODE)  and ycode = '" + ClassCustom.codeSub(this.toolStripComboBox3.Text) + "'";
            DataTable dt = DBAdo.DtFillSql(sql);
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum / 10) { this.toolStripProgressBar1.Value++; }
            souce.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                souce.Rows.Add(new object[] { dt.Rows[i][1].ToString(), "外部", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });
                souce.Rows.Add(new object[] { dt.Rows[i][1].ToString(), "内部", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            }

            foreach (DataRow r in souce.Rows)
            {

                string ycode = DBAdo.ExecuteScalarSql("SELECT YCODE FROM AYWY WHERE YNAME = '" + r[0].ToString() + "'").ToString();
                if (r[1].ToString() == "内部")
                {
                    r[2] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "'");
                    r[3] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)= '" + this.toolStripComboBox2.Text + "'");
                    r[4] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<= '" + this.toolStripComboBox2.Text + "'");
                    r[5] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND (YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "' OR (YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<= '" + this.toolStripComboBox2.Text + "'))");
                    r[6] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[7] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[8] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[9] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[10] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[9].ToString() == "" ? "0" : r[9].ToString());
                    r[11] = decimal.Parse(r[10].ToString() == "" ? "0" : r[10].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());
                    r[12] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[13] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[14] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[15] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[16] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[15].ToString() == "" ? "0" : r[15].ToString());
                    r[17] = decimal.Parse(r[16].ToString() == "" ? "0" : r[16].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());

                }
                else
                {
                    r[2] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "'");
                    r[3] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)= '" + this.toolStripComboBox2.Text + "'");
                    r[4] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<='" + this.toolStripComboBox2.Text + "'");
                    r[5] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND (YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "' OR (YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<= '" + this.toolStripComboBox2.Text + "'))");
                    r[6] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[7] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[8] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[9] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '付款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[10] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[9].ToString() == "" ? "0" : r[9].ToString());
                    r[11] = decimal.Parse(r[10].ToString() == "" ? "0" : r[10].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());
                    r[12] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[13] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[14] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[15] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '进项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[16] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[15].ToString() == "" ? "0" : r[15].ToString());
                    r[17] = decimal.Parse(r[16].ToString() == "" ? "0" : r[16].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());
                }


            }
            for (int i = 0; i < souce.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    souce.Rows[i][0] = null;
                }
            }
            this.dataGridView1.DataSource = souce;
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum) { this.toolStripProgressBar1.Value++; }
        }

        private void BB_SK_HZ()//////////////////////////////////////////////////////////////////////////////
        {
            //Thread.Sleep(1000);
            this.toolStripProgressBar1.Value = 0;

            string sql = "SELECT YCODE, YNAME FROM AYWY WHERE YCODE IN(SELECT DISTINCT SUBSTRING(YCODE,1,6) FROM ACONTRACT H,AYWY Y WHERE H.HYWY=Y.YCODE)  and ycode = '" + ClassCustom.codeSub(this.toolStripComboBox3.Text) + "'";
            DataTable dt = DBAdo.DtFillSql(sql);
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum / 10) { this.toolStripProgressBar1.Value++; }
            souce.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                souce.Rows.Add(new object[] { dt.Rows[i][1].ToString(), "外部", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });
                souce.Rows.Add(new object[] { dt.Rows[i][1].ToString(), "内部", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });
            }

            foreach (DataRow r in souce.Rows)
            {

                string ycode = DBAdo.ExecuteScalarSql("SELECT YCODE FROM AYWY WHERE YNAME = '" + r[0].ToString() + "'").ToString();
                if (r[1].ToString() == "内部")
                {
                    r[2] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "'");
                    r[3] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)= '" + this.toolStripComboBox2.Text + "'");
                    r[4] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<= '" + this.toolStripComboBox2.Text + "'");
                    r[5] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND (YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "' OR (YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<= '" + this.toolStripComboBox2.Text + "'))");
                    r[6] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[7] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[8] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[9] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[10] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[9].ToString() == "" ? "0" : r[9].ToString());
                    r[11] = decimal.Parse(r[10].ToString() == "" ? "0" : r[10].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());
                    r[12] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[13] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[14] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[15] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[16] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[15].ToString() == "" ? "0" : r[15].ToString());
                    r[17] = decimal.Parse(r[16].ToString() == "" ? "0" : r[16].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());

                }
                else
                {
                    r[2] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "'");
                    r[3] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)= '" + this.toolStripComboBox2.Text + "'");
                    r[4] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<='" + this.toolStripComboBox2.Text + "'");
                    r[5] = DBAdo.ExecuteScalarSql("SELECT SUM(结算金额) FROM VCONTRACTS WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%' AND (YEAR(签定日期)< '" + this.toolStripComboBox1.Text + "' OR (YEAR(签定日期)= '" + this.toolStripComboBox1.Text + "' AND  MONTH(签定日期)<= '" + this.toolStripComboBox2.Text + "'))");
                    r[6] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[7] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[8] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[9] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '回款' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[10] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[9].ToString() == "" ? "0" : r[9].ToString());
                    r[11] = decimal.Parse(r[10].ToString() == "" ? "0" : r[10].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());
                    r[12] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date])<  '" + this.toolStripComboBox1.Text + "'");
                    r[13] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])=  '" + this.toolStripComboBox2.Text + "'");
                    r[14] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND YEAR([date]) = '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'");
                    r[15] = DBAdo.ExecuteScalarSql("SELECT SUM(RMB) FROM AFKXX WHERE TYPE = '销项发票' AND HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "' AND HLX LIKE '" + ClassCustom.codeSub(this.toolStripComboBox4.Text) + "%' AND HKH  NOT LIKE '01%' AND HYWY LIKE '" + ycode + "%') AND (YEAR([date])< '" + this.toolStripComboBox1.Text + "' OR (YEAR([date])= '" + this.toolStripComboBox1.Text + "' AND  MONTH([date])<= '" + this.toolStripComboBox2.Text + "'))");
                    r[16] = decimal.Parse(r[5].ToString() == "" ? "0" : r[5].ToString()) - decimal.Parse(r[15].ToString() == "" ? "0" : r[15].ToString());
                    r[17] = decimal.Parse(r[16].ToString() == "" ? "0" : r[16].ToString()) / decimal.Parse(r[5].ToString() == "" ? "1" : r[5].ToString());
                }


            }
            for (int i = 0; i < souce.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    souce.Rows[i][0] = null;
                }
            }
            this.dataGridView1.DataSource = souce;
            while (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum) { this.toolStripProgressBar1.Value++; }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("无数据");
                return;
            }
            if (this.name == "付款情况明细表")
            {
                EXCEL_FKMX(ClassCustom.codeSub1(this.toolStripComboBox4.Text) + "合同明细表", ClassConstant.DW_NAME + "   " + ClassCustom.codeSub1(this.toolStripComboBox3.Text), this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");
            }
            if (this.name == "回款情况明细表")
            {
                EXCEL_SKMX(ClassCustom.codeSub1(this.toolStripComboBox4.Text) + "合同明细表", ClassConstant.DW_NAME + "   " + ClassCustom.codeSub1(this.toolStripComboBox3.Text), this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");
            }
            if (this.name == "付款情况汇总表")
            {
                EXCEL_BB_FK_HZ(ClassCustom.codeSub1(this.toolStripComboBox4.Text) + "合同汇总情况表", ClassConstant.DW_NAME, this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");
            }
            if (this.name == "回款情况汇总表")
            {
                EXCEL_BB_SK_HZ(ClassCustom.codeSub1(this.toolStripComboBox4.Text) + "合同汇总情况表", ClassConstant.DW_NAME, this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");
            }
        }

        private void A_RPT_FK_Load(object sender, EventArgs e)
        {
            foreach (DataRow r in DBAdo.DtFillSql("SELECT YCODE, YNAME FROM AYWY WHERE YCODE LIKE '" + ClassConstant.DW_ID + "__'").Rows)
            {
                this.toolStripComboBox3.Items.Add(r[0].ToString() + ":" + r[1].ToString());
            }
            if (name == "回款情况明细表" || name == "回款情况汇总表")
            {
                foreach (DataRow r in DBAdo.DtFillSql("SELECT LID, LNAME FROM ALX WHERE LID = '02'").Rows)
                {
                    this.toolStripComboBox4.Items.Add(r[0].ToString() + ":" + r[1].ToString());
                }
            }
            else
            {
                foreach (DataRow r in DBAdo.DtFillSql("SELECT LID, LNAME FROM ALX WHERE LEN(LID)=2 and LID != '02'").Rows)
                {
                    this.toolStripComboBox4.Items.Add(r[0].ToString() + ":" + r[1].ToString());
                }
            }



            this.Text = name;
            this.toolStripComboBox1.Text = DateTime.Now.Year.ToString();
            this.toolStripComboBox2.Text = DateTime.Now.Month.ToString();
            if (name == "付款情况明细表" || name == "回款情况明细表")
            {
                souce.Columns.Add("A", typeof(int));//序号
                souce.Columns.Add("B", typeof(string));//内外
                souce.Columns.Add("C", typeof(string));//合同号
                souce.Columns.Add("D", typeof(string));//客户
                souce.Columns.Add("E", typeof(decimal));//结算
                souce.Columns.Add("F", typeof(decimal));//上年
                souce.Columns.Add("G", typeof(decimal));//本月
                souce.Columns.Add("H", typeof(decimal));//本年
                souce.Columns.Add("I", typeof(decimal));//总累计
                souce.Columns.Add("J", typeof(decimal));//金额
                souce.Columns.Add("K", typeof(decimal));//比例
                souce.Columns.Add("L", typeof(decimal));//上年
                souce.Columns.Add("M", typeof(decimal));//本月
                souce.Columns.Add("N", typeof(decimal));//本年
                souce.Columns.Add("O", typeof(decimal));//总累计
                souce.Columns.Add("P", typeof(decimal));//金额
                souce.Columns.Add("Q", typeof(decimal));//比例
                souce.Columns.Add("R", typeof(decimal));//财务余额
                souce.Columns.Add("S", typeof(string));//备注
            }
            else
            {
                //this.toolStripComboBox3.Visible = false;
                souce.Columns.Add("A", typeof(string));//序号
                souce.Columns.Add("B", typeof(string));//内外
                souce.Columns.Add("C", typeof(decimal));//合同号
                souce.Columns.Add("D", typeof(decimal));//客户
                souce.Columns.Add("E", typeof(decimal));//结算
                souce.Columns.Add("F", typeof(decimal));//上年
                souce.Columns.Add("G", typeof(decimal));//本月
                souce.Columns.Add("H", typeof(decimal));//本年
                souce.Columns.Add("I", typeof(decimal));//总累计
                souce.Columns.Add("J", typeof(decimal));//金额
                souce.Columns.Add("K", typeof(decimal));//比例
                souce.Columns.Add("L", typeof(decimal));//上年
                souce.Columns.Add("M", typeof(decimal));//本月
                souce.Columns.Add("N", typeof(decimal));//本年
                souce.Columns.Add("O", typeof(decimal));//总累计
                souce.Columns.Add("P", typeof(decimal));//金额
                souce.Columns.Add("Q", typeof(decimal));//比例
                souce.Columns.Add("R", typeof(decimal));//财务余额
                souce.Columns.Add("S", typeof(string));//备注
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            if (this.name == "付款情况明细表")
            {
                EXCEL_FKHZ("付款情况客户汇总表", ClassConstant.DW_NAME, this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");
            }
            if (this.name == "回款情况明细表")
            {
                EXCEL_SKHZ("付款情况客户汇总表", ClassConstant.DW_NAME, this.toolStripComboBox1.Text + "年" + this.toolStripComboBox2.Text + "月");
            }

        }

        /// <summary>
        /// 付款情况明细表导出EXCEL
        /// </summary>
        /// <param name="name">表头</param>
        /// <param name="dw">分公司名</param>
        /// <param name="date">年月</param>
        private void EXCEL_FKMX(string name, string dw, string date)
        {
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("C4", "C6").Merge(false);
            excel.get_Range("D4", "D6").Merge(false);
            excel.get_Range("E4", "E6").Merge(false);
            excel.get_Range("F4", "I4").Merge(false);
            excel.get_Range("J4", "K4").Merge(false);
            excel.get_Range("L4", "O4").Merge(false);
            excel.get_Range("P4", "Q4").Merge(false);
            excel.get_Range("R4", "R6").Merge(false);
            excel.get_Range("S4", "S6").Merge(false);
            excel.get_Range("G5", "H5").Merge(false);
            excel.get_Range("M5", "N5").Merge(false);
            excel.get_Range("F5", "F6").Merge(false);
            excel.get_Range("I5", "I6").Merge(false);
            excel.get_Range("J5", "J6").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("L5", "L6").Merge(false);
            excel.get_Range("O5", "O6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("Q5", "Q6").Merge(false);
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
            excel.Cells[4, "E"] = "结算金额";
            excel.Cells[4, "F"] = "已付货款";
            excel.Cells[4, "J"] = "未付货款";
            excel.Cells[4, "L"] = "已收发票金额";
            excel.Cells[4, "P"] = "未收发票金额";
            excel.Cells[4, "R"] = "财务余额";
            excel.Cells[4, "S"] = "备注";
            excel.Cells[5, "G"] = "本年";
            excel.Cells[5, "M"] = "本年";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[5, "F"] = "上年";
            excel.Cells[5, "L"] = "上年";
            excel.Cells[5, "I"] = "总累计";
            excel.Cells[5, "O"] = "总累计";
            excel.Cells[5, "J"] = "金额";
            excel.Cells[5, "P"] = "金额";
            excel.Cells[5, "K"] = "比例";
            excel.Cells[5, "Q"] = "比例";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "H"] = "本年";
            excel.Cells[6, "N"] = "本年";
            excel.get_Range("E7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            excel.get_Range("k7", excel.Cells[dataGridView1.Rows.Count + 6, "k"]).NumberFormat = "0%";
            excel.get_Range("q7", excel.Cells[dataGridView1.Rows.Count + 6, "q"]).NumberFormat = "0%";
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
        /// <summary>
        /// 付款情况明细表导出EXCEL
        /// </summary>
        /// <param name="name">表头</param>
        /// <param name="dw">分公司名</param>
        /// <param name="date">年月</param>
        private void EXCEL_FKHZ(string name, string dw, string date)
        {
            DataView dv = souce.DefaultView;
            dv.RowFilter = "C = '小计' OR C='总计'";
            souce = dv.ToTable();
            for (int i = 0; i < souce.Rows.Count; i++)
            {
                souce.Rows[i][0] = (i + 1).ToString();
            }
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("C4", "C6").Merge(false);
            excel.get_Range("D4", "D6").Merge(false);
            excel.get_Range("E4", "E6").Merge(false);
            excel.get_Range("F4", "I4").Merge(false);
            excel.get_Range("J4", "K4").Merge(false);
            excel.get_Range("L4", "O4").Merge(false);
            excel.get_Range("P4", "Q4").Merge(false);
            excel.get_Range("R4", "R6").Merge(false);
            excel.get_Range("S4", "S6").Merge(false);
            excel.get_Range("G5", "H5").Merge(false);
            excel.get_Range("M5", "N5").Merge(false);
            excel.get_Range("F5", "F6").Merge(false);
            excel.get_Range("I5", "I6").Merge(false);
            excel.get_Range("J5", "J6").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("L5", "L6").Merge(false);
            excel.get_Range("O5", "O6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("Q5", "Q6").Merge(false);
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
            excel.Cells[4, "E"] = "结算金额";
            excel.Cells[4, "F"] = "已付货款";
            excel.Cells[4, "J"] = "未付货款";
            excel.Cells[4, "L"] = "已收发票金额";
            excel.Cells[4, "P"] = "未收发票金额";
            excel.Cells[4, "R"] = "财务余额";
            excel.Cells[4, "S"] = "备注";
            excel.Cells[5, "G"] = "本年";
            excel.Cells[5, "M"] = "本年";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[5, "F"] = "上年";
            excel.Cells[5, "L"] = "上年";
            excel.Cells[5, "I"] = "总累计";
            excel.Cells[5, "O"] = "总累计";
            excel.Cells[5, "J"] = "金额";
            excel.Cells[5, "P"] = "金额";
            excel.Cells[5, "K"] = "比例";
            excel.Cells[5, "Q"] = "比例";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "H"] = "本年";
            excel.Cells[6, "N"] = "本年";
            excel.get_Range("E7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            excel.get_Range("k7", excel.Cells[dataGridView1.Rows.Count + 6, "k"]).NumberFormat = "0%";
            excel.get_Range("q7", excel.Cells[dataGridView1.Rows.Count + 6, "q"]).NumberFormat = "0%";
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
            excel.get_Range("A4", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
            ClassCustom.DrawExcelBorders(excel, "A1", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$6";
            //sheet1.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperFanfoldUS;
        }
        /// <summary>
        /// 回款情况明细表导出EXCEL
        /// </summary>
        /// <param name="name">表头</param>
        /// <param name="dw">分公司名</param>
        /// <param name="date">年月</param>
        private void EXCEL_SKMX(string name, string dw, string date)
        {
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("C4", "C6").Merge(false);
            excel.get_Range("D4", "D6").Merge(false);
            excel.get_Range("E4", "E6").Merge(false);
            excel.get_Range("F4", "I4").Merge(false);
            excel.get_Range("J4", "K4").Merge(false);
            excel.get_Range("L4", "O4").Merge(false);
            excel.get_Range("P4", "Q4").Merge(false);
            excel.get_Range("R4", "R6").Merge(false);
            excel.get_Range("S4", "S6").Merge(false);
            excel.get_Range("G5", "H5").Merge(false);
            excel.get_Range("M5", "N5").Merge(false);
            excel.get_Range("F5", "F6").Merge(false);
            excel.get_Range("I5", "I6").Merge(false);
            excel.get_Range("J5", "J6").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("L5", "L6").Merge(false);
            excel.get_Range("O5", "O6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("Q5", "Q6").Merge(false);
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
            excel.Cells[4, "E"] = "结算金额";
            excel.Cells[4, "F"] = "回款金额";
            excel.Cells[4, "J"] = "尚欠金额";
            excel.Cells[4, "L"] = "已开发票金额";
            excel.Cells[4, "P"] = "未开发票金额";
            excel.Cells[4, "R"] = "财务余额";
            excel.Cells[4, "S"] = "备注";
            excel.Cells[5, "G"] = "本年";
            excel.Cells[5, "M"] = "本年";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[5, "F"] = "上年";
            excel.Cells[5, "L"] = "上年";
            excel.Cells[5, "I"] = "总累计";
            excel.Cells[5, "O"] = "总累计";
            excel.Cells[5, "J"] = "金额";
            excel.Cells[5, "P"] = "金额";
            excel.Cells[5, "K"] = "比例";
            excel.Cells[5, "Q"] = "比例";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "H"] = "本年";
            excel.Cells[6, "N"] = "本年";
            excel.get_Range("E7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            excel.get_Range("k7", excel.Cells[dataGridView1.Rows.Count + 6, "k"]).NumberFormat = "0%";
            excel.get_Range("q7", excel.Cells[dataGridView1.Rows.Count + 6, "q"]).NumberFormat = "0%";
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
        /// <summary>
        /// 回款情况明细表导出EXCEL
        /// </summary>
        /// <param name="name">表头</param>
        /// <param name="dw">分公司名</param>
        /// <param name="date">年月</param>
        private void EXCEL_SKHZ(string name, string dw, string date)
        {
            DataView dv = souce.DefaultView;
            dv.RowFilter = "C = '小计' OR C='总计'";
            souce = dv.ToTable();
            for (int i = 0; i < souce.Rows.Count; i++)
            {
                souce.Rows[i][0] = (i + 1).ToString();
            }
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("C4", "C6").Merge(false);
            excel.get_Range("D4", "D6").Merge(false);
            excel.get_Range("E4", "E6").Merge(false);
            excel.get_Range("F4", "I4").Merge(false);
            excel.get_Range("J4", "K4").Merge(false);
            excel.get_Range("L4", "O4").Merge(false);
            excel.get_Range("P4", "Q4").Merge(false);
            excel.get_Range("R4", "R6").Merge(false);
            excel.get_Range("S4", "S6").Merge(false);
            excel.get_Range("G5", "H5").Merge(false);
            excel.get_Range("M5", "N5").Merge(false);
            excel.get_Range("F5", "F6").Merge(false);
            excel.get_Range("I5", "I6").Merge(false);
            excel.get_Range("J5", "J6").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("L5", "L6").Merge(false);
            excel.get_Range("O5", "O6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("Q5", "Q6").Merge(false);
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
            excel.Cells[4, "E"] = "结算金额";
            excel.Cells[4, "F"] = "回款金额";
            excel.Cells[4, "J"] = "尚欠金额";
            excel.Cells[4, "L"] = "已开发票金额";
            excel.Cells[4, "P"] = "未开发票金额";
            excel.Cells[4, "R"] = "财务余额";
            excel.Cells[4, "S"] = "备注";
            excel.Cells[5, "G"] = "本年";
            excel.Cells[5, "M"] = "本年";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[5, "F"] = "上年";
            excel.Cells[5, "L"] = "上年";
            excel.Cells[5, "I"] = "总累计";
            excel.Cells[5, "O"] = "总累计";
            excel.Cells[5, "J"] = "金额";
            excel.Cells[5, "P"] = "金额";
            excel.Cells[5, "K"] = "比例";
            excel.Cells[5, "Q"] = "比例";
            excel.Cells[6, "G"] = "本月";
            excel.Cells[6, "M"] = "本月";
            excel.Cells[6, "H"] = "本年";
            excel.Cells[6, "N"] = "本年";
            excel.get_Range("E7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            excel.get_Range("k7", excel.Cells[dataGridView1.Rows.Count + 6, "k"]).NumberFormat = "0%";
            excel.get_Range("q7", excel.Cells[dataGridView1.Rows.Count + 6, "q"]).NumberFormat = "0%";
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
        /// <summary>
        /// 回款情况汇总表导出EXCEL
        /// </summary>
        /// <param name="name">表头</param>
        /// <param name="dw">分公司名</param>
        /// <param name="date">年月</param>
        private void EXCEL_BB_FK_HZ(string name, string dw, string date)///////////////////////////////////////////////////////////////////////////////
        {
            //MessageBox.Show("Test");
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("S4", "S6").Merge(false);
            excel.get_Range("C4", "F4").Merge(false);
            excel.get_Range("G4", "J4").Merge(false);
            excel.get_Range("K4", "L4").Merge(false);
            excel.get_Range("M4", "P4").Merge(false);
            excel.get_Range("Q4", "R4").Merge(false);
            excel.get_Range("D5", "E5").Merge(false);
            excel.get_Range("H5", "I5").Merge(false);
            excel.get_Range("N5", "O5").Merge(false);
            //CFGJKLMPR
            excel.get_Range("C5", "C6").Merge(false);
            excel.get_Range("F5", "F6").Merge(false);
            excel.get_Range("G5", "G6").Merge(false);
            excel.get_Range("J5", "J6").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("L5", "L6").Merge(false);
            excel.get_Range("M5", "M6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("Q5", "Q6").Merge(false);
            excel.get_Range("R5", "R6").Merge(false);

            excel.Cells[1, 1] = name;
            excel.Cells[3, 1] = dw + "    " + date;
            (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A4", "S4").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            excel.get_Range("A1", "S6").Font.Bold = true;

            excel.Cells[4, "A"] = "部门";
            excel.Cells[4, "B"] = "客户类型";
            excel.Cells[4, "C"] = "签定合同总额";
            excel.Cells[4, "G"] = "已付货款";
            excel.Cells[4, "K"] = "未付货款";
            excel.Cells[4, "M"] = "已收发票金额";
            excel.Cells[4, "Q"] = "未收发票金额";
            excel.Cells[4, "S"] = "备注";

            excel.Cells[5, "D"] = "本年";
            excel.Cells[5, "H"] = "本年";
            excel.Cells[5, "N"] = "本年";

            excel.Cells[5, "C"] = "上年";
            excel.Cells[5, "G"] = "上年";
            excel.Cells[5, "M"] = "上年";
            excel.Cells[5, "F"] = "总累计";
            excel.Cells[5, "J"] = "总累计";
            excel.Cells[5, "P"] = "总累计";
            excel.Cells[5, "K"] = "金额";
            excel.Cells[5, "Q"] = "金额";
            excel.Cells[5, "L"] = "比例";
            excel.Cells[5, "R"] = "比例";

            excel.Cells[6, "D"] = "本月";
            excel.Cells[6, "H"] = "本月";
            excel.Cells[6, "N"] = "本月";
            excel.Cells[6, "E"] = "本年";
            excel.Cells[6, "I"] = "本年";
            excel.Cells[6, "O"] = "本年";

            excel.get_Range("C7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            excel.get_Range("R7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "0%";
            excel.get_Range("L7", excel.Cells[dataGridView1.Rows.Count + 6, "L"]).NumberFormat = "0%";
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
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                //MessageBox.Show(excel.Cells[i + 7, "A"].ToString());
                if (excel.get_Range("A" + (i + 7), "A" + (i + 7)).Value.ToString() == "")
                {
                    excel.Cells[i + 7, "A"] = null;
                    excel.get_Range(excel.Cells[i + 7, "A"], excel.Cells[i + 6, "A"]).Merge(false);
                }

            }
            excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$6";
        }
        /// <summary>
        /// 付款情况汇总表导出EXCEL
        /// </summary>
        /// <param name="name">表头</param>
        /// <param name="dw">分公司名</param>
        /// <param name="date">年月</param>
        private void EXCEL_BB_SK_HZ(string name, string dw, string date)
        {
            //MessageBox.Show("Test");
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A3", excel.Cells[3, dataGridView1.Columns.Count]).Merge(false);
            excel.get_Range("A4", "A6").Merge(false);
            excel.get_Range("B4", "B6").Merge(false);
            excel.get_Range("S4", "S6").Merge(false);
            excel.get_Range("C4", "F4").Merge(false);
            excel.get_Range("G4", "J4").Merge(false);
            excel.get_Range("K4", "L4").Merge(false);
            excel.get_Range("M4", "P4").Merge(false);
            excel.get_Range("Q4", "R4").Merge(false);
            excel.get_Range("D5", "E5").Merge(false);
            excel.get_Range("H5", "I5").Merge(false);
            excel.get_Range("N5", "O5").Merge(false);
            //CFGJKLMPR
            excel.get_Range("C5", "C6").Merge(false);
            excel.get_Range("F5", "F6").Merge(false);
            excel.get_Range("G5", "G6").Merge(false);
            excel.get_Range("J5", "J6").Merge(false);
            excel.get_Range("K5", "K6").Merge(false);
            excel.get_Range("L5", "L6").Merge(false);
            excel.get_Range("M5", "M6").Merge(false);
            excel.get_Range("P5", "P6").Merge(false);
            excel.get_Range("Q5", "Q6").Merge(false);
            excel.get_Range("R5", "R6").Merge(false);

            excel.Cells[1, 1] = name;
            excel.Cells[3, 1] = dw + "    " + date;
            (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            (excel.Cells[3, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            excel.get_Range("A4", "S4").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            excel.get_Range("A1", "S6").Font.Bold = true;

            excel.Cells[4, "A"] = "部门";
            excel.Cells[4, "B"] = "客户类型";
            excel.Cells[4, "C"] = "签定合同总额";
            excel.Cells[4, "G"] = "回款金额";
            excel.Cells[4, "K"] = "尚欠金额";
            excel.Cells[4, "M"] = "已开发票金额";
            excel.Cells[4, "Q"] = "未开发票金额";
            excel.Cells[4, "S"] = "备注";

            excel.Cells[5, "D"] = "本年";
            excel.Cells[5, "H"] = "本年";
            excel.Cells[5, "N"] = "本年";

            excel.Cells[5, "C"] = "上年";
            excel.Cells[5, "G"] = "上年";
            excel.Cells[5, "M"] = "上年";
            excel.Cells[5, "F"] = "总累计";
            excel.Cells[5, "J"] = "总累计";
            excel.Cells[5, "P"] = "总累计";
            excel.Cells[5, "K"] = "金额";
            excel.Cells[5, "Q"] = "金额";
            excel.Cells[5, "L"] = "比例";
            excel.Cells[5, "R"] = "比例";

            excel.Cells[6, "D"] = "本月";
            excel.Cells[6, "H"] = "本月";
            excel.Cells[6, "N"] = "本月";
            excel.Cells[6, "E"] = "本年";
            excel.Cells[6, "I"] = "本年";
            excel.Cells[6, "O"] = "本年";

            excel.get_Range("C7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "#,##0.00";
            excel.get_Range("R7", excel.Cells[dataGridView1.Rows.Count + 6, "R"]).NumberFormat = "0%";
            excel.get_Range("L7", excel.Cells[dataGridView1.Rows.Count + 6, "L"]).NumberFormat = "0%";
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
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                //MessageBox.Show(excel.Cells[i + 7, "A"].ToString());
                if (excel.get_Range("A" + (i + 7), "A" + (i + 7)).Value.ToString() == "")
                {
                    excel.Cells[i + 7, "A"] = null;
                    excel.get_Range(excel.Cells[i + 7, "A"], excel.Cells[i + 6, "A"]).Merge(false);
                }

            }
            excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
            ClassCustom.DrawExcelBorders(excel, "A4", excel.Cells[dataGridView1.Rows.Count + 6, dataGridView1.Columns.Count]);
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PrintTitleRows = "$1:$6";
        }

        private void toolStripComboBox3_VisibleChanged(object sender, EventArgs e)
        {
            this.toolStripLabel4.Visible = this.toolStripComboBox3.Visible;
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedCells.Count == 0)
                return;
            string s = "";
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                bool flag = false;
                foreach (DataGridViewCell c in r.Cells)
                {
                    if (c.Selected)
                    {
                        s += c.Value.ToString() + "\t";
                        flag = true;
                    }
                }
                if (flag)
                {
                    s += "\n";
                    flag = false;
                }
            }

            foreach (DataGridViewCell c in this.dataGridView1.SelectedCells)
            {
                Console.WriteLine(c.RowIndex + "\t" + c.ColumnIndex + "\n");
            }
            Clipboard.SetText(s);
        }


    }
}
