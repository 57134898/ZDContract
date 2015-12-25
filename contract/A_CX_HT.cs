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
    public partial class A_CX_HT : Form, IChildForm, IsetText
    {
        private ToolStripItem[] bts = null;
        private DataTable dtgx;
        private DataTable dtfields;
        private DataTable dt_lx;
        private DataTable dt_lx1;
        private DataTable dt_sp;
        private DataTable dt_cz;
        private DataTable dt_xm;
        private DataTable dt_hth;
        private DataView dv_lx;
        private DataView dv_lx1;
        private DataView dv_sp;
        private DataView dv_cz;
        private DataView dv_hth;
        private DataView dv_xm;
        private string curhth;
        private bool loaded;
        private DataView souce;


        public A_CX_HT()
        {
            InitializeComponent();
        }



        private void A_CX_HT_Load(object sender, EventArgs e)
        {
            try
            {
                string sql_s1 = @"SELECT [bcode],[bname],[shortcode],[shortname]
FROM [Bcode] WHERE LEN(Bcode)<=4 AND (Bcode LIKE '01%' OR  Bcode LIKE '02%')";
                DataTable dt_dt1 = DBAdo.DtFillSql(sql_s1);
                this.comboBox1.Items.Clear();
                foreach (DataRow r in dt_dt1.Rows)
                {
                    this.comboBox1.Items.Add(r["bcode"].ToString() + ":" + r["bname"].ToString());
                }

                this.sumGridView1.Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.sumGridView1.Grid.MultiSelect = false;
                dtgx = new DataTable();
                dtgx.Columns.Add("key", typeof(string));
                dtgx.Columns.Add("value", typeof(string));
                dtgx.Rows.Add(new object[] { "等于", " = " });
                dtgx.Rows.Add(new object[] { "相似于", " LIKE " });
                dtgx.Rows.Add(new object[] { "大于", " > " });
                dtgx.Rows.Add(new object[] { "小于", " < " });
                dtgx.Rows.Add(new object[] { "不等于", " != " });
                dtgx.Rows.Add(new object[] { "不相似于", " NOT LIKE " });
                (this.dataGridView2.Columns[2] as DataGridViewComboBoxColumn).DataSource = dtgx;
                (this.dataGridView2.Columns[2] as DataGridViewComboBoxColumn).DisplayMember = "key";
                (this.dataGridView2.Columns[2] as DataGridViewComboBoxColumn).ValueMember = "value";
                dtfields = DBAdo.DtFillSql("SELECT [ID], [key], [value], [field], [table], [order] FROM [ACXSET]");
                (this.dataGridView2.Columns[1] as DataGridViewComboBoxColumn).DataSource = dtfields;
                (this.dataGridView2.Columns[1] as DataGridViewComboBoxColumn).DisplayMember = "key";
                (this.dataGridView2.Columns[1] as DataGridViewComboBoxColumn).ValueMember = "value";
                //this.dataGridView2.Rows.Add(20);
                loaded = true;
                this.sumGridView1.Grid.CellDoubleClick += new DataGridViewCellEventHandler(sumGridView1_Grid_CellDoubleClick);
                this.sumGridView1.Grid.CellClick += new DataGridViewCellEventHandler(sumGridView1_Grid_CellClick);
                Reg();
                DataLoad();

                if (ClassConstant.USER_NAME == "于萍" || ClassConstant.USER_ID == "0101999999")
                {
                    this.comboBox1.Enabled = true;
                }
                else
                {
                    this.comboBox1.Text = ClassConstant.DW_ID + ":" + ClassConstant.DW_NAME;
                    foreach (object item in this.comboBox1.Items)
                    {
                        if (ClassCustom.codeSub(item.ToString()) == ClassConstant.DW_ID)
                        {
                            this.comboBox1.SelectedItem = item;
                        }
                    }
                }





            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void Reg()
        {//ToolStripItem    new ToolStripSeparator(),
            OnButtonClick st = (object sender, EventArgs e) =>
            {
                if (souce != null)
                {
                    Sort s = new Sort(this, souce.Table);
                    s.ShowDialog();
                }
            };

            bts = new ToolStripItem[]{
                new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  查询  ","  查询  ",ClassCustom.getImage("sel.png"),btn_cx,null,true).TBtnProduce(),
                    new Factory_ToolBtn("编辑条件","  编辑条件",ClassCustom.getImage("upd.png"),btn_tj,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  排序  ","    排序  ",ClassCustom.getImage("yj.png"),st,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  关闭  ","  关闭  ",ClassCustom.getImage("tc.png"), btn_close,null,true).TBtnProduce()
                    };
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        private void DataLoad()
        {
            try
            {
                //dt1 = DBAdo.DtFillSql("SELECT DISTINCT HCODE FROM ACONTRACT");
                //dt2 = DBAdo.DtFillSql("SELECT DISTINCT GNAME FROM ASP");
                //dt3 = DBAdo.DtFillSql("SELECT DISTINCT GCZ FROM ASP");
                //dv1 = dt1.DefaultView;
                //dv2 = dt2.DefaultView;
                //dv3 = dt3.DefaultView;

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

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
            btn1.Margin = new Padding(0, 0, 0, 0);
            btn1.Text = this.Text;
            btn1.UseVisualStyleBackColor = true;
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

        #region 按钮事件
        private void btn_cx(object sender, EventArgs e)
        {
            //MessageBox.Show(this.textBox2KH.Tag.ToString());
            try
            {
                string filter = "";
                filter += " AND HDW LIKE '" + ClassCustom.codeSub(this.comboBox1.Text) + "%'";
                if (this.checkBoxLX.Checked)
                {
                    if (this.checkBoxLX1.Checked)
                    {
                        filter += " AND HLX = '" + this.comboBoxLX1.SelectedValue.ToString() + "'";
                    }
                    else
                    {
                        filter += " AND HLX LIKE '" + this.comboBoxLX.SelectedValue.ToString() + "%'";
                    }
                }
                if (this.checkBoxKH.Checked)
                {
                    if (this.textBox2KH.Tag == null)
                    {

                    }
                    else
                    {
                        if (this.textBox2KH.Tag.ToString().Trim() == "")
                        {

                        }
                        else
                        {
                            filter += " AND HKH LIKE '" + this.textBox2KH.Tag.ToString() + "%'";
                        }

                    }

                }
                if (this.checkBoxDATE.Checked)
                {
                    filter += " AND 签定日期 >= '" + this.dateTimePicker1.Value.ToShortDateString() + "' AND 签定日期 <= '" + this.dateTimePicker2.Value.ToShortDateString() + "'";
                }
                if (this.checkBoxXM.Checked)
                {
                    filter += " AND 项目名称 ='" + this.comboBoxXM.Text + "'";
                }
                if (this.checkBoxSP.Checked)
                {
                    filter += " AND 商品 ='" + this.comboBoxSP.Text + "'";
                }
                if (this.checkBoxCZ.Checked)
                {
                    filter += " AND 材质 ='" + this.comboBoxCZ.Text + "'";
                }
                if (this.checkBoxHTH.Checked)
                {
                    filter += " AND 合同号 = '" + this.textBox1.Text + "'";
                }
                if (this.radioButton1.Checked)
                {
                    filter += " AND 结算金额 = 金额 AND 结算金额 = 发票";
                }
                if (this.radioButton2.Checked)
                {
                    filter += " AND (结算金额 <> 金额 OR 结算金额 <> 发票 OR 金额 <> 发票)";
                }
                string sql = string.Format(@"SELECT FLAG 审批, [客户名], [合同号], [签定日期],[标号],[合同金额], [结算金额], [质保金], [运费], [其它费用], [合同类型],({0}) AS [金额],0.00 [发票],0.00 [金额1],({1}) AS [发票1],({2}) AS [估验],0.00 [财务余额], [地区],签订日期, 中标方式,[合同备注], [业务员], [状态], [交货日期], [操作员], [含税], [比例],[HAREA], [HLX], [HKH], [HYWY], [HDW], [HID], [代理费], [选型费], [标书费] 
                                            FROM [vcontracts] h  WHERE 合同号 IN (SELECT DISTINCT 合同号 FROM VCX1 WHERE 1=1 " + filter + ")"
                                            , "select sum(rmb) from afkxx f1 where 1=1 and f1.hth = h.[合同号] and type = (case when Substring(h.[HLX],1,2)='02' then '回款' else '付款' end) and f1.DATE between '" + this.dateTimePicker3.Value.ToShortDateString() + "' and '" + this.dateTimePicker4.Value.ToShortDateString() + "'",
                                            "select sum(rmb) from afkxx f2 where 1=1 and f2.hth = h.[合同号] and type = (case when Substring(h.[HLX],1,2)='02' then '销项发票' else '进项发票' end) and f2.DATE between '" + this.dateTimePicker3.Value.ToShortDateString() + "' and '" + this.dateTimePicker4.Value.ToShortDateString() + "'",
                                            "select sum(rmb) from afkxx f3 where 1=1 and f3.hth = h.[合同号] and type = '估验' and f3.DATE between '" + this.dateTimePicker3.Value.ToShortDateString() + "' and '" + this.dateTimePicker4.Value.ToShortDateString() + "'");
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Value = 50;

                DataTable dt = DBAdo.DtFillSql(sql);
                dt.Columns["金额1"].Expression = "结算金额-金额";
                dt.Columns["发票1"].Expression = "结算金额-发票";
                dt.Columns["财务余额"].Expression = "发票-金额";


                //foreach (DataRow r in dt.Rows)
                //{
                //    string sqlrmb = string.Format("select sum(rmb) from afkxx where 1=1 and hth = '{0}' and type = '{1}'", r["合同号"].ToString(), r["hlx"].ToString().Substring(0, 2) == "02" ? "回款" : "付款");
                //    sqlrmb += (this.checkBox1.Checked ? string.Format(" AND [DATE] BETWEEN '{0}' AND '{1}' ", this.dateTimePicker3.Value.ToShortDateString(), this.dateTimePicker4.Value.ToShortDateString()) : "");
                //    object rmb = DBAdo.ExecuteScalarSql(sqlrmb);

                //    r["金额"] = (rmb == null || rmb.ToString() == "" ? "0" : rmb.ToString());

                //    string sqlfp = string.Format("select sum(rmb) from afkxx where 1=1 and hth = '{0}' and type = '{1}'", r["合同号"].ToString(), r["hlx"].ToString().Substring(0, 2) == "02" ? "销项发票" : "进项发票");
                //    sqlfp += (this.checkBox1.Checked ? string.Format(" AND [DATE] BETWEEN '{0}' AND '{1}' ", this.dateTimePicker3.Value.ToShortDateString(), this.dateTimePicker4.Value.ToShortDateString()) : "");
                //    object fp = DBAdo.ExecuteScalarSql(sqlfp);
                //    r["发票"] = (fp == null || fp.ToString() == "" ? "0" : fp.ToString());

                //    string sqlgy = string.Format("select sum(rmb) from afkxx where 1=1 and hth = '{0}' and type = '{1}'", r["合同号"].ToString(), "估验");
                //    sqlgy += (this.checkBox1.Checked ? string.Format(" AND [DATE] BETWEEN '{0}' AND '{1}' ", this.dateTimePicker3.Value.ToShortDateString(), this.dateTimePicker4.Value.ToShortDateString()) : "");
                //    object gy = DBAdo.ExecuteScalarSql(sqlgy);
                //    r["估验"] = (gy == null || gy.ToString() == "" ? "0" : gy.ToString());
                //    Application.DoEvents();
                //    this.progressBar1.Value++;
                //}

                //object[] sum = new object[dt.Columns.Count];
                //sum[0] = "合计";
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Columns.Count; i++)
                //    {
                //        if (dt.Columns[i].DataType == typeof(decimal))
                //        {
                //            sum[i] = 0;
                //            for (int j = 0; j < dt.Rows.Count; j++)
                //            {
                //                sum[i] = decimal.Parse(sum[i].ToString() == "" ? "0" : sum[i].ToString()) + decimal.Parse(dt.Rows[j][i].ToString() == "" ? "0" : dt.Rows[j][i].ToString());
                //            }
                //        }
                //    }
                //    dt.Rows.Add(sum);
                //}
                souce = dt.DefaultView;
                List<string> _Ltemp = new List<string>();
                foreach (DataColumn c in souce.ToTable().Columns)
                {
                    if (c.DataType == typeof(decimal))
                    {

                        _Ltemp.Add(c.ColumnName);
                    }
                }
                this.sumGridView1.SumColumnNames = _Ltemp;


                this.sumGridView1.DataSouce = souce.ToTable();
                this.sumGridView1.Grid.DataSource = souce;
                if (this.checkBoxLX.Checked)
                {
                    if (this.comboBoxLX.SelectedValue.ToString() == "02")
                    {
                        this.sumGridView1.Grid.Columns["发票"].HeaderText = "销项发票";
                        this.sumGridView1.Grid.Columns["金额"].HeaderText = "已收货款";
                        this.sumGridView1.Grid.Columns["发票1"].HeaderText = "未开销项发票";
                        this.sumGridView1.Grid.Columns["金额1"].HeaderText = "未收货款";
                    }
                    else
                    {
                        this.sumGridView1.Grid.Columns["发票"].HeaderText = "进项发票";
                        this.sumGridView1.Grid.Columns["金额"].HeaderText = "已付货款";
                        this.sumGridView1.Grid.Columns["发票1"].HeaderText = "未收进项发票";
                        this.sumGridView1.Grid.Columns["金额1"].HeaderText = "未付货款";
                    }
                }
                else
                {
                    this.sumGridView1.Grid.Columns["发票"].HeaderText = "进项/销项发票";
                    this.sumGridView1.Grid.Columns["金额"].HeaderText = "已付/已收货款";
                    this.sumGridView1.Grid.Columns["发票1"].HeaderText = "未收进项/未开销项发票";
                    this.sumGridView1.Grid.Columns["金额1"].HeaderText = "未收/未付货款";
                }

                this.splitContainer1.Panel1Collapsed = true;
                foreach (DataGridViewColumn c in this.sumGridView1.Grid.Columns)
                {
                    if (c.ValueType == typeof(decimal))
                    {
                        c.DefaultCellStyle.Format = "N2";
                        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                //this.sumGridView1.Grid.AutoResizeColumns();
                //for (int i = 36; i < this.sumGridView1.Grid.Columns.Count - 3; i++)
                //{
                //    this.sumGridView1.Grid.Columns[i].Visible = false;
                //}

                this.sumGridView1.Grid.Columns["签定日期"].HeaderText = "录入日期";
                //this.sumGridView1.Grid.AutoResizeColumns();

                this.sumGridView1.Grid.Columns[2].Frozen = true;
                this.sumGridView1_Grid_CellClick(this.sumGridView1.Grid, new DataGridViewCellEventArgs(0, 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageView.MessageErrorShow(ex);
            }
        }

        private void btn_tj(object sender, EventArgs e)
        {
            try
            {
                this.splitContainer1.Panel1Collapsed = false;

                //if (ahtop == null)
                //    return;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }
        }

        private void btn_close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportExcel(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.sumGridView1.Grid, "合同明细信息");
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        #endregion

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    if (this.dataGridView2.Rows[e.RowIndex].IsNewRow)
                        return;
                    this.dataGridView2.Rows.Remove(this.dataGridView2.Rows[e.RowIndex]);
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!loaded)
                    return;
                if (e.ColumnIndex == 1)
                {
                    (this.dataGridView2.Rows[e.RowIndex].Cells[3] as DataGridViewComboBoxCell).Value = null;
                    string tabname = "";
                    foreach (DataRow r in dtfields.Rows)
                    {
                        if (this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == r[2].ToString())
                        {
                            tabname = r[4].ToString();
                            break;
                        }
                    }
                    DataTable dt = DBAdo.DtFillSql("SELECT DISTINCT " + this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + " FROM " + tabname + " order by " + this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    (this.dataGridView2.Rows[e.RowIndex].Cells[3] as DataGridViewComboBoxCell).DataSource = dt;
                    (this.dataGridView2.Rows[e.RowIndex].Cells[3] as DataGridViewComboBoxCell).DisplayMember = this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }
        }

        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            try
            {
                this.textBox2KH.Text = value;
                this.textBox2KH.Tag = key;
                //string sql = "SELECT * FROM ACLIENTS WHERE CCODE ='" + this.hKHTextBox.Tag.ToString() + "'";
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

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

        private void sumGridView1_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.sumGridView1.Grid.Rows.Count == 0)
                {
                    this.dataGridView3.DataSource = null;
                    this.dataGridView4.DataSource = null;
                    this.dataGridView5.DataSource = null;
                    this.dataGridView6.DataSource = null;
                    return;
                }
                this.curhth = this.sumGridView1.Grid.SelectedRows[0].Cells["合同号"].Value.ToString();
                this.dataGridView4.DataSource = DBAdo.DtFillSql("SELECT * FROM AFKXX WHERE (type ='付款' or type ='抵消'  or type ='回款')  AND HTH='" + this.sumGridView1.Grid.SelectedRows[0].Cells["合同号"].Value.ToString() + "' order by date");
                this.dataGridView4.Columns["ID"].Visible = false;
                this.dataGridView4.Columns["hth"].Visible = false;
                this.dataGridView4.Columns["date"].HeaderText = "日期";
                this.dataGridView4.Columns["rmb"].HeaderText = "金额";
                this.dataGridView4.Columns["date"].HeaderText = "日期";
                this.dataGridView4.Columns["xshth"].HeaderText = "销售合同号/一手合同号";
                this.dataGridView4.Columns["fkfs"].HeaderText = "方式";
                this.dataGridView4.Columns["fklx"].HeaderText = "类型";
                this.dataGridView4.Columns["flag"].HeaderText = "是否生成凭证";
                this.dataGridView4.Columns["vid"].HeaderText = "凭证号";
                this.dataGridView4.Columns["vtype"].HeaderText = "凭证类型";
                this.dataGridView4.Columns["vyear"].HeaderText = "凭证年份";
                this.dataGridView4.Columns["vmonth"].HeaderText = "凭证月份";
                this.dataGridView4.Columns["bl1"].HeaderText = "开行";
                this.dataGridView4.Columns["bl2"].HeaderText = "新公司";
                this.dataGridView4.Columns["bl3"].HeaderText = "原公司";
                this.dataGridView4.Columns["flag"].Visible = false;
                this.dataGridView4.Columns["vid"].Visible = false;
                this.dataGridView4.Columns["vtype"].Visible = false;
                this.dataGridView4.Columns["vyear"].Visible = false;
                this.dataGridView4.Columns["vmonth"].Visible = false;
                this.dataGridView4.Columns["bl1"].Visible = false;
                this.dataGridView4.Columns["bl2"].Visible = false;
                this.dataGridView4.Columns["bl3"].Visible = false;

                this.dataGridView4.Columns["type"].HeaderText = "类型";
                this.dataGridView4.Columns["rmb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView4.Columns["rmb"].DefaultCellStyle.Format = "N2";
                this.dataGridView4.AutoResizeColumns();
                this.dataGridView5.DataSource = DBAdo.DtFillSql("SELECT * FROM AFKXX WHERE ( type ='销项发票' or type ='进项发票'  or type ='估验' ) AND HTH='" + this.sumGridView1.Grid.SelectedRows[0].Cells["合同号"].Value.ToString() + "' order by date");
                this.dataGridView5.Columns["ID"].Visible = false;
                this.dataGridView5.Columns["hth"].Visible = false;
                this.dataGridView5.Columns["date"].HeaderText = "日期";
                this.dataGridView5.Columns["rmb"].HeaderText = "金额";
                this.dataGridView5.Columns["date"].HeaderText = "日期";
                this.dataGridView5.Columns["xshth"].HeaderText = "销售合同号/一手合同号";
                this.dataGridView5.Columns["fkfs"].HeaderText = "方式";
                this.dataGridView5.Columns["fklx"].HeaderText = "类型";
                this.dataGridView5.Columns["flag"].HeaderText = "是否生成凭证";
                this.dataGridView5.Columns["vid"].HeaderText = "凭证号";
                this.dataGridView5.Columns["vtype"].HeaderText = "凭证类型";
                this.dataGridView5.Columns["vyear"].HeaderText = "凭证年份";
                this.dataGridView5.Columns["vmonth"].HeaderText = "凭证月份";
                this.dataGridView5.Columns["bl1"].HeaderText = "开行";
                this.dataGridView5.Columns["bl2"].HeaderText = "新公司";
                this.dataGridView5.Columns["bl3"].HeaderText = "原公司";

                this.dataGridView5.Columns["flag"].Visible = false;
                this.dataGridView5.Columns["vid"].Visible = false;
                this.dataGridView5.Columns["vtype"].Visible = false;
                this.dataGridView5.Columns["vyear"].Visible = false;
                this.dataGridView5.Columns["vmonth"].Visible = false;
                this.dataGridView5.Columns["bl1"].Visible = false;
                this.dataGridView5.Columns["bl2"].Visible = false;
                this.dataGridView5.Columns["bl3"].Visible = false;

                this.dataGridView5.Columns["type"].HeaderText = "类型";
                this.dataGridView5.Columns["rmb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView5.Columns["rmb"].DefaultCellStyle.Format = "N2";
                this.dataGridView5.AutoResizeColumns();

                this.dataGridView6.DataSource = DBAdo.DtFillSql("SELECT [GNAME] 商品名, [GCZ] 材质, [GXH] 型号, [GDW1] 数量单位, [GSL] 数量, [GDJ1] 单价, [GDW2] 重量单位, [GJM] 净毛, [GDZ] 单重, [GMEMO] 备注, [TH] 图号 FROM [ASP] where HTH='" + this.sumGridView1.Grid.SelectedRows[0].Cells["合同号"].Value.ToString() + "'");
                this.dataGridView6.Columns["单价"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView6.Columns["单价"].DefaultCellStyle.Format = "N2";
                this.dataGridView6.AutoResizeColumns();


                this.dataGridView3.DataSource = DBAdo.DtFillSql("SELECT * from vcontracts where 合同号 IN (SELECT WXHTH FROM AWX WHERE XSHTH ='" + this.sumGridView1.Grid.SelectedRows[0].Cells["合同号"].Value.ToString() + "') order by 客户名,合同号");

                this.dataGridView3.Columns["发票"].HeaderText = "进项发票";
                this.dataGridView3.Columns["金额"].HeaderText = "已付货款";
                this.dataGridView3.Columns["发票1"].HeaderText = "未收进项发票";
                this.dataGridView3.Columns["金额1"].HeaderText = "未付货款";
                this.dataGridView3.Columns["财务余额"].HeaderText = "财务余额(已付货款-进项发票)";

                this.dataGridView3.Columns[4].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[5].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[6].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[7].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[8].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[10].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[11].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[12].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[13].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[14].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[15].DefaultCellStyle.Format = "N2";
                this.dataGridView3.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.AutoResizeColumns();
                for (int i = 36; i < this.dataGridView3.Columns.Count - 3; i++)
                {
                    this.dataGridView3.Columns[i].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void checkBoxLX_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxLX.Checked)
                {
                    string sql = "SELECT LID,LNAME FROM ALX WHERE LEN(LID) = 2";
                    dt_lx = DBAdo.DtFillSql(sql);
                    dv_lx = dt_lx.DefaultView;
                    this.comboBoxLX.DataSource = dv_lx;
                    this.comboBoxLX.DisplayMember = "LNAME";
                    this.comboBoxLX.ValueMember = "LID";
                }
                else
                {
                    this.comboBoxLX.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void checkBox6LX1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.checkBoxLX.Checked)
                    return;
                if (this.checkBoxLX1.Checked)
                {
                    string sql = "SELECT LID,LNAME FROM ALX WHERE LID LIKE '" + this.comboBoxLX.SelectedValue.ToString() + "__'";
                    dt_lx1 = DBAdo.DtFillSql(sql);
                    dv_lx1 = dt_lx1.DefaultView;
                    this.comboBoxLX1.DataSource = dv_lx1;
                    this.comboBoxLX1.DisplayMember = "LNAME";
                    this.comboBoxLX1.ValueMember = "LID";
                }
                else
                {
                    this.comboBoxLX1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void comboBoxLX_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxLX.Checked && this.checkBoxLX1.Checked)
                {
                    string sql = "SELECT LID,LNAME FROM ALX WHERE LID LIKE '" + this.comboBoxLX.SelectedValue.ToString() + "__'";
                    dt_lx1 = DBAdo.DtFillSql(sql);
                    dv_lx1 = dt_lx1.DefaultView;
                    this.comboBoxLX1.DataSource = dv_lx1;
                    this.comboBoxLX1.DisplayMember = "LNAME";
                    this.comboBoxLX1.ValueMember = "LID";
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void checkBoxXM_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxXM.Checked)
                {
                    string sql = "SELECT DISTINCT HXM FROM ACONTRACT where hxm <>'' order by hxm";
                    dt_xm = DBAdo.DtFillSql(sql);
                    dv_xm = dt_xm.DefaultView;
                    this.comboBoxXM.DataSource = dv_xm;
                    this.comboBoxXM.DisplayMember = "HXM";
                    //this.comboBoxLX.ValueMember = "LID";
                }
                else
                {
                    this.comboBoxXM.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void checkBoxSP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxSP.Checked)
                {
                    string sql = "SELECT DISTINCT gname FROM Asp where gname <>'' order by gname";
                    dt_sp = DBAdo.DtFillSql(sql);
                    dv_sp = dt_sp.DefaultView;
                    this.comboBoxSP.DataSource = dv_sp;
                    this.comboBoxSP.DisplayMember = "gname";
                    //this.comboBoxLX.ValueMember = "LID";
                }
                else
                {
                    this.comboBoxSP.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void checkBoxCZ_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxCZ.Checked)
                {
                    string sql = "SELECT DISTINCT gcz FROM Asp where gcz <>'' order by gcz";
                    dt_cz = DBAdo.DtFillSql(sql);
                    dv_cz = dt_cz.DefaultView;
                    this.comboBoxCZ.DataSource = dv_cz;
                    this.comboBoxCZ.DisplayMember = "gcz";
                    //this.comboBoxLX.ValueMember = "LID";
                }
                else
                {
                    this.comboBoxCZ.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView7.Visible = !this.dataGridView7.Visible;
        }

        private void checkBoxHTH_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxHTH.Checked)
                {
                    string sql = "SELECT  合同号,合同类型,结算金额,客户名,签定日期 FROM vcontracts  WHERE HDW  = '" + ClassConstant.DW_ID + "' order by 合同号";
                    dt_hth = DBAdo.DtFillSql(sql);
                    dv_hth = dt_hth.DefaultView;
                    this.dataGridView7.DataSource = dv_hth;
                    this.dataGridView7.AutoResizeColumns();
                    this.dataGridView7.Columns[2].DefaultCellStyle.Format = "N2";
                    this.dataGridView7.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    //this.comboBoxLX.ValueMember = "LID";
                    this.dataGridView7.Visible = true;
                }
                else
                {
                    this.comboBoxCZ.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void dataGridView7_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (this.dataGridView7.Rows.Count > 0)
                {
                    this.textBox1.Text = this.dataGridView7.SelectedRows[0].Cells[0].Value.ToString();
                    this.dataGridView7.Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dv_hth.RowFilter = "合同号 LIKE '" + this.textBox1.Text + "%'";
                this.dataGridView7.Visible = true;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void 导出收付款明细到EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.dataGridView4, curhth + "    收付款明细");
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void 导出发票明细到EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.dataGridView5, curhth + "    发票明细");
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void 导出商品明细到EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.dataGridView6, curhth + "    商品明细");
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void 导出外协合同明细到EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.dataGridView3, "    外协明细");
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void sumGridView1_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            A_HT_OP cm = new A_HT_OP(3, null, this.sumGridView1.Grid.SelectedRows[0], null);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }


        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //if (e.RowIndex < 0)
            //    return;
            //if (bool.Parse((sender as DataGridView).SelectedRows[0].Cells["flag"].Value.ToString()))
            //{
            //    MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    return;
            //}
            //A_HT_FK_OP aop = new A_HT_FK_OP((sender as DataGridView).SelectedRows[0].Cells["hth"].Value.ToString(), null, null, (sender as DataGridView).SelectedRows[0]);
            //aop.MdiParent = this.MdiParent;
            //aop.Show();

        }

        public void sort(string sort)
        {
            try
            {
                (this.sumGridView1.Grid.DataSource as DataView).Sort = sort;
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
