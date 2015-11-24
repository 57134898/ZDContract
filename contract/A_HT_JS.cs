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
    public partial class A_HT_JS : Form
    {
        private string HTH;
        private DataTable js;
        private DataTable jss;
        public A_HT_JS()
        {
            InitializeComponent();
        }

        public A_HT_JS(string HTH)
        {
            InitializeComponent();
            this.HTH = HTH;
        }
        #region Form基本方法
        private ToolStripItem[] bts = null;

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = HTH + "-发票数据";

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
            foreach (DataGridViewColumn c in this.dataGridView1.Columns)
            {
                if (c.Index != 0)
                {
                    c.ReadOnly = true;
                }
            }
        }

        private void DataLoad()
        {
            try
            {
                js = DBAdo.DtFillSql("SELECT C.CNAME 客户名称,C.CSH 税号,C.CADDRESS + C.CFPTEL 地址电话,C.CFPBAND+ C.CFPPHONE 银行帐号,G.GNAME 货物名称,G.GXH 规格型号,G.GDW1 单位,G.GSL 数量,G.GDJ1*G.GSL 金额,G.GMEMO 备注,H.HCODE 合同号 "
                       + "FROM ACONTRACT H INNER JOIN ACLIENTS C ON H.HKH=C.CCODE "
                       + "INNER JOIN ASP G ON H.HCODE = G.HTH AND H.HCODE = '" + HTH + "'");
                js.Columns.Add("分公司名称", typeof(string));
                foreach (DataRow r in js.Rows)
                {
                    r["分公司名称"] = ClassConstant.DW_NAME;
                }
                this.dataGridView1.DataSource = js;
                jss = js.DefaultView.ToTable();
                jss.Rows.Clear();
                this.dataGridView2.DataSource = jss;
                this.dataGridView1.AutoResizeColumns();
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
                    new Factory_ToolBtn("  导出  ", "  导出  ",ClassCustom.getImage("js.png"), this.btn_sav,null,true).TBtnProduce(),
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



        private void btn_sav(object sender, EventArgs e)
        {
            if (this.dataGridView2.Rows.Count > 0)
            {
                ClassCustom.ExportDataGridview1(this.dataGridView2, this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString() + " 发票数据");
            }
            else
            {
                MessageBox.Show("没有数据");
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
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.SelectedRows)
            {
                r.Cells[0].Value = true;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.SelectedRows)
            {
                r.Cells[0].Value = false;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.dataGridView1.Rows.Count == 0)
                {
                    return;
                }

                jss.Rows.Clear();
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    if (bool.Parse(r.Cells[0].Value == null ? "false" : r.Cells[0].Value.ToString()))
                    {
                        jss.Rows.Add(new object[] { 
                    r.Cells[1].Value,
                    r.Cells[2].Value,
                    r.Cells[3].Value,
                    r.Cells[4].Value,
                    r.Cells[5].Value,
                    r .Cells[6].Value,
                    r .Cells[7].Value,
                    r .Cells[8].Value,
                    r .Cells[9].Value,
                    r .Cells[10].Value,
                    r .Cells[11].Value,
                    r.Cells[12].Value
                });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    r.Cells[0].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    r.Cells[0].Value = false;
                }
            }
        }
    }
}
