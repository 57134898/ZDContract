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
    public partial class Warehouse : Form, IsetText
    {
        public Warehouse()
        {
            InitializeComponent();
        }

        public Warehouse(int op, DataGridViewRow dgrv, string hlx)
        {
            InitializeComponent();
            this.op = op;
            this.dgrv = dgrv;
            this.HLX = hlx;
        }
        #region Form基本方法
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private string guid;

        private string HLX;
        private DataGridViewRow dgrv;
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                this.splitContainer1.Panel1Collapsed = true;
                this.splitContainer1.Panel2Collapsed = true;
                Reg();
                DataLoad();

                if (this.op == 1)
                {



                }
                if (this.op == 3 || this.op == 2)
                {

                    this.splitContainer1.Panel1Collapsed = true;
                    this.hcodeCB.DropDownStyle = ComboBoxStyle.DropDown;
                    this.hcodeCB.Enabled = false;
                    this.hcodeCB.Text = dgrv.Cells["合同号"].Value.ToString();
                    this.dateTimePicker1.Value = DateTime.Parse(dgrv.Cells["单据日期"].Value.ToString());
                    this.invoiceCodeTB.Text = dgrv.Cells["发票号"].Value.ToString();
                    this.todoTB.Text = dgrv.Cells["备注"].Value.ToString();
                    this.salemanTB.Text = dgrv.Cells["业务员"].Value.ToString();
                    this.totalTB.Text = dgrv.Cells["库存金额"].Value.ToString();
                    string sql = string.Format(@"SELECT      [GCODE]
                                                             ,[GNAME]
                                                             ,[GCZ]
                                                             ,[GXH]
                                                             ,[HTH]
                                                             ,T0.INVID
                                                             ,[zj]-ISNULL((SELECT SUM(N1.Total) FROM Invioce N0 INNER JOIN InvoiceRows N1 ON N0.InvID=N1.InvID                                                                           WHERE N0.Hcode=T0.HTH),0) AS zj
                                                             ,T1.Unit '单位'
                                                             ,T1.Price '本次单价'
                                                             ,T1.Qua '本次数量'
                                                             ,T1.Price*T1.Qua '本次总价'
                                                             ,T1.Tax '税额'
                                                             ,T1.Total '本次发票金额'
                                                             ,T1.Todo '备注'
                                                             ,T1.EtcCost 'EtcCost'
                                                  FROM [ASP] T0 LEFT JOIN InvoiceRows T1 ON T0.InvID=T1.SpID 
                                                   WHERE T1.InvID='{0}'
                                                  UNION SELECT      [GCODE]
                                                                          ,[GNAME]
                                                                          ,[GCZ]
                                                                          ,[GXH]
                                                                          ,[HTH]
                                                                          ,INVID
                                                                          ,[zj]-ISNULL((SELECT SUM(N1.Total) FROM Invioce N0 INNER JOIN InvoiceRows N1 ON N0.InvID=N1.InvID WHERE N0.Hcode=T0.HTH),0) AS zj
                                                                          ,'' '单位'
                                                                          ,0.0 '本次单价'
                                                                          ,0.0 '本次数量'
                                                                          ,0.0 '本次总价'
                                                                          ,0.0 '税额'
                                                                          ,0.0 '本次发票金额'
                                                                          ,'' '备注'
                                                                          ,0.0 'EtcCost'
                                                                      FROM [ASP]T0 WHERE InvID NOT IN(SELECT SpID FROM InvoiceRows WHERE InvID='{0}') AND HTH='{1}'", dgrv.Cells["InvID"].Value.ToString(), dgrv.Cells["合同号"].Value.ToString());
                    this.dataGridView2.DataSource = DBAdo.DtFillSql(sql);
                    DgvCssSet();
                }

            }
            catch (Exception ex)
            {

                MessageView.MessageErrorShow(ex);
            }
        }

        private void DgvCssSet()
        {
            this.dataGridView2.Columns["GCODE"].Visible = false;
            this.dataGridView2.Columns["HTH"].Visible = false;
            this.dataGridView2.Columns["HTH"].Visible = false;
            this.dataGridView2.Columns["INVID"].Visible = false;

            this.dataGridView2.Columns["GNAME"].HeaderText = "商品";
            this.dataGridView2.Columns["GCZ"].HeaderText = "材质";
            this.dataGridView2.Columns["GXH"].HeaderText = "型号";
            this.dataGridView2.Columns["zj"].HeaderText = "总价";

            //if (this.comboBox1.SelectedValue.ToString().Substring(0, 2) == "02")
            //{
            //    this.dataGridView2.Columns["EtcCost"].HeaderText = "成本";
            //}
            //else
            //{
            this.dataGridView2.Columns["EtcCost"].HeaderText = "其他费用";
            //}


            this.dataGridView2.Columns["本次数量"].HeaderText = "数量";
            this.dataGridView2.Columns["本次单价"].HeaderText = "单价";
            this.dataGridView2.Columns["本次总价"].HeaderText = "总价";


            this.dataGridView2.Columns["zj"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["EtcCost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["本次数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["本次单价"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["本次总价"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["税额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["本次发票金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["zj"].DefaultCellStyle.Format = "N6";
            this.dataGridView2.Columns["EtcCost"].DefaultCellStyle.Format = "N6";
            this.dataGridView2.Columns["本次数量"].DefaultCellStyle.Format = "N6";
            this.dataGridView2.Columns["本次单价"].DefaultCellStyle.Format = "N6";
            this.dataGridView2.Columns["本次总价"].DefaultCellStyle.Format = "N6";

            this.dataGridView2.Columns["本次单价"].DefaultCellStyle.Format = "N6";
            this.dataGridView2.Columns["本次总价"].DefaultCellStyle.Format = "N6";

            //默认行样式
            this.dataGridView2.Columns["本次数量"].DefaultCellStyle.BackColor = Color.Blue;
            this.dataGridView2.Columns["本次单价"].DefaultCellStyle.BackColor = Color.Yellow;
            this.dataGridView2.Columns["本次总价"].DefaultCellStyle.BackColor = Color.LightGreen;
            //默认列头样式
            this.dataGridView2.Columns["本次数量"].HeaderCell.Style.BackColor = Color.Blue;
            this.dataGridView2.Columns["本次单价"].HeaderCell.Style.BackColor = Color.Yellow;
            this.dataGridView2.Columns["本次总价"].HeaderCell.Style.BackColor = Color.LightGreen;
            this.dataGridView2.AutoResizeColumns();
        }

        private void DataLoad()
        {
            try
            {
                if (this.op == 1)
                {
                    string sql = "SELECT LID,LNAME FROM ALX WHERE LEN(LID) = 2";
                    DataTable dt1 = DBAdo.DtFillSql(sql);
                    this.comboBox1.DataSource = dt1;
                    this.comboBox1.DisplayMember = "LNAME";
                    this.comboBox1.ValueMember = "LID";
                }
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
                    new Factory_ToolBtn("  查询  ","  查询  ",ClassCustom.getImage("sel.png"),btn_cx,null,true).TBtnProduce(),
                    new Factory_ToolBtn("编辑条件","  编辑条件",ClassCustom.getImage("upd.png"),btn_tj,null,true).TBtnProduce(),
                                        new ToolStripSeparator(),
                    new Factory_ToolBtn("  保存  ","    保存  ",ClassCustom.getImage("sav.png"),btn_sav,null,true).TBtnProduce(),
                                        new ToolStripSeparator(),
                    new Factory_ToolBtn("打印", "打印",ClassCustom.getImage("print.png"), btn_print,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
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



        private void btn_print(object sender, EventArgs e)
        {

            ShowPrintForm();

        }

        private void ShowPrintForm()
        {
            string title = string.Empty;
            string head1Title = string.Empty;

            string hLx = string.Empty;
            if (op == 1)
            {
                hLx = this.comboBox1.SelectedValue.ToString().Substring(0, 2);
            }
            else
            {
                hLx = HLX.Substring(0, 2);
            }

            if (hLx == "02")
            {
                title = "产品出库单";
                head1Title = "成本费用";
            }
            else if (hLx == "03")
            {
                title = "产品入库单";
                head1Title = "其他费用";
            }
            else
            {
                title = "材料入库单";
                head1Title = "其他费用";
            }
            string gg = string.Empty;
            string Customer = string.Empty;
            string Hcode = string.Empty;
            string InvCode = string.Empty;
            string IDate = string.Empty;
            string ITitle = string.Empty;
            string Head1Title = string.Empty;
            decimal Total = 0;
            if (op == 1)
            {
                gg = guid;
                Customer = this.textBox1.Text;
                Hcode = this.comboBox1.SelectedValue.ToString();
                InvCode = this.invoiceCodeTB.Text;
                IDate = this.dateTimePicker1.Value.ToShortDateString();

                if (hLx == "02")
                {

                    Total = decimal.Parse(this.totalTB.Text);
                }
                else
                {
                    decimal etc = 0;
                    foreach (DataGridViewRow r in this.dataGridView2.Rows)
                    {
                        if (r.Cells["EtcCost"].Value == null || r.Cells["EtcCost"].Value.ToString() == string.Empty)
                        {
                            continue;
                        }
                        etc += decimal.Parse(r.Cells["EtcCost"].Value.ToString());
                    }
                    Total = decimal.Parse(this.totalTB.Text) + etc;
                }

            }
            else
            {
                
                gg = this.dgrv.Cells["InvID"].Value.ToString();
                Customer = this.dgrv.Cells["客户名"].Value.ToString();
                Hcode = this.dgrv.Cells["合同号"].Value.ToString();
                InvCode = this.dgrv.Cells["发票号"].Value.ToString();
                IDate = this.dgrv.Cells["单据日期"].Value.ToString();
               

                if (hLx == "02")
                {

                    Total = decimal.Parse(dgrv.Cells["库存金额"].Value.ToString());
                }
                else
                {
                    Total = decimal.Parse(dgrv.Cells["库存金额"].Value.ToString()) + decimal.Parse(dgrv.Cells["其他费用"].Value.ToString());
                }


            }

            WhPrint wp = new WhPrint(gg, Customer, Hcode, InvCode, IDate, title, head1Title, Total);
            wp.ShowDialog();
        }
        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                if (this.totalTB.Text.Length <= 0)
                {
                    MessageBox.Show("总金额必须填写！");
                    return;
                }
                decimal d = 0;
                foreach (DataGridViewRow r in this.dataGridView2.Rows)
                {
                    d += decimal.Parse(r.Cells["本次总价"].Value.ToString());
                }
                if (decimal.Parse(this.totalTB.Text) != d)
                {
                    MessageBox.Show("总金额与明细不符");
                    return;

                }

                if (op == 1)
                {
                    this.guid = Guid.NewGuid().ToString();


                    string[] mPars = new string[6];
                    mPars[0] = guid;
                    mPars[1] = salemanTB.Text;
                    mPars[2] = dateTimePicker1.Value.ToShortDateString();
                    mPars[3] = todoTB.Text;
                    mPars[4] = hcodeCB.Text;
                    mPars[5] = invoiceCodeTB.Text;
                    string mSql = string.Format(@"INSERT INTO [Invioce]([InvID],[Saleman],[IDate],[Todo],[Hcode],[InvCode]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')", mPars);


                    foreach (DataGridViewRow r in this.dataGridView2.Rows)
                    {
                        if (decimal.Parse(r.Cells["本次数量"].Value.ToString()) == 0
                           && decimal.Parse(r.Cells["本次单价"].Value.ToString()) == 0
                           && decimal.Parse(r.Cells["本次总价"].Value.ToString()) == 0)
                        {
                            continue;
                        }
                        string rSql = string.Empty;
                        string[] rPars = new string[8];
                        rPars[0] = r.Cells["本次单价"].Value.ToString();
                        rPars[1] = r.Cells["本次数量"].Value.ToString();
                        rPars[2] = r.Cells["税额"].Value.ToString();
                        rPars[3] = r.Cells["备注"].Value.ToString();
                        rPars[4] = r.Cells["INVID"].Value.ToString();
                        rPars[5] = guid;
                        rPars[6] = r.Cells["EtcCost"].Value.ToString();
                        rPars[7] = r.Cells["单位"].Value.ToString();
                        rSql = string.Format(@"  INSERT INTO [InvoiceRows]
                                               ([Price]
                                               ,[Qua]
                                               ,[Tax]
                                               ,[Todo]
                                               ,[SpID]
                                               ,[InvID]
                                               ,[EtcCost]
                                               ,[Unit])
                                         VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')  ", rPars);

                        mSql += rSql;
                    }

                    if (mSql.Length > 0)
                    {
                        DBAdo.ExecuteNonQuerySql(mSql);
                        if (DialogResult.Yes == MessageBox.Show("是非打印单据？", "操作成功", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            ShowPrintForm();
                        }
                    }
                }
                if (op == 2)
                {
                    if (dgrv.Cells["InvID"].Value == null || dgrv.Cells["InvID"].Value.ToString() == string.Empty)
                    {
                        return;
                    }
                    if (DialogResult.Yes == MessageBox.Show("确定要删除该单据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        string sql = string.Format(@" DELETE FROM Invioce WHERE InvID='{0}'
                                                      DELETE FROM InvoiceRows WHERE InvID='{0}'", dgrv.Cells["InvID"].Value.ToString());
                        DBAdo.ExecuteNonQuerySql(sql);
                        MessageBox.Show("删除成功");
                    }
                }
                if (op == 3)
                {
                    if (dgrv.Cells["InvID"].Value == null || dgrv.Cells["InvID"].Value.ToString() == string.Empty)
                    {
                        return;
                    }
                    if (DialogResult.Yes == MessageBox.Show("确定要保存修改后单据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        string sql = string.Format(@" UPDATE [Invioce] SET 
                                                        [Saleman] = '{1}'
                                                        ,[IDate] ='{2}' 
                                                        ,[Todo] = '{3}'
                                                        ,[InvCode] = '{4}'
                                                         WHERE InvID='{0}'  ",
                                       new string[] { dgrv.Cells["InvID"].Value.ToString()
                                       ,this.salemanTB.Text,this.dateTimePicker1.Value.ToShortDateString(),this.todoTB.Text,this.invoiceCodeTB.Text                     
                                       });

                        sql += string.Format(@"  DELETE FROM InvoiceRows WHERE InvID='{0}'  ", dgrv.Cells["InvID"].Value.ToString());
                        foreach (DataGridViewRow r in this.dataGridView2.Rows)
                        {
                            if (decimal.Parse(r.Cells["本次数量"].Value.ToString()) == 0
                               && decimal.Parse(r.Cells["本次单价"].Value.ToString()) == 0
                               && decimal.Parse(r.Cells["本次总价"].Value.ToString()) == 0)
                            {
                                continue;
                            }
                            string rSql = string.Empty;
                            string[] rPars = new string[8];
                            rPars[0] = r.Cells["本次单价"].Value.ToString();
                            rPars[1] = r.Cells["本次数量"].Value.ToString();
                            rPars[2] = r.Cells["税额"].Value.ToString();
                            rPars[3] = r.Cells["备注"].Value.ToString();
                            rPars[4] = r.Cells["INVID"].Value.ToString();
                            rPars[5] = dgrv.Cells["InvID"].Value.ToString();
                            rPars[6] = r.Cells["EtcCost"].Value.ToString();
                            rPars[7] = r.Cells["单位"].Value.ToString();
                            rSql = string.Format(@"  INSERT INTO [InvoiceRows]
                                               ([Price]
                                               ,[Qua]
                                               ,[Tax]
                                               ,[Todo]
                                               ,[SpID]
                                               ,[InvID]
                                               ,[EtcCost]
                                               ,[Unit])
                                         VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')  ", rPars);

                            sql += rSql;
                        }
                        DBAdo.ExecuteNonQuerySql(sql);
                        MessageBox.Show("修改成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_cx(object sender, EventArgs e)
        {
            try
            {
                if (this.op != 1)
                {
                    return;
                }
                if (this.comboBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("合同类型不能为空！");
                    return;
                }
                if (this.textBox1.Text == string.Empty)
                {
                    MessageBox.Show("客户不能为空！");
                    return;
                }
                this.splitContainer1.Panel1Collapsed = true;
                this.splitContainer1.Panel2Collapsed = false;
                string sql1 = string.Format(@"SELECT  合同号,HLX FROM vcontracts  WHERE HDW  = '{0}'
                and HLX LIKE '{1}%' AND HKH = '{2}' order by 合同号", ClassConstant.DW_ID, this.comboBox1.SelectedValue.ToString(), this.textBox1.Tag.ToString());
                DataTable dt = DBAdo.DtFillSql(sql1);
                this.hcodeCB.DataSource = dt;
                this.hcodeCB.DisplayMember = "合同号";
                this.hcodeCB.ValueMember = "HLX";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_tj(object sender, EventArgs e)
        {
            if (op == 1)
            {
                this.splitContainer1.Panel1Collapsed = false;
                this.splitContainer1.Panel2Collapsed = true;
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

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // A
        //    // 
        //    this.ClientSize = new System.Drawing.Size(778, 262);
        //    this.Name = "A";
        //    this.ResumeLayout(false);

        //}
        #endregion

        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            this.textBox1.Text = value;
            this.textBox1.Tag = key;
        }

        #endregion

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        public void GenerateTotal()
        {
            try
            {
                decimal t = 0;
                foreach (DataGridViewRow r in this.dataGridView2.Rows)
                {
                    if (r.Cells["本次总价"].Value == null || r.Cells["本次总价"].Value.ToString() == string.Empty)
                    {
                        continue;
                    }
                    t += decimal.Parse(r.Cells["本次总价"].Value.ToString());
                }
                this.totalTB.Text = t.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Invoice inv = new Invoice(this.dataGridView2.SelectedRows[0], this);
                inv.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DBAdo.DtFillSql(String.Format(@"SELECT      [GCODE]
                                                                          ,[GNAME]
                                                                          ,[GCZ]
                                                                          ,[GXH]
                                                                          ,[HTH]
                                                                          ,INVID
                                                                          ,[zj]-ISNULL((SELECT SUM(N1.Total) FROM Invioce N0 INNER JOIN InvoiceRows N1 ON N0.InvID=N1.InvID WHERE N0.Hcode=T0.HTH),0) AS zj
                                                                          ,'' '单位'
                                                                          ,0.0 '本次单价'
                                                                          ,0.0 '本次数量'
                                                                          ,0.0 '本次总价'
                                                                          ,0.0 '税额'
                                                                          ,0.0 '本次发票金额'
                                                                          ,'' '备注'
                                                                          ,0.0 'EtcCost'
                                                                      FROM [ASP] T0
                    WHERE HTH ='{0}'", this.hcodeCB.Text
                      ));

                this.dataGridView2.DataSource = dt;
                this.DgvCssSet();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                //throw;
            }
        }
    }
}
