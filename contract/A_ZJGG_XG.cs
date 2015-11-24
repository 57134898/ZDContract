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
    public partial class A_ZJGG_XG : Form
    {
        private ToolStripItem[] bts = null;
        private string id;
        private DataTable dt;
        private A_ZJGC_CX cx;

        public A_ZJGG_XG()
        {
            InitializeComponent();
        }
        public A_ZJGG_XG(string id, A_ZJGC_CX cx)
        {
            this.cx = cx;
            this.id = id;
            InitializeComponent();
        }
        private void A_ZJGG_XG_Load(object sender, EventArgs e)
        {
            Reg();
            DataLoad();
        }
        private void Reg()
        {//ToolStripItem    new ToolStripSeparator(),
            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  保存  ","  保存  ",ClassCustom.getImage("sav.png"),btn_bc,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("编辑条件","  编辑条件",ClassCustom.getImage("upd.png"),btn_tj,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    //new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
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
                this.comboBox1.DataSource = new string[3] { "现金", "银行", "转账" };
                string sql = "select az.ID,az.客户编码,ac.cname 客户名,az.摘要,az.年,az.月,case az.凭证类别 when '1' then '现金' when '2' then '银行' when '3' then '转账'end 凭证类别,az.凭证号,az.金额,az.类型,az.备注 from AZJQQ az,ACLIENTS ac where az.客户编码 = ac.ccode and ID='" + this.id + "'";
                dt = DBAdo.DtFillSql(sql);
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Visible = true;
                this.dataGridView1.AutoResizeColumns();
                this.dataGridView1.Columns["ID"].Visible = false;
                this.textBoxBM.Text = this.dataGridView1.Rows[0].Cells["客户编码"].Value.ToString();
                this.textBoxKHM.Text = this.dataGridView1.Rows[0].Cells["客户名"].Value.ToString();
                this.textBoxN.Text = this.dataGridView1.Rows[0].Cells["年"].Value.ToString();
                this.textBoxY.Text = this.dataGridView1.Rows[0].Cells["月"].Value.ToString();
                this.comboBox1.Text = this.dataGridView1.Rows[0].Cells["凭证类别"].Value.ToString();
                this.textBoxH.Text = this.dataGridView1.Rows[0].Cells["凭证号"].Value.ToString();
                this.textBoxJE.Text = this.dataGridView1.Rows[0].Cells["金额"].Value.ToString();
                this.textBoxLX.Text = this.dataGridView1.Rows[0].Cells["类型"].Value.ToString();
                this.textBoxZY.Text = this.dataGridView1.Rows[0].Cells["摘要"].Value.ToString();
                this.textBoxBZ.Text = this.dataGridView1.Rows[0].Cells["备注"].Value.ToString();
           
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


        private void btn_bc(object sender, EventArgs e)
        {
            try
            {
                //保存修改
                if (DialogResult.Yes == MessageBox.Show("是否确定修改", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    string sql="";
                    if (this.comboBox1.Text == "现金")
                    {
                        sql = "update AZJQQ set 摘要='" + this.textBoxZY.Text + "',年='" + this.textBoxN.Text + "',月='" + this.textBoxY.Text + "',凭证类别='1',凭证号='" + this.textBoxH.Text + "',金额='" + this.textBoxJE.Text + "',类型='" + this.textBoxLX.Text + "',备注='" + this.textBoxBZ.Text + "' where ID='" + this.id + "'";
                    }
                    if (this.comboBox1.Text == "银行")
                    {
                        sql = "update AZJQQ set 摘要='" + this.textBoxZY.Text + "',年='" + this.textBoxN.Text + "',月='" + this.textBoxY.Text + "',凭证类别='2',凭证号='" + this.textBoxH.Text + "',金额='" + this.textBoxJE.Text + "',类型='" + this.textBoxLX.Text + "',备注='" + this.textBoxBZ.Text + "' where ID='" + this.id + "'";
                    }
                    if (this.comboBox1.Text == "转账")
                    {
                        sql = "update AZJQQ set 摘要='" + this.textBoxZY.Text + "',年='" + this.textBoxN.Text + "',月='" + this.textBoxY.Text + "',凭证类别='3',凭证号='" + this.textBoxH.Text + "',金额='" + this.textBoxJE.Text + "',类型='" + this.textBoxLX.Text + "',备注='" + this.textBoxBZ.Text + "' where ID='" + this.id + "'";
                    }
                    DBAdo.ExecuteNonQuerySql(sql);
                }

                this.cx.flash();
                
                this.Close();

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



        #endregion


    }
}
