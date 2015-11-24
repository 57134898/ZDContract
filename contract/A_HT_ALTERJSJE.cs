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
    public partial class A_HT_ALTERJSJE : Form
    {
        private DataTable dt;
        private DataView souce;
        public A_HT_ALTERJSJE()
        {
            InitializeComponent();

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
            this.dataGridView1.Columns["ID"].Visible = false;
            this.dataGridView1.Columns["sqlstr"].Visible = false;
            this.dataGridView1.Columns["dw"].Visible = false;
            this.dataGridView1.Columns["hcode"].HeaderText = "合同号";
            this.dataGridView1.Columns["jsjeOld"].HeaderText = "原结算金额";
            this.dataGridView1.Columns["jsjeNew"].HeaderText = "新结算金额";
            this.dataGridView1.Columns["date"].HeaderText = "日期";
            this.dataGridView1.Columns["flag"].HeaderText = "标记";
            this.dataGridView1.Columns["uname"].HeaderText = "修改人";
            this.dataGridView1.Columns["jsjeOld"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["jsjeNew"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["jsjeNew"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns["jsjeOld"].DefaultCellStyle.Format = "N2";
        }

        private void DataLoad()
        {
            try
            {
                string sql = string.Format("SELECT j.[ID],j.[date], j.[hcode], j.[jsjeOld], j.[jsjeNew], j.[sqlstr], j.[flag], u.uname,  j.[dw] FROM [AalterJsje] j inner join ausers u on j.userid=u.ucode where  dw = '{0}'", ClassConstant.DW_ID);
                dt = DBAdo.DtFillSql(sql);
                souce = dt.DefaultView;
                this.dataGridView1.DataSource = souce;
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

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.radioButton1.Checked)
            //{
            //    souce.RowFilter = "flag = 0";
            //}
            //if (this.radioButton2.Checked)
            //{
            //    souce.RowFilter = "flag = 1";
            //}
            //if (this.radioButton3.Checked)
            //{
            //    souce.RowFilter = "";
            //}
        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count == 0)
                {
                    return;
                }
                if (bool.Parse(this.dataGridView1.SelectedRows[0].Cells["flag"].Value.ToString()))
                {
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("确定要修改结算金额", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    string updatestr = "update ACONTRACT set hjsje = " + this.dataGridView1.SelectedRows[0].Cells["jsjeNew"].Value.ToString() + " where hcode = '" + this.dataGridView1.SelectedRows[0].Cells["hcode"].Value.ToString() + "'";
                    updatestr += this.dataGridView1.SelectedRows[0].Cells["sqlstr"].Value.ToString() + " update AalterJsje set flag = 1 where id = " + this.dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();
                    DBAdo.ExecuteNonQuerySql(updatestr);
                    this.DataLoad();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bool.Parse(this.dataGridView1.SelectedRows[0].Cells["flag"].Value.ToString()))
            {
                MessageBox.Show("该信息已经审批不能删除！");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("确定删除选中结算金额修改信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                string sql = string.Format("DELETE FROM AalterJsje WHERE ID = {0}", this.dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
                DBAdo.ExecuteNonQuerySql(sql);
                this.DataLoad();
                this.radioButton_CheckedChanged(null, null);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string _RowFilter = "";
            if (this.radioButton1.Checked)
            {
                _RowFilter = "flag = 0 ";
            }
            else if (this.radioButton2.Checked)
            {
                _RowFilter = "flag = 1 ";
            }
            else
            {
                _RowFilter = " 1=1 ";
            }

            _RowFilter += (this.checkBox1.Checked ? string.Format(" and date >= '{0} 0:0:0' and date <= '{1} 23:59:59' ", this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString()) : "");
            Console.WriteLine(_RowFilter);
            souce.RowFilter = _RowFilter;
        }


    }
}
