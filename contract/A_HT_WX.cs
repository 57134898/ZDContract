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
    public partial class A_HT_WX : Form
    {
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT_OP ah;//合同表单
        public A_HT_WX()
        {
            InitializeComponent();
        }

        public A_HT_WX(int op, A_HT_OP ah)
        {
            InitializeComponent();
            this.op = op;
            this.ah = ah;
        }

        private void A_HT_WX_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT HCODE FROM ACONTRACT WHERE HLX LIKE '02%'";
                DataTable souce = DBAdo.DtFillSql(sql);
                this.dataGridView1.DataSource = souce;
                this.dataGridView1.Columns[0].HeaderText = "待选合同号";
                this.dataGridView1.AutoResizeColumns();
                Reg();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Rows.Add(new object[] { this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString() });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Rows.Remove(this.dataGridView2.SelectedRows[0]);
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
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };

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
                foreach (DataGridViewRow r in this.dataGridView2.Rows)
                {
                    if (r.Cells[0].Value != null)
                    {
                        L.Add(r.Cells[0].Value.ToString());
                    }
                }
                ah.WX_ADD(L);
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
    }
}
