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
    public partial class A_ZJGC_CX : Form, IsetText
    {
        private ToolStripItem[] bts = null;
        private DataTable dt_zy;
        private DataTable dt_bz;
        private DataView dv_zy;
        private DataView dv_bz;
        public A_ZJGC_CX()
        {
            InitializeComponent();
        }
        private void A_ZJGC_CX_Load(object sender, EventArgs e)
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
                    new Factory_ToolBtn("  查询  ","  查询  ",ClassCustom.getImage("sel.png"),btn_cx,null,true).TBtnProduce(),
                    new Factory_ToolBtn("编辑条件","  编辑条件",ClassCustom.getImage("set.png"),btn_tj,null,true).TBtnProduce(),
                     new Factory_ToolBtn("修改","  修改",ClassCustom.getImage("upd.png"),btn_xg,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
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

        public void flash()
        {
            btn_cx(null, null);

        }


        #endregion

        #region settext
        public void SetTextKH(string key, string value)
        {
            try
            {
                this.textBoxKH.Text = value;
                this.textBoxKH.Tag = key;

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }


        #endregion

        #region 按钮事件
        private void btn_cx(object sender, EventArgs e)
        {
            try
            {
                string filter = "";
                if (this.checkBoxKH.Checked)
                {
                    if (this.textBoxKH.Tag == null)
                    {

                    }
                    else
                    {
                        if (this.textBoxKH.Tag.ToString().Trim() == "")
                        {

                        }
                        else
                        {
                            filter += "and az.客户编码='" + this.textBoxKH.Tag.ToString() + "'";
                        }
                    }
                }
                if (this.checkBoxDATE.Checked)
                {
                    filter += "and ((az.年='" + this.dateTimePicker1.Value.Year + "' AND az.月>='" + this.dateTimePicker1.Value.Month + "') or (az.年>'" + this.dateTimePicker1.Value.Year + "' AND az.年<'" + this.dateTimePicker2.Value.Year + "') or (az.年='" + this.dateTimePicker2.Value.Year + "' AND az.月<='" + this.dateTimePicker2.Value.Month + "'))";
                    //filter += "and ((az.年='YEAR(" + this.dateTimePicker1.Value + ")' AND az.月>='MONTH(" + this.dateTimePicker2.Value + ")') or (az.年>'YEAR(" + this.dateTimePicker1.Value + ")' AND az.年<'YEAR(" + this.dateTimePicker2.Value + ")') or (az.年='YEAR(" + this.dateTimePicker2.Value + ")' AND az.月<'MONTH(" + this.dateTimePicker2.Value+ ")'))";
                }
                if (this.checkBoxLB.Checked)
                {
                    if (this.comboBoxLB.Text == "现金")
                    {
                        filter += "and 凭证类别='1'";
                    }
                    if (this.comboBoxLB.Text == "银行")
                    {
                        filter += "and 凭证类别='2'";
                    }
                    if (this.comboBoxLB.Text == "转账")
                    {
                        filter += "and 凭证类别='3'";
                    }

                }
                if (this.checkBoxH.Checked)
                {
                    filter += "and 凭证号='" + this.comboBoxH.Text + "'";
                }
                if (this.checkBoxZY.Checked)
                {
                    filter += "and 摘要='" + this.textBoxZY.Text + "'";
                }
                if (this.checkBoxBZ.Checked)
                {
                    filter += "and 备注='" + this.textBoxBZ.Text + "'";
                }
                string sql = "select az.ID,az.客户编码,ac.cname 客户名,az.摘要,az.年,az.月,CASE az.凭证类别 WHEN '1' THEN '现金' WHEN '2' THEN '银行' ELSE '转账' END 凭证类别,az.凭证号,az.金额,az.类型,az.备注 from AZJQQ az,ACLIENTS ac where az.客户编码 = ac.ccode " + filter + " order by az.ID";
                this.dataGridView3.DataSource = DBAdo.DtFillSql(sql);
                this.dataGridView3.Columns[0].Visible = false;
                this.dataGridView3.AutoResizeColumns();
                this.dataGridView3.Columns[0].Frozen = true;
                this.dataGridView3.Columns[1].Frozen = true;
                this.dataGridView3.Columns[2].Frozen = true;
                this.dataGridView3.Visible = true;
                this.splitContainer1.Panel1Collapsed = true;
                this.toolStripStatusLabel2.Text = DBAdo.ExecuteScalarSql("select sum(金额) from AZJQQ az where 类型 = '付款' " + filter + " ").ToString();
                this.toolStripStatusLabel4.Text = DBAdo.ExecuteScalarSql("select sum(金额) from AZJQQ az where 类型 = '发票' " + filter + " ").ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btn_tj(object sender, EventArgs e)
        {
            try
            {
                this.splitContainer1.Panel1Collapsed = false;

                //if (ahtop == null)
                //    return;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }
        }

        private void btn_xg(object sender, EventArgs e)
        {
            if (this.dataGridView3.SelectedRows.Count == 0)
            {
                return;
            }
            dataGridView3_CellMouseDoubleClick(null, null);
        }

        private void btn_close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportExcel(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.dataGridView3, "在建工程明细");
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_FZ_KH)))
            {
                return;
            }
            A_FZ_KH akh = new A_FZ_KH(this);
            akh.MdiParent = this.MdiParent;
            akh.Show();
        }

        private void checkBoxLB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxLB.Checked)
                {
                    string[] lb;
                    lb = new string[] { "现金", "银行", "转账" };
                    this.comboBoxLB.DataSource = lb;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void checkBoxH_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBoxH.Checked)
                {
                    this.comboBoxH.DataSource = DBAdo.DtFillSql("select distinct 凭证号 from AZJQQ order by 凭证号 asc");
                    this.comboBoxH.DisplayMember = "凭证号";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void checkBoxZY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxZY.Checked)
            {
                string sql = "select az.摘要 from AZJQQ az,ACLIENTS ac where az.客户编码 = ac.ccode order by az.摘要";
                dt_zy = DBAdo.DtFillSql(sql);
                dv_zy = dt_zy.DefaultView;
                this.dataGridView1.DataSource = dv_zy;
                this.dataGridView1.Visible = true;
                this.dataGridView1.AutoResizeColumns();

            }
            else
            {
                this.dataGridView1.Visible = false;
                this.textBoxZY.Text = "";
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                this.textBoxZY.Text = this.dataGridView1.SelectedRows[0].Cells["摘要"].Value.ToString();
                this.dataGridView1.Visible = false;
            }
        }

        private void textBoxZY_TextChanged(object sender, EventArgs e)
        {
            dv_zy.RowFilter = "摘要 like '" + this.textBoxZY.Text + "%'";
            this.dataGridView1.Visible = true;
        }

        private void checkBoxBZ_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxBZ.Checked)
            {
                string sql = "select DISTINCT az.备注 from AZJQQ az,ACLIENTS ac where az.客户编码 = ac.ccode order by az.备注";
                dt_bz = DBAdo.DtFillSql(sql);
                dv_bz = dt_bz.DefaultView;
                this.dataGridView2.DataSource = dv_bz;
                this.dataGridView2.Visible = true;
                this.dataGridView2.AutoResizeColumns();
            }
            else
            {
                this.dataGridView2.Visible = false;
                this.textBoxBZ.Text = "";
            }


        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.textBoxBZ.Text = this.dataGridView2.SelectedRows[0].Cells["备注"].Value.ToString();
            this.dataGridView2.Visible = false;
        }

        private void textBoxBZ_TextChanged(object sender, EventArgs e)
        {
            dv_bz.RowFilter = "备注 like '" + this.textBoxBZ.Text + "%'";
            this.dataGridView2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Visible = !this.dataGridView1.Visible;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Visible = !this.dataGridView2.Visible;
        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {




            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_ZJGG_XG)))
            {
                return;
            }
            else
            {
                string id = this.dataGridView3.SelectedRows[0].Cells["ID"].Value.ToString();
                A_ZJGG_XG XG = new A_ZJGG_XG(id, this);
                XG.MdiParent = this.MdiParent;
                XG.Visible = true;
            }

        }



    }
}
