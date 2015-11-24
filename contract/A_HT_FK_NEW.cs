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
    public partial class A_HT_FK_NEW : Form, IsetText
    {
        private ToolStripItem[] bts = null;
        private decimal rmb;
        private decimal rmb_t;
        private DataTable souce;
        private DataTable dt;
        private DataTable dt1;
        private DataTable dt2;
        private DataGridViewComboBoxColumn col1;
        private DataGridViewComboBoxColumn col2;
        private DataGridViewComboBoxColumn col3;
        private DataGridViewTextBoxColumn tol1;
        private DataGridViewTextBoxColumn tol2;
        private DataGridViewTextBoxColumn tol3;


        public A_HT_FK_NEW()
        {
            InitializeComponent();
        }

        private void A_HT_FK_NEW_Load(object sender, EventArgs e)
        {
            try
            {
                this.splitContainer1.Panel2.Enabled = false;
                Reg();
                DataLoad();
                DgvCssSet();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_FZ_KH)))
                return;
            A_FZ_KH cm = new A_FZ_KH(this);
            cm.MdiParent = this.MdiParent;
            cm.Show();

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            return;
            //this.dataGridView1.CurrentCell.Value = DBNull.Value;
        }

        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            this.textBox1.Tag = key;
            this.textBox1.Text = value;
        }

        #endregion

        #region Form基本方法

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void DgvCssSet()
        {
            return;
            dt = new DataTable();

            dt.Columns.Add("合同号", typeof(string));
            dt.Columns.Add("余额", typeof(decimal));
            dt.Columns.Add("本次金额", typeof(decimal));
            col1 = new DataGridViewComboBoxColumn();
            col2 = new DataGridViewComboBoxColumn();
            col3 = new DataGridViewComboBoxColumn();
            tol1 = new DataGridViewTextBoxColumn();
            tol2 = new DataGridViewTextBoxColumn();
            tol3 = new DataGridViewTextBoxColumn();

            tol1.HeaderText = "开行";
            tol2.HeaderText = "新公司";
            tol3.HeaderText = "原公司";
            col1.HeaderText = "类型";
            col2.HeaderText = "方式";
            col3.HeaderText = "销售合同号";

            tol1.Name = "开行";
            tol2.Name = "新公司";
            tol3.Name = "原公司";
            col1.Name = "类型";
            col2.Name = "方式";
            col3.Name = "销售合同号";
            tol1.ValueType = typeof(decimal);
            tol2.ValueType = typeof(decimal);
            tol3.ValueType = typeof(decimal);
            tol1.DefaultCellStyle.Format = "N2";
            tol2.DefaultCellStyle.Format = "N2";
            tol3.DefaultCellStyle.Format = "N2";
            col1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            col2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            col3.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;


            col1.Items.AddRange(ClassConstant.fklx);
            col2.Items.AddRange(ClassConstant.fkfs);
            col1.Width = 60;
            col2.Width = 60;
            tol1.Width = 80;
            tol2.Width = 80;
            tol3.Width = 80;
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { col1, col2, col3, tol1, tol2, tol3 });

        }

        private void DataLoad()
        {
            try
            {
                this.dataGridView1.Columns[1].ValueType = typeof(decimal);
                this.dataGridView1.Columns[2].ValueType = typeof(decimal);
                foreach (string s in ClassConstant.fklx)
                {
                    (this.dataGridView1.Columns[3] as DataGridViewComboBoxColumn).Items.Add(s);
                }
                foreach (string s in ClassConstant.fkfs)
                {
                    (this.dataGridView1.Columns[4] as DataGridViewComboBoxColumn).Items.Add(s);
                }


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

        private void Reg()
        {
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
            if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
            {
                e.Handled = true;
            }
        }

        private void QK()
        {

        }

        #region 按钮事件
        //private void setSouce(string sql, string name)
        //{
        //    try
        //    {

        //        dt.Rows.Clear();

        //        foreach (DataRow r in DBAdo.DtFillSql(sql).Rows)
        //        {
        //            dt.Rows.Add(new object[] { r[0].ToString(), r[1].ToString() == "" ? decimal.Parse("0") : decimal.Parse(r[1].ToString()) });

        //        }

        //        if (this.comboBoxLX.SelectedValue.ToString() == "04")
        //        {
        //            tol1.Visible = true;
        //            tol2.Visible = true;
        //            tol3.Visible = true;
        //        }
        //        else
        //        {
        //            tol1.Visible = false;
        //            tol2.Visible = false;
        //            tol3.Visible = false;
        //        }

        //        //this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { col1, col2, col3 });
        //        this.dataGridView1.Columns["合同号"].ReadOnly = true;
        //        this.dataGridView1.Columns["余额"].ReadOnly = true;
        //        foreach (DataGridViewColumn c in this.dataGridView1.Columns)
        //        {
        //            c.SortMode = DataGridViewColumnSortMode.NotSortable;
        //        }
        //        this.dataGridView1.Columns["余额"].HeaderText = name;
        //        this.dataGridView1.Columns["余额"].DefaultCellStyle.Format = "N2";
        //        this.dataGridView1.Columns["本次金额"].DefaultCellStyle.Format = "N2";
        //        this.dataGridView1.Columns["余额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //        this.dataGridView1.Columns["本次金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        //        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
        //        {
        //            foreach (DataRow row in DBAdo.DtFillSql("SELECT xshth from awx where wxhth='" + this.dataGridView1["合同号", i].Value.ToString() + "'").Rows)
        //            {
        //                (this.dataGridView1["销售合同号", i] as DataGridViewComboBoxCell).Items.Add(row[0].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("Test" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageView.MessageErrorShow(ex);
        //        return;
        //    }
        //}


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

                this.dataGridView1.Sort(this.dataGridView1.Columns["balance"], ListSortDirection.Ascending);
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    r.Cells["je"].Value = 0;
                }
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    if (rmb <= 0)
                        break;
                    if (op == 0)
                    {
                        string b = DBAdo.ExecuteScalarSql("SELECT 结算金额*" + value / 100 + " FROM vcontracts WHERE 合同号 ='" + r.Cells["hcode"].Value.ToString() + "'").ToString();
                        decimal zbj = decimal.Parse(b == "" ? "0" : b);
                        decimal bal = decimal.Parse(r.Cells["balance"].Value.ToString());
                        decimal cur = (zbj > bal ? 0 : bal - zbj);
                        if (rmb > cur)
                        {
                            r.Cells["je"].Value = cur;
                            rmb -= cur;
                        }
                        else
                        {
                            r.Cells["je"].Value = rmb;
                            rmb -= decimal.Parse(r.Cells["je"].Value.ToString());
                        }
                    }
                    else if (op == 1)
                    {
                        decimal bal = decimal.Parse(r.Cells["balance"].Value.ToString());
                        decimal cur = (value > bal ? 0 : bal - value);
                        if (rmb > decimal.Parse(r.Cells["balance"].Value.ToString()))
                        {
                            r.Cells["je"].Value = cur;
                            rmb -= cur;
                        }
                        else
                        {
                            r.Cells["je"].Value = rmb;
                            rmb -= decimal.Parse(r.Cells["je"].Value.ToString());
                        }
                    }
                    else
                    {

                        if (rmb > decimal.Parse(r.Cells["balance"].Value.ToString()))
                        {
                            r.Cells["je"].Value = r.Cells["balance"].Value;
                            rmb -= decimal.Parse(r.Cells["je"].Value.ToString());
                        }
                        else
                        {
                            r.Cells["je"].Value = rmb;
                            rmb -= decimal.Parse(r.Cells["je"].Value.ToString());
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
            //A_HT_AUTO auto = new A_HT_AUTO(this);
            //auto.ShowDialog();
        }

        private void btn_atuo(object sender, EventArgs e)
        {

        }

        private void btn_tj(object sender, EventArgs e)
        {
            this.splitContainer1.Panel1Collapsed = false;
            this.splitContainer1.Panel2.Enabled = false;
        }

        private void btn_Sel(object sender, EventArgs e)
        {

            try
            {
                if (this.textBox1.Text == "" || this.comboBoxLX1.Text == "" || this.comboBox1.Text == "" || this.splitContainer1.Panel1Collapsed)
                    return;
                this.textBox2.Text = "";
                this.textBox2.ReadOnly = false;
                string filter = " AND HKH = '" + this.textBox1.Tag.ToString() + "' AND HDW ='" + ClassConstant.DW_ID + "' AND HLX = '" + this.comboBoxLX1.SelectedValue.ToString() + "'";
                if (this.dataGridView1.Rows.Count != 0)
                {
                    this.dataGridView1.Rows.Clear();
                }
                string sql = "";
                switch (this.comboBox1.Text)
                {
                    case "付款":
                        sql = "select 合同号,金额1 未付货款 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 金额 OR 金额 IS NULL)  ORDER BY 合同号";
                        this.dataGridView1.Columns[1].HeaderText = "未付货款";
                        break;
                    case "回款":
                        sql = "select 合同号,金额1 未收货款 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 金额 OR 金额 IS NULL)  ORDER BY 合同号";
                        this.dataGridView1.Columns[1].HeaderText = "未收货款";
                        break;
                    case "进项发票":
                        sql = "select 合同号,发票1 未收发票 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票  OR 发票 IS NULL)  ORDER BY 合同号";
                        this.dataGridView1.Columns[1].HeaderText = "未收发票";
                        break;
                    case "销项发票":
                        sql = "select 合同号,发票1 未开发票 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票  OR 发票 IS NULL)  ORDER BY 合同号";
                        this.dataGridView1.Columns[1].HeaderText = "未开发票";
                        break;
                    case "估验":
                        sql = "select 合同号,(发票1) AS 剩余金额 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票)  ORDER BY 合同号";
                        //sql = "select 合同号,(发票1-估验) AS 剩余金额 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票+估验)  ORDER BY 合同号";
                        this.dataGridView1.Columns[1].HeaderText = "剩余金额";
                        break;
                    default:
                        break;
                }
                if (this.comboBoxLX.SelectedValue.ToString() == "03") { this.dataGridView1.Columns[5].Visible = true; } else { this.dataGridView1.Columns[5].Visible = false; }
                if ((this.comboBoxLX.SelectedValue.ToString() == "04" || this.comboBoxLX.SelectedValue.ToString() == "05") && this.comboBox1.Text == "付款")
                {
                    this.panel3.Visible = true;
                }
                else
                {
                    this.panel3.Visible = false;
                }
                if (this.comboBox1.Text == "进项发票" || this.comboBox1.Text == "销项发票" || this.comboBox1.Text == "估验")
                {
                    this.panel2.Visible = false;
                }
                else
                {
                    this.panel2.Visible = true;
                }
                if (sql != "")
                {
                    DataTable dt = DBAdo.DtFillSql(sql);
                    foreach (DataRow r in dt.Rows)
                    {
                        this.dataGridView1.Rows.Add(new object[] { r[0].ToString(), decimal.Parse(r[1].ToString() == "" ? "0" : r[1].ToString()), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value });
                    }
                }

                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    foreach (DataRow row in DBAdo.DtFillSql("SELECT xshth from awx where wxhth='" + this.dataGridView1[0, i].Value.ToString() + "'").Rows)
                    {
                        (this.dataGridView1[5, i] as DataGridViewComboBoxCell).Items.Add(row[0].ToString());
                    }
                }

                this.splitContainer1.Panel1Collapsed = true;
                this.splitContainer1.Panel2.Enabled = true;
                this.splitContainer1.Panel2Collapsed = false;
                //MessageBox.Show(this.dataGridView1.Rows.Count.ToString());
                //if (this.comboBox1.Text == "付款")
                //{
                //    sql = "select 合同号,金额1 未付货款 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 金额 OR 金额 IS NULL)  ORDER BY 合同号";
                //    if (sql != "")
                //    {
                //        setSouce(sql, "未付货款");
                //    }
                //}
                //if (this.comboBox1.Text == "回款")
                //{
                //    sql = "select 合同号,金额1 未收货款 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 金额 OR 金额 IS NULL)  ORDER BY 合同号";
                //    if (sql != "")
                //    {
                //        setSouce(sql, "未收货款");
                //    }
                //}
                //if (this.comboBox1.Text == "进项发票")
                //{
                //    sql = "select 合同号,发票1 未收发票 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票  OR 发票 IS NULL)  ORDER BY 合同号";
                //    if (sql != "")
                //    {
                //        setSouce(sql, "未收发票");
                //    }
                //}
                //if (this.comboBox1.Text == "销项发票")
                //{
                //    sql = "select 合同号,发票1 未开发票 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票  OR 发票 IS NULL)  ORDER BY 合同号";
                //    if (sql != "")
                //    {
                //        setSouce(sql, "未开发票");
                //    }
                //}
                //if (this.comboBox1.Text == "估验")
                //{
                //    sql = "select 合同号,(发票1-估验) AS 剩余金额 from vcontracts where 1=1 " + filter + " AND (结算金额 <> 发票+估验)  ORDER BY 合同号";
                //    if (sql != "")
                //    {
                //        setSouce(sql, "剩余金额");
                //    }
                //}
                this.dataGridView1.Columns[1].DefaultCellStyle.Format = "N4";
                this.dataGridView1.Columns[2].DefaultCellStyle.Format = "N4";
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
                if (this.textBox2.Text == "")
                    return;
                if (decimal.Parse(this.textBox3.Text) != 0)
                    return;
                if (this.comboBox1.Text == "付款" || this.comboBox1.Text == "回款")
                {
                    if (decimal.Parse(this.textBox2.Text) != (decimal.Parse((this.textBox4.Text == "" ? "0" : this.textBox4.Text))
                        + decimal.Parse((this.textBox5.Text == "" ? "0" : this.textBox5.Text)) + decimal.Parse((this.textBox9.Text == "" ? "0" : this.textBox9.Text))))
                    {
                        return;
                    }
                }

                #region 判断是否该月结账
                string tsql = string.Format("SELECT flag FROM AMONTH WHERE [YEAR] ={0} AND [MONTH]={1} AND HDW = {2}", this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), ClassConstant.DW_ID);
                object result = DBAdo.ExecuteScalarSql(tsql);
                if (bool.Parse(result == null ? false.ToString() : result.ToString()))
                {
                    MessageBox.Show("本月已结账不能添加进度信息", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                #endregion



                string sql;
                if (this.comboBox1.Text == "进项发票" || this.comboBox1.Text == "销项发票" || this.comboBox1.Text == "估验")
                {
                    sql = "INSERT INTO ACash (ExchangeDate, Cash, Note,VoucherFlag, ZJBL1, ZJBL2, ZJBL3,Ccode,Type,Mz,hdw) VALUES (";
                    sql += "'" + this.dateTimePicker1.Value.ToShortDateString() + "',";
                    sql += "'" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "',";
                    sql += "'" + (this.textBox2.Text == "" ? "0" : this.textBox2.Text) + "',";
                    sql += "'0',";
                    sql += "'" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "',";
                    sql += "'" + (this.textBox7.Text == "" ? "0" : this.textBox7.Text) + "',";
                    sql += "'" + (this.textBox8.Text == "" ? "0" : this.textBox8.Text) + "',";
                    sql += "'" + this.textBox1.Tag.ToString() + "',";
                    sql += "'" + this.comboBox1.Text + "',";
                    sql += "'" + (this.textBox9.Text == "" ? "0" : this.textBox9.Text) + "'";
                    sql += ",'" + ClassConstant.DW_ID + "') ";
                }
                else
                {
                    sql = "INSERT INTO ACash (ExchangeDate, Cash, Note,VoucherFlag, ZJBL1, ZJBL2, ZJBL3,Ccode,Type,Mz,hdw) VALUES (";
                    sql += "'" + this.dateTimePicker1.Value.ToShortDateString() + "',";
                    sql += "'" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "',";
                    sql += "'" + (this.textBox5.Text == "" ? "0" : this.textBox5.Text) + "',";
                    sql += "'0',";
                    sql += "'" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "',";
                    sql += "'" + (this.textBox7.Text == "" ? "0" : this.textBox7.Text) + "',";
                    sql += "'" + (this.textBox8.Text == "" ? "0" : this.textBox8.Text) + "',";
                    sql += "'" + this.textBox1.Tag.ToString() + "',";
                    sql += "'" + this.comboBox1.Text + "',";
                    sql += "'" + (this.textBox9.Text == "" ? "0" : this.textBox9.Text) + "'";
                    sql += ",'" + ClassConstant.DW_ID + "') ";
                }




                string id = DBAdo.ExecuteScalarSql(sql).ToString();

                sql = "";
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    if (r.Cells[2].Value != null)
                    {
                        if (decimal.Parse(r.Cells[2].Value.ToString() == "" ? "0" : r.Cells[2].Value.ToString()) != 0)
                        {
                            sql += " INSERT INTO AFKXX ([rmb], [hth], [xshth], [type], [fkfs], [fklx],Cid,date) VALUES(";
                            sql += "'" + (r.Cells[2].Value.ToString() == "" ? "0" : r.Cells[2].Value.ToString()) + "',";
                            sql += "'" + r.Cells[0].Value.ToString() + "',";
                            sql += "'" + (r.Cells[5].Value == null ? "" : r.Cells[5].Value.ToString()) + "',";
                            sql += "'" + this.comboBox1.Text + "',";
                            sql += "'" + (r.Cells[4].Value == null ? "" : r.Cells[4].Value.ToString()) + "',";
                            sql += "'" + (r.Cells[3].Value == null ? "" : r.Cells[3].Value.ToString()) + "',";
                            sql += id + ",'" + this.dateTimePicker1.Value.ToShortDateString() + "') ";
                        }
                    }
                }
                if (sql == "")
                    return;

                DBAdo.ExecuteNonQuerySql(sql);
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.textBox2.Text = "";
                this.textBox4.Text = "";
                this.textBox5.Text = "";
                this.textBox6.Text = "";
                this.textBox7.Text = "";
                this.textBox8.Text = "";
                this.textBox9.Text = "";
                this.textBox2.ReadOnly = false;
                this.splitContainer1.Panel1Collapsed = false;
                //this.splitContainer1.Panel2Collapsed = true;
                this.dataGridView1.Rows.Clear();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    return;
                }
                this.Text = "合同进度明细管理 - " + this.textBox1.Text;
                //    string sql = "select 合同号 from vcontracts where HKH = '" + this.textBox1.Tag.ToString() + "'";
                //    foreach (DataRow r in DBAdo.DtFillSql(sql).Rows)
                //    {
                //        dt.Rows.Add(r[0].ToString());
                //    }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }


        #endregion

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
                    this.comboBox1.Items.AddRange(new object[] { "付款", "进项发票", "估验" });
                }


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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (this.dataGridView1.CurrentCell.Value == null)
                    return;
                if (String.IsNullOrEmpty(this.dataGridView1.CurrentCell.Value.ToString()) || e.ColumnIndex != this.dataGridView1.Columns[2].Index)
                    return;

                if (decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()) < decimal.Parse(this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()))
                {
                    if (this.comboBox1.Text == "付款" || this.comboBox1.Text == "回款")
                    {
                        if (DialogResult.Yes == MessageBox.Show("输入金额大于余额是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {

                        }
                        else
                        {
                            this.dataGridView1.CurrentCell.Value = DBNull.Value;
                        }
                    }
                    else
                    {
                        this.dataGridView1.CurrentCell.Value = DBNull.Value;

                    }
                }
                decimal rmb1 = 0;
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    rmb1 += decimal.Parse(r.Cells[2].Value.ToString() == "" ? "0" : r.Cells[2].Value.ToString());
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

        private void textBox2_Leave(object sender, EventArgs e)
        {
            try
            {

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
                //this.textBox4.Text = this.textBox2.Text;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.textBox2.Text == "") return;
            //    this.textBox5.Text = (decimal.Parse(this.textBox2.Text) - decimal.Parse(this.textBox4.Text)).ToString();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.textBox2.Text == "") return;
            //    this.textBox4.Text = (decimal.Parse(this.textBox2.Text) - decimal.Parse(this.textBox5.Text)).ToString();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}
