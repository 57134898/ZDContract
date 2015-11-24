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
    public partial class A_HT_GY : Form, IChildForm
    {
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT_FK_GZT gzt;//合同表单
        private DataGridViewRow dgvr;
        private decimal mje;
        private string hth;
        public A_HT_GY()
        {
            InitializeComponent();
        }
        public A_HT_GY(int op, A_HT_FK_GZT gzt, DataGridViewRow dgvr)
        {
            InitializeComponent();
            this.op = op;
            this.dgvr = dgvr;
            this.gzt = gzt;
            this.hth = dgvr.Cells["合同号"].Value.ToString();
            this.mje = decimal.Parse(dgvr.Cells["发票1"].Value.ToString() == "" ? "0" : dgvr.Cells["发票1"].Value.ToString()) - decimal.Parse(dgvr.Cells["估验"].Value.ToString() == "" ? "0" : dgvr.Cells["估验"].Value.ToString());
        }
        #region Form基本方法

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {

                this.toolStripStatusLabel1.Text = mje.ToString();
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


        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                string sql = "INSERT INTO [AFKXX]( [date], [rmb], [hth],  [type], [flag]) VALUES ("
                            + "'" + this.dateTimePicker1.Value.ToShortDateString() + "',"
                            + "'" + this.textBox1.Text + "',"
                            + "'" + hth + "','估验','0')";
                DBAdo.ExecuteNonQuerySql(sql);
                this.gzt.ReLoad();
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
        #endregion

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Text = "";
                if (this.radioButton2.Checked)
                {
                    this.textBox1.ReadOnly = true;
                    this.dataGridView1.DataSource = DBAdo.DtFillSql("SELECT DATE ,RMB FROM AFKXX WHERE TYPE ='估验' AND RMB > 0 AND FLAG = 0");
                    this.dataGridView1.Columns[0].HeaderText = "日期";
                    this.dataGridView1.Columns[1].HeaderText = "金额";
                    this.dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    this.dataGridView1.Columns[1].DefaultCellStyle.Format = "N2";
                    this.dataGridView1.Columns[0].Width = 150;
                    this.dataGridView1.Columns[1].Width = 190;
                }
                else
                {
                    this.textBox1.ReadOnly = false;
                    this.dataGridView1.DataSource = null;

                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.radioButton2.Checked)
                {
                    this.textBox1.Text = "-" + this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                return;
            }
        }
    }
}
