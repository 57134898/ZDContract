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
    public partial class A_HT_TK : Form, IChildForm
    {
        private int op;//操作 1添加 2删除 3修改 4查询
        private string hth;
        private A_HT_OP ahtop;
        private ToolStripItem[] bts = null;
        private DataGridViewRow dgvr;//修改用
        private string htype;//合同类型01采购02销售03外协04在建
        private DataTable dt_tk;//条款
        public A_HT_TK()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hth">合同号</param>
        /// <param name="aht">A_HT_OP</param>
        /// <param name="op">1添加 2删除 3修改 4查询</param>
        public A_HT_TK(string hth, A_HT_OP ahtop, int op)
        {
            InitializeComponent();
            this.ahtop = ahtop;
            this.hth = hth;
            this.op = op;

        }

        public A_HT_TK(string hth, A_HT_OP ahtop, int op, DataTable dt)
        {
            InitializeComponent();
            this.ahtop = ahtop;
            this.hth = hth;
            this.op = op;
            this.dt_tk = dt;

        }

        private void A_HT_TK_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = hth + "-条款明细";
                Reg();
                dataLoad();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
            ;
        }

        private void dataLoad()
        {
            if (op == 3)
            {
                this.flowLayoutPanel1.Controls.Clear();
                foreach (DataRow dr in dt_tk.Rows)
                {
                    RichTextBox richTextBox1 = new RichTextBox();
                    richTextBox1.Size = new System.Drawing.Size(400, 40);
                    richTextBox1.Margin = new Padding(0, 0, 0, 0);
                    richTextBox1.Text = dr[0].ToString();// + " " + dr[2].ToString().Replace(@"\n", System.Environment.NewLine);
                    this.flowLayoutPanel1.Controls.Add(richTextBox1);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Clear();
            DataTable dt = DBAdo.DtFillSql("SELECT * FROM ATK_MB ORDER BY [ORDER]");
            foreach (DataRow dr in dt.Rows)
            {
                RichTextBox richTextBox1 = new RichTextBox();
                //richTextBox1.Location = new System.Drawing.Point(3, 3);
                //richTextBox1.Name = "richTextBox1";
                richTextBox1.Size = new System.Drawing.Size(400, 40);
                richTextBox1.Margin = new Padding(0, 0, 0, 0);
                //richTextBox1.TabIndex = 0;
                richTextBox1.Text = dr[1].ToString() + " " + dr[2].ToString().Replace(@"\n", System.Environment.NewLine);
                this.flowLayoutPanel1.Controls.Add(richTextBox1);
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
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };
            //ToolStripItem[] t = new ToolStripItem[] { new ToolStripButton() };
            //ToolStripItem t = new ToolStripButton();
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        #region 按钮事件

        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                List<string> L = new List<string>();

                foreach (RichTextBox r in this.flowLayoutPanel1.Controls)
                {
                    L.Add(r.Text);
                }
                ahtop.TK_ADD(L);
                this.Close();
            }
            catch (Exception ex)
            {
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
