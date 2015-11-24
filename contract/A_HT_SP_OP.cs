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
    public partial class A_HT_SP_OP : Form, IChildForm
    {
        private string hth;
        private A_HT_OP ahtop;
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private DataGridViewRow dgvr;//修改用

        public A_HT_SP_OP()
        {
            InitializeComponent();
        }

        public A_HT_SP_OP(string hth, int op, A_HT_OP ahtop)//添加
        {
            InitializeComponent();
            this.ahtop = ahtop;
            this.hth = hth;
            this.op = op;
        }

        public A_HT_SP_OP(string hth, int op, A_HT_OP ahtop, DataGridViewRow dgvr)//修改
        {
            InitializeComponent();
            this.ahtop = ahtop;
            this.hth = hth;
            this.op = op;
            this.dgvr = dgvr;
        }

        private void A_SP_OP_Load(object sender, EventArgs e)
        {

            this.Text = hth + "-商品信息-" + (op == 1 ? "添加" : "修改");


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
                    new Factory_ToolBtn("从EXCEL导入","  从EXCEL导入  ",ClassCustom.getImage("dr.png"),btn_exprot,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  保存  ","  保存  ",ClassCustom.getImage("sav.png"),btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  关闭  ","  关闭  ",ClassCustom.getImage("tc.png"), btn_close,null,true).TBtnProduce()
                    };
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        private void DataLoad()
        {
            if (op == 3)
            {
                try
                {
                    this.textBox1.Text = dgvr.Cells[1].Value.ToString();
                    this.textBox3.Text = dgvr.Cells[2].Value.ToString();
                    this.textBox2.Text = dgvr.Cells[3].Value.ToString();
                    this.textBox6.Text = dgvr.Cells[4].Value.ToString();
                    this.comboBox1.Text = dgvr.Cells[5].Value.ToString();
                    this.textBox4.Text = dgvr.Cells[6].Value.ToString();

                    this.comboBox2.Text = dgvr.Cells[8].Value.ToString();
                    this.comboBox3.Text = dgvr.Cells[9].Value.ToString();
                    this.textBox7.Text = dgvr.Cells[10].Value.ToString();

                    this.richTextBox1.Text = dgvr.Cells[12].Value.ToString();
                    if (dgvr.Cells[13].Value.ToString() == "")
                        return;
                    int sl = 1;
                    foreach (char c in dgvr.Cells[13].Value.ToString())
                    {
                        if (c == ',')
                        {
                            sl++;
                        }
                    }
                    string th = dgvr.Cells[13].Value.ToString() + ",";
                    for (int i = 0; i < sl; i++)
                    {
                        this.dataGridView1.Rows.Add(new object[] { th.Substring(0, th.IndexOf(",")) });
                        th = th.Substring(th.IndexOf(",") + 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageView.MessageErrorShow(ex);
                }

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
        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "" || this.textBox5.Text == "" || this.textBox6.Text == "")
                    return;
                if (op == 1)
                {
                    //string sql = "INSERT INTO [ASP]([GNAME], [GCZ], [GXH], [GDW1], [GSL], [GDJ1], [GDW2], [GJM], [GDZ], [GMEMO]) VALUES("
                    //            + "'" + this.textBox1.Text + "'"
                    //             + "'" + this.textBox3.Text + "'"
                    //              + "'" + this.textBox2.Text + "'"
                    //               + "'" + this.comboBox1.Text + "'"
                    //                + "'" + this.textBox6.Text + "'"
                    //                 + "'" + this.textBox4.Text + "'"
                    //                  + "'" + this.comboBox2.Text + "'"
                    //                   + "'" + this.comboBox3.Text + "'"
                    //                    + "'" + this.textBox7.Text + "'"
                    //                     + "'" + this.richTextBox1.Text + "','" + hth + "')";
                    string th = "";

                    foreach (DataGridViewRow r in this.dataGridView1.Rows)
                    {
                        if (r.Cells[0].Value != null)
                        {
                            th += "," + r.Cells[0].Value.ToString();

                        }
                    }

                    ahtop.SP_ADD(new object[] { 
                        null,
                        this.textBox1.Text, 
                        this.textBox3.Text, 
                        this.textBox2.Text, 
                        this.textBox6.Text, 
                        this.comboBox1.Text, 
                        this.textBox4.Text,
                        null, 
                        this.comboBox2.Text, 
                        this.comboBox3.Text,
                        (this.textBox7.Text==""?null:this.textBox7.Text),
                        null,
                        this.richTextBox1.Text,
                        (th==""?"":th.Substring(1)) });
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    this.textBox3.Text = "";
                    this.textBox6.Text = "";
                    this.textBox4.Text = "";
                    this.textBox5.Text = "";
                    this.textBox7.Text = "";
                    this.textBox8.Text = "";
                    this.comboBox1.Text = "";
                    this.comboBox2.Text = "";
                    this.comboBox3.Text = "";
                    this.richTextBox1.Text = "";
                    this.dataGridView1.Rows.Clear();
                }
                if (op == 3)
                {
                    string th = "";

                    foreach (DataGridViewRow r in this.dataGridView1.Rows)
                    {
                        if (r.Cells[0].Value != null)
                        {
                            th += "," + r.Cells[0].Value.ToString();

                        }
                    }
                    //MessageBox.Show(dgvr.Cells[0].Value.ToString());
                    ahtop.SP_UPD(new object[] { 
                        dgvr.Cells[0].Value.ToString(),
                        this.textBox1.Text, 
                        this.textBox3.Text, 
                        this.textBox2.Text, 
                        this.textBox6.Text, 
                        this.comboBox1.Text, 
                        this.textBox4.Text,
                        null, 
                        this.comboBox2.Text, 
                        this.comboBox3.Text,
                        (this.textBox7.Text==""?null:this.textBox7.Text),
                        null,
                        this.richTextBox1.Text,
                        (th==""?"":th.Substring(1)) });

                    this.Close();
                }
                //if (ahtop == null)
                //    return;
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
        private void btn_exprot(object sender, EventArgs e)
        {
            A_EXCEL_INPUT ain = new A_EXCEL_INPUT(this.hth, this.ahtop);
            ain.MdiParent = this.MdiParent;
            ain.Show();
            this.Close();
        }
        #endregion

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton4.Checked)
            {
                this.textBox5.ReadOnly = true;
                this.textBox4.ReadOnly = false;
            }
            else
            {
                this.textBox5.ReadOnly = false;
                this.textBox4.ReadOnly = true;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton6.Checked)
            {
                this.textBox8.ReadOnly = true;
                this.textBox7.ReadOnly = false;
            }
            else
            {
                this.textBox8.ReadOnly = false;
                this.textBox7.ReadOnly = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.radioButton4.Checked)
                    return;
                if (this.textBox4.Text == "-" || this.textBox4.Text == "" || this.textBox4.Text.Substring(0, 1) == ".")
                {
                    this.textBox5.Text = "";
                    return;
                }
                if (this.textBox6.Text == "")
                    this.textBox6.Text = "1";
                this.textBox5.Text = (decimal.Parse(this.textBox6.Text) * decimal.Parse(this.textBox4.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                this.textBox4.Text = "";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.radioButton5.Checked)
                    return;
                if (this.textBox5.Text == "-" || this.textBox5.Text == "" || this.textBox5.Text.Substring(0, 1) == ".")
                {
                    this.textBox4.Text = "";
                    return;
                }
                if (this.textBox6.Text == "")
                    this.textBox6.Text = "1";
                this.textBox4.Text = (decimal.Round(decimal.Parse(this.textBox5.Text) / decimal.Parse(this.textBox6.Text), 4)).ToString();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                this.textBox5.Text = "";
            }
        }

        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
            {
                e.Handled = true;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.radioButton6.Checked)
                    return;
                if (this.textBox7.Text == "-" || this.textBox7.Text == "" || this.textBox7.Text.Substring(0, 1) == ".")
                {
                    this.textBox8.Text = "";
                    return;
                }
                if (this.textBox6.Text == "")
                    this.textBox6.Text = "1";
                this.textBox8.Text = (decimal.Parse(this.textBox7.Text) * decimal.Parse(this.textBox6.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                this.textBox7.Text = "";
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.radioButton7.Checked)
                    return;
                if (this.textBox8.Text == "-" || this.textBox8.Text == "" || this.textBox8.Text.Substring(0, 1) == ".")
                {
                    this.textBox7.Text = "";
                    return;
                }
                if (this.textBox6.Text == "")
                    this.textBox6.Text = "1";
                this.textBox7.Text = (decimal.Round(decimal.Parse(this.textBox8.Text) / decimal.Parse(this.textBox6.Text), 4)).ToString();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
                this.textBox8.Text = "";
            }
        }

        private void A_HT_SP_OP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (op == 1)
            {
                if (this.textBox1.Text == "")
                    return;
                if (DialogResult.Yes == MessageBox.Show("确定不保存并关闭？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }


    }
}
