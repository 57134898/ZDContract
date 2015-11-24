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
    public partial class A_HT_TOOL_GY : Form
    {
        private string hth;
        private DataTable dt;
        private DataView dv;
        private A_HT_FKFPGY fy;
        private bool val;

        public A_HT_TOOL_GY()
        {
            InitializeComponent();
        }

        public A_HT_TOOL_GY(string hth, A_HT_FKFPGY fy)
        {
            InitializeComponent();
            this.hth = hth;
            this.fy = fy;
        }

        private void A_HT_TOOL_GY_Load(object sender, EventArgs e)
        {
            try
            {
                //                DJ2
                //GCODE
                //GDW1
                //GDW2
                //GJM
                //GMEMO
                //HTH
                //JHRQ
                //TH

                string sql = string.Format("SELECT gcode,GNAME 商品名,GCZ 材质,GXH 型号,GSL 数量,ZZ 总重,H.HHSBL 税率 ,GDJ1/(case hhsbl when 0 then CONVERT(decimal(18,4),'1') else CONVERT(decimal(18,4), hhsbl)/100+1 end)  [单价],DJ2/(case hhsbl when 0 then CONVERT(decimal(18,4),'1') else CONVERT(decimal(18,4), hhsbl)/100+1 end) [吨价] FROM ASP S,ACONTRACT H WHERE S.HTH=H.HCODE AND  HTH = '{0}'", hth);
                dt = DBAdo.DtFillSql(sql);
                //dt.Columns.Add("按重量估验", typeof(bool));
                //dt.Columns.Add("按数量估验", typeof(bool));
                //dt.Columns.Add("本次数量", typeof(decimal));

                //dt.Columns.Add("本次估验金额", typeof(decimal), "不含税单价*本次数量");
                foreach (DataRow r in dt.Rows)
                {
                    this.dataGridView1.Rows.Add(new object[] { r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString(), r[6].ToString(), r[7].ToString(), r[8].ToString(), false });
                }


                this.dataGridView1.Columns["数量"].ValueType = typeof(decimal);
                this.dataGridView1.Columns["单价"].ValueType = typeof(decimal);
                this.dataGridView1.Columns["吨价"].ValueType = typeof(decimal);
                this.dataGridView1.Columns["总重"].ValueType = typeof(decimal);
                this.dataGridView1.Columns["本次估验"].ValueType = typeof(decimal);
                this.dataGridView1.Columns["本次金额"].ValueType = typeof(decimal);

                foreach (DataGridViewColumn c in this.dataGridView1.Columns)
                {
                    if (c.ValueType == typeof(decimal))
                    {
                        c.DefaultCellStyle.Format = "N4";
                        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                this.dataGridView1.AutoResizeColumns();

                this.dataGridView2.DataSource = DBAdo.DtFillSql(string.Format("SELECT date 日期,rmb 金额,flag 连接凭证,vyear 年,vmonth 月,Vtype 凭证类别,vid 凭证号 FROM AFKXX WHERE HTH='{1}' AND TYPE = '{0}'", "估验", this.hth));
                foreach (DataGridViewRow r in this.dataGridView2.Rows)
                {
                    if (decimal.Parse(r.Cells["金额"].Value.ToString()) < 0)
                    {
                        r.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                this.dataGridView2.Columns["金额"].DefaultCellStyle.Format = "N4";
                this.dataGridView2.Columns["金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                val = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.label3.Text = ClassCustom.UpMoney(decimal.Parse(this.label2.Text));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (this.dataGridView1.Rows.Count == 0)
                {
                    return;
                }
                if (this.dataGridView1["本次估验", e.RowIndex].Value == DBNull.Value)
                {
                    return;
                }
                decimal res = 0;
                if (e.ColumnIndex == this.dataGridView1.Columns["本次估验"].Index)
                {
                    bool mark = bool.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["按重量估验"].Value.ToString());
                    decimal d1 = (mark ? decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["吨价"].Value == null || this.dataGridView1.Rows[e.RowIndex].Cells["吨价"].Value.ToString() == "" ? "0" : this.dataGridView1.Rows[e.RowIndex].Cells["吨价"].Value.ToString()) : decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["单价"].Value.ToString()));
                    decimal d2 = decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["本次估验"].Value.ToString());
                    this.dataGridView1.Rows[e.RowIndex].Cells["本次金额"].Value = d1 * d2;
                    Console.WriteLine(d1 * d2);
                    foreach (DataGridViewRow r in this.dataGridView1.Rows)
                    {
                        if (r.Cells[10].Value == DBNull.Value)
                            continue;

                        res += decimal.Parse(r.Cells[10].Value == null || r.Cells[10].Value.ToString() == "" ? "0" : r.Cells[10].Value.ToString());
                        //res += (mark ? decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["吨价"].Value == null || this.dataGridView1.Rows[e.RowIndex].Cells["吨价"].Value.ToString() == "" ? "0" : this.dataGridView1.Rows[e.RowIndex].Cells["吨价"].Value.ToString()) : decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["单价"].Value.ToString())) * decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["本次估验"].Value.ToString());
                    }
                    this.label2.Text = res.ToString();
                    this.Validate();
                    this.dataGridView1.AutoResizeColumns();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                fy.setGy(decimal.Parse(this.label2.Text));
                this.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (!val)
                    return;
                if (e.ColumnIndex != this.dataGridView1.Columns["本次估验"].Index)
                    return;
                if (this.dataGridView1[e.ColumnIndex, e.RowIndex].Value == DBNull.Value)
                    return;
                //if ((bool.Parse(this.dataGridView1["按重量估验", e.RowIndex].Value.ToString())) && decimal.Parse(e.FormattedValue.ToString()) > (this.dataGridView1["总重", e.RowIndex].Value == null || this.dataGridView1["总重", e.RowIndex].Value.ToString() == "" ? 0 : decimal.Parse(this.dataGridView1["总重", e.RowIndex].Value.ToString())))
                //{
                //    MessageBox.Show("估验重量不可以大于商品总重");
                //    e.Cancel = true;
                //}

                //if ((!bool.Parse(this.dataGridView1["按重量估验", e.RowIndex].Value.ToString())) && decimal.Parse(e.FormattedValue.ToString()) > (this.dataGridView1["数量", e.RowIndex].Value == null || this.dataGridView1["数量", e.RowIndex].Value.ToString() == "" ? 0 : decimal.Parse(this.dataGridView1["数量", e.RowIndex].Value.ToString())))
                //{
                //    MessageBox.Show("估验数量不可以大于商品数量");
                //    e.Cancel = true;
                //}
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
