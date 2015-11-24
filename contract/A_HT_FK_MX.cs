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
    public partial class A_HT_FK_MX : Form, IChildForm
    {
        public A_HT_FK_MX()
        {
            InitializeComponent();
        }
        public A_HT_FK_MX(string hth, A_HT_FK_GZT gzt)
        {
            InitializeComponent();
            this.hth = hth;
            this.gzt = gzt;
        }
        private string hth;
        private DataTable dt1 = new DataTable();
        private DataTable dt2 = new DataTable();
        private DataTable dt3 = new DataTable();

        #region Form基本方法
        private ToolStripItem[] bts = null;
        //private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT_FK_GZT gzt;//合同表单

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = hth + "-进度明细";

                Reg();
                DataLoad();
                DgvCssSet();

            }
            catch (Exception ex)
            {

                MessageView.MessageErrorShow(ex);
            }
        }

        public void ReLoad()
        {
            this.Form_Load(null, null);
        }

        private void DgvCssSet()
        {
            this.dataGridView1.Columns["ID"].Visible = false;
            this.dataGridView1.Columns["hth"].Visible = false;
            this.dataGridView1.Columns["date"].HeaderText = "日期";
            this.dataGridView1.Columns["rmb"].HeaderText = "金额";
            this.dataGridView1.Columns["date"].HeaderText = "日期";
            this.dataGridView1.Columns["xshth"].HeaderText = "销售合同号";
            this.dataGridView1.Columns["fkfs"].HeaderText = "方式";
            this.dataGridView1.Columns["fklx"].HeaderText = "类型";
            this.dataGridView1.Columns["flag"].HeaderText = "是否生成凭证";
            this.dataGridView1.Columns["vid"].HeaderText = "凭证号";
            this.dataGridView1.Columns["vtype"].HeaderText = "凭证类型";
            this.dataGridView1.Columns["vyear"].HeaderText = "凭证年份";
            this.dataGridView1.Columns["vmonth"].HeaderText = "凭证月份";
            this.dataGridView1.Columns["bl1"].HeaderText = "开行";
            this.dataGridView1.Columns["bl2"].HeaderText = "新公司";
            this.dataGridView1.Columns["bl3"].HeaderText = "原公司";
            this.dataGridView1.Columns["type"].HeaderText = "类型";
            this.dataGridView1.Columns["rmb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["rmb"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.AutoResizeColumns();
            this.dataGridView2.Columns["ID"].Visible = false;
            this.dataGridView2.Columns["hth"].Visible = false;
            this.dataGridView2.Columns["date"].HeaderText = "日期";
            this.dataGridView2.Columns["rmb"].HeaderText = "金额";
            this.dataGridView2.Columns["date"].HeaderText = "日期";
            this.dataGridView2.Columns["xshth"].HeaderText = "销售合同号";
            this.dataGridView2.Columns["fkfs"].HeaderText = "方式";
            this.dataGridView2.Columns["fklx"].HeaderText = "类型";
            this.dataGridView2.Columns["flag"].HeaderText = "是否生成凭证";
            this.dataGridView2.Columns["vid"].HeaderText = "凭证号";
            this.dataGridView2.Columns["vtype"].HeaderText = "凭证类型";
            this.dataGridView2.Columns["vyear"].HeaderText = "凭证年份";
            this.dataGridView2.Columns["vmonth"].HeaderText = "凭证月份";
            this.dataGridView2.Columns["bl1"].Visible = false;
            this.dataGridView2.Columns["bl2"].Visible = false;
            this.dataGridView2.Columns["bl3"].Visible = false;
            this.dataGridView2.Columns["type"].HeaderText = "类型";
            this.dataGridView2.Columns["rmb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView2.Columns["rmb"].DefaultCellStyle.Format = "N2";
            this.dataGridView2.AutoResizeColumns();
            if (this.tabControl1.TabPages.Count > 2)
            {
                this.dataGridView3.Columns["ID"].Visible = false;
                this.dataGridView3.Columns["hth"].Visible = false;
                this.dataGridView3.Columns["date"].HeaderText = "日期";
                this.dataGridView3.Columns["rmb"].HeaderText = "金额";
                this.dataGridView3.Columns["date"].HeaderText = "日期";
                this.dataGridView3.Columns["xshth"].HeaderText = "销售合同号";
                this.dataGridView3.Columns["fkfs"].HeaderText = "方式";
                this.dataGridView3.Columns["fklx"].HeaderText = "类型";
                this.dataGridView3.Columns["flag"].HeaderText = "是否生成凭证";
                this.dataGridView3.Columns["vid"].HeaderText = "凭证号";
                this.dataGridView3.Columns["vtype"].HeaderText = "凭证类型";
                this.dataGridView3.Columns["vyear"].HeaderText = "凭证年份";
                this.dataGridView3.Columns["vmonth"].HeaderText = "凭证月份";
                this.dataGridView3.Columns["bl1"].Visible = false;
                this.dataGridView3.Columns["bl2"].Visible = false;
                this.dataGridView3.Columns["bl3"].Visible = false;
                this.dataGridView3.Columns["type"].HeaderText = "类型";
                this.dataGridView3.Columns["rmb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView3.Columns["rmb"].DefaultCellStyle.Format = "N2";
                this.dataGridView3.AutoResizeColumns();
            }

        }

        private void DataLoad()
        {
            try
            {
                this.tabControl1.TabPages.Clear();
                string hlx = DBAdo.ExecuteScalarSql("select hlx from ACONTRACT where hcode = '" + hth + "'").ToString();
                if (hlx.Length > 0)
                {
                    if (hlx.Substring(0, 2) == "02")
                    {
                        this.tabPage1.Text = "已回款";
                        this.tabPage2.Text = "已开发票";
                        this.tabControl1.Controls.Add(this.tabPage1);
                        this.tabControl1.Controls.Add(this.tabPage2);
                    }
                    else
                    {
                        this.tabPage1.Text = "已付款";
                        this.tabPage2.Text = "已收发票";
                        this.tabControl1.Controls.Add(this.tabPage1);
                        this.tabControl1.Controls.Add(this.tabPage2);
                        this.tabControl1.Controls.Add(this.tabPage3);
                    }
                }

                dt1 = DBAdo.DtFillSql("SELECT [ID], [date], [rmb], [hth], [xshth], [type], [fkfs], [fklx], [flag], [vid], [vtype], [vyear], [vmonth], [bl1], [bl2], [bl3] FROM AFKXX WHERE HTH = '" + hth + "' AND (TYPE ='付款' OR TYPE ='回款'  OR TYPE ='抵消')");
                dt2 = DBAdo.DtFillSql("SELECT [ID], [date], [rmb], [hth], [xshth], [type], [fkfs], [fklx], [flag], [vid], [vtype], [vyear], [vmonth], [bl1], [bl2], [bl3] FROM AFKXX WHERE HTH = '" + hth + "' AND (TYPE ='进项发票' OR TYPE ='销项发票')");
                dt3 = DBAdo.DtFillSql("SELECT [ID], [date], [rmb], [hth], [xshth], [type], [fkfs], [fklx], [flag], [vid], [vtype], [vyear], [vmonth], [bl1], [bl2], [bl3] FROM AFKXX WHERE HTH = '" + hth + "' AND (TYPE ='估验')");
                this.dataGridView1.DataSource = dt1;
                this.dataGridView2.DataSource = dt2;
                this.dataGridView3.DataSource = dt3;
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
                    //new Factory_ToolBtn(" 删 除 ", " 删 除 ",ClassCustom.getImage("del.png"), this.btn_Del,null,true).TBtnProduce(),
                    //new Factory_ToolBtn(" 修 改 "  ," 修 改 ",ClassCustom.getImage("upd.png"),this.btn_Update,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("明细条款","明细条款",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("商品明细", "商品明细",ClassCustom.getImage("sp.png"), this.btn_sp,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("  意见  ", "  意见  ",ClassCustom.getImage("yj.png"), this.btn_yj,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("客户信息", "客户信息",ClassCustom.getImage("khxx.png"), this.btn_kh,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("公司信息", "公司信息",ClassCustom.getImage("gs.png"), this.btn_gs,null,true).TBtnProduce(),

                    new ToolStripSeparator(),
                    //new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
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

        private void btn_Update(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                if (this.dataGridView1.Rows.Count == 0)
                    return;
                if (bool.Parse(this.dataGridView1.SelectedRows[0].Cells["flag"].Value.ToString()))
                {
                    MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK_OP)))
                    return;
                A_HT_FK_OP aop = new A_HT_FK_OP(this.dataGridView1.SelectedRows[0].Cells["hth"].Value.ToString(), this.gzt, this, this.dataGridView1.SelectedRows[0]);
                aop.MdiParent = this.MdiParent;
                aop.Show();
            }
            if (this.tabControl1.SelectedIndex == 1)
            {
                if (this.dataGridView2.Rows.Count == 0)
                    return;
                if (bool.Parse(this.dataGridView2.SelectedRows[0].Cells["flag"].Value.ToString()))
                {
                    MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK_OP)))
                    return;
                A_HT_FK_OP aop = new A_HT_FK_OP(this.dataGridView2.SelectedRows[0].Cells["hth"].Value.ToString(), this.gzt, this, this.dataGridView2.SelectedRows[0]);
                aop.MdiParent = this.MdiParent;
                aop.Show();
            }
            if (this.tabControl1.SelectedIndex == 2)
            {
                if (this.dataGridView3.Rows.Count == 0)
                    return;
                if (bool.Parse(this.dataGridView3.SelectedRows[0].Cells["flag"].Value.ToString()))
                {
                    MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK_OP)))
                    return;
                A_HT_FK_OP aop = new A_HT_FK_OP(this.dataGridView3.SelectedRows[0].Cells["hth"].Value.ToString(), this.gzt, this, this.dataGridView3.SelectedRows[0]);
                aop.MdiParent = this.MdiParent;
                aop.Show();
            }

        }

        private void btn_Del(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControl1.SelectedIndex == 0)
                {
                    if (bool.Parse(this.dataGridView1.SelectedRows[0].Cells["flag"].Value.ToString()))
                    {
                        MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    if (DialogResult.Yes == MessageView.MessageYesNoShow("是否删除选中信息?"))
                    {
                        DBAdo.ExecuteNonQuerySql("DELETE FROM AFKXX WHERE ID ='" + this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'");
                        this.gzt.ReLoad();
                        this.Form_Load(null, null);
                    }
                }
                if (this.tabControl1.SelectedIndex == 1)
                {
                    if (bool.Parse(this.dataGridView2.SelectedRows[0].Cells["flag"].Value.ToString()))
                    {
                        MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    if (DialogResult.Yes == MessageView.MessageYesNoShow("是否删除选中信息?"))
                    {
                        DBAdo.ExecuteNonQuerySql("DELETE FROM AFKXX WHERE ID ='" + this.dataGridView2.SelectedRows[0].Cells[0].Value.ToString() + "'");
                        this.gzt.ReLoad();
                        this.Form_Load(null, null);
                    }
                }
                if (this.tabControl1.SelectedIndex == 2)
                {
                    if (bool.Parse(this.dataGridView3.SelectedRows[0].Cells["flag"].Value.ToString()))
                    {
                        MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    if (DialogResult.Yes == MessageView.MessageYesNoShow("是否删除选中信息?"))
                    {
                        DBAdo.ExecuteNonQuerySql("DELETE FROM AFKXX WHERE ID ='" + this.dataGridView3.SelectedRows[0].Cells[0].Value.ToString() + "'");
                        this.gzt.ReLoad();
                        this.Form_Load(null, null);
                    }
                }

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

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (this.tabControl1.SelectedIndex == 0)
            //{
            //    if (this.dataGridView1.Rows.Count == 0)
            //        return;
            //    if (bool.Parse(this.dataGridView1.SelectedRows[0].Cells["flag"].Value.ToString()))
            //    {
            //        MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //        return;
            //    }
            //    if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK_OP)))
            //        return;
            //    A_HT_FK_OP aop = new A_HT_FK_OP(this.dataGridView1.SelectedRows[0].Cells["hth"].Value.ToString(), this.gzt, this, this.dataGridView1.SelectedRows[0]);
            //    aop.MdiParent = this.MdiParent;
            //    aop.Show();
            //}
            //if (this.tabControl1.SelectedIndex == 1)
            //{
            //    if (this.dataGridView2.Rows.Count == 0)
            //        return;
            //    if (bool.Parse(this.dataGridView2.SelectedRows[0].Cells["flag"].Value.ToString()))
            //    {
            //        MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //        return;
            //    }
            //    if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK_OP)))
            //        return;
            //    A_HT_FK_OP aop = new A_HT_FK_OP(this.dataGridView2.SelectedRows[0].Cells["hth"].Value.ToString(), this.gzt, this, this.dataGridView2.SelectedRows[0]);
            //    aop.MdiParent = this.MdiParent;
            //    aop.Show();
            //}
            //if (this.tabControl1.SelectedIndex == 2)
            //{
            //    if (this.dataGridView3.Rows.Count == 0)
            //        return;
            //    if (bool.Parse(this.dataGridView3.SelectedRows[0].Cells["flag"].Value.ToString()))
            //    {
            //        MessageBox.Show("该信息财务已做凭证不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //        return;
            //    }
            //    if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_FK_OP)))
            //        return;
            //    A_HT_FK_OP aop = new A_HT_FK_OP(this.dataGridView3.SelectedRows[0].Cells["hth"].Value.ToString(), this.gzt, this, this.dataGridView3.SelectedRows[0]);
            //    aop.MdiParent = this.MdiParent;
            //    aop.Show();
            //}
        }
        #endregion
    }
}
