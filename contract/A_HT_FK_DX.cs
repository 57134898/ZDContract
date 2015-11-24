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
    public partial class A_HT_FK_DX : Form, IChildForm, IsetText
    {
        public A_HT_FK_DX()
        {
            InitializeComponent();
        }

        public A_HT_FK_DX(A_HT_FK_GZT gzt)
        {
            InitializeComponent();
            this.gzt = gzt;
        }
        #region Form基本方法
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT_FK_GZT gzt;//合同表单

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {


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
                    //new Factory_ToolBtn("明细条款","明细条款",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("商品明细", "商品明细",ClassCustom.getImage("sp.png"), this.btn_sp,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("  意见  ", "  意见  ",ClassCustom.getImage("yj.png"), this.btn_yj,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("客户信息", "客户信息",ClassCustom.getImage("khxx.png"), this.btn_kh,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("公司信息", "公司信息",ClassCustom.getImage("gs.png"), this.btn_gs,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };
            #region old
            //bts = new ToolStripItem[]{
            //        new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        //new Factory_ToolBtn("外协明细","外协明细",ClassCustom.getImage("wx.png"),this.btn_wx,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("明细条款","合同明细",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("商品明细", "商品明细",ClassCustom.getImage("sp.png"), this.btn_sp,null,true).TBtnProduce(),
            //        //new Factory_ToolBtn("  意见  ", "  意见  ",ClassCustom.getImage("yj.png"), this.btn_yj,null,true).TBtnProduce(),
            //        //new Factory_ToolBtn("客户信息", "客户信息",ClassCustom.getImage("khxx.png"), this.btn_kh,null,true).TBtnProduce(),
            //        //new Factory_ToolBtn("公司信息", "公司信息",ClassCustom.getImage("gs.png"), this.btn_gs,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
            //        };
            #endregion
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

        private void btn_sp(object sender, EventArgs e)
        {
            //if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_SK)))
            //    return;
            //A_HT_SK cm = new A_HT_SK();
            //cm.MdiParent = this.MdiParent;
            //cm.Show();
        }

        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBox1.Text == "" || this.comboBox2.Text == "" || this.textBox5.Text == "")
                    return;
                string sql = " INSERT INTO [AFKXX]([date], [rmb], [hth], [xshth], [type], [flag]) VALUES("
                           + "'" + this.dateTimePicker1.Value.ToShortDateString() + "',"
                           + "'" + this.textBox5.Text + "',"
                           + "'" + this.comboBox1.Text + "',"
                           + "'','抵消','0'); "
                           + "INSERT INTO [AFKXX]([date], [rmb], [hth], [xshth], [type], [flag]) VALUES("
                           + "'" + this.dateTimePicker1.Value.ToShortDateString() + "',"
                           + "'" + this.textBox5.Text + "',"
                           + "'" + this.comboBox2.Text + "',"
                           + "'" + this.comboBox3.Text + "',"
                           + "'抵消','0'); ";

                DBAdo.ExecuteNonQuerySql(sql);
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

        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            try
            {
                this.textBox1.Text = value;
                this.textBox1.Tag = key;
                string sql = "SELECT HCODE,HLX FROM ACONTRACT WHERE  FP > RMB and HKH ='" + this.textBox1.Tag.ToString() + "'";
                DataTable dtsql = DBAdo.DtFillSql(sql);
                foreach (DataRow r in dtsql.Rows)
                {
                    if (r[1].ToString().Substring(0, 2) == "02")
                    {
                        this.comboBox1.Items.Add(r[0].ToString());
                    }
                    else
                    {
                        this.comboBox2.Items.Add(r[0].ToString());
                    }
                }

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
            //this.panel1.Visible = false;
        }
        #endregion

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.textBox3.Text = "";
                DataTable tem = DBAdo.DtFillSql("SELECT  发票-金额 FROM [VCONTRACTS]  WHERE 发票 > 金额 and  合同号='" + this.comboBox2.Text + "'");
                if (tem.Rows.Count > 0)
                {
                    this.textBox3.Text = tem.Rows[0][0].ToString();
                }
                DataTable temp = DBAdo.DtFillSql("SELECT XSHTH FROM AWX WHERE WXHTH='" + this.comboBox2.Text + "'");
                this.comboBox3.DataSource = temp;
                this.comboBox3.DisplayMember = "XSHTH";
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.textBox2.Text = "";
                DataTable tem = DBAdo.DtFillSql("SELECT 发票-金额 FROM [VCONTRACTS] WHERE 发票 > 金额 and 合同号='" + this.comboBox1.Text + "'");
                if (tem.Rows.Count > 0)
                {
                    this.textBox2.Text = tem.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }
        private void TextBoxChanged(object sender, EventArgs e)
        {
            try
            {
                this.textBox5.Text = (decimal.Parse(this.textBox2.Text == "" ? "0" : this.textBox2.Text) < decimal.Parse(this.textBox3.Text == "" ? "0" : this.textBox3.Text) ? decimal.Parse(this.textBox2.Text == "" ? "0" : this.textBox2.Text) : decimal.Parse(this.textBox3.Text == "" ? "0" : this.textBox3.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

    }
}
