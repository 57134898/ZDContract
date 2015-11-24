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
    public partial class InvoiceV : Form
    {
        public InvoiceV()
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
            this.dataGridView1.Columns["InvID"].Visible = false;

            this.dataGridView1.Columns["结算金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["库存金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["余额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["结算金额"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns["库存金额"].DefaultCellStyle.Format = "N2";
            this.dataGridView1.Columns["余额"].DefaultCellStyle.Format = "N2";

            this.dataGridView1.Columns["其他费用"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["其他费用"].DefaultCellStyle.Format = "N2";


            this.dataGridView1.AutoResizeColumns();
        }

        private void DataLoad()
        {
            try
            {
                string sql = string.Format(@"SELECT 
T0.InvID
,T1.客户名,T1.合同号,T1.合同类型,T1.结算金额,T1.签订日期
,(SELECT  TOP 1 GNAME FROM ASP N0 WHERE N0.HTH=T0.Hcode ) AS '主要商品'
,(SELECT SUM(N1.Total) FROM InvoiceRows N1 WHERE N1.InvID=T0.InvID) AS '库存金额'
,t0.IDate '单据日期',T0.Saleman '业务员',T0.Todo '备注',T0.InvCode '发票号',T1.HLX 类型编号,ISNULL((SELECT SUM(EtcCost) from InvoiceRows N2 WHERE N2.InvID=T0.InvID),0) '其他费用'
FROM Invioce T0 INNER JOIN vcontracts T1 ON T0.Hcode=T1.合同号 WHERE T1.HDW='{0}'", ClassConstant.DW_ID);
                DataTable dt = DBAdo.DtFillSql(sql);
                dt.Columns.Add("余额", typeof(decimal), "结算金额-库存金额");
                this.dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void Reg()
        {
            OnButtonClick add = (s, e) =>
            {
                Warehouse wh = new Warehouse(1, null, this.dataGridView1.SelectedRows[0].Cells["类型编号"].Value.ToString());
                wh.MdiParent = this.MdiParent;
                wh.Show();
            };
            OnButtonClick upd = (s, e) =>
            {
                if (this.dataGridView1.SelectedRows == null || this.dataGridView1.SelectedRows.Count <= 0)
                {
                    return;
                }
                Warehouse wh = new Warehouse(3, this.dataGridView1.SelectedRows[0], this.dataGridView1.SelectedRows[0].Cells["类型编号"].Value.ToString());
                wh.MdiParent = this.MdiParent;
                wh.Show();
            };
            OnButtonClick del = (s, e) =>
            {
                if (this.dataGridView1.SelectedRows == null || this.dataGridView1.SelectedRows.Count <= 0)
                {
                    return;
                }
                Warehouse wh = new Warehouse(2, this.dataGridView1.SelectedRows[0], this.dataGridView1.SelectedRows[0].Cells["类型编号"].Value.ToString());
                wh.MdiParent = this.MdiParent;
                wh.Show();
            };
            OnButtonClick filter = (s, e) =>
            {
                Warehouse wh = new Warehouse(1, null, this.dataGridView1.SelectedRows[0].Cells["类型编号"].Value.ToString());
                wh.MdiParent = this.MdiParent;
                wh.Show();
            };

            OnButtonClick btn_print = (s, e) =>
            {

                string title = string.Empty;
                string head1Title = string.Empty;

                if (this.dataGridView1.SelectedRows[0].Cells["类型编号"].Value.ToString().Substring(0, 2) == "02")
                {
                    title = "产品出库单";
                    head1Title = "成本费用";
                }
                else if (this.dataGridView1.SelectedRows[0].Cells["类型编号"].Value.ToString().Substring(0, 2) == "03")
                {
                    title = "产品入库单";
                    head1Title = "其他费用";
                }
                else
                {
                    title = "材料入库单";
                    head1Title = "其他费用";
                }
                //客户名	合同号	合同类型	结算金额	签订日期	主要商品	库存金额	单据日期	业务员	备注	发票号	类型编号
                decimal total = 0;
                if (this.dataGridView1.SelectedRows[0].Cells["类型编号"].Value.ToString().Substring(0, 2) == "02")
                {

                    total += decimal.Parse(this.dataGridView1.SelectedRows[0].Cells["库存金额"].Value.ToString());
                }
                else
                {
                    total += decimal.Parse(this.dataGridView1.SelectedRows[0].Cells["库存金额"].Value.ToString()) + decimal.Parse(this.dataGridView1.SelectedRows[0].Cells["其他费用"].Value.ToString());
                }


                WhPrint wp = new WhPrint(this.dataGridView1.SelectedRows[0].Cells["InvID"].Value.ToString(),
                    this.dataGridView1.SelectedRows[0].Cells["客户名"].Value.ToString(),
                   this.dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString(),
                    this.dataGridView1.SelectedRows[0].Cells["发票号"].Value.ToString(),
                    this.dataGridView1.SelectedRows[0].Cells["单据日期"].Value.ToString(), title, head1Title,
                     total);
                wp.ShowDialog();

            };
            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("添加单据","添加单据",ClassCustom.getImage("add.png"),add,null,true).TBtnProduce(),
                    new Factory_ToolBtn("删除单据", "删除单据",ClassCustom.getImage("del.png"), del,null,true).TBtnProduce(),
                    new Factory_ToolBtn("修改单据","修改单据",ClassCustom.getImage("upd.png"),upd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  过滤  ", "  过滤  ",ClassCustom.getImage("sel.png"), filter,null,true).TBtnProduce(),
                    new Factory_ToolBtn("刷新", "刷新",ClassCustom.getImage("sx.png"), this.Form_Load,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                                                            //new ToolStripSeparator(),
                    new Factory_ToolBtn("打印", "打印",ClassCustom.getImage("print.png"), btn_print,null,true).TBtnProduce(),
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
