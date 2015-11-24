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
    public partial class A_HT_BB : Form
    {
        public A_HT_BB()
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
                if (ClassConstant.USER_NAME == "于萍")
                {

                }

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
                    //new Factory_ToolBtn("明细条款","明细条款",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
                    new Factory_ToolBtn("打开报表", "打残报表",ClassCustom.getImage("sel.png"), this.btn_sp,null,true).TBtnProduce(),
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

        private void btn_sp(object sender, EventArgs e)
        {
            this.listView1_MouseDoubleClick(null, null);
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

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            if (this.listView1.SelectedItems[0].Group.Name == "listViewGroup2" && (ClassConstant.USER_ID != "0101999999" && ClassConstant.USER_ID != "0101010001"))
            {
                return;
            }
            Form rv;
            switch (this.listView1.SelectedItems[0].Text)
            {
                case "合同收付款明细表":
                    rv = new ReportView("合同收付款明细表", false);
                    break;
                case "合同收付款明细表全部":
                    rv = new ReportView("合同收付款明细表全部", false);
                    break;
                case "合同收付款客户明细表":
                    rv = new ReportView("合同收付款明细表_按客户", false);
                    break;
                case "回款汇总表":
                    rv = new ReportView("回款总汇", false);
                    break;
                case "合同类型汇总表":
                    rv = new ReportView("合同汇总", false);
                    break;
                case "签定合同明细表":
                    rv = new ReportView("签订明细", false);
                    break;
                case "各单位货款回收明细汇总表":
                    rv = new ReportView("Report_JT_各单位货款同期对比表", false);
                    break;
                case "合同类型总览表":
                    rv = new ReportView("Report_JT_合同类型汇总表.rdlc", false);
                    break;
                case "各单位货款回收汇总表":
                    rv = new ReportView("Report_JT_各单位收付款汇总表", false);
                    break;
                case "集团情况表":
                    rv = new ReprotView_Chart();
                    break;
                case "各单位签订合同情况表":
                    rv = new ReportView("各单位签订合同情况表", false);
                    break;
                case "集团合同类型汇总表":
                    rv = new ReportView("集团合同类型汇总表", false);
                    break;
                case "销售毛利表":
                    rv = new ReportView("销售毛利表", false);
                    break;
                case "销售合同（对应）外协产品部件明细表":
                    rv = new ReprotView3("销售合同（对应）外协产品部件明细表", false);
                    break;
                case "销售合同商品明细表":
                    rv = new ReprotView3("销售合同商品明细表", false);
                    break;
                case "外协合同部件明细表":
                    rv = new ReprotView3("外协合同部件明细表", false);
                    break;
                case "铸锻公司全部采购外协合同汇总表":
                    rv = new ReportView("铸锻公司全部采购外协合同汇总表", true);
                    break;
                case "毛利新":
                    rv = new ReportView("毛利新", false);
                    break;
                case "毛利新1":
                    rv = new ReportView("毛利新1", false);
                    break;
                case "毛利(自制销售)":
                    Reprot_ML ml = new Reprot_ML();
                    ml.ShowDialog();
                    return;
                case "毛利":
                    rv = new ReportView("集团毛利新", true);
                    break;
                case "集团合同类型汇总表(新)":
                    rv = new ReprotViewNew("集团合同类型汇总表(新)");
                     break;
                    ///集团报表/集团合同类型汇总表(新)
                    ///集团报表/各单位签订合同情况表(新)
                    ///集团报表/各单位货款同期对比表(新)
                    ///集团报表/各单位货款回收汇总表(新)
                    ///集团报表/铸锻公司全部采购外协合同汇总表(新)
                    ///集团报表/合同类型总览表新)
                    ///集团报表/集团毛利(新)
                case "各单位签订合同情况表(新)":
                     rv = new ReprotViewNew("各单位签订合同情况表(新)");
                     break;
                case "各单位货款同期对比表(新)":
                     rv = new ReprotViewNew("各单位货款同期对比表(新)");
                     break;
                case "各单位货款回收汇总表(新)":
                     rv = new ReprotViewNew("各单位货款回收汇总表(新)");
                     break;
                case "铸锻公司全部采购外协合同汇总表(新)":
                     rv = new ReprotViewNew("铸锻公司全部采购外协合同汇总表(新)");
                     break;
                case "合同类型总览表(新)":
                     rv = new ReprotViewNew("合同类型总览表(新)");
                     break;




                default:
                    throw new Exception("未知报表！");
            }
            rv.MdiParent = this.MdiParent;
            rv.Show();
            //A_RPT_GY qt11 = new A_RPT_GY("估验报表");
            //qt11.Show();

            return;


            if (this.listView1.SelectedItems[0].Text == "付款情况明细表")
            {
                A_RPT_FK fk = new A_RPT_FK("付款情况明细表");
                fk.Show();
            }
            if (this.listView1.SelectedItems[0].Text == "回款情况明细表")
            {
                A_RPT_FK fk = new A_RPT_FK("回款情况明细表");
                fk.Show();
            }

            if (this.listView1.SelectedItems[0].Text == "回款情况汇总表")
            {
                A_RPT_FK fk = new A_RPT_FK("回款情况汇总表");
                fk.Show();
            }
            if (this.listView1.SelectedItems[0].Text == "付款情况汇总表")
            {
                A_RPT_FK fk = new A_RPT_FK("付款情况汇总表");
                fk.Show();
            }
            if (this.listView1.SelectedItems[0].Text == "销售合同毛利明细表")
            {
                A_RPT_ML qt = new A_RPT_ML("销售合同毛利明细表");
                qt.Show();
            }
            if (this.listView1.SelectedItems[0].Text == "签订合同情况明细表")
            {
                A_RPT_QD qt = new A_RPT_QD("签订合同情况明细表");
                qt.Show();
            }
            if (this.listView1.SelectedItems[0].Text == "估验报表")
            {
                A_RPT_GY qt = new A_RPT_GY("估验报表");
                qt.Show();
            }


            if (this.listView1.SelectedItems[0].Text == "销售合同汇总表")
            {
                A_RPT_HZ qt = new A_RPT_HZ("销售合同汇总表");
                qt.Show();
            }
            if (this.listView1.SelectedItems[0].Text == "采购合同汇总表")
            {
                A_RPT_HZ qt = new A_RPT_HZ("采购合同汇总表");
                qt.Show();
            }
            if (this.listView1.SelectedItems[0].Text == "材料采购总览表")
            {

                A_RPT_ZL zl = new A_RPT_ZL("材料采购总览表");
                zl.Show();


            }
            if (this.listView1.SelectedItems[0].Text == "产品销售总览表")
            {

                A_RPT_ZL zl = new A_RPT_ZL("产品销售总览表");
                zl.Show();

            }
            if (this.listView1.SelectedItems[0].Text == "客户回款表")
            {

                A_RPT_KHHK zl = new A_RPT_KHHK();
                zl.Show();
            }

            if (this.listView1.SelectedItems[0].Text == "各单位货款回收明细汇总表")
            {

                A_RPT_SFKMX sf = new A_RPT_SFKMX();
                sf.Show();


            }
            if (this.listView1.SelectedItems[0].Text == "各单位货款回收汇总表")
            {

                A_RPT_SFKHZ sf = new A_RPT_SFKHZ();
                sf.Show();
            }

            //铸锻公司全部采购外协合同汇总表
            //if (this.listView1.SelectedItems[0].Text == "在建一期客户汇总表")
            //{
            //    A_RPT_ZJGC_KHHZ zj = new A_RPT_ZJGC_KHHZ();
            //    zj.Show();
            //}


        }



    }
}
