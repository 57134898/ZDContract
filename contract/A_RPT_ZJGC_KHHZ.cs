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
    public partial class A_RPT_ZJGC_KHHZ : Form
    {
        private DataTable souce;
        public A_RPT_ZJGC_KHHZ()
        {
            InitializeComponent();
        }

        private void A_RPT_ZJGC_KHHZ_Load(object sender, EventArgs e)
        {
            try
            {
                Reg();
                string sql = "select az.ID,az.客户编码,ac.cname 客户名,az.摘要,az.年,az.月,case az.凭证类别 when '1' then '现金' when '2' then '银行' when '3' then '转账'end 凭证类别,az.凭证号,az.金额,az.类型,az.备注 from AZJQQ az,ACLIENTS ac where az.客户编码 = ac.ccode order by az.ID";
                souce = DBAdo.DtFillSql(sql);
                this.dataGridView1.DataSource = souce;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.AutoResizeColumns();
                this.dataGridView1.Columns[0].Frozen = true;
                this.dataGridView1.Columns[1].Frozen = true;
                this.dataGridView1.Columns[2].Frozen = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }


        #region Form基本方法
        private ToolStripItem[] bts = null;
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Reg();
                DataLoad();
                DgvCssSet();

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
                    new Factory_ToolBtn("查询","查询",ClassCustom.getImage("sel.png"),this.btn_cx,null,true).TBtnProduce(),
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
        
        private void btn_cx(object sender, EventArgs e)
        {
            //查询
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_ZJGC_CX)))
            {
                return;
            }
            A_ZJGC_CX ZCX = new A_ZJGC_CX();
            ZCX.MdiParent = this.MdiParent;
            ZCX.Visible=true;
        }

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

        #endregion
    }
}
