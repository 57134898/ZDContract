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
    public partial class A_CX_JT : Form
    {
        public A_CX_JT()
        {
            InitializeComponent();
        }


        #region Form基本方法
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT ht;//合同表单

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                //DataTable dtLx = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LEN(LID) = 2");
                //this.comboBox1.DataSource = dtLx;
                //this.comboBox1.DisplayMember = "LNAME";
                //this.comboBox1.ValueMember = "LID";
                //DataTable dtDw = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE CCODE LIKE '01%'");
                //dtDw.Rows[0][1] = "沈阳铸锻工业有限公司";
                //this.comboBox2.DataSource = dtDw;
                //this.comboBox2.DisplayMember = "CNAME";
                //this.comboBox2.ValueMember = "CCODE";


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
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  查询  ","  查询  ",ClassCustom.getImage("sel.png"),btn_cx,null,true).TBtnProduce(),
                    new Factory_ToolBtn("编辑条件","  编辑条件",ClassCustom.getImage("upd.png"),btn_tj,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("导出EXCEL", "导出EXCEL",ClassCustom.getImage("ex.jpg"), this.ExportExcel,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };
            #region old

            #endregion
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
            try
            {
                string sql = "SELECT * FROM vcontracts";
                DataTable dt = DBAdo.DtFillSql(sql);
                //this.sumGrid1.Dgv.DataSource = dt.DefaultView;
                //this.sumGrid1.Dgv.AutoResizeColumns();

                this.splitContainer1.Panel1Collapsed = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
            }

        }

        private void btn_tj(object sender, EventArgs e)
        {
            this.splitContainer1.Panel1Collapsed = false;
        }

        private void ExportExcel(object sender, EventArgs e)
        {

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
