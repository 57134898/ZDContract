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
    public partial class A_HT_KHFK : Form, IsetText
    {
        private ToolStripItem[] bts = null;
        public A_HT_KHFK()
        {
            InitializeComponent();
        }

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
            this.textBox1.Tag = key;
            this.textBox1.Text = value;

        }

        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
                return;
            try
            {
                string columnNames = "合同号,合同类型,结算金额,金额,金额1,发票,发票1,财务余额,估验,签定日期,交货日期,业务员,操作员,合同金额,HDW,HID,HKH,HLX,HYWY";
                string sql = "SELECT " + columnNames + " FROM VCONTRACTS WHERE HKH = '" + this.textBox1.Tag.ToString() + "'";
                Console.WriteLine(sql);
                DataTable souce1 = DBAdo.DtFillSql(sql);
                DataView dv1 = new DataView(souce1, "HLX LIKE '02%'", "", DataViewRowState.OriginalRows);
                //this.dataGridView1.DataSource = dv1;
                DataView dv4 = new DataView(souce1, "HLX NOT LIKE '02%'", "", DataViewRowState.OriginalRows);
                //this.dataGridView4.DataSource = dv4;
                string columnNames1 = "CID ID,ExchangeDate 发生日期,Cash+Note+Mz 合计金额,Cash 现汇,Note 票据,Mz 抹账,Type 类型,VoucherFlag 连接凭证,VoucherYear 凭证年份,VoucherMonth 凭证月份,VoucherType 凭证类型,VoucherId 凭证号,ZJBL1 开发银行,ZJBL2 原单位,ZJBL3 新单位,ccode";
                string sql1 = "SELECT " + columnNames1 + " FROM ACASH WHERE 1=1 and cid in(SELECT DISTINCT CID FROM AFKXX WHERE HTH IN (SELECT HCODE FROM ACONTRACT WHERE HDW = '" + ClassConstant.DW_ID + "')) and Ccode = '" + this.textBox1.Tag.ToString() + "'";
                DataTable souce2 = DBAdo.DtFillSql(sql1);
                Console.WriteLine(sql);
                DataView dv2 = new DataView(souce2, "类型 = '回款'", string.Empty, DataViewRowState.OriginalRows);
                this.dataGridView2.DataSource = dv2;
                DataView dv3 = new DataView(souce2, "类型 = '销项发票'", string.Empty, DataViewRowState.OriginalRows);
                this.dataGridView3.DataSource = dv3;
                DataView dv5 = new DataView(souce2, "类型 = '付款'", string.Empty, DataViewRowState.OriginalRows);
                this.dataGridView5.DataSource = dv5;
                DataView dv6 = new DataView(souce2, "类型 = '进项发票'", string.Empty, DataViewRowState.OriginalRows);
                this.dataGridView6.DataSource = dv6;

                DataView dv7 = new DataView(souce2, "类型 = '估验'", string.Empty, DataViewRowState.OriginalRows);
                this.dataGridView1.DataSource = dv7;

                string sql7 = "SELECT ExchangeDate 日期,Cash+Note+Mz 合计金额,Cash 现汇,Note 票据,Mz 抹账,Type 类型,case VoucherFlag when 1 THEN '是' else '否' end as 连接凭证,VoucherYear 凭证年份,VoucherMonth 凭证月份,VoucherType 凭证类型,VoucherId 凭证号  FROM ACASH WHERE Ccode = '" + this.textBox1.Tag.ToString() + "'";
                //this.dataGridView7.DataSource = DBAdo.DtFillSql(sql7);
                Console.WriteLine(sql7);
                foreach (DataGridView dgv in new DataGridView[] { this.dataGridView2, this.dataGridView3, this.dataGridView5, this.dataGridView6 })
                {
                    foreach (DataGridViewColumn c in dgv.Columns)
                    {
                        if (c.ValueType == typeof(decimal))
                        {
                            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            c.DefaultCellStyle.Format = "N2";
                        }
                        dgv.AutoResizeColumns();
                        dgv.Columns[1].Frozen = true;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void A_HT_KHFK_Load(object sender, EventArgs e)
        {
            Reg();
        }
        private void Reg()
        {
            OnButtonClick dzd = (object sender, EventArgs e) =>
                      {
                          try
                          {
                              if (this.textBox1.Text == "")
                              {
                                  MessageBox.Show("请选择客户！");
                                  return;
                              }
                              A_RTP_DZD dz = new A_RTP_DZD(this.textBox1.Tag.ToString(), this.textBox1.Text);
                              dz.Show();
                          }
                          catch (Exception ex)
                          {
                              //MessageBox.Show(ex.ToString());
                              MessageBox.Show(ex.Message);
                              return;
                          }
                      };

            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    //new ToolStripSeparator(),
                    new Factory_ToolBtn("导出对账单","  导出对账单",ClassCustom.getImage("zj.png"),dzd,null,true).TBtnProduce(),
                     
                    //new Factory_ToolBtn("  查询  ", " 查询 ",ClassCustom.getImage("sel.png"), this.btn_Sel,null,true).TBtnProduce(),
                    //new ToolStripSeparator(),
                    //new Factory_ToolBtn("自动计算", "自动计算",ClassCustom.getImage("auto.png"), this.btn_atuo,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    //new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };

            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        public void reLoad()
        {
            this.textBox1_TextChanged(null, null);
        }

        #region 按钮事件


        private void btn_atuo(object sender, EventArgs e)
        {
            try
            {


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

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            try
            {
                //if (bool.Parse((sender as DataGridView)["连接凭证", e.RowIndex].Value.ToString()))
                //{
                //    MessageBox.Show("该信息已经连接凭证");
                //    return;
                //}
                A_HT_FKKH_OP op = new A_HT_FKKH_OP(int.Parse((sender as DataGridView)["ID", (sender as DataGridView).CurrentCell.RowIndex].Value.ToString()), this, (sender as DataGridView)["类型", (sender as DataGridView).CurrentCell.RowIndex].Value.ToString(), bool.Parse((sender as DataGridView)["连接凭证", e.RowIndex].Value.ToString()));
                op.MdiParent = this.MdiParent;
                op.Show();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }


        }


    }
}
