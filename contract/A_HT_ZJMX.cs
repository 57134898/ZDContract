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
    public partial class A_HT_ZJMX : Form
    {

        private string hcode;
        private decimal rmb;
        private decimal fp;
        public A_HT_ZJMX()
        {
            InitializeComponent();
        }

        public A_HT_ZJMX(string hcode, decimal rmb, decimal fp)
        {
            InitializeComponent();
            this.hcode = hcode;
            this.rmb = rmb;
            this.fp = fp;
        }

        #region Form基本方法

        private ToolStripItem[] bts = null;

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                this.toolStripStatusLabel2.Text = rmb.ToString();
                this.toolStripStatusLabel4.Text = fp.ToString();

                

                this.radioButton1.Checked = true;
                this.radioButton3.Checked = true;
                this.radioButton8.Checked = true;
                this.numericUpDown1.Value = DateTime.Now.Year;
                this.numericUpDown2.Value = DateTime.Now.Month;
                Reg();
                DataLoad();
                DgvCssSet();
              
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }
        }

        private void getsumRmb()
        {
            try
            {
                decimal sumrmb = 0;
                decimal sumfp = 0;

                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    if (r.Cells["type"].Value.ToString() == "付款")
                    {
                        r.DefaultCellStyle.BackColor = Color.LightGreen;
                        sumrmb += decimal.Parse(r.Cells["sum"].Value.ToString());
                    }
                    else if (r.Cells["type"].Value.ToString() == "进项发票")
                    {
                        r.DefaultCellStyle.BackColor = Color.LightBlue;
                        sumfp += decimal.Parse(r.Cells["sum"].Value.ToString());
                    }
                    else
                    {
                        throw new Exception("未知类型");
                    }
                    this.toolStripStatusLabel6.Text = (rmb - sumrmb).ToString();
                    this.toolStripStatusLabel8.Text = (fp - sumfp).ToString();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void DgvCssSet()
        {
            this.dataGridView1.Columns["ID"].Visible = false;
            this.dataGridView1.Columns["HCODE"].Visible = false;
            this.dataGridView1.Columns["sum"].HeaderText = "金额";
            this.dataGridView1.Columns["RMB"].HeaderText = "现汇";
            this.dataGridView1.Columns["NODE"].HeaderText = "票据";
            this.dataGridView1.Columns["type"].HeaderText = "类型";
            this.dataGridView1.Columns["voucherYear"].HeaderText = "年";
            this.dataGridView1.Columns["voucherMonth"].HeaderText = "月";
            this.dataGridView1.Columns["vType"].HeaderText = "凭证类型";
            this.dataGridView1.Columns["voucherId"].HeaderText = "凭证号";
            this.dataGridView1.Columns["sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["RMB"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["sum"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns["RMB"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns["NODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["NODE"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns["mz"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns["mz"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["MZ"].HeaderText = "抹账";
            this.dataGridView1.Columns["MEMO"].HeaderText = "备注";
            this.dataGridView1.AutoResizeColumns();

        }

        private void DataLoad()
        {
            try
            {
                string sql = string.Format("SELECT [ID], [hcode], [rmb] + [node]+[MZ] sum, [rmb], [node],[mz], [type], [voucherYear], [voucherMonth],CASE [voucherType] WHEN 1 THEN '现金' WHEN 2 THEN '银行' ELSE '转账' END AS VTYPE, [voucherId],[memo] FROM [AzjgcMx]  where hcode = '{0}'", hcode);
                DataTable souce = DBAdo.DtFillSql(sql);
                this.dataGridView1.DataSource = souce.DefaultView;
                //this.splitContainer1.Panel1Collapsed = true;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }

        }

        private void Reg()
        {
            OnButtonClick insert = (object sender, EventArgs e) =>
            {
                //this.splitContainer1.Panel1Collapsed = false;
                this.button1.Text = "添加";
            };
            OnButtonClick delete = (object sender, EventArgs e) => { };
            OnButtonClick update = (object sender, EventArgs e) =>
            {
                //this.splitContainer1.Panel1Collapsed = false;
                this.button1.Text = "修改";
                if (this.dataGridView1.Rows.Count == 0) return;
                this.numericUpDown1.Value = int.Parse(this.dataGridView1.SelectedRows[0].Cells["voucherYear"].Value.ToString());
                this.numericUpDown2.Value = int.Parse(this.dataGridView1.SelectedRows[0].Cells["vouchermonth"].Value.ToString());
                //this.comboBox1.Text = this.dataGridView1.SelectedRows[0].Cells["vtype"].Value.ToString();
                this.comboBox2.Text = this.dataGridView1.SelectedRows[0].Cells["voucherId"].Value.ToString();
                //this.comboBox3.Text = this.dataGridView1.SelectedRows[0].Cells["type"].Value.ToString();
                this.textBox1.Text = this.dataGridView1.SelectedRows[0].Cells["rmb"].Value.ToString();
                this.textBox2.Text = this.dataGridView1.SelectedRows[0].Cells["node"].Value.ToString();
            };
            OnButtonClick select = (object sender, EventArgs e) =>
            {

            };
            OnButtonClick exportExcel = (object sender, EventArgs e) =>
            {
                ClassCustom.ExportDataGridview1(this.dataGridView1, hcode + "前期明细");
            };
            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    //new ToolStripSeparator(),
                    //new Factory_ToolBtn("添加明细","添加明细",ClassCustom.getImage("add.png"),insert,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("删除明细", "删除明细",ClassCustom.getImage("del.png"), delete,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("修改明细","修改明细",ClassCustom.getImage("upd.png"),update,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("查询明细", "查询明细",ClassCustom.getImage("sel.png"), select,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  刷新  ", "  刷新  ",ClassCustom.getImage("sx.png"), this.Form_Load,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                     new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), exportExcel,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
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

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (this.comboBox1.Text == "" || this.comboBox2.Text == "" || this.comboBox3.Text == "")
                //    return;
                //string vtype = "";
                //if (this.comboBox3.Text == "现金") { vtype = "1"; } else if (this.comboBox3.Text == "银行") { vtype = "2"; } else { vtype = "3"; }
                //string sql = string.Format("SELECT H.YEAR 年,H.MONTH 月,H.VTYPE 凭证类型,H.VNO 凭证号,SUM(RMB) 发生额, H.VEXPL 摘要 FROM "
                //             + "N7_铸锻公司.DBO.HVOUCHER H INNER JOIN  N7_铸锻公司.DBO.IVOUCHER I ON H.ID=I.HID WHERE I.VDC=1 AND H.YEAR = '{0}' AND H.MONTH ='{1}' AND H.VTYPE ='{2}' AND H.BCODE = '{3}' "
                //              + "GROUP BY H.YEAR,H.MONTH,H.VTYPE,H.VNO,H.VEXPL", new string[] { this.comboBox1.Text, this.comboBox2.Text, vtype, ClassConstant.DW_ID });
                //DataTable souce = DBAdo.DtFillSql(sql);
                //this.dataGridView2.DataSource = souce.DefaultView;
                //this.dataGridView2.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.textBox1.Text == "" || this.textBox2.Text == "" || this.comboBox1.Text == "" || this.comboBox2.Text == "" || this.comboBox3.Text == "")
                // return;
                if (this.button1.Text == "删除")
                {

                }
                else
                {
                    if (this.textBox1.Text == "" && this.textBox2.Text == "" && this.textBox3.Text == "")
                    {
                        MessageBox.Show("未输入金额！");
                        return;
                    }
                    if (this.comboBox2.Text == "")
                    {
                        MessageBox.Show("未输入凭证号！");
                        return;
                    }
                }

                string sql = "";
                string hcode = this.hcode;
                string type = "";
                if (this.radioButton1.Checked)
                {
                    type = "付款";
                }
                else
                {
                    type = "进项发票";
                }
                string rmb = (this.textBox1.Text == "" ? "0" : this.textBox1.Text);
                string node = (this.textBox2.Text == "" ? "0" : this.textBox2.Text);
                string voucherYear = this.numericUpDown1.Value.ToString();
                string voucherMonth = this.numericUpDown2.Value.ToString();
                string voucherId = this.comboBox2.Text;
                string voucherType = "";
                string mz = (this.textBox3.Text == "" ? "0" : this.textBox3.Text);
                string memo = this.richTextBox1.Text;
                if (this.radioButton3.Checked) { voucherType = "1"; } else if (this.radioButton4.Checked) { voucherType = "2"; } else if (this.radioButton5.Checked) { voucherType = "3"; } else { throw new Exception("未知类型！"); }

                switch (this.button1.Text)
                {
                    case "添加":
                        sql = string.Format("INSERT INTO [AzjgcMx]([hcode],  [type],[rmb], [node], [voucherYear], [voucherMonth], [voucherType], [voucherId],[mz],[memo])VALUES('{0}', '{1}', {2}, {3}, {4}, {5}, {6}, {7},{8},'{9}')",
                            new object[] { hcode, type, rmb, node, voucherYear, voucherMonth, voucherType, voucherId, mz, memo });
                        break;
                    case "修改":
                        sql = string.Format("UPDATE [AzjgcMx] SET [type] = '{0}',[rmb] = {1}, [node] = {2}, [voucherYear] = {3}, [voucherMonth] = {4}, [voucherType] = {5}, [voucherId] = {6},[mz]={8},memo='{9}'  WHERE ID = {7}",
                            new object[] { type, rmb, node, voucherYear, voucherMonth, voucherType, voucherId, this.dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString(), mz, memo });
                        break;
                    case "删除":
                        sql = string.Format("DELETE FROM [AzjgcMx]  WHERE ID = {0}", this.dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
                        break;
                    default:
                        throw new Exception("未知操作");
                }
                if (sql == "") return;
                if (DialogResult.Yes == MessageBox.Show("是否执行[" + this.button1.Text + "]操作", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    DBAdo.ExecuteNonQuerySql(sql);
                    MessageBox.Show(this.button1.Text + "成功");
                    this.DataLoad();
                    //this.splitContainer1.Panel1Collapsed = (this.button1.Text == "修改" ? true : false);
                    getsumRmb();
                    this.TextClear();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void TextClear()
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.richTextBox1.Text = "";
            //this.comboBox1.Text = "";
            this.comboBox2.Text = "";
            //this.comboBox3.Text = "";
        }



        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string vtype = "";
                if (this.radioButton3.Checked) { vtype = "1"; } else if (this.radioButton4.Checked) { vtype = "2"; } else if (this.radioButton5.Checked) { vtype = "3"; } else { throw new Exception("未知类型！"); }

                string sql = string.Format("SELECT VNO FROM N7_铸锻公司.DBO.HVOUCHER WHERE 1=1 AND bcode like '0101' AND VTYPE = {0} AND YEAR = {1} AND MONTH = {2}", new object[] { vtype, this.numericUpDown1.Value, this.numericUpDown2.Value });
                this.comboBox2.Items.Clear();
                //this.comboBox2.Items.Add("");
                foreach (DataRow r in DBAdo.DtFillSql(sql).Rows)
                {
                    this.comboBox2.Items.Add(r[0].ToString());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void radioButton_1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                this.button1.Text = (sender as RadioButton).Text;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string vtype = "";
                if (this.radioButton3.Checked) { vtype = "1"; } else if (this.radioButton4.Checked) { vtype = "2"; } else if (this.radioButton5.Checked) { vtype = "3"; } else { throw new Exception("未知类型！"); }

                string sql = string.Format("SELECT VNO FROM N7_铸锻公司.DBO.HVOUCHER WHERE 1=1 AND bcode like '0101' AND VTYPE = {0} AND YEAR = {1} AND MONTH = {2}", new object[] { vtype, this.numericUpDown1.Value, this.numericUpDown2.Value });
                this.comboBox2.Items.Clear();
                //this.comboBox2.Items.Add("");
                foreach (DataRow r in DBAdo.DtFillSql(sql).Rows)
                {
                    this.comboBox2.Items.Add(r[0].ToString());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                if (this.radioButton7.Checked)
                {
                    this.numericUpDown1.Value = int.Parse(this.dataGridView1.SelectedRows[0].Cells["voucherYear"].Value.ToString());
                    this.numericUpDown2.Value = int.Parse(this.dataGridView1.SelectedRows[0].Cells["vouchermonth"].Value.ToString());
                    if (this.dataGridView1.SelectedRows[0].Cells["vtype"].Value.ToString() == "现金")
                    {
                        this.radioButton3.Checked = true;
                    }
                    else if (this.dataGridView1.SelectedRows[0].Cells["vtype"].Value.ToString() == "银行")
                    {
                        this.radioButton4.Checked = true;
                    }
                    else if (this.dataGridView1.SelectedRows[0].Cells["vtype"].Value.ToString() == "转账")
                    {
                        this.radioButton5.Checked = true;
                    }
                    else
                    {
                        throw new Exception("未知类型");
                    }
                    if (this.dataGridView1.SelectedRows[0].Cells["type"].Value.ToString() == "付款")
                    {
                        this.radioButton1.Checked = true;
                    }
                    else if (this.dataGridView1.SelectedRows[0].Cells["type"].Value.ToString() == "进项发票")
                    {
                        this.radioButton2.Checked = true;
                    }
                    else
                    {
                        throw new Exception("未知类型");
                    }
                    //this.comboBox1.Text = this.dataGridView1.SelectedRows[0].Cells["vtype"].Value.ToString();
                    this.comboBox2.Text = this.dataGridView1.SelectedRows[0].Cells["voucherId"].Value.ToString();
                    //this.comboBox3.Text = this.dataGridView1.SelectedRows[0].Cells["type"].Value.ToString();
                    this.textBox1.Text = this.dataGridView1.SelectedRows[0].Cells["rmb"].Value.ToString();
                    this.textBox2.Text = this.dataGridView1.SelectedRows[0].Cells["node"].Value.ToString();
                    this.textBox3.Text = this.dataGridView1.SelectedRows[0].Cells["mz"].Value.ToString();
                    this.richTextBox1.Text = this.dataGridView1.SelectedRows[0].Cells["memo"].Value.ToString();
                    //this.splitContainer1.Panel1Collapsed = false;
                    //this.button1.Text = "修改";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            getsumRmb();
        }
    }
}
