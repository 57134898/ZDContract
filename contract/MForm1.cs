using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace contract
{
    public partial class MForm1 : Form
    {

        public MForm1()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            Application.Exit();
        }

        //注册工具栏按钮
        public void AddButtons(ToolStripItem[] btns)
        {
            this.toolStrip1.Items.Clear();
            //this.toolStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip1.Items.AddRange(btns);
            foreach (var item in this.toolStrip1.Items)
            {
                if (item is ToolStripButton)
                {
                    (item as ToolStripButton).TextImageRelation = TextImageRelation.ImageAboveText;
                }
            }
        }

        //注消工具栏按钮
        public void ClearButtons()
        {
            this.toolStrip1.Items.Clear();
            //this.toolStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            if (this.MdiChildren.Length == 1)
            {
                this.ShowMaiToolBar();
            }
        }
        /// <summary>
        /// 添加状态栏按钮
        /// </summary>
        /// <param name="child"></param>
        /// <param name="name"></param>

        public void AddStatus(Button b)
        {
            foreach (Control c in this.flowLayoutPanel1.Controls)
            {
                if ((c as Button).Name == b.Name)
                {
                    return;
                }
            }
            this.flowLayoutPanel1.Controls.Add(b);
        }

        /// <summary>
        /// 删除状态栏按钮
        /// </summary>
        /// <param name="child"></param>
        /// <param name="name"></param>
        public void DelStatus(Button b)
        {
            try
            {
                foreach (Control c in this.flowLayoutPanel1.Controls)
                {
                    if ((c as Button).Name == b.Name)
                    {
                        c.Dispose();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //关闭所有子窗体后显示主窗体工具栏按键钮
        #region 关闭所有子窗体后显示主窗体工具栏按键钮
        public void ShowMaiToolBar()
        {
            OnButtonClick alterjsje = (object sender, EventArgs e) =>
            {
                A_HT_ALTERJSJE aljs = new A_HT_ALTERJSJE();
                aljs.MdiParent = this;
                aljs.Show();
            };

            bool jsjeupdate = false;
            if (ClassConstant.BNAME == "财务部" || ClassConstant.BNAME == "办公室")
            {
                jsjeupdate = true;
            }
            OnButtonClick jg = (object sender, EventArgs e) =>
            {
                A_HT c1 = new A_HT("07");
                c1.MdiParent = this;
                c1.Show();
            };
            if (ClassConstant.USER_ID == "0101999999" || ClassConstant.USER_ID == "0201999999")
            {
                this.menuStrip1.Enabled = false;
                OnButtonClick sssp = (object sender, EventArgs e) =>
                {
                    ExportSp sp = new ExportSp();
                    sp.ShowDialog();
                };
                this.AddButtons(new ToolStripItem[]{
                    //new Factory_ToolBtn("采购合同", "采购合同",ClassCustom.getImage("cg.png"5),this.采购合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("销售合同", "销售合同",ClassCustom.getImage("xs.png"), this.销售合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("外协合同", "外协合同",ClassCustom.getImage("wx.png"),this.外协合同ToolStripMenuItem_Click,null,true).TBtnProduce(),

                    new Factory_ToolBtn("在建一期", "在建一期",ClassCustom.getImage("zj.png"), this.在建工程合同ToolStripMenuItem_Click,null,false).TBtnProduce(),
                    
                    new Factory_ToolBtn("在建二期", "在建二期",ClassCustom.getImage("zj.png"), this.在建工程二期ToolStripMenuItem_Click,null,false).TBtnProduce(),
                      new Factory_ToolBtn("技改", "技改",ClassCustom.getImage("gdzc.png"),this.技改ToolStripMenuItem_Click,null,false).TBtnProduce(),
                    new Factory_ToolBtn("固定资产", "固定资产",ClassCustom.getImage("gdzc.png"),this.购置固定资产ToolStripMenuItem_Click,null,false).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("往来客户", "往来客户",ClassCustom.getImage("kh.png"),this.客户ToolStripMenuItem_Click,null,false).TBtnProduce(),
                    new Factory_ToolBtn("进度管理", "进度管理",ClassCustom.getImage("mx.png"),this.btn_jd,null,false).TBtnProduce(),
                    new Factory_ToolBtn("生成金税数据", "生成金税数据",ClassCustom.getImage("rc.png"),rc,null,false).TBtnProduce(),

                    new Factory_ToolBtn("综合查询", "综合查询",ClassCustom.getImage("zhcx.png"), this.合同综合查询ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("客户对账及修改", "客户对账及修改",ClassCustom.getImage("NetByte Design Studio - 0483.png"),合同客户进度查询ToolStripMenuItem_Click,null,false).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("财务凭证确认", "财务凭证确认",ClassCustom.getImage("cw.png"),this.btn_cw,null,false).TBtnProduce(),
                    new Factory_ToolBtn("合同报表", "合同报表",ClassCustom.getImage("bb.png"),合同报表ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("商品查询", "商品查询",ClassCustom.getImage("sp.png"),sssp,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("结算金额确认", "结算金额修改确认",ClassCustom.getImage("upd.png"),alterjsje,null,jsjeupdate).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  EXCEL ", "  EXCEL ",ClassCustom.getImage("ex.jpg"),this.eXCELToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn(" 计算器 ", " 计算器 ",ClassCustom.getImage("jsq.png"),this.计算器ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  " ,"   退出  ",ClassCustom.getImage("tc.png"),ExitSystem,null,true).TBtnProduce()
                });
                return;
            }
            if (ClassConstant.DW_ID == "0101" || ClassConstant.DW_ID == "0201")
            {

                this.AddButtons(new ToolStripItem[]{
                    //new Factory_ToolBtn("采购合同", "采购合同",ClassCustom.getImage("cg.png"),this.采购合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("销售合同", "销售合同",ClassCustom.getImage("xs.png"), this.销售合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("外协合同", "外协合同",ClassCustom.getImage("wx.png"),this.外协合同ToolStripMenuItem_Click,null,true).TBtnProduce(),

                    new Factory_ToolBtn("在建一期", "在建一期",ClassCustom.getImage("zj.png"), this.在建工程合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    
                    new Factory_ToolBtn("在建二期", "在建二期",ClassCustom.getImage("zj.png"), this.在建工程二期ToolStripMenuItem_Click,null,true).TBtnProduce(),
                      new Factory_ToolBtn("技改", "技改",ClassCustom.getImage("gdzc.png"),this.技改ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("固定资产", "固定资产",ClassCustom.getImage("gdzc.png"),this.购置固定资产ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("往来客户", "往来客户",ClassCustom.getImage("kh.png"),this.客户ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("进度管理", "进度管理",ClassCustom.getImage("mx.png"),this.btn_jd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("生成金税数据", "生成金税数据",ClassCustom.getImage("rc.png"),rc,null,true).TBtnProduce(),

                    new Factory_ToolBtn("综合查询", "综合查询",ClassCustom.getImage("zhcx.png"), this.合同综合查询ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("客户对账及修改", "客户对账及修改",ClassCustom.getImage("NetByte Design Studio - 0483.png"),合同客户进度查询ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("财务凭证确认", "财务凭证确认",ClassCustom.getImage("cw.png"),this.btn_cw,null,true).TBtnProduce(),
                    new Factory_ToolBtn("合同报表", "合同报表",ClassCustom.getImage("bb.png"),合同报表ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("集团查询", "集团查询",ClassCustom.getImage("sel.png"),btn_jtcx,null,false).TBtnProduce(),
                    new Factory_ToolBtn("结算金额确认", "结算金额修改确认",ClassCustom.getImage("upd.png"),alterjsje,null,jsjeupdate).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  EXCEL ", "  EXCEL ",ClassCustom.getImage("ex.jpg"),this.eXCELToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn(" 计算器 ", " 计算器 ",ClassCustom.getImage("jsq.png"),this.计算器ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  " ,"   退出  ",ClassCustom.getImage("tc.png"),ExitSystem,null,true).TBtnProduce()
                });
            }
            else
            {
                this.AddButtons(new ToolStripItem[]{
                    new Factory_ToolBtn("采购合同", "采购合同",ClassCustom.getImage("cg.png"),this.采购合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("销售合同", "销售合同",ClassCustom.getImage("xs.png"), this.销售合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("外协合同", "外协合同",ClassCustom.getImage("wx.png"),this.外协合同ToolStripMenuItem_Click,null,true).TBtnProduce(),

                    //new Factory_ToolBtn("在建一期", "在建一期",ClassCustom.getImage("zj.png"), this.在建工程合同ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("技改", "技改",ClassCustom.getImage("gdzc.png"),this.技改ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("在建二期", "在建二期",ClassCustom.getImage("zj.png"), this.在建工程二期ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("固定资产", "固定资产",ClassCustom.getImage("gdzc.png"),this.购置固定资产ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("往来客户", "往来客户",ClassCustom.getImage("kh.png"),this.客户ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("进度管理", "进度管理",ClassCustom.getImage("mx.png"),this.btn_jd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("生成金税数据", "生成金税数据",ClassCustom.getImage("rc.png"),rc,null,true).TBtnProduce(),

                    new Factory_ToolBtn("综合查询", "综合查询",ClassCustom.getImage("zhcx.png"), this.合同综合查询ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("客户对账及修改", "客户对账及修改",ClassCustom.getImage("NetByte Design Studio - 0483.png"),合同客户进度查询ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("财务凭证确认", "财务凭证确认",ClassCustom.getImage("cw.png"),this.btn_cw,null,true).TBtnProduce(),
                    new Factory_ToolBtn("合同报表", "合同报表",ClassCustom.getImage("bb.png"),合同报表ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("集团查询", "集团查询",ClassCustom.getImage("sel.png"),btn_jtcx,null,false).TBtnProduce(),
                    new Factory_ToolBtn("合同修改确认", "合同修改确认",ClassCustom.getImage("upd.png"),alterjsje,null,jsjeupdate).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  EXCEL ", "  EXCEL ",ClassCustom.getImage("ex.jpg"),this.eXCELToolStripMenuItem_Click,null,true).TBtnProduce(),
                    //new Factory_ToolBtn(" 计算器 ", " 计算器 ",ClassCustom.getImage("jsq.png"),this.计算器ToolStripMenuItem_Click,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  " ,"   退出  ",ClassCustom.getImage("tc.png"),ExitSystem,null,true).TBtnProduce()
                });
            }

        }


        #region 按键事件
        private void btn_cw(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FX_XX)))
                return;
            A_HT_FX_XX cm = new A_HT_FX_XX();
            cm.MdiParent = this;
            cm.Show();
        }
        public void btn2_Click(object sender, EventArgs e) { }

        public void ExitSystem(object sender, EventArgs e)
        {
            if (MessageView.MessageYesNoShow("是否退出系统?") == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void rc(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }
        #endregion



        #endregion

        //进度条走3/4
        public void JdtStep0()
        {
            //while (this.toolStripProgressBar1.Value < 100 / 4 * 3)
            //{
            //    this.toolStripProgressBar1.Value++;
            //}
        }

        //进度条走完 并归0
        public void JdtStep100()
        {
            //while (this.toolStripProgressBar1.Value < 100)
            //{
            //    this.toolStripProgressBar1.Value++;
            //}
            //this.toolStripProgressBar1.Value = 0;
        }

        public void LogInApplication()
        {
            this.toolStrip1.Enabled = true;
            this.menuStrip1.Enabled = true;
            this.Text = "合同管理系统 - " + ClassConstant.AccountingBookName + "/" + ClassConstant.DW_NAME + "/" + ClassConstant.BNAME + "/" + ClassConstant.USER_NAME;
            this.ShowMaiToolBar();
            if (ClassConstant.BCODE == "010102" || ClassConstant.BCODE == "020102")
            {
                BitToolStripMenuItem.Visible = true;
            }
        }

        private void MForm_Load(object sender, EventArgs e)
        {

            ClassConstant.MF1 = this;
            ClassConstant.CW_IP = "192.168.7.70";
            DBAdo.setConStr("Provider=SQLOLEDB;Data Source=192.168.7.70;User ID=sa;Initial Catalog=contract1");
            if (System.Environment.GetCommandLineArgs().Length > 1)
            {
                ClassConstant.CONNECT_STRING = "Data Source=127.0.0.1;Initial Catalog=n7_铸锻公司;Provider=SQLOLEDB;User ID=sa";
                ClassConstant.CW_IP = "192.168.1.105";
                //DBAdo.setConStr("Provider=SQLOLEDB;Data Source=.;Integrated Security=SSPI;Initial Catalog=contract1");
                DBAdo.setConStr("Provider=SQLOLEDB;Data Source=127.0.0.1;User ID=sa;password=abcd_1234;Initial Catalog=contract1");
                //DBAdo.setConStr(@"Provider=SQLNCLI10.1;Data Source=.;Password=123;User ID=SA;Initial Catalog=contract1");
                //ClassConstant.CW_IP = ".";
                //DBAdo.setConStr("Provider=SQLOLEDB;Data Source=.;User ID=sa;Initial Catalog=contract1");
            }

            Login log = new Login();
            log.MdiParent = this;
            this.toolStrip1.Enabled = false;
            this.menuStrip1.Enabled = false;
            log.Show();

            this.Text = "合同管理系统  ";// +ClassConstant.DW_NAME + "/" + ClassConstant.BNAME + "/" + ClassConstant.USER_NAME;
            //this.timer1.Start();
            //this.label1.Text = "" +


            this.BackgroundImage = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Images\scf.jpg");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ShowMaiToolBar();


            List<string> Li = ClassCustom.XmlNodeView("100");
            //string str = "user id=" + Li[4] + ";password=" + Li[5] + ";initial catalog=" + Li[3] + ";datasource=" + Li[2] + ";Provider=SQLOLEDB;connect Timeout=20";
            //str = "user id=sa;password='';initial catalog=contract1;datasource=192.168.2.158;Provider=SQLOLEDB;connect Timeout=20";
            //str = "user id=sa;password='';initial catalog=contract1;datasource=.;Provider=SQLOLEDB;connect Timeout=20";
            //DBAdo.setConStr("user id=sa;password='';initial catalog=contract1;datasource=.;Provider=SQLOLEDB;connect Timeout=20");
            //#region test
            //DataTable dt = new DataTable();
            //dt.Columns.Add("ID");
            //dt.Columns.Add("NAME");
            //dt.Columns.Add("AGE");
            //dt.Columns.Add("RMB");
            //dt.Columns.Add("GOOD");
            //dt.Columns.Add("BAL");
            //for (int i = 0; i < 320; i++)
            //{
            //    dt.Rows.Add(new object[] { 1, "ALICE00000000000000000000000000000000000000000000000000000000000000000000", 20, 1000, "PER", 500 });
            //}
            ////this.dgvWithSum1.DataSouce = dt;
            //#endregion
            //Process proc = new Process();
            //proc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "speak.vbs";
            //proc.Start();
        }

        #region 主窗体菜单按钮

        private void 客户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_CLIENT)))
                return;
            A_CLIENT c1 = new A_CLIENT(true);
            c1.MdiParent = this;
            c1.Show();
        }

        private void fORM1ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fORM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region 窗口布局菜单
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }
        public void cd(object sender, EventArgs e)
        {
            CascadeToolStripMenuItem_Click(this, null);
        }
        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }
        public void cz(object sender, EventArgs e)
        {
            TileVerticalToolStripMenuItem_Click(this, null);
        }
        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }
        public void sp(object sender, EventArgs e)
        {
            TileHorizontalToolStripMenuItem_Click(this, null);
        }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            this.ShowMaiToolBar();
        }
        public void CloseAll(object sender, EventArgs e)
        {
            CloseAllToolStripMenuItem_Click(this, null);
        }
        #endregion

        #region 工具菜单
        public void jsq(object sender, EventArgs e)
        {
            计算器ToolStripMenuItem_Click(this, null);
        }
        private void 计算器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "calc.exe";
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 记事本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "notepad";
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void wORDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "winword";
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "excel";
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        //如果是已经打开的窗体,激活即可.
        public bool OpenChildForm(Type _type)
        {
            foreach (Form _child in this.MdiChildren)
            {
                if (_child.GetType().ToString().ToUpper() == _type.ToString().ToUpper())
                {
                    _child.Activate();
                    return true;
                }
            }
            return false;
        }

        #endregion
        private void MForm1_SizeChanged(object sender, EventArgs e)
        {
            this.BackgroundImage = null;
            this.BackgroundImage = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Images\scf.jpg");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

        }

        private void 数据连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBConnL dbl = new DBConnL();
            dbl.ShowDialog();
        }

        private void 供应商ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_CLIENT)))
                return;
            A_CLIENT c1 = new A_CLIENT(false);
            c1.MdiParent = this;
            c1.Show();

        }

        private void 采购合同ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("01");
            c1.MdiParent = this;
            c1.Show();
        }

        private void 销售合同ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("02");
            c1.MdiParent = this;
            c1.Show();
        }

        private void 外协合同ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("03");
            c1.MdiParent = this;
            c1.Show();
        }

        private void 在建工程合同ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("04");
            c1.MdiParent = this;
            c1.Show();
        }

        private void 合同综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_CX_HT)))
                return;
            A_CX_HT c1 = new A_CX_HT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 合同模版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_TK_MB)))
                return;
            A_HT_TK_MB c1 = new A_HT_TK_MB();
            c1.MdiParent = this;
            c1.Show();
        }

        private void btn_jd(object sender, EventArgs e)
        {
            //if (OpenChildForm(typeof(A_HT_FK_NEW)))
            //    return;
            //A_HT_FK_NEW fknew = new A_HT_FK_NEW();
            //fknew.MdiParent = this;
            //fknew.Show();

            if (OpenChildForm(typeof(A_HT_FKFPGY)))
                return;
            A_HT_FKFPGY fknew = new A_HT_FKFPGY();
            fknew.MdiParent = this;
            fknew.Show();
        }

        private void 应收货款ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 应付货款ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 估验ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 合同抵消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 收发票ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 开发票ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 生成金税数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 财务凭证确认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FX_XX)))
                return;
            A_HT_FX_XX c1 = new A_HT_FX_XX();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 合同评审ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 合同报表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_BB)))
                return;
            A_HT_BB c1 = new A_HT_BB();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 新版本上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            A_UPLOAD up = new A_UPLOAD();
            up.ShowDialog();
        }

        private void 系统初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 操作员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            A_FZ_CZY czy = new A_FZ_CZY();
            czy.ShowDialog();
        }

        private void MForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.MdiChildren.Length > 0)
            {
                Console.WriteLine(this.MdiChildren[0].Name + "           " + typeof(Login).ToString());
                if (this.MdiChildren.Length == 1 && this.MdiChildren[0].Name == "Login")
                {

                }
                else
                {
                    MessageBox.Show("请关闭所有窗体后再退出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }

            }
            //else
            //{
            //    e.Cancel = false;
            //}
        }

        private void 在建工程二期ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("05");
            c1.MdiParent = this;
            c1.Show();
        }

        private void 购置固定资产ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("06");
            c1.MdiParent = this;
            c1.Show();
        }

        private void 技改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("07");
            c1.MdiParent = this;
            c1.Show();
        }

        private void btn_jtcx(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_CX_JT)))
                return;
            A_CX_JT c1 = new A_CX_JT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 合同进度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FKFPGY)))
                return;
            A_HT_FKFPGY c1 = new A_HT_FKFPGY();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 日常ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_FK_GZT)))
                return;
            A_HT_FK_GZT c1 = new A_HT_FK_GZT();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            A_PASSWORD_UPDATE p = new A_PASSWORD_UPDATE();
            p.ShowDialog();
        }

        private void sQL工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQL s = new SQL();
            s.Show();
        }

        private void 合同客户进度查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT_KHFK)))
                return;
            A_HT_KHFK c1 = new A_HT_KHFK();
            c1.MdiParent = this;
            c1.Show();
        }

        private void 重新登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MForm1 mmf1 = new MForm1();
            mmf1.Show();
        }

        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUser cu = new ChangeUser();
            cu.ShowDialog();
        }

        private void 结账ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClassConstant.BNAME == "财务部" || ClassConstant.BNAME == "办公室")
            {
                A_LOCK lo = new A_LOCK();
                lo.ShowDialog();
            }
        }

        private void 商品明细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportSp sp = new ExportSp();
            sp.ShowDialog();
        }

        private void 对账ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void 出入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvoiceV wh = new InvoiceV();
            wh.MdiParent = this;
            wh.Show();
            //Warehouse wh = new Warehouse();
            //wh.MdiParent = this;
            //wh.Show();
        }

        private void BitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new BitForm();
            f.Show();
            //MessageBox.Show("Test");
            //Warehouse wh = new Warehouse();
            //wh.MdiParent = this;
            //wh.Show();
        }

        private void 标号更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new BidUpdate();
            f.MdiParent = this;
            f.Show();
        }

        private void 技改ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (OpenChildForm(typeof(A_HT)))
                return;
            A_HT c1 = new A_HT("07");
            c1.MdiParent = this;
            c1.Show();
        }
    }
}
