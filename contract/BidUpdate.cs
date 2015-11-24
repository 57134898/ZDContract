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
    public partial class BidUpdate : Form, IsetText, IBidCode
    {
        public BidUpdate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            A_FZ_KH cm = new A_FZ_KH(this);
            cm.MdiParent = this.MdiParent;
            cm.Show();
        }
        private DataTable dt_bid;
        public void LoadHcode()
        {
            if (this.comboBox2.SelectedIndex >= 0)
            {
                if (this.button1.Tag == null || this.comboBox2.SelectedValue == null)
                {
                    return;
                }

                string sql = string.Format(@"SELECT  HCODE,ISNULL(BIDCODE,'') BIDCODE,SUBSTRING(HLX,1,2) HLX,ZBFS  FROM ACONTRACT
                                            WHERE HDW='{0}'
                                            AND HKH='{1}'
                                            AND HLX='{2}'"
                    , ClassConstant.DW_ID, this.button1.Tag.ToString(), this.comboBox2.SelectedValue.ToString());
                dt_bid = DBAdo.DtFillSql(sql);
                if (dt_bid.Rows.Count <= 0)
                {
                    this.button2.Text = "--请选择--";
                }
                this.comboBox3.DataSource = dt_bid;
                this.comboBox3.DisplayMember = "HCODE";
                this.comboBox3.ValueMember = "BIDCODE";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.button2.Text == "--请选择--" || this.button2.Text == "待定")
            {
                if (this.comboBox3.SelectedIndex < 0)
                {
                    return;
                }
                A_FZ_Bid bid = new A_FZ_Bid(this, dt_bid.Rows[this.comboBox3.SelectedIndex]["HLX"].ToString(), ClassConstant.DW_ID, DateTime.Now.Year.ToString(), dt_bid.Rows[this.comboBox3.SelectedIndex]["ZBFS"].ToString());
                bid.ShowDialog();
            }
            else
            {
                MessageBox.Show("该合同已经选择标号！");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.button2.Text == "" || this.comboBox3.SelectedIndex < 0)
            {
                MessageBox.Show("请选择合同！");
                return;
            }
            string sql = string.Format("UPDATE ACONTRACT SET BIDCODE='{0}' WHERE HCODE = '{1}' ", this.button2.Text, this.comboBox3.Text);
            DBAdo.ExecuteNonQuerySql(sql);
            MessageBox.Show("标号修改成功！");
            this.button1.Text = "--请选择--";
            //this.comboBox1.SelectedIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            this.button1.Text = value;
            this.button1.Tag = key;
            LoadHcode();
        }

        #endregion
        #region Form基本方法
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT ht;//合同表单

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT LID,LNAME FROM ALX WHERE LEN(LID) = 2";
                DataTable dt_lx = DBAdo.DtFillSql(sql);
                this.comboBox1.DataSource = dt_lx;
                this.comboBox1.DisplayMember = "LNAME";
                this.comboBox1.ValueMember = "LID";
                this.comboBox1.SelectedIndex = 1;
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
                    new ToolStripSeparator(),
                    //new Factory_ToolBtn("明细条款","明细条款",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
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

        private void btn_wx(object sender, EventArgs e)
        {

        }

        private void btn_yj(object sender, EventArgs e)
        {

        }

        private void btn_kh(object sender, EventArgs e)
        {

        }

        private void btn_gs(object sender, EventArgs e)
        {

        }

        private void btn_mx(object sender, EventArgs e)
        {

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT LID,LNAME FROM ALX WHERE lid like '{0}__'", this.comboBox1.SelectedValue.ToString());
            DataTable dt_lx = DBAdo.DtFillSql(sql);
            this.comboBox2.DataSource = dt_lx;
            this.comboBox2.DisplayMember = "LNAME";
            this.comboBox2.ValueMember = "LID";

        }


        #endregion

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox3.SelectedValue.ToString() != "")
            {
                this.button2.Text = this.comboBox3.SelectedValue.ToString();
            }
            else
            {
                this.button2.Text = "--请选择--";
            }
        }

        #region IBidCode 成员

        public void SetBidCode(string code)
        {
            this.button2.Text = code;
        }

        #endregion

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHcode();
        }
    }
}
