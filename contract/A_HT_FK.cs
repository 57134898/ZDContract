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
    public partial class A_HT_FK : Form
    {
        private string httype;
        private DataGridViewRow dgvr;
        private string type;//0款1发票
        private string hth;
        private decimal mje;
        public A_HT_FK()
        {
            InitializeComponent();
        }

        public A_HT_FK(string hth, int op, string httype, A_HT_FK_GZT gzt, string type, DataGridViewRow dgvr, decimal mje)
        {
            InitializeComponent();
            this.op = op;
            this.httype = httype;
            this.gzt = gzt;
            this.type = type;
            this.hth = hth;
            this.mje = mje;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    this.toolStripStatusLabel2.Text = ClassCustom.UpMoney(0);
                    this.toolStripStatusLabel1.Text = mje.ToString();
                    return;
                }

                if (this.textBox1.Text != "-")
                {
                    this.toolStripStatusLabel1.Text = (mje - decimal.Parse(this.textBox1.Text == "" ? "0" : this.textBox1.Text)).ToString();
                    if (decimal.Parse(this.textBox1.Text) >= 0)
                    {
                        this.toolStripStatusLabel2.Text = ClassCustom.UpMoney(decimal.Parse(this.textBox1.Text));
                    }
                    else
                    {
                        this.toolStripStatusLabel2.Text = "负" + ClassCustom.UpMoney(decimal.Parse(this.textBox1.Text));
                    }

                }
                else
                {
                    this.toolStripStatusLabel1.Text = mje.ToString();
                    this.toolStripStatusLabel2.Text = "负";
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }


        }

        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            try
            {
                if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        #region Form基本方法
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT_FK_GZT gzt;//合同表单

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                //销项发票 进项发票 抵消
                this.Text = hth + "/" + type;
                if (type == "付款" || type == "回款" || type == "")
                {
                    if (this.httype.Substring(0, 2) == "01")
                    {
                        this.panelfslx.Visible = true;
                        this.panelwx.Visible = false;
                        this.panelzjgc.Visible = false;
                    }
                    if (this.httype.Substring(0, 2) == "02")
                    {
                        this.panelfslx.Visible = true;
                        this.panelwx.Visible = false;
                        this.panelzjgc.Visible = false;
                    }
                    if (this.httype.Substring(0, 2) == "03")
                    {
                        this.panelfslx.Visible = true;
                        this.panelwx.Visible = true;
                        this.panelzjgc.Visible = false;
                    }
                    if (this.httype.Substring(0, 2) == "04")
                    {
                        this.panelfslx.Visible = true;
                        this.panelwx.Visible = false;
                        this.panelzjgc.Visible = true;
                    }
                }
                else
                {
                    if (this.httype.Substring(0, 2) == "01")
                    {
                        this.panelfslx.Visible = false;
                        this.panelwx.Visible = false;
                        this.panelzjgc.Visible = false;
                    }
                    if (this.httype.Substring(0, 2) == "02")
                    {
                        this.panelfslx.Visible = false;
                        this.panelwx.Visible = false;
                        this.panelzjgc.Visible = false;
                    }
                    if (this.httype.Substring(0, 2) == "03")
                    {
                        this.panelfslx.Visible = false;
                        this.panelwx.Visible = true;
                        this.panelzjgc.Visible = false;
                    }
                    if (this.httype.Substring(0, 2) == "04")
                    {
                        this.panelfslx.Visible = false;
                        this.panelwx.Visible = false;
                        this.panelzjgc.Visible = true;
                    }

                }

                Reg();
                DataLoad();
                DgvCssSet();
                if (this.op == 1)
                {



                }
                if (this.op == 3)
                {

                }
            }
            catch (Exception ex)
            {

                MessageView.MessageErrorShow(ex);
            }
        }

        private void DgvCssSet()
        {

        }

        private void DataLoad()
        {
            try
            {
                this.toolStripStatusLabel1.Text = mje.ToString();
                this.comboBox1.Items.AddRange(ClassConstant.fkfs);
                this.comboBox2.Items.AddRange(ClassConstant.fklx);
                //Console.WriteLine("SELECT XSHTH FROM AWX WHERE XSHTH='" + hth + "'");
                foreach (DataRow dr in DBAdo.DtFillSql("SELECT XSHTH FROM AWX WHERE WXHTH='" + hth + "'").Rows)
                {
                    this.comboBox3.Items.Add(dr[0].ToString());
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
                    //new ToolStripSeparator(),
                    
                    //new Factory_ToolBtn("商品明细", "商品明细",ClassCustom.getImage("sp.png"), this.btn_sp,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("  意见  ", "  意见  ",ClassCustom.getImage("yj.png"), this.btn_yj,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("客户信息", "客户信息",ClassCustom.getImage("khxx.png"), this.btn_kh,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("公司信息", "公司信息",ClassCustom.getImage("gs.png"), this.btn_gs,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };

            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }


        private void QK()
        {
            this.textBox1.Text = "";
            //this.textBox2.Text = "";
            //this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
            this.comboBox3.Text = "";
            this.comboBox1.Text = "";
            this.comboBox2.Text = "";
        }

        #region 按钮事件

        private void btn_sav(object sender, EventArgs e)
        {
            try
            {

                string sql = "";
                if (op == 1)
                {
                    sql = " INSERT INTO [AFKXX]([date], [rmb], [hth], [xshth], [type], [fkfs], [fklx], [bl1], [bl2], [bl3],[flag]) VALUES("
                        + "'" + this.dateTimePicker1.Value.ToShortDateString() + "',"
                          + "'" + this.textBox1.Text + "',"
                            + "'" + hth + "',"
                              + "'" + this.comboBox3.Text + "',"
                                + "'" + type + "',"
                                  + "'" + this.comboBox1.Text + "',"
                                    + "'" + this.comboBox2.Text + "',"
                                    + "'" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "',"
                                        + "'" + (this.textBox5.Text == "" ? "0" : this.textBox5.Text) + "',"
                                          + "'" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "','0'); ";
                }
                if (op == 3)
                {

                }
                if (sql == "")
                    return;
                DBAdo.ExecuteNonQuerySql(sql);
                QK();
                gzt.ReLoad();
                this.Close();
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
        #endregion
    }
}
