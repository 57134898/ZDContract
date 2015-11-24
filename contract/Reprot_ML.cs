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
    public partial class Reprot_ML : Form
    {
        public Reprot_ML()
        {
            InitializeComponent();
        }

        private void Reprot_ML_Load(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = DateTime.Now;
            this.dateTimePicker2.Value = DateTime.Now;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = string.Format(@"SELECT T0.客户名 客户1,T0.合同号 合同号1,T0.签订日期 签订日期1,T0.结算金额 结算金额1,0.00 已收货款,0.00 已开发票,T2.客户名 客户2,T2.合同号 合同号2,T2.签订日期 签订日期2,T2.结算金额 结算金额2,0.00 已付货款,0.00 已收发票,
                                                                CASE SUBSTRING(T0.HKH,1,2) WHEN '01' THEN '内部' WHEN '02' THEN '外部' WHEN '05' THEN '北方重工' when  '05' then '在建工程' ELSE '鼓风'  END as 客户类别
                                                                FROM vcontracts T0 LEFT JOIN AWX T1 ON T0.合同号=T1.XSHTH LEFT JOIN vcontracts T2 ON t1.WXHTH=T2.合同号 WHERE T0.HLX LIKE '0201%' AND T0.HDW = '{0}' AND (T0.签定日期 BETWEEN '{1}' AND '{2}')", ClassCustom.codeSub(ClassConstant.DW_ID), this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());
                DataTable dt = DBAdo.DtFillSql(sql);
                dt.Columns.Add("直接材料", typeof(decimal));
                dt.Columns.Add("人工制造费", typeof(decimal));
                dt.Columns.Add("毛利额", typeof(decimal));
                dt.Columns.Add("毛利率", typeof(decimal));
                this.toolStripProgressBar1.Value = 0;
                this.toolStripProgressBar1.Maximum = dt.Rows.Count;
                foreach (DataRow r in dt.Rows)
                {
                    //bool hs = DBAdo.ExecuteScalarSql(string.Format("SELECT {0}"));

                    Application.DoEvents();
                    this.toolStrip1.Refresh();
                    this.toolStripProgressBar1.Value++;

                    decimal a1 = 0;
                    decimal a2 = 0;

                    decimal b1 = 0;
                    decimal b2 = 0;

                    object o1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号1"].ToString(), this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), "回款" }));
                    object o2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号1"].ToString(), this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), "销项发票" }));

                    object oo1 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号2"].ToString(), this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), "付款" }));
                    object oo2 = DBAdo.ExecuteScalarSql(string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 AND HTH ='{0}' AND TYPE = '{3}' AND (YEAR(DATE)<{1} OR (YEAR(DATE) = {1} AND MONTH(DATE) <={2}))", new string[] { r["合同号2"].ToString(), this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), "进项发票" }));

                    a1 = decimal.Parse(o1 == null || o1.ToString() == "" ? "0" : o1.ToString());
                    a2 = decimal.Parse(o2 == null || o2.ToString() == "" ? "0" : o2.ToString());

                    b1 = decimal.Parse(oo1 == null || oo1.ToString() == "" ? "0" : oo1.ToString());
                    b2 = decimal.Parse(oo2 == null || oo2.ToString() == "" ? "0" : oo2.ToString());

                    r["已收货款"] = a1;
                    r["已开发票"] = a2;
                    r["已付货款"] = b1;
                    r["已收发票"] = b2;
                }
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns["客户1"].HeaderText = "购货单位名称";
                this.dataGridView1.Columns["合同号1"].HeaderText = "销售合同号";
                this.dataGridView1.Columns["签订日期1"].HeaderText = "签订日期";
                this.dataGridView1.Columns["结算金额1"].HeaderText = "结算金额";
                this.dataGridView1.Columns["已收货款"].HeaderText = "已收货款";
                this.dataGridView1.Columns["已开发票"].HeaderText = "已开发票";
                this.dataGridView1.Columns["客户2"].HeaderText = "单位名称";
                this.dataGridView1.Columns["合同号2"].HeaderText = "外协合同号";
                this.dataGridView1.Columns["签订日期2"].HeaderText = "签订日期";
                this.dataGridView1.Columns["结算金额2"].HeaderText = "结算金额";
                this.dataGridView1.Columns["已付货款"].HeaderText = "已付货款";
                this.dataGridView1.Columns["已收发票"].HeaderText = "已收发票";
                this.dataGridView1.AutoResizeColumns();
                this.dataGridView1.Columns[0].Frozen = true;
                this.dataGridView1.Columns[1].Frozen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.dataGridView1, "自 制 销 售(含 部 分 外 委) 合 同 执 行 明 细 表");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }
    }
}
