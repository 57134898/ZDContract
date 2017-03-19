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
    public partial class A_HT_FKFPGY : Form, IsetText
    {
        public A_HT_FKFPGY()
        {
            InitializeComponent();
        }

        private ToolStripItem[] bts = null;
        private decimal rmb;
        private decimal rmb_t;
        private DataTable souce;
        private DataTable dt;
        private DataTable dt1;
        private DataTable dt2;
        private bool mark = true;

        private void A_HT_FKFPGY_Load(object sender, EventArgs e)
        {
            try
            {
                Reg();
                DataLoad();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }

        }


        private void DataLoad()
        {
            try
            {
                this.customDataGridView1.ColumnHeadersVisible = false;

                this.customDataGridView1.Columns[1].ValueType = typeof(decimal);
                this.customDataGridView1.Columns[2].ValueType = typeof(decimal);
                this.customDataGridView1.Columns[3].ValueType = typeof(decimal);
                this.customDataGridView1.Columns[4].ValueType = typeof(decimal);

                //this.customDataGridView1.Columns[1].DefaultCellStyle.Format = "N4";
                //this.customDataGridView1.Columns[2].DefaultCellStyle.Format = "N4";
                //this.customDataGridView1.Columns[3].DefaultCellStyle.Format = "N4";
                //this.customDataGridView1.Columns[4].DefaultCellStyle.Format = "N4";

                //this.customDataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //this.customDataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //this.customDataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //this.customDataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dt1 = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LEN(LID)=2 ");
                this.comboBoxLX.DataSource = dt1;
                this.comboBoxLX.DisplayMember = "LNAME";
                this.comboBoxLX.ValueMember = "LID";
                this.comboBoxLX_SelectedIndexChanged(null, null);

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            this.textBox2.ReadOnly = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox2.Text == "-")
                {
                    return;
                }
                rmb = decimal.Parse(this.textBox2.Text == "" ? "0" : this.textBox2.Text);
                rmb_t = decimal.Parse(this.textBox2.Text == "" ? "0" : this.textBox2.Text);
                this.textBox3.Text = this.textBox2.Text;
                if (this.textBox2.Text != "")
                {
                    this.customDataGridView1.Enabled = true;
                }
                else
                {
                    this.customDataGridView1.Enabled = false;
                }
                //this.textBox4.Text = this.textBox2.Text;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //dt.Rows.Clear();
                //if (this.comboBox1.Text == "付款" || this.comboBox1.Text == "进项发票")
                //{
                //    string sql = "select 合同号 from vcontracts where HLX = '' AND HKH = '" + this.textBox1.Tag.ToString() + "'";
                //}
                //if (this.comboBox1.Text == "回款" || this.comboBox1.Text == "销项发票")
                //{
                //    string sql = "select 合同号 from vcontracts where HKH = '" + this.textBox1.Tag.ToString() + "'";
                //}
                //if (this.comboBox1.Text == "估验")
                //{
                //    string sql = "select 合同号 from vcontracts where HKH = '" + this.textBox1.Tag.ToString() + "'";
                //}
                //foreach (DataRow r in DBAdo.DtFillSql("").Rows)
                //{
                //    dt.Rows.Add(r[0].ToString());
                //}
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void comboBoxLX_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comboBox1.Items.Clear();
                if (this.comboBoxLX.SelectedValue.ToString() == "02")
                {
                    this.comboBox1.Items.AddRange(new object[] { "回款", "销项发票" });
                }
                else
                {
                    this.comboBox1.Items.AddRange(new object[] { "付款", "进项发票" });
                    //this.comboBox1.Items.AddRange(new object[] { "付款", "进项发票", "估验" });
                }
                this.comboBox1.SelectedIndex = 0;

                dt2 = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LID LIKE '" + this.comboBoxLX.SelectedValue.ToString() + "__' ");
                this.comboBoxLX1.DataSource = dt2;
                this.comboBoxLX1.DisplayMember = "LNAME";
                this.comboBoxLX1.ValueMember = "LID";
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void Reg()
        {
            OnButtonClick gy = (object sender, EventArgs e) =>
            {
                //估验
                if (this.customDataGridView1.Rows.Count == 0)
                    return;
                if (!this.customDataGridView1.Columns["本次估验金额"].Visible)
                    return;
                if (this.customDataGridView1.CurrentRow.ReadOnly)
                {
                    return;
                }
                A_HT_TOOL_GY gy1 = new A_HT_TOOL_GY(this.customDataGridView1[0, this.customDataGridView1.CurrentCell.RowIndex].Value.ToString(), this);
                gy1.ShowDialog();

            };

            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("编辑条件","  编辑条件",ClassCustom.getImage("upd.png"),btn_tj,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  查询  ", " 查询 ",ClassCustom.getImage("sel.png"), this.btn_Sel,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("自动计算", "自动计算",ClassCustom.getImage("auto.png"), this.btn_atuoxx,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  估验  ", "  估验  ",ClassCustom.getImage("gdzc.png"), gy,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };

            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (this.textBox2.ReadOnly || this.textBox2.Text == "")
                    {
                        e.Handled = true;
                        return;
                    }
                    if (this.splitContainer1.Panel1Collapsed && DialogResult.Yes == MessageBox.Show("确定本次总金额为[" + ClassCustom.UpMoney(decimal.Parse(this.textBox2.Text == "" ? "0" : this.textBox2.Text)) + "]", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        e.Handled = true;
                        this.textBox2.ReadOnly = true;
                    }
                    else
                    {
                        e.Handled = true;
                        this.textBox2.Focus();
                    }
                }
                if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void QK()
        {

        }

        #region 按钮事件

        /// <summary>
        /// 自动计算
        /// </summary>
        /// <param name="op">0 百分比 1 金额 2 不含</param>
        /// <param name="value">值</param>
        public void atuo(int op, decimal value)
        {
            try
            {
                if (this.textBox2.Text == "")
                    return;
                this.textBox2.ReadOnly = true;
                rmb = rmb_t;

                this.customDataGridView1.Sort(this.customDataGridView1.Columns["余额"], ListSortDirection.Ascending);
                foreach (DataGridViewRow r in this.customDataGridView1.Rows)
                {
                    r.Cells["本次金额"].Value = 0;
                }
                foreach (DataGridViewRow r in this.customDataGridView1.Rows)
                {
                    if (rmb <= 0)
                        break;
                    if (r.ReadOnly)
                    {
                        continue;
                    }
                    if (op == 0)
                    {
                        string b = DBAdo.ExecuteScalarSql("SELECT 结算金额*" + value / 100 + " FROM vcontracts WHERE 合同号 ='" + r.Cells["合同号"].Value.ToString() + "'").ToString();
                        decimal zbj = decimal.Parse(b == "" ? "0" : b);
                        decimal bal = decimal.Parse(r.Cells["余额"].Value.ToString());
                        decimal cur = (zbj > bal ? 0 : bal - zbj);
                        if (rmb > cur)
                        {
                            r.Cells["本次金额"].Value = cur;
                            rmb -= cur;
                        }
                        else
                        {
                            r.Cells["本次金额"].Value = rmb;
                            rmb -= decimal.Parse(r.Cells["本次金额"].Value.ToString());
                        }
                    }
                    else if (op == 1)
                    {
                        decimal bal = decimal.Parse(r.Cells["余额"].Value.ToString());
                        decimal cur = (value > bal ? 0 : bal - value);
                        if (rmb > decimal.Parse(r.Cells["余额"].Value.ToString()))
                        {
                            r.Cells["本次金额"].Value = cur;
                            rmb -= cur;
                        }
                        else
                        {
                            r.Cells["本次金额"].Value = rmb;
                            rmb -= decimal.Parse(r.Cells["本次金额"].Value.ToString());
                        }
                    }
                    else
                    {

                        if (rmb > decimal.Parse(r.Cells["余额"].Value.ToString()))
                        {
                            r.Cells["本次金额"].Value = r.Cells["余额"].Value;
                            rmb -= decimal.Parse(r.Cells["本次金额"].Value.ToString());
                        }
                        else
                        {
                            r.Cells["本次金额"].Value = rmb;
                            rmb -= decimal.Parse(r.Cells["本次金额"].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void btn_atuoxx(object sender, EventArgs e)
        {
            A_HT_AUTO auto = new A_HT_AUTO(this);
            auto.ShowDialog();
        }

        private void btn_atuo(object sender, EventArgs e)
        {

        }

        private void btn_tj(object sender, EventArgs e)
        {
            this.customDataGridView1.ColumnHeadersVisible = false;
            this.splitContainer1.Panel1Collapsed = false;
            this.splitContainer1.Panel2.Enabled = false;
        }

        private void btn_Sel(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "" || this.comboBoxLX1.Text == "" || this.comboBox1.Text == "" || this.splitContainer1.Panel1Collapsed)
                    return;
                if (this.comboBox1.Text == "付款")
                {
                    this.label3.Text = "本次付款总金额";
                }
                if (this.comboBox1.Text == "回款")
                {
                    this.label3.Text = "本次回款总金额";
                }
                if (this.comboBox1.Text == "进项发票")
                {
                    this.label3.Text = "本次进项发票";
                }
                if (this.comboBox1.Text == "销项发票")
                {
                    this.label3.Text = "本次销项发票";
                }

                this.textBox2.Text = "";
                this.textBox2.ReadOnly = false;
                string filter = " AND HKH = '" + this.textBox1.Tag.ToString() + "' AND HDW ='" + ClassConstant.DW_ID + "' AND HLX = '" + this.comboBoxLX1.SelectedValue.ToString() + "'";
                if (this.customDataGridView1.Rows.Count != 0)
                {
                    this.customDataGridView1.Rows.Clear();
                }
                string sql = "";
                switch (this.comboBox1.Text)
                {
                    case "付款":
                        sql = "select 合同号,金额1 未付货款 from vcontracts where 1=1   " + filter + " AND (结算金额 <> 金额 OR 金额 IS NULL)  ORDER BY 合同号";
                        this.customDataGridView1.Columns[1].HeaderText = "未付货款";
                        break;
                    case "回款":
                        sql = "select 合同号,金额1 未收货款 from vcontracts where 1=1   " + filter + " AND (结算金额 <> 金额 OR 金额 IS NULL)  ORDER BY 合同号";
                        this.customDataGridView1.Columns[1].HeaderText = "未收货款";
                        break;
                    case "进项发票":
                        sql = "select 合同号,发票1 未收发票,估验 from vcontracts where 1=1  " + filter + " AND (结算金额 <> 发票  OR 发票 IS NULL OR 估验<>0)  ORDER BY 合同号";
                        this.customDataGridView1.Columns[1].HeaderText = "未收发票";
                        break;
                    case "销项发票":
                        sql = "select 合同号,发票1 未开发票 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票  OR 发票 IS NULL)  ORDER BY 合同号";
                        this.customDataGridView1.Columns[1].HeaderText = "未开发票";
                        break;
                    //case "估验":
                    //    sql = "select 合同号,(发票1) AS 剩余金额 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票)  ORDER BY 合同号";
                    //    //sql = "select 合同号,(发票1-估验) AS 剩余金额 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票+估验)  ORDER BY 合同号";
                    //    this.customDataGridView1.Columns[1].HeaderText = "剩余金额";
                    //    break;
                    default:
                        break;
                }
                if (this.comboBoxLX.SelectedValue.ToString() == "03") { this.customDataGridView1.Columns[5].Visible = true; } else { this.customDataGridView1.Columns[5].Visible = false; }
                if (!(this.comboBox1.Text == "进项发票"))
                {
                    this.customDataGridView1.Columns[3].Visible = false;
                    this.customDataGridView1.Columns[4].Visible = false;
                }
                else
                {
                    this.customDataGridView1.Columns[3].Visible = true;
                    this.customDataGridView1.Columns[4].Visible = true;
                }
                if (this.comboBox1.Text == "进项发票" || this.comboBox1.Text == "销项发票" || this.comboBox1.Text == "估验")
                {
                    this.panel2.Visible = false;
                }
                else
                {
                    this.panel2.Visible = true;
                }
                this.customDataGridView1.ColName1 = this.comboBox1.Text;
                this.customDataGridView1.ColumnHeadersVisible = true;


                if (sql != "")
                {
                    Console.WriteLine(sql);
                    DataTable dt = DBAdo.DtFillSql(sql);
                    if (this.comboBox1.Text == "进项发票")
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            this.customDataGridView1.Rows.Add(new object[] { r[0].ToString(), decimal.Parse(r[1].ToString() == "" ? "0" : r[1].ToString()), DBNull.Value, decimal.Parse(r[2].ToString() == "" ? "0" : r[2].ToString()), DBNull.Value, DBNull.Value });
                        }
                    }
                    else
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            this.customDataGridView1.Rows.Add(new object[] { r[0].ToString(), decimal.Parse(r[1].ToString() == "" ? "0" : r[1].ToString()), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value });
                        }
                    }
                }

                for (int i = 0; i < this.customDataGridView1.Rows.Count; i++)
                {
                    bool mark = bool.Parse(DBAdo.ExecuteScalarSql(string.Format("SELECT FLAG FROM acontract where hcode ='{0}'", this.customDataGridView1[0, i].Value.ToString())).ToString());
                    if (!mark)
                    {
                        this.customDataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                        this.customDataGridView1.Rows[i].ReadOnly = true;
                    }
                    foreach (DataRow row in DBAdo.DtFillSql("SELECT xshth from awx where wxhth='" + this.customDataGridView1[0, i].Value.ToString() + "'").Rows)
                    {
                        (this.customDataGridView1[5, i] as DataGridViewComboBoxCell).Items.Add(row[0].ToString());
                    }
                }
                this.customDataGridView1.Invalidate();
                this.splitContainer1.Panel1Collapsed = true;
                this.splitContainer1.Panel2.Enabled = true;
                this.textBox2.Text = "";
                this.textBox4.Text = "";
                this.textBox5.Text = "";
                this.textBox6.Text = "";
                this.Text = "合同进度明细管理 → " + this.textBox1.Text + " → " + this.comboBoxLX.Text + " → " + this.comboBox1.Text;
                //this.splitContainer1.Panel2Collapsed = false;
                this.customDataGridView1.CurrentCell = null;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                #region 判断是否该月结账
                string tsql = string.Format("SELECT flag FROM AMONTH WHERE [YEAR] ={0} AND [MONTH]={1} AND HDW = {2}", this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), ClassConstant.DW_ID);
                object result = DBAdo.ExecuteScalarSql(tsql);
                if (bool.Parse(result == null ? false.ToString() : result.ToString()))
                {
                    MessageBox.Show("本月已结账不能添加进度信息", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                #endregion


                this.customDataGridView1.EndEdit();
                this.Validate();
                if (this.textBox2.Text == "")
                    return;
                if (decimal.Parse(this.textBox3.Text) != 0)
                    return;
                if (this.comboBox1.Text == "付款" || this.comboBox1.Text == "回款")
                {
                    if (decimal.Parse(this.textBox2.Text) != (decimal.Parse((this.textBox4.Text == "" ? "0" : this.textBox4.Text))
                        + decimal.Parse((this.textBox5.Text == "" ? "0" : this.textBox5.Text)) + decimal.Parse((this.textBox6.Text == "" ? "0" : this.textBox6.Text))))
                    {
                        return;
                    }
                }

                //#region 判断是否该月结账
                //string tsql = string.Format("SELECT flag FROM AMONTH WHERE [YEAR] ={0} AND [MONTH]={1} AND HDW = {2}", this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), ClassConstant.DW_ID);
                //object result = DBAdo.ExecuteScalarSql(tsql);
                //if (bool.Parse(result == null ? false.ToString() : result.ToString()))
                //{
                //    MessageBox.Show("本月已结账不能添加进度信息", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //    return;
                //}
                //#endregion


                bool flag = false;
                bool flag1 = false;
                string sql;
                decimal gysum = 0;
                foreach (DataGridViewRow r in this.customDataGridView1.Rows)
                {
                    if (r.Cells["本次估验金额"].Value != null)
                    {
                        if (r.Cells["本次估验金额"].Value.ToString() != "")
                        {
                            if (decimal.Parse(r.Cells["本次估验金额"].Value.ToString()) != 0)
                            {
                                flag = true;
                                gysum += decimal.Parse(r.Cells["本次估验金额"].Value.ToString());
                            }
                        }
                    }
                    if (r.Cells["本次金额"].Value != null)
                    {
                        if (r.Cells["本次金额"].Value.ToString() != "")
                        {
                            if (decimal.Parse(r.Cells["本次金额"].Value.ToString()) != 0)
                            {
                                flag1 = true;
                            }
                        }
                    }
                }
                if (this.comboBox1.Text == "销项发票")
                {
                    sql = "INSERT INTO ACash (ExchangeDate, Cash, Note,VoucherFlag,Ccode,Type,Mz,hdw) VALUES (";
                    sql += "'" + this.dateTimePicker1.Value.ToShortDateString() + "',";
                    sql += "'" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "',";
                    sql += "'" + (this.textBox2.Text == "" ? "0" : this.textBox2.Text) + "',";
                    sql += "'0',";
                    sql += "'" + this.textBox1.Tag.ToString() + "',";
                    sql += "'" + this.comboBox1.Text + "',";
                    sql += "'" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "'";
                    sql += ",'" + ClassConstant.DW_ID + "') ";
                }
                else if (this.comboBox1.Text == "进项发票")
                {
                    sql = "INSERT INTO ACash (ExchangeDate, Cash, Note,VoucherFlag,Ccode,Type,Mz,hdw) VALUES (";
                    sql += "'" + this.dateTimePicker1.Value.ToShortDateString() + "',";
                    sql += "'" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "',";
                    sql += "'" + (this.textBox2.Text == "" ? "0" : this.textBox2.Text) + "',";
                    sql += "'0',";
                    sql += "'" + this.textBox1.Tag.ToString() + "',";
                    sql += "'" + this.comboBox1.Text + "',";
                    sql += "'" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "'";
                    sql += ",'" + ClassConstant.DW_ID + "') ";
                }
                else
                {
                    sql = "INSERT INTO ACash (ExchangeDate, Cash, Note,VoucherFlag,Ccode,Type,Mz,hdw) VALUES (";
                    sql += "'" + this.dateTimePicker1.Value.ToShortDateString() + "',";
                    sql += "'" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "',";
                    sql += "'" + (this.textBox5.Text == "" ? "0" : this.textBox5.Text) + "',";
                    sql += "'0',";
                    sql += "'" + this.textBox1.Tag.ToString() + "',";
                    sql += "'" + this.comboBox1.Text + "',";
                    sql += "'" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "'";
                    sql += ",'" + ClassConstant.DW_ID + "') ";
                }

                string id = "";
                string gyid = "";
                string sqlgy = "";
                if (flag1)
                {
                    id = DBAdo.ExecuteScalarSql(sql).ToString();
                }

                if (flag)
                {
                    sqlgy = "INSERT INTO ACash (ExchangeDate, Cash, Note,VoucherFlag,Ccode,Type,Mz,hdw) VALUES (";
                    sqlgy += "'" + this.dateTimePicker1.Value.ToShortDateString() + "',";
                    sqlgy += "'" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "',";
                    sqlgy += "'" + gysum + "',";
                    sqlgy += "'0',";
                    sqlgy += "'" + this.textBox1.Tag.ToString() + "',";
                    sqlgy += "'估验',";
                    sqlgy += "'" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "'";
                    sqlgy += ",'" + ClassConstant.DW_ID + "') ";
                    gyid = DBAdo.ExecuteScalarSql(sqlgy).ToString();
                }


                sql = "";
                foreach (DataGridViewRow r in this.customDataGridView1.Rows)
                {
                    if (flag1 && r.Cells["本次金额"].Value != null)
                    {
                        if (decimal.Parse(r.Cells["本次金额"].Value.ToString() == "" ? "0" : r.Cells["本次金额"].Value.ToString()) != 0)
                        {
                            sql += " INSERT INTO AFKXX ([rmb], [hth], [xshth], [type],Cid,date) VALUES(";
                            sql += "'" + (r.Cells["本次金额"].Value.ToString() == "" ? "0" : r.Cells["本次金额"].Value.ToString()) + "',";
                            sql += "'" + r.Cells["合同号"].Value.ToString() + "',";
                            sql += "'" + (r.Cells["销售合同号"].Value == null ? "" : r.Cells["销售合同号"].Value.ToString()) + "',";
                            sql += "'" + this.comboBox1.Text + "',";
                            sql += id + ",'" + this.dateTimePicker1.Value.ToShortDateString() + "') ";
                        }
                    }
                    if (flag && r.Cells["本次估验金额"].Value != null)
                    {
                        if (decimal.Parse(r.Cells["本次估验金额"].Value.ToString() == "" ? "0" : r.Cells["本次估验金额"].Value.ToString()) != 0)
                        {
                            sql += " INSERT INTO AFKXX ([rmb], [hth], [xshth], [type],Cid,date) VALUES(";
                            sql += "'" + (r.Cells["本次估验金额"].Value.ToString() == "" ? "0" : r.Cells["本次估验金额"].Value.ToString()) + "',";
                            sql += "'" + r.Cells["合同号"].Value.ToString() + "',";
                            sql += "'" + (r.Cells["销售合同号"].Value == null ? "" : r.Cells["销售合同号"].Value.ToString()) + "',";
                            sql += "'估验',";
                            sql += gyid + ",'" + this.dateTimePicker1.Value.ToShortDateString() + "') ";
                        }
                    }

                }
                if (sql == "")
                    return;
                string strTemp = string.Empty;
                foreach (DataGridViewRow r in this.customDataGridView1.Rows)
                {
                    strTemp += string.Format(",'{0}'", r.Cells["合同号"].Value.ToString());
                }
                if (strTemp.Length > 0)
                {
                    string checkSq = string.Format("SELECT ISNULL(MIN(HDATE),GETDATE()) FROM ACONTRACT WHERE HCODE IN({0})", strTemp.Substring(1));
                    object obj = DBAdo.ExecuteScalarSql(checkSq);
                    if (obj != null)
                    {
                        if (this.dateTimePicker1.Value < DateTime.Parse(obj.ToString()))
                        {
                            MessageBox.Show("业务日期不能小于合同日期!");
                            return;
                        }
                    }
                }


                DBAdo.ExecuteNonQuerySql(sql);
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.textBox2.Text = "";
                this.textBox4.Text = "";
                this.textBox5.Text = "";
                this.textBox6.Text = "";
                this.textBox2.ReadOnly = false;
                this.splitContainer1.Panel1Collapsed = false;
                //this.splitContainer1.Panel2Collapsed = true;
                this.customDataGridView1.Rows.Clear();
                this.splitContainer1.Panel2.Enabled = false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
        }


        private void btn_close(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region IChildForm 成员
        /// <summary>
        /// FORM 激活时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Button btn1;
        public void FormActivated(object sender, EventArgs e)
        {
            (this.MdiParent as MForm1).AddButtons(this.bts);
            btn1 = new Button();
            btn1.Location = new System.Drawing.Point(3, 3);
            btn1.Name = this.Name;
            btn1.Size = new System.Drawing.Size(150, 23);
            btn1.Text = this.Text;
            btn1.UseVisualStyleBackColor = true;
            btn1.Margin = new Padding(0, 0, 0, 0);
            btn1.Tag = this;
            btn1.Click += new EventHandler(btn1_Click);
            (this.MdiParent as MForm1).AddStatus(btn1);

        }
        void btn1_Click(object sender, EventArgs e)
        {
            this.Activate();
        }
        /// <summary>
        /// FORM 停用时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FormDeactivate(object sender, EventArgs e)
        {
            //MAINFROM工具栏按钮清空
            (this.MdiParent as MForm1).ClearButtons();

        }

        public void Form_Closing(object sender, EventArgs e)
        {
            (this.MdiParent as MForm1).DelStatus(btn1);
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_FZ_KH)))
                return;
            A_FZ_KH cm = new A_FZ_KH(this);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }



        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            this.textBox1.Text = value;
            this.textBox1.Tag = key;
        }

        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    return;
                }
                this.Text = "合同进度明细管理 → " + this.textBox1.Text + " → " + this.comboBoxLX.Text + " → " + this.comboBox1.Text;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void customDataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = 92;
        }

        public void setGy(decimal gy)
        {
            try
            {
                this.customDataGridView1["本次估验金额", this.customDataGridView1.CurrentCell.RowIndex].Value = gy;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!mark)
                {
                    return;
                }
                if (this.textBox2.ReadOnly || this.textBox2.Text == "")
                {
                    return;
                }
                if (this.splitContainer1.Panel1Collapsed && DialogResult.Yes == MessageBox.Show("确定本次总金额为[" + ClassCustom.UpMoney(decimal.Parse(this.textBox2.Text == "" ? "0" : this.textBox2.Text)) + "]", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    this.textBox2.ReadOnly = true;
                }
                else
                {
                    this.textBox2.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void customDataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (this.customDataGridView1.CurrentCell.Value == null)
                    return;
                if (String.IsNullOrEmpty(this.customDataGridView1.CurrentCell.Value.ToString()) || e.ColumnIndex != this.customDataGridView1.Columns[2].Index)
                    return;

                //if (decimal.Parse(this.customDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()) < decimal.Parse(this.customDataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()))
                //{
                //    if (this.comboBox1.Text == "付款" || this.comboBox1.Text == "回款")
                //    {
                //        if (DialogResult.Yes == MessageBox.Show("输入金额大于余额是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                //        {

                //        }
                //        else
                //        {
                //            this.customDataGridView1.CurrentCell.Value = DBNull.Value;
                //        }
                //    }
                //    else
                //    {
                //        this.customDataGridView1.CurrentCell.Value = DBNull.Value;

                //    }
                //}
                decimal rmb1 = 0;
                foreach (DataGridViewRow r in this.customDataGridView1.Rows)
                {
                    rmb1 += decimal.Parse(r.Cells["本次金额"].Value.ToString() == "" ? "0" : r.Cells["本次金额"].Value.ToString());
                }
                this.textBox3.Text = (rmb_t - rmb1).ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void A_HT_FKFPGY_FormClosing(object sender, FormClosingEventArgs e)
        {
            mark = false;
        }

        private void customDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (this.comboBox1.Text == "进项发票" || this.comboBox1.Text == "销项发票")
                {
                    if (e.ColumnIndex == this.customDataGridView1.Columns["本次金额"].Index)
                    {
                        if (e.FormattedValue != null)
                        {
                            if (e.FormattedValue.ToString() != "")
                            {
                                if (decimal.Parse(e.FormattedValue.ToString()) > decimal.Parse(this.customDataGridView1["余额", e.RowIndex].Value.ToString()))
                                {
                                    MessageBox.Show(string.Format("发票金额不能大于[{0}]金额", this.comboBox1.Text == "进项发票" ? "未开发票" : "未收发票"));
                                    e.Cancel = true;
                                }
                            }
                        }
                    }
                }
                if (this.comboBox1.Text == "付款" || this.comboBox1.Text == "回款")
                {
                    if (e.ColumnIndex == this.customDataGridView1.Columns["本次金额"].Index)
                    {
                        if (e.FormattedValue != null)
                        {
                            if (e.FormattedValue.ToString() != "")
                            {
                                if (decimal.Parse(e.FormattedValue.ToString()) > decimal.Parse(this.customDataGridView1["余额", e.RowIndex].Value.ToString()))
                                {
                                    MessageBox.Show(string.Format("本次金额不能大于[{0}]金额", this.comboBox1.Text == "付款" ? "付款" : "回款"));
                                    e.Cancel = true;
                                }
                            }
                        }
                    }
                }
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
