using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace contract
{
    public partial class A_HT_OP : Form, IChildForm, IsetText, IBidCode
    {
        private decimal jsje;
        private ToolStripItem[] bts = null;
        private int op;//操作 1添加 2删除 3修改 4查询
        private A_HT ht;//合同表单
        private DataGridViewRow dgvr;//修改用
        private string htype;//合同类型01采购02销售03外协04在建
        private DataTable dt_sp = new DataTable();//商品
        private DataTable dt_tk = new DataTable();//条款
        private DataTable dt_wx = new DataTable();//外协
        private DataTable dt_c3 = new DataTable();//c3
        private DataTable dt_c4 = new DataTable();//c4
        private DataTable dt_c5 = new DataTable();//c5
        private DataTable dt_c6 = new DataTable();//c6
        //private DataTable dt_kh = new DataTable();//客户
        //private DataTable dt_gs = new DataTable();//公司
        List<int> LdeL = new List<int>();//要删除的商品ID



        public A_HT_OP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op">操作 1添加 2删除 3修改 4查询</param>
        /// <param name="ht">合同表单</param>
        /// <param name="htype">01采购02销售03外协04在建</param>
        public A_HT_OP(int op, A_HT ht, string htype)//添加
        {
            InitializeComponent();
            this.op = op;
            this.ht = ht;
            this.htype = htype;
        }

        public A_HT_OP(int op, A_HT ht, DataGridViewRow dgvr, string htype)//修改
        {
            InitializeComponent();
            this.op = op;
            this.ht = ht;
            this.dgvr = dgvr;
            this.htype = htype;
            try
            {
                jsje = decimal.Parse(dgvr.Cells["结算金额"].Value.ToString());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void A_HT_OP_Load(object sender, EventArgs e)
        {
            try
            {
                this.toolStripComboBox1.SelectedIndex = 0;

                this.hDWTextBox.Text = ClassConstant.DW_NAME;
                this.hDWTextBox.Tag = ClassConstant.DW_ID;
                this.customSpGrid1.ColName1 = "数量单价";
                this.customSpGrid1.ColName2 = "重量单价";

                this.customSpGrid1.Columns["序号"].ValueType = typeof(int);
                this.customSpGrid1.Columns["交货日期"].ValueType = typeof(DateTime);
                this.customSpGrid1.Columns["产品名称"].ValueType = typeof(string);
                this.customSpGrid1.Columns["规格型号"].ValueType = typeof(string);
                this.customSpGrid1.Columns["材质"].ValueType = typeof(string);
                this.customSpGrid1.Columns["计量单位1"].ValueType = typeof(string);
                this.customSpGrid1.Columns["计量单位2"].ValueType = typeof(string);
                this.customSpGrid1.Columns["净毛"].ValueType = typeof(string);
                this.customSpGrid1.Columns["备注"].ValueType = typeof(string);
                this.customSpGrid1.Columns["数量"].ValueType = typeof(decimal);
                this.customSpGrid1.Columns["单价1"].ValueType = typeof(decimal);
                this.customSpGrid1.Columns["单价2"].ValueType = typeof(decimal);
                this.customSpGrid1.Columns["单重"].ValueType = typeof(decimal);
                this.customSpGrid1.Columns["总重"].ValueType = typeof(decimal);
                this.customSpGrid1.Columns["总价"].ValueType = typeof(decimal);
                this.customSpGrid1.Columns["数量"].DefaultCellStyle.Format = "N4";
                this.customSpGrid1.Columns["单价1"].DefaultCellStyle.Format = "N4";
                this.customSpGrid1.Columns["单价2"].DefaultCellStyle.Format = "N4";
                this.customSpGrid1.Columns["单重"].DefaultCellStyle.Format = "N4";
                this.customSpGrid1.Columns["总重"].DefaultCellStyle.Format = "N4";
                this.customSpGrid1.Columns["总价"].DefaultCellStyle.Format = "N4";
                this.customSpGrid1.Columns["数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.customSpGrid1.Columns["单价1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.customSpGrid1.Columns["单价2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.customSpGrid1.Columns["单重"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.customSpGrid1.Columns["总重"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.customSpGrid1.Columns["总价"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.customSpGrid1.AutoResizeColumns();
                DT();
                FZXX();
                Reg();
                if (this.op == 1)
                {
                    this.hUSERTextBox.Text = ClassConstant.USER_NAME;
                    this.Text = "合同 -- 添加合同";
                }
                if (this.op == 3)
                {

                    //this.button10.Enabled = false;
                    this.button1.Enabled = false;

                    this.Text = "合同 -- 修改合同";
                    this.hCODETextBox.ReadOnly = true;
                    DataTable dt_t1 = DBAdo.DtFillSql("SELECT [GCODE], [GNAME], [GCZ], [GXH], [GDW1], [GSL], [GDJ1],  ZJ,  ZZ,[GDW2], [GJM], [GDZ], [GMEMO], [HTH], [TH], [DJ2], [JHRQ],[InvID] FROM [ASP] WHERE HTH ='" + dgvr.Cells["合同号"].Value.ToString() + "'");
                    int index1 = 1;
                    for (int i = 0; i < dt_t1.Rows.Count; i++)
                    {
                        //                                          序号	         产品名称	          规格型号(图号)	材质	           数量	                      计量单位	             单价	                 	净/毛	               单重	                总重	单价	总价	交货日期	备注	GCODE
                        this.customSpGrid1.Rows.Add(new object[] { index1++, dt_t1.Rows[i]["GNAME"], dt_t1.Rows[i]["GXH"], dt_t1.Rows[i]["GCZ"], dt_t1.Rows[i]["GSL"], dt_t1.Rows[i]["GDW1"], dt_t1.Rows[i]["GDJ1"], dt_t1.Rows[i]["GDW2"], dt_t1.Rows[i]["GJM"], dt_t1.Rows[i]["GDZ"], dt_t1.Rows[i]["ZZ"], dt_t1.Rows[i]["DJ2"], dt_t1.Rows[i]["ZJ"], dt_t1.Rows[i]["JHRQ"], dt_t1.Rows[i]["GMEMO"], dt_t1.Rows[i]["GCODE"], dt_t1.Rows[i]["InvID"] });
                        //dt_sp.Rows.Add(new object[] { index1++, dt_t1.Rows[i]["GNAME"], dt_t1.Rows[i]["GCZ"], dt_t1.Rows[i]["GXH"], dt_t1.Rows[i]["GSL"], dt_t1.Rows[i]["GDW1"], dt_t1.Rows[i]["GDJ1"], 0, dt_t1.Rows[i]["GDW2"], dt_t1.Rows[i]["GJM"], dt_t1.Rows[i]["GDZ"], 0, dt_t1.Rows[i]["GMEMO"], dt_t1.Rows[i]["TH"], dt_t1.Rows[i]["GCODE"] });
                    }
                    DataTable dt_t2 = DBAdo.DtFillSql("SELECt * FROM ATK WHERE HTH ='" + dgvr.Cells["合同号"].Value.ToString() + "' ORDER BY [ORDER]");
                    for (int i = 0; i < dt_t2.Rows.Count; i++)
                    {
                        dt_tk.Rows.Add(new object[] { dt_t2.Rows[i][1], dt_t2.Rows[i][2] });
                    }
                    //DataTable dt_t3 = DBAdo.DtFillSql("SELECt XSHTH, (SELECT WXSTATE FROM [ACONTRACT] WHERE HCODE = '" + dgvr.Cells["合同号"].Value.ToString() + "') AS 'WXSTATE' FROM AWX WHERE WXHTH ='" + dgvr.Cells["合同号"].Value.ToString() + "'");
                    DataTable dt_t3 = DBAdo.DtFillSql(string.Format("SELECt XSHTH,T1.WXState FROM AWX T0 LEFT JOIN [ACONTRACT] T1 ON T0.XSHTH=T1.HCODE WHERE T0.WXHTH ='{0}'", dgvr.Cells["合同号"].Value.ToString()));

                    for (int i = 0; i < dt_t3.Rows.Count; i++)
                    {
                        //dt_wx.Rows.Add(new object[] { dt_t3.Rows[i][2] });
                        this.dataGridView4.Rows.Add(new object[] { dt_t3.Rows[i][0].ToString(), dt_t3.Rows[i][1].ToString() });
                    }
                    //dt_sp = DBAdo.DtFillSql("");
                    //dt_tk = DBAdo.DtFillSql("");
                    //dt_wx = DBAdo.DtFillSql("");
                    this.dataGridView1.AutoResizeColumns();
                    this.dataGridView2.AutoResizeColumns();


                    DataTable dt_t4 = DBAdo.DtFillSql("SELECT * FROM VCONTRACTS WHERE 合同号='" + dgvr.Cells["合同号"].Value.ToString() + "'");


                    this.button10.Text = dt_t4.Rows[0]["标号"].ToString();
                    this.hKHTextBox.Tag = dt_t4.Rows[0]["HKH"].ToString();
                    this.hKHTextBox.Text = dt_t4.Rows[0]["客户名"].ToString();
                    this.hDWTextBox.Tag = dt_t4.Rows[0]["HDW"].ToString();
                    this.hDWTextBox.Text = ClassConstant.DW_NAME;
                    this.hCODETextBox.Text = dt_t4.Rows[0]["合同号"].ToString();
                    this.hAREATextBox.Tag = dt_t4.Rows[0]["HAREA"].ToString();
                    this.hAREATextBox.Text = dt_t4.Rows[0]["地区"].ToString();
                    this.hDATEDateTimePicker.Value = DateTime.Parse(dt_t4.Rows[0]["签定日期"].ToString());
                    this.hYWYTextBox.Tag = dt_t4.Rows[0]["HYWY"].ToString();
                    this.hYWYTextBox.Text = dt_t4.Rows[0]["业务员"].ToString();
                    this.hXMTextBox.Text = dt_t4.Rows[0]["项目名称"].ToString();
                    this.hZBJTextBox.Text = dt_t4.Rows[0]["质保金"].ToString();
                    this.hHSCheckBox.Checked = bool.Parse(dt_t4.Rows[0]["含税"].ToString());
                    this.hHSBLTextBox.Text = dt_t4.Rows[0]["比例"].ToString();
                    this.hHTJETextBox.Text = dt_t4.Rows[0]["合同金额"].ToString();
                    this.hJSJETextBox.Text = dt_t4.Rows[0]["结算金额"].ToString();
                    this.hLXTextBox.Tag = dt_t4.Rows[0]["HLX"].ToString();
                    this.hLXTextBox.Text = dt_t4.Rows[0]["合同类型"].ToString();
                    this.HBZrichTextBox.Text = dt_t4.Rows[0]["合同备注"].ToString();
                    this.HZTComboBox.Text = dt_t4.Rows[0]["状态"].ToString();
                    this.hJHDATEDateTimePicker.Value = DateTime.Parse(dt_t4.Rows[0]["交货日期"].ToString());
                    this.hUSERTextBox.Text = dt_t4.Rows[0]["操作员"].ToString();
                    this.textBox1.Text = dt_t4.Rows[0]["代理费"].ToString();
                    this.textBox2.Text = dt_t4.Rows[0]["选型费"].ToString();
                    this.textBox3.Text = dt_t4.Rows[0]["标书费"].ToString();
                    this.dataGridView6.Rows[7].Cells[1].Value = dt_t4.Rows[0]["银行名称"].ToString();//银行名
                    this.dataGridView6.Rows[8].Cells[1].Value = dt_t4.Rows[0]["账号"].ToString();//账号
                    this.dataGridView5.Rows[3].Cells[1].Value = dt_t4.Rows[0]["对方委托人"].ToString();//委托人1
                    this.dataGridView6.Rows[3].Cells[1].Value = dt_t4.Rows[0]["己方委托人"].ToString();//委托人2
                    this.HYFTextBox.Text = dt_t4.Rows[0]["运费"].ToString();
                    this.HqtfyTextBox.Text = dt_t4.Rows[0]["其它费用"].ToString();

                    this.checkBox1.Checked = bool.Parse(dt_t4.Rows[0]["flag"].ToString());
                    this.comboBox7.Text = dt_t4.Rows[0]["中标方式"].ToString();
                    this.dateTimePicker1.Value = DateTime.Parse(dt_t4.Rows[0]["签订日期"].ToString() == "" ? "2010-1-1" : dt_t4.Rows[0]["签订日期"].ToString());

                    this.dataGridView3.Rows[0].Cells[1].Value = dt_t4.Rows[0]["部门主管"].ToString();
                    this.dataGridView3.Rows[1].Cells[1].Value = dt_t4.Rows[0]["财务主管"].ToString();
                    this.dataGridView3.Rows[2].Cells[1].Value = dt_t4.Rows[0]["公司主管"].ToString();
                    this.dataGridView3.Rows[3].Cells[1].Value = dt_t4.Rows[0]["审计主管"].ToString();

                    this.dataGridView3.Rows[0].Cells[2].Value = dt_t4.Rows[0]["部门意见"].ToString();
                    this.dataGridView3.Rows[1].Cells[2].Value = dt_t4.Rows[0]["财务意见"].ToString();
                    this.dataGridView3.Rows[2].Cells[2].Value = dt_t4.Rows[0]["公司意见"].ToString();
                    this.dataGridView3.Rows[3].Cells[2].Value = dt_t4.Rows[0]["审计意见"].ToString();

                    this.dataGridView7.Rows[0].Cells[1].Value = dt_t4.Rows[0]["MACODE"].ToString();
                    this.dataGridView7.Rows[1].Cells[1].Value = dt_t4.Rows[0]["MBCODE"].ToString();
                    this.dataGridView7.Rows[2].Cells[1].Value = dt_t4.Rows[0]["MCCODE"].ToString();
                    this.dataGridView7.Rows[3].Cells[1].Value = dt_t4.Rows[0]["MDCODE"].ToString();
                    this.dataGridView7.Rows[4].Cells[1].Value = dt_t4.Rows[0]["MECODE"].ToString();
                    this.dataGridView7.Rows[5].Cells[1].Value = dt_t4.Rows[0]["MFCODE"].ToString();
                    this.dataGridView7.Rows[6].Cells[1].Value = dt_t4.Rows[0]["MGCODE"].ToString();

                    DataTable dt_t5 = DBAdo.DtFillSql("SELECT * FROM ACLIENTS WHERE CCODE = '" + this.hKHTextBox.Tag + "'");
                    this.dataGridView5.Rows[0].Cells[1].Value = dt_t5.Rows[0]["CNAME"].ToString();
                    this.dataGridView5.Rows[1].Cells[1].Value = dt_t5.Rows[0]["CADDRESS"].ToString();
                    this.dataGridView5.Rows[2].Cells[1].Value = dt_t5.Rows[0]["CFR"].ToString();
                    //this.dataGridView5.Rows[3].Cells[1].Value = dt_t5.Rows[0][""].ToString();
                    this.dataGridView5.Rows[4].Cells[1].Value = dt_t5.Rows[0]["CTEL"].ToString();
                    this.dataGridView5.Rows[5].Cells[1].Value = dt_t5.Rows[0]["CFAXNO"].ToString();
                    this.dataGridView5.Rows[6].Cells[1].Value = dt_t5.Rows[0]["CFPTEL"].ToString();
                    this.dataGridView5.Rows[7].Cells[1].Value = dt_t5.Rows[0]["CBANKNAME"].ToString();
                    this.dataGridView5.Rows[8].Cells[1].Value = dt_t5.Rows[0]["CACCOUNT"].ToString();
                    this.dataGridView5.Rows[9].Cells[1].Value = dt_t5.Rows[0]["CSH"].ToString();
                    this.dataGridView5.Rows[10].Cells[1].Value = dt_t5.Rows[0]["CPOSNO"].ToString();

                    DataTable dt_t6 = DBAdo.DtFillSql("SELECT * FROM ACLIENTS WHERE CCODE = '" + this.hDWTextBox.Tag + "'");
                    //this.hDWTextBox.Text = dt_t6.Rows[0]["CNAME"].ToString();
                    this.dataGridView6.Rows[0].Cells[1].Value = dt_t6.Rows[0]["CNAME"].ToString();
                    this.dataGridView6.Rows[1].Cells[1].Value = dt_t6.Rows[0]["CADDRESS"].ToString();
                    this.dataGridView6.Rows[2].Cells[1].Value = dt_t6.Rows[0]["CFR"].ToString();
                    //this.dataGridView5.Rows[3].Cells[1].Value = dt_t6.Rows[0][""].ToString();
                    this.dataGridView6.Rows[4].Cells[1].Value = dt_t6.Rows[0]["CTEL"].ToString();
                    this.dataGridView6.Rows[5].Cells[1].Value = dt_t6.Rows[0]["CFAXNO"].ToString();
                    this.dataGridView6.Rows[6].Cells[1].Value = dt_t6.Rows[0]["CFPTEL"].ToString();
                    //this.dataGridView6.Rows[7].Cells[1].Value = dt_t6.Rows[0]["CBANKNAME"].ToString();
                    //this.dataGridView6.Rows[8].Cells[1].Value = dt_t6.Rows[0]["CACCOUNT"].ToString();
                    this.dataGridView6.Rows[9].Cells[1].Value = dt_t6.Rows[0]["CSH"].ToString();
                    this.dataGridView6.Rows[10].Cells[1].Value = dt_t6.Rows[0]["CPOSNO"].ToString();
                    this.comboBox1.Items.Clear();

                    if (htype != "03")
                    {
                        this.button8.Enabled = false;
                        this.button9.Enabled = false;
                    }
                    else
                    {
                        this.button8.Enabled = true;
                        this.button9.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }
        }

        private void FZXX()
        {


            this.dataGridView3.Rows.Add(new object[] { "部门主管", null });
            this.dataGridView3.Rows.Add(new object[] { "财务主管", null });
            this.dataGridView3.Rows.Add(new object[] { "公司主管", null });
            this.dataGridView3.Rows.Add(new object[] { "审计主管", null });
            //this.dataGridView3.Columns[0].ReadOnly = true;

            this.dataGridView5.Rows.Add(new object[] { "客户名", null });
            this.dataGridView5.Rows.Add(new object[] { "住所", null });
            this.dataGridView5.Rows.Add(new object[] { "法定代表人", null });
            this.dataGridView5.Rows.Add(new object[] { "委托代理人", null });
            this.dataGridView5.Rows.Add(new object[] { "电话", null });
            this.dataGridView5.Rows.Add(new object[] { "传真", null });
            this.dataGridView5.Rows.Add(new object[] { "开发票电话", null });
            this.dataGridView5.Rows.Add(new object[] { "开户银行", null });
            this.dataGridView5.Rows.Add(new object[] { "账号", null });
            this.dataGridView5.Rows.Add(new object[] { "税号", null });
            this.dataGridView5.Rows.Add(new object[] { "邮编", null });
            //foreach (DataGridViewRow r in this.dataGridView5.Rows)
            //{
            //    r.Cells[0].ReadOnly = true;
            //    r.Cells[1].ReadOnly = true;
            //}
            //this.dataGridView5.Rows[3].Cells[1].ReadOnly = false;

            this.dataGridView6.Rows.Add(new object[] { "公司名", null });
            this.dataGridView6.Rows.Add(new object[] { "住所", null });
            this.dataGridView6.Rows.Add(new object[] { "法定代表人", null });
            this.dataGridView6.Rows.Add(new object[] { "委托代理人", null });
            this.dataGridView6.Rows.Add(new object[] { "电话", null });
            this.dataGridView6.Rows.Add(new object[] { "传真", null });
            this.dataGridView6.Rows.Add(new object[] { "开发票电话", null });
            this.dataGridView6.Rows.Add(new object[] { "开户银行", null });
            this.dataGridView6.Rows.Add(new object[] { "账号", null });
            this.dataGridView6.Rows.Add(new object[] { "税号", null });
            this.dataGridView6.Rows.Add(new object[] { "邮编", null });
            //foreach (DataGridViewRow r1 in this.dataGridView6.Rows)
            //{
            //    r1.Cells[0].ReadOnly = true;
            //    r1.Cells[1].ReadOnly = true;
            //}
            //this.dataGridView6.Rows[3].Cells[1].ReadOnly = false;


            this.dataGridView7.Rows.Add(new object[] { "其它信息A", null });
            this.dataGridView7.Rows.Add(new object[] { "其它信息B", null });
            this.dataGridView7.Rows.Add(new object[] { "其它信息C", null });
            this.dataGridView7.Rows.Add(new object[] { "其它信息D", null });
            this.dataGridView7.Rows.Add(new object[] { "其它信息E", null });
            this.dataGridView7.Rows.Add(new object[] { "其它信息F", null });
            this.dataGridView7.Rows.Add(new object[] { "其它信息G", null });
            //this.dataGridView7.Columns[0].ReadOnly = true;
        }

        private void DT()
        {
            try
            {
                //dt_wx.Columns.Add("销售合同号/批号", typeof(string));
                ////this.dataGridView4.DataSource = dt_wx;

                //dt_sp.Columns.Add("序号", typeof(int));
                //dt_sp.Columns.Add("商品名称", typeof(string));
                //dt_sp.Columns.Add("材质", typeof(string));
                //dt_sp.Columns.Add("型号", typeof(string));
                //dt_sp.Columns.Add("数量", typeof(decimal));
                //dt_sp.Columns.Add("数量单位", typeof(string));
                //dt_sp.Columns.Add("单价", typeof(decimal));
                //dt_sp.Columns.Add("总价", typeof(decimal), "数量*单价");
                //dt_sp.Columns.Add("重量单位", typeof(string));
                //dt_sp.Columns.Add(@"净/毛", typeof(string));
                //dt_sp.Columns.Add("单重", typeof(decimal));
                //dt_sp.Columns.Add("总重", typeof(decimal), "单重*数量");
                //dt_sp.Columns.Add("备注", typeof(string));
                //dt_sp.Columns.Add("图号", typeof(string));
                //dt_sp.Columns.Add("ID", typeof(int));
                //this.dataGridView1.DataSource = dt_sp;
                //this.dataGridView1.Columns["重量单位"].Visible = false;
                //this.dataGridView1.Columns["单重"].Visible = false;
                //this.dataGridView1.Columns["总重"].Visible = false;
                //this.dataGridView1.Columns["数量单位"].HeaderText = "单位";




                dt_tk.Columns.Add("条款", typeof(string));
                dt_tk.Columns.Add("序号", typeof(int));
                this.dataGridView2.DataSource = dt_tk;

                dt_c3 = DBAdo.DtFillSql("SELECT NAME FROM APS WHERE BM='部门主管' AND BCODE='" + ClassConstant.DW_ID + "'");
                dt_c4 = DBAdo.DtFillSql("SELECT NAME FROM APS WHERE BM='财务主管' AND BCODE='" + ClassConstant.DW_ID + "'");
                dt_c5 = DBAdo.DtFillSql("SELECT NAME FROM APS WHERE BM='公司主管' AND BCODE='" + ClassConstant.DW_ID + "'");
                dt_c6 = DBAdo.DtFillSql("SELECT NAME FROM APS WHERE BM='审计主管' AND BCODE='" + ClassConstant.DW_ID + "'");
                foreach (DataRow r in dt_c3.Rows)
                {
                    if (r[0].ToString() != "")
                        this.comboBox3.Items.Add(r[0].ToString());
                }
                foreach (DataRow r in dt_c4.Rows)
                {
                    if (r[0].ToString() != "")
                        this.comboBox4.Items.Add(r[0].ToString());
                }
                foreach (DataRow r in dt_c5.Rows)
                {
                    if (r[0].ToString() != "")
                        this.comboBox5.Items.Add(r[0].ToString());
                }
                foreach (DataRow r in dt_c6.Rows)
                {
                    if (r[0].ToString() != "")
                        this.comboBox6.Items.Add(r[0].ToString());
                }

                DataTable souce = DBAdo.DtFillSql("SELECT HCODE FROM ACONTRACT WHERE HLX LIKE '02%' and hdw = '" + ClassConstant.DW_ID + "' order by hcode");
                this.dataGridView8.DataSource = souce;
                this.dataGridView8.Columns[0].HeaderText = "待选销售合同号";
                this.dataGridView8.Columns[0].Width = 180;

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void Reg()
        {
            OnButtonClick check = (object sender, EventArgs e) =>
            {
                if (DialogResult.Yes == MessageBox.Show("确定选中合同录入完整？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    DBAdo.ExecuteNonQuerySql(string.Format("update acontract set flag =1 where hcode ='{0}'", this.hCODETextBox.Text));
                    if (this.ht != null)
                    {
                        ht.DataLoad();
                    }
                    this.Close();
                }
            };
            OnButtonClick uncheck = (object sender, EventArgs e) =>
            {
                if (DialogResult.Yes == MessageBox.Show("确定选中为不完整合同？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    DBAdo.ExecuteNonQuerySql(string.Format("update acontract set flag =0 where hcode ='{0}'", this.hCODETextBox.Text));
                    if (this.ht != null)
                    {
                        ht.DataLoad();
                    }
                    this.Close();
                }
            };
            bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    //new ToolStripSeparator(),
                    ////new Factory_ToolBtn("明细条款","明细条款",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("商品明细", "商品明细",ClassCustom.getImage("sp.png"), this.btn_sp,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("  意见  ", "  意见  ",ClassCustom.getImage("yj.png"), this.btn_yj,null,true).TBtnProduce(),
                    //new Factory_ToolBtn("客户信息", "客户信息",ClassCustom.getImage("khxx.png"), this.btn_kh,null,true).TBtnProduce(),
                    new Factory_ToolBtn("合同审批", "合同审批",ClassCustom.getImage("t1.png"), check,null,(ClassConstant.BNAME == "财务部" || ClassConstant.BNAME == "办公室"?true:false)).TBtnProduce(),
                    new Factory_ToolBtn("合同取消审批", "合同取消审批",ClassCustom.getImage("NetByte Design Studio - 0126.png"), uncheck,null,(ClassConstant.BNAME == "财务部" || ClassConstant.BNAME == "办公室"?true:false)).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };
            #region old
            //bts = new ToolStripItem[]{
            //        new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        //new Factory_ToolBtn("外协明细","外协明细",ClassCustom.getImage("wx.png"),this.btn_wx,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("明细条款","合同明细",ClassCustom.getImage("zj.png"),this.btn_mx,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("商品明细", "商品明细",ClassCustom.getImage("sp.png"), this.btn_sp,null,true).TBtnProduce(),
            //        //new Factory_ToolBtn("  意见  ", "  意见  ",ClassCustom.getImage("yj.png"), this.btn_yj,null,true).TBtnProduce(),
            //        //new Factory_ToolBtn("客户信息", "客户信息",ClassCustom.getImage("khxx.png"), this.btn_kh,null,true).TBtnProduce(),
            //        //new Factory_ToolBtn("公司信息", "公司信息",ClassCustom.getImage("gs.png"), this.btn_gs,null,true).TBtnProduce(),
            //        new ToolStripSeparator(),
            //        new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
            //        new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
            //        };
            #endregion
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_KH.Visible)
                {
                    this.FZ_KH.Visible = false;
                    return;
                }

                this.FZ_KH.Size = new Size(300, 350);
                this.FZ_KH.Location = new Point(this.hKHTextBox.Location.X, this.hKHTextBox.Location.Y + 35);
                this.FZ_KH.Visible = true;
                this.treeView5.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE LEN(CCODE)=" + ClassConstant.GetLeveLChar("LEVEL_KH", 0).Length + "  ORDER BY CCODE");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE (SUBSTRING(CCODE,3,2) LIKE '" + (ClassConstant.USER_ID == "0101999999" ? "__" : ClassConstant.DW_ID.Substring(2)) + "' OR CCODE LIKE '01%' OR CCODE LIKE '11%'  OR CCODE LIKE '05%' ) AND  CCODE LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 1) + "'  ORDER BY CCODE");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.treeView5.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Tag = dr1[0].ToString();
                        tn1.Name = dr1[0].ToString();
                        this.treeView5.Nodes[tn.Name].Nodes.Add(tn1);
                        DataTable dt3 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE CCODE LIKE'" + dr1[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 2) + "'  ORDER BY CNAME");
                        foreach (DataRow dr2 in dt3.Rows)
                        {
                            TreeNode tn2 = new TreeNode(dr2[1].ToString());
                            tn2.Tag = dr2[0].ToString();
                            tn2.Name = dr2[0].ToString();
                            this.treeView5.Nodes[tn.Name].Nodes[tn1.Name].Nodes.Add(tn2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_DW.Visible)
                {
                    this.FZ_DW.Visible = false;
                    return;
                }
                this.FZ_DW.Size = new Size(300, 350);
                this.FZ_DW.Location = new Point(this.hDWTextBox.Location.X, this.hDWTextBox.Location.Y + 25);
                this.FZ_DW.Visible = true;
                this.FZ_DW.Nodes.Clear();
                //DataTable dt1 = DBAdo.DtFillSql("SELECT YCODE,YNAME FROM AYWY WHERE LEN(YCODE)=" + ClassConstant.GetLeveLChar("LEVEL_YWY", 0).Length + "  ORDER BY YCODE");
                DataTable dt1 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE CCODE like '01__' ORDER BY CCODE");
                foreach (DataRow dr in dt1.Rows)
                {
                    //DataTable dt2 = DBAdo.DtFillSql("SELECT YCODE,YNAME FROM AYWY WHERE YCODE LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_YWY", 1) + "'  ORDER BY YCODE");
                    //DataTable dt2 = DBAdo.DtFillSql("SELECT CCODE,CNAME FROM ACLIENTS WHERE CCODE LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_KH", 1) + "'  ORDER BY CCODE");

                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.FZ_DW.Nodes.Add(tn);
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_YWY.Visible)
                {
                    this.FZ_YWY.Visible = false;
                    return;
                }
                this.FZ_YWY.Size = new Size(200, 350);
                this.FZ_YWY.Location = new Point(this.hYWYTextBox.Location.X, this.hYWYTextBox.Location.Y + 35);
                this.FZ_YWY.Visible = true;
                this.FZ_YWY.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT YCODE,YNAME FROM AYWY WHERE YCODE LIKE '" + ClassConstant.DW_ID + "%' AND LEN(YCODE)=" + ClassConstant.GetLeveLChar("LEVEL_YWY", 0).Length + "  ORDER BY YCODE");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT YCODE,YNAME FROM AYWY WHERE YCODE LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_YWY", 1) + "'  ORDER BY YCODE");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.FZ_YWY.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Tag = dr1[0].ToString();
                        tn1.Name = dr1[0].ToString();
                        this.FZ_YWY.Nodes[tn.Name].Nodes.Add(tn1);
                        DataTable dt3 = DBAdo.DtFillSql("SELECT YCODE,YNAME FROM AYWY WHERE YCODE LIKE'" + dr1[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_YWY", 2) + "'  ORDER BY YCODE");
                        foreach (DataRow dr2 in dt3.Rows)
                        {
                            TreeNode tn2 = new TreeNode(dr2[1].ToString());
                            tn2.Tag = dr2[0].ToString();
                            tn2.Name = dr2[0].ToString();
                            this.FZ_YWY.Nodes[tn.Name].Nodes[tn1.Name].Nodes.Add(tn2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_DQ.Visible)
                {
                    this.FZ_DQ.Visible = false;
                    return;
                }
                this.FZ_DQ.Size = new Size(200, 350);
                this.FZ_DQ.Location = new Point(this.hAREATextBox.Location.X, this.hAREATextBox.Location.Y + 35);
                this.FZ_DQ.Visible = true;
                this.FZ_DQ.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT * FROM AREAS WHERE LEN(ACODE)=1  ORDER BY ACODE");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT * FROM AREAS WHERE ACODE LIKE'" + dr[0].ToString() + "__'  ORDER BY ACODE");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Tag = dr[0].ToString();
                    tn.Name = dr[0].ToString();
                    this.FZ_DQ.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Tag = dr1[0].ToString();
                        tn1.Name = dr1[0].ToString();
                        this.FZ_DQ.Nodes[tn.Name].Nodes.Add(tn1);
                        DataTable dt3 = DBAdo.DtFillSql("SELECT * FROM AREAS WHERE ACODE LIKE'" + dr1[0].ToString() + "___'  ORDER BY ACODE");
                        foreach (DataRow dr2 in dt3.Rows)
                        {
                            TreeNode tn2 = new TreeNode(dr2[1].ToString());
                            tn2.Tag = dr2[0].ToString();
                            tn2.Name = dr2[0].ToString();
                            this.FZ_DQ.Nodes[tn.Name].Nodes[tn1.Name].Nodes.Add(tn2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_HTLX.Visible)
                {
                    this.FZ_HTLX.Visible = false;
                    return;
                }
                this.FZ_HTLX.Size = new Size(200, 350);
                this.FZ_HTLX.Location = new Point(this.hLXTextBox.Location.X, this.hLXTextBox.Location.Y + 35);
                this.FZ_HTLX.Visible = true;
                this.FZ_HTLX.Nodes.Clear();
                DataTable dt1 = DBAdo.DtFillSql("SELECT * FROM ALX WHERE LEN(LID)=" + ClassConstant.GetLeveLChar("LEVEL_HTLX", 0).Length + " ORDER BY LID");
                foreach (DataRow dr in dt1.Rows)
                {
                    DataTable dt2 = DBAdo.DtFillSql("SELECT * FROM ALX WHERE LID LIKE'" + dr[0].ToString() + ClassConstant.GetLeveLChar("LEVEL_HTLX", 1) + "'  ORDER BY LID");
                    TreeNode tn = new TreeNode(dr[1].ToString());
                    tn.Name = dr[0].ToString();
                    tn.Tag = dr[0].ToString();
                    this.FZ_HTLX.Nodes.Add(tn);
                    foreach (DataRow dr1 in dt2.Rows)
                    {
                        TreeNode tn1 = new TreeNode(dr1[1].ToString());
                        tn1.Name = dr1[0].ToString();
                        tn1.Tag = dr1[0].ToString();
                        this.FZ_HTLX.Nodes[tn.Name].Nodes.Add(tn1);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_FZ_KH)))
                return;
            A_FZ_KH cm = new A_FZ_KH(this);
            cm.MdiParent = this.MdiParent;
            cm.Show();
            this.FZ_KH.Visible = false;
        }

        private void 修改商品信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1_CellDoubleClick(null, null);
        }

        private void 删除商品信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageView.MessageYesNoShow("确定删除商品[" + this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "][" + this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "]的信息?"))
            {
                foreach (DataRow dr in this.dt_sp.Rows)
                {
                    if (dr[0].ToString() == this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString())
                    {
                        if (dr["ID"].ToString() != "")
                        {
                            LdeL.Add(int.Parse(dr["ID"].ToString()));
                        }
                        dt_sp.Rows.Remove(dr);
                        break;
                    }
                }
                for (int i = 0; i < dt_sp.Rows.Count; i++)
                {
                    dt_sp.Rows[i][0] = i + 1;
                }
                decimal d = 0;
                foreach (DataRow dr in dt_sp.Rows)
                {
                    d += decimal.Parse(dr["总价"].ToString());
                }
                decimal je = decimal.Round(d, 2);
                this.JE.Text = d.ToString();
                this.RMB.Text = ClassCustom.UpMoney(je);
                this.hJSJETextBox.Text = d.ToString();
                //this.dataGridView1.DataSource = dt_sp;
                this.dataGridView1.AutoResizeColumns();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.hCODETextBox.Text == "")
            {
                MessageBox.Show("请先输入合同号");
                return;
            }
            A_HT_SP_OP sp = new A_HT_SP_OP(this.hCODETextBox.Text, 3, this, this.dataGridView1.SelectedRows[0]);
            sp.MdiParent = this.MdiParent;
            sp.Show();
        }

        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
            {
                e.Handled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            A_CLIENT sp = new A_CLIENT();
            sp.MdiParent = this.MdiParent;
            sp.Show();
            this.FZ_KH.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comboBox2.Items.Clear();
                DataTable zh = DBAdo.DtFillSql("SELECT DISTINCT YZH FROM ABANK WHERE BCODE ='" + this.hDWTextBox.Tag.ToString() + "' AND YNAME = '" + this.comboBox1.Text + "'");
                foreach (DataRow r in zh.Rows)
                {
                    if (r[0].ToString() != "")
                        this.comboBox2.Items.Add(r[0].ToString());
                }
                this.dataGridView6.Rows[7].Cells[1].Value = this.comboBox1.Text;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView6.Rows[8].Cells[1].Value = this.comboBox2.Text;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        #region 按钮事件

        private void btn_sp(object sender, EventArgs e)
        {
            if (this.hCODETextBox.Text == "")
            {
                MessageBox.Show("请先输入合同号");
                return;
            }
            A_HT_SP_OP sp = new A_HT_SP_OP(this.hCODETextBox.Text, 1, this);
            sp.MdiParent = this.MdiParent;
            sp.Show();
            //this.tabControl1.SelectTab(0);
        }

        private void btn_wx(object sender, EventArgs e)
        {
            if (this.hCODETextBox.Text == "")
            {
                MessageBox.Show("请先输入合同号");
                return;
            }
            A_HT_WX sp = new A_HT_WX(1, this);
            sp.MdiParent = this.MdiParent;
            sp.Show();
            //this.tabControl1.SelectTab(3);
        }

        private void btn_yj(object sender, EventArgs e)
        {
            //this.tabControl1.SelectTab(2);
        }

        private void btn_kh(object sender, EventArgs e)
        {
            //this.tabControl1.SelectTab(4);
        }

        private void btn_gs(object sender, EventArgs e)
        {
            //this.tabControl1.SelectTab(5);
        }

        private void btn_mx(object sender, EventArgs e)
        {
            if (this.hCODETextBox.Text == "")
            {
                MessageBox.Show("请先输入合同号");
                return;
            }
            A_HT_TK tk = new A_HT_TK(this.hCODETextBox.Text, this, 1);
            tk.MdiParent = this.MdiParent;
            tk.Show();
            //this.tabControl1.SelectTab(1);
        }

        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                this.Validate(false);
                bool _mark = false;
                foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                {
                    if (r.Cells["总价"].Value == null)
                    {
                        _mark = true;
                        break;
                    }
                }
                if (_mark)
                {
                    MessageBox.Show("商品信息不完整！");
                    return;
                }
                string message = "";
                if (this.hCODETextBox.Text == "" || this.hJSJETextBox.Text == "" || this.hHTJETextBox.Text == "" || this.hLXTextBox.Text == "" || this.hDWTextBox.Text == "" || this.hKHTextBox.Text == "" || this.hYWYTextBox.Text == "" || this.comboBox7.Text == "" || this.button10.Text == "标号")
                {


                    if (this.hCODETextBox.Text == "") { message += "合同号不能为空\r"; }
                    if (this.hHTJETextBox.Text == "") { message += "合同金额不能为空\r"; }
                    if (this.hLXTextBox.Text == "") { message += "合同类型不能为空\r"; }
                    if (this.hYWYTextBox.Text == "") { message += "业务员不能为空\r"; }
                    if (this.hJSJETextBox.Text == "") { message += "结算金额不能为空\r"; }
                    if (this.hKHTextBox.Text == "") { message += "客户不能为空\r"; }
                    if (this.comboBox7.Text == "") { message += "中标方式不能为空\r"; }
                    if (this.hHSBLTextBox.Text == "") { message += "含税比例不能为空\r"; }
                    if (this.button10.Text == "标号") { message += "标号不能为空\r"; }
                }
                if (this.hLXTextBox.Tag.ToString().IndexOf("03") > -1)
                {

                    foreach (DataGridViewRow r in this.dataGridView4.Rows)
                    {
                        if (r.Cells[1].Value == null || r.Cells[1].Value.ToString() == "")
                        {
                            message += "销售合同外委状态不能为空\r";
                            break;
                        }
                    }
                }
                if (message != "")
                {
                    MessageBox.Show(message, "合同信息录入不完整", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (decimal.Parse(this.hJSJETextBox.Text) == 0)
                {
                    MessageBox.Show("结算金额不能为0");
                    return;
                }
                if (this.hLXTextBox.Tag != null)
                {
                    if (this.hLXTextBox.Tag.ToString() == "0301")
                    {
                        if (this.dataGridView4.Rows.Count <= 0)
                        {
                            //if (this.dataGridView4.Rows[0].IsNewRow)
                            MessageBox.Show("外委合同销售合同号不能为空");
                            return;
                        }
                        else
                        {
                            if (this.dataGridView4.Rows[0].Cells[0].Value == null || this.dataGridView4.Rows[0].Cells[1].Value == null)
                            {
                                MessageBox.Show("销售合同外委状态不能为空");
                            }
                        }
                    }
                }
                if (op == 1)
                {
                    #region 判断是否该月结账
                    string tsql = string.Format("SELECT flag FROM AMONTH WHERE [YEAR] ={0} AND [MONTH]={1} AND HDW = {2}", this.hDATEDateTimePicker.Value.Year.ToString(), this.hDATEDateTimePicker.Value.Month.ToString(), ClassConstant.DW_ID);
                    object result = DBAdo.ExecuteScalarSql(tsql);
                    if (bool.Parse(result == null ? false.ToString() : result.ToString()))
                    {
                        MessageBox.Show("本月已结账不能添加进度信息", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    #endregion
                    string sql = "INSERT INTO [ACONTRACT]( [HKH], [HDW], [HCODE], [HAREA], [HDATE], [HYWY], [HXM], [HZBJ], [HHS], [HHSBL], [HHTJE], [HJSJE], [HLX], [HMEMO], [HZT], [HJHDATE], [HUSER], [HYHMC], [HYHZH], [HWTR1], [HWTR2], [HYF], [HQTFY],"
                               + " [FLAG], [PS1], [PS2], [PS3], [PS4], [YJ1], [YJ2], [YJ3], [YJ4], [MACODE], [MBCODE], [MCCODE], [MDCODE], [MECODE], [MFCODE], [MGCODE],[DLF],[XXF],[BSF],[QDRQ],[zbfs],[BIDCODE])VALUES("
                               + "'" + (this.hKHTextBox.Tag == null ? "" : this.hKHTextBox.Tag.ToString()) + "',"
                               + "'" + (this.hDWTextBox.Tag == null ? "" : this.hDWTextBox.Tag.ToString()) + "',"
                               + "'" + this.hCODETextBox.Text + "',"
                               + "'" + (this.hAREATextBox.Tag == null ? "" : this.hAREATextBox.Tag.ToString()) + "',"
                               + "'" + this.hDATEDateTimePicker.Value.ToString("yyyy-MM-dd") + "',"
                               + "'" + (this.hYWYTextBox.Tag == null ? "" : this.hYWYTextBox.Tag.ToString()) + "',"
                               + "'" + this.hXMTextBox.Text + "',"
                               + "'" + (this.hZBJTextBox.Text == "" ? "0" : this.hZBJTextBox.Text) + "',"
                               + "'" + (this.hHSCheckBox.Checked ? 1 : 0) + "',"
                               + "'" + (this.hHSBLTextBox.Text == "" ? "0" : this.hHSBLTextBox.Text) + "',"
                               + "'" + (this.hHTJETextBox.Text == "" ? "0" : this.hHTJETextBox.Text) + "',"
                               + "'" + (this.hJSJETextBox.Text == "" ? "0" : this.hJSJETextBox.Text) + "',"
                               + "'" + (this.hLXTextBox.Tag == null ? "" : this.hLXTextBox.Tag.ToString()) + "',"
                               + "'" + this.HBZrichTextBox.Text + "',"
                               + "'" + this.HZTComboBox.Text + "',"
                               + "'" + this.hJHDATEDateTimePicker.Value.ToString("yyyy-MM-dd") + "',"
                               + "'" + this.hUSERTextBox.Text + "',"
                               + "'" + (this.dataGridView6.Rows[7].Cells[1].Value == null ? "" : this.dataGridView6.Rows[7].Cells[1].Value.ToString()) + "',"//银行名
                               + "'" + (this.dataGridView6.Rows[8].Cells[1].Value == null ? "" : this.dataGridView6.Rows[8].Cells[1].Value.ToString()) + "',"//账号
                               + "'" + (this.dataGridView5.Rows[3].Cells[1].Value == null ? "" : this.dataGridView5.Rows[3].Cells[1].Value.ToString()) + "',"//委托人1
                               + "'" + (this.dataGridView6.Rows[3].Cells[1].Value == null ? "" : this.dataGridView6.Rows[3].Cells[1].Value.ToString()) + "',"//委托人2
                               + "'" + (this.HYFTextBox.Text == "" ? "0" : this.HYFTextBox.Text) + "',"
                               + "'" + (this.HqtfyTextBox.Text == "" ? "0" : this.HqtfyTextBox.Text) + "',"
                               + "'" + (this.checkBox1.Checked ? "1" : "0") + "',"
                               + "'" + (this.dataGridView3.Rows[0].Cells[1].Value == null ? "" : this.dataGridView3.Rows[0].Cells[1].Value.ToString()) + "',"
                               + "'" + (this.dataGridView3.Rows[1].Cells[1].Value == null ? "" : this.dataGridView3.Rows[1].Cells[1].Value.ToString()) + "',"
                               + "'" + (this.dataGridView3.Rows[2].Cells[1].Value == null ? "" : this.dataGridView3.Rows[2].Cells[1].Value.ToString()) + "',"
                               + "'" + (this.dataGridView3.Rows[3].Cells[1].Value == null ? "" : this.dataGridView3.Rows[3].Cells[1].Value.ToString()) + "',"

                               + "'" + (this.dataGridView3.Rows[0].Cells[2].Value == null ? "" : this.dataGridView3.Rows[0].Cells[2].Value.ToString()) + "',"
                               + "'" + (this.dataGridView3.Rows[1].Cells[2].Value == null ? "" : this.dataGridView3.Rows[1].Cells[2].Value.ToString()) + "',"
                               + "'" + (this.dataGridView3.Rows[2].Cells[2].Value == null ? "" : this.dataGridView3.Rows[2].Cells[2].Value.ToString()) + "',"
                               + "'" + (this.dataGridView3.Rows[3].Cells[2].Value == null ? "" : this.dataGridView3.Rows[3].Cells[2].Value.ToString()) + "',"

                                + "'" + (this.dataGridView7.Rows[0].Cells[1].Value == null ? "" : this.dataGridView7.Rows[0].Cells[1].Value.ToString()) + "',"
                                + "'" + (this.dataGridView7.Rows[1].Cells[1].Value == null ? "" : this.dataGridView7.Rows[1].Cells[1].Value.ToString()) + "',"
                                + "'" + (this.dataGridView7.Rows[2].Cells[1].Value == null ? "" : this.dataGridView7.Rows[2].Cells[1].Value.ToString()) + "',"
                                + "'" + (this.dataGridView7.Rows[3].Cells[1].Value == null ? "" : this.dataGridView7.Rows[3].Cells[1].Value.ToString()) + "',"
                                + "'" + (this.dataGridView7.Rows[4].Cells[1].Value == null ? "" : this.dataGridView7.Rows[4].Cells[1].Value.ToString()) + "',"
                                + "'" + (this.dataGridView7.Rows[5].Cells[1].Value == null ? "" : this.dataGridView7.Rows[5].Cells[1].Value.ToString()) + "',"
                                + "'" + (this.dataGridView7.Rows[6].Cells[1].Value == null ? "" : this.dataGridView7.Rows[6].Cells[1].Value.ToString()) + "',"
                                + "'" + (this.textBox1.Text == "" ? "0" : this.textBox1.Text) + "',"
                                + "'" + (this.textBox2.Text == "" ? "0" : this.textBox2.Text) + "',"
                                + "'" + (this.textBox3.Text == "" ? "0" : this.textBox3.Text) + "','" + this.dateTimePicker1.Value.ToShortDateString() + "','" + this.comboBox7.Text + "','" + this.button10.Text + "'); ";

                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        sql += " INSERT INTO [ASP]([GNAME], [GCZ], [GXH], [GDW1], [GSL], [GDJ1], [GDW2], [GJM], [GDZ], [GMEMO], [HTH], [DJ2],zz,zj ,[JHRQ]) VALUES(";
                        sql += (r.Cells["产品名称"].Value == null ? "'未知商品'," : "'" + r.Cells["产品名称"].Value.ToString() + "',");
                        sql += (r.Cells["材质"].Value == null ? "null," : "'" + r.Cells["材质"].Value.ToString() + "',");
                        sql += (r.Cells["规格型号"].Value == null ? "null," : "'" + r.Cells["规格型号"].Value.ToString() + "',");
                        sql += (r.Cells["计量单位1"].Value == null ? "null," : "'" + r.Cells["计量单位1"].Value.ToString() + "',");
                        sql += (r.Cells["数量"].Value == null || r.Cells["数量"].Value.ToString() == "" ? "NULL," : string.Format("'{0}',", r.Cells["数量"].Value.ToString()));
                        sql += (r.Cells["单价1"].Value == null || r.Cells["单价1"].Value.ToString() == "" ? "NULL," : string.Format("'{0}',", r.Cells["单价1"].Value.ToString()));
                        //sql += (r.Cells["数量"].Value == null ? "0" : "'" + r.Cells["数量"].Value.ToString() + "',");
                        //sql += (r.Cells["单价1"].Value == null ? "0" : "'" + r.Cells["单价1"].Value.ToString() + "',");
                        sql += (r.Cells["计量单位2"].Value == null ? "null," : "'" + r.Cells["计量单位2"].Value.ToString() + "',");
                        sql += (r.Cells["净毛"].Value == null ? "null," : "'" + r.Cells["净毛"].Value.ToString() + "',");
                        sql += (r.Cells["单重"].Value == null ? "null," : (r.Cells["单重"].Value.ToString() == "" ? "null," : "'" + r.Cells["单重"].Value.ToString() + "',"));
                        sql += (r.Cells["备注"].Value == null ? "null," : "'" + r.Cells["备注"].Value.ToString() + "',");
                        sql += "'" + this.hCODETextBox.Text + "',";
                        sql += (r.Cells["单价2"].Value == null ? "null," : (r.Cells["单价2"].Value.ToString() == "" ? "null," : "'" + r.Cells["单价2"].Value.ToString() + "',"));
                        sql += (r.Cells["总重"].Value == null || r.Cells["总重"].Value.ToString() == "" ? "NULL, " : string.Format("'{0}', ", r.Cells["总重"].Value.ToString()));
                        sql += (r.Cells["总价"].Value == null || r.Cells["总价"].Value.ToString() == "" ? "NULL, " : string.Format("'{0}', ", r.Cells["总价"].Value.ToString()));
                        sql += (r.Cells["交货日期"].Value == null ? "null" : (r.Cells["交货日期"].Value.ToString() == "" ? "null" : "'" + DateTime.Parse(r.Cells["交货日期"].Value.ToString()).ToShortDateString() + "'"));
                        sql += "); ";

                    }
                    foreach (DataRow r in dt_tk.Rows)
                    {
                        sql += " INSERT INTO ATK([TNAME],[ORDER],[HTH]) VALUES ('" + r[0].ToString() + "','" + r[1].ToString() + "','" + this.hCODETextBox.Text + "') ; ";
                    }

                    foreach (DataGridViewRow r in this.dataGridView4.Rows)
                    {
                        if (this.hLXTextBox.Text.Substring(0, 2) == "03")
                        {
                            return;
                        }
                        if (r.Cells[0].Value != null)
                        {
                            sql += " INSERT INTO AWX(WXHTH,XSHTH) VALUES ('" + this.hCODETextBox.Text + "','" + r.Cells[0].Value.ToString() + "'); ";
                            sql += string.Format("UPDATE ACONTRACT SET WXSTATE = '{0}' WHERE HCODE = '{1}'", r.Cells[1].Value.ToString(), r.Cells[0].Value.ToString());
                        }
                    }


                    //foreach (DataGridViewRow r in this.dataGridView4.Rows)
                    //{
                    //    if (r.Cells[0].Value != null)
                    //        sql += " INSERT INTO AWX(WXHTH,XSHTH) VALUES ('" + this.hCODETextBox.Text + "','" + r.Cells[0].Value.ToString() + "'); ";
                    //}


                    DBAdo.ExecuteNonQuerySql(sql);
                    QK();
                    if (ht != null)
                    {
                        ht.DataLoad();
                    }
                    MessageBox.Show("合同添加成功请等待财务确认");
                }
                if (op == 3)
                {
                    //Console.WriteLine(this.customSpGrid1.Rows[0].Cells["单价2"].Value == DBNull.Value);
                    string sql = " UPDATE [ACONTRACT] SET "
                               + "[HKH]='" + (this.hKHTextBox.Tag == null ? "" : this.hKHTextBox.Tag.ToString()) + "',"
                               + "[HDW]='" + (this.hDWTextBox.Tag == null ? "" : this.hDWTextBox.Tag.ToString()) + "',"
                               + "[HCODE]='" + this.hCODETextBox.Text + "',"
                               + "[HAREA]='" + (this.hAREATextBox.Tag == null ? "" : this.hAREATextBox.Tag.ToString()) + "',"
                               + "[HDATE]='" + this.hDATEDateTimePicker.Value.ToString("yyyy-MM-dd") + "',"
                               + "[HYWY]='" + (this.hYWYTextBox.Tag == null ? "" : this.hYWYTextBox.Tag.ToString()) + "',"
                               + "[HXM] ='" + this.hXMTextBox.Text + "',"
                               + "[HZBJ] ='" + (this.hZBJTextBox.Text == "" ? "0" : this.hZBJTextBox.Text) + "',"
                               + "[HHS] ='" + (this.hHSCheckBox.Checked ? 1 : 0) + "',"
                               + "[HHSBL] ='" + (this.hHSBLTextBox.Text == "" ? "0" : this.hHSBLTextBox.Text) + "',"
                               + "[HHTJE] ='" + (this.hHTJETextBox.Text == "" ? "0" : this.hHTJETextBox.Text) + "',"
                        //+ "[HJSJE] ='" + (this.hJSJETextBox.Text == "" ? "0" : this.hJSJETextBox.Text) + "',"
                               + "[HLX]='" + (this.hLXTextBox.Tag == null ? "" : this.hLXTextBox.Tag.ToString()) + "',"
                               + "[HMEMO]='" + this.HBZrichTextBox.Text + "',"
                               + "[HZT] ='" + this.HZTComboBox.Text + "',"
                               + "[HJHDATE]='" + this.hJHDATEDateTimePicker.Value.ToString("yyyy-MM-dd") + "',"
                               + "[HUSER] ='" + this.hUSERTextBox.Text + "',"
                               + "[HYHMC] ='" + (this.dataGridView6.Rows[7].Cells[1].Value == null ? "" : this.dataGridView6.Rows[7].Cells[1].Value.ToString()) + "',"//银行名
                               + "[HYHZH]='" + (this.dataGridView6.Rows[8].Cells[1].Value == null ? "" : this.dataGridView6.Rows[8].Cells[1].Value.ToString()) + "',"//账号
                               + "[HWTR1] ='" + (this.dataGridView5.Rows[3].Cells[1].Value == null ? "" : this.dataGridView5.Rows[3].Cells[1].Value.ToString()) + "',"//委托人1
                               + "[HWTR2] ='" + (this.dataGridView6.Rows[3].Cells[1].Value == null ? "" : this.dataGridView6.Rows[3].Cells[1].Value.ToString()) + "',"//委托人2
                               + "[HYF] ='" + (this.HYFTextBox.Text == "" ? "0" : this.HYFTextBox.Text) + "',"
                               + "[HQTFY]='" + (this.HqtfyTextBox.Text == "" ? "0" : this.HqtfyTextBox.Text) + "',"
                               + "[PS1]='" + (this.dataGridView3.Rows[0].Cells[1].Value == null ? "" : this.dataGridView3.Rows[0].Cells[1].Value.ToString()) + "',"
                               + "[PS2] ='" + (this.dataGridView3.Rows[1].Cells[1].Value == null ? "" : this.dataGridView3.Rows[1].Cells[1].Value.ToString()) + "',"
                               + "[PS3] ='" + (this.dataGridView3.Rows[2].Cells[1].Value == null ? "" : this.dataGridView3.Rows[2].Cells[1].Value.ToString()) + "',"
                               + "[PS4] ='" + (this.dataGridView3.Rows[3].Cells[1].Value == null ? "" : this.dataGridView3.Rows[3].Cells[1].Value.ToString()) + "',"
                               + "[YJ1] ='" + (this.dataGridView3.Rows[0].Cells[2].Value == null ? "" : this.dataGridView3.Rows[0].Cells[2].Value.ToString()) + "',"
                               + "[YJ2] ='" + (this.dataGridView3.Rows[1].Cells[2].Value == null ? "" : this.dataGridView3.Rows[1].Cells[2].Value.ToString()) + "',"
                               + "[YJ3]='" + (this.dataGridView3.Rows[2].Cells[2].Value == null ? "" : this.dataGridView3.Rows[2].Cells[2].Value.ToString()) + "',"
                               + "[YJ4] ='" + (this.dataGridView3.Rows[3].Cells[2].Value == null ? "" : this.dataGridView3.Rows[3].Cells[2].Value.ToString()) + "',"
                               + "[MACODE]='" + (this.dataGridView7.Rows[0].Cells[1].Value == null ? "" : this.dataGridView7.Rows[0].Cells[1].Value.ToString()) + "',"
                               + "[MBCODE] ='" + (this.dataGridView7.Rows[1].Cells[1].Value == null ? "" : this.dataGridView7.Rows[1].Cells[1].Value.ToString()) + "',"
                               + "[MCCODE] ='" + (this.dataGridView7.Rows[2].Cells[1].Value == null ? "" : this.dataGridView7.Rows[2].Cells[1].Value.ToString()) + "',"
                               + "[MDCODE] ='" + (this.dataGridView7.Rows[3].Cells[1].Value == null ? "" : this.dataGridView7.Rows[3].Cells[1].Value.ToString()) + "',"
                               + "[MECODE] ='" + (this.dataGridView7.Rows[4].Cells[1].Value == null ? "" : this.dataGridView7.Rows[4].Cells[1].Value.ToString()) + "',"
                               + "[MFCODE]='" + (this.dataGridView7.Rows[5].Cells[1].Value == null ? "" : this.dataGridView7.Rows[5].Cells[1].Value.ToString()) + "',"
                               + "[FLAG] = " + (this.checkBox1.Checked ? "1" : "0") + ","
                               + "[QDRQ] = '" + this.dateTimePicker1.Value.ToShortDateString() + "',"
                               + "[MGCODE]='" + (this.dataGridView7.Rows[6].Cells[1].Value == null ? "" : this.dataGridView7.Rows[6].Cells[1].Value.ToString()) + "',"
                               + "[DLF] ='" + (this.textBox1.Text == "" ? "0" : this.textBox1.Text) + "',"
                               + "[XXF] ='" + (this.textBox2.Text == "" ? "0" : this.textBox2.Text) + "',"
                               + "[zbfs] ='" + (this.comboBox7.Text == "" ? "0" : this.comboBox7.Text) + "',"
                               + "[BSF] ='" + (this.textBox3.Text == "" ? "0" : this.textBox3.Text) + "',"
                               + "[BIDCODE]='" + this.button10.Text + "'"
                               + " WHERE HCODE ='" + dgvr.Cells["合同号"].Value.ToString() + "'; ";


                    sql += " DELETE FROM ATK WHERE HTH ='" + dgvr.Cells["合同号"].Value.ToString() + "'; ";
                    sql += " DELETE FROM AWX WHERE WXHTH ='" + dgvr.Cells["合同号"].Value.ToString() + "'; ";

                    sql += " DELETE FROM ASP WHERE HTH ='" + dgvr.Cells["合同号"].Value.ToString() + "'; ";
                    //if (decimal.Parse(this.hJSJETextBox.Text) != this.jsje)
                    //{
                    //    string alterjsje = "";

                    string sqlcheck = "";//" DELETE FROM ASP WHERE HTH ='" + dgvr.Cells["合同号"].Value.ToString() + "'; ";
                    //foreach (int i in LdeL)
                    //{
                    //    sqlcheck += string.Format(" DELETE FROM ASP WHERE GCODE ={0};  ", i.ToString());
                    //}

                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        //if (r.Cells["GCODE"].Value == null)
                        //{
                        sqlcheck += " INSERT INTO [ASP]([GNAME], [GCZ], [GXH], [GDW1], [GSL], [GDJ1], [GDW2], [GJM], [GDZ], [GMEMO], [HTH], [DJ2],ZZ,ZJ, [JHRQ],[INVID]) VALUES(";
                        sqlcheck += (r.Cells["产品名称"].Value == null ? "'未知商品'," : "'" + r.Cells["产品名称"].Value.ToString() + "',");
                        sqlcheck += (r.Cells["材质"].Value == null ? "null," : "'" + r.Cells["材质"].Value.ToString() + "',");
                        sqlcheck += (r.Cells["规格型号"].Value == null ? "null," : "'" + r.Cells["规格型号"].Value.ToString() + "',");
                        sqlcheck += (r.Cells["计量单位1"].Value == null ? "null," : "'" + r.Cells["计量单位1"].Value.ToString() + "',");
                        sqlcheck += (r.Cells["数量"].Value == null || r.Cells["数量"].Value.ToString() == "" ? "NULL," : string.Format("'{0}',", r.Cells["数量"].Value.ToString()));
                        sqlcheck += (r.Cells["单价1"].Value == null || r.Cells["单价1"].Value.ToString() == "" ? "NULL," : string.Format("'{0}',", r.Cells["单价1"].Value.ToString()));
                        sqlcheck += (r.Cells["计量单位2"].Value == null ? "null," : "'" + r.Cells["计量单位2"].Value.ToString() + "',");
                        sqlcheck += (r.Cells["净毛"].Value == null ? "null," : "'" + r.Cells["净毛"].Value.ToString() + "',");
                        sqlcheck += (r.Cells["单重"].Value == null ? "null," : (r.Cells["单重"].Value.ToString() == "" ? "null," : "'" + r.Cells["单重"].Value.ToString() + "',"));
                        sqlcheck += (r.Cells["备注"].Value == null ? "null," : "'" + r.Cells["备注"].Value.ToString() + "',");
                        sqlcheck += "'" + this.hCODETextBox.Text + "',";
                        sqlcheck += (r.Cells["单价2"].Value == null ? "null," : (r.Cells["单价2"].Value.ToString() == "" ? "null," : "'" + r.Cells["单价2"].Value.ToString() + "',"));
                        sqlcheck += (r.Cells["总重"].Value == null || r.Cells["总重"].Value.ToString() == "" ? "NULL, " : string.Format("'{0}', ", r.Cells["总重"].Value.ToString()));
                        sqlcheck += (r.Cells["总价"].Value == null || r.Cells["总价"].Value.ToString() == "" ? "NULL, " : string.Format("'{0}', ", r.Cells["总价"].Value.ToString()));
                        sqlcheck += (r.Cells["交货日期"].Value == null ? "null" : (r.Cells["交货日期"].Value.ToString() == "" ? "null" : "'" + DateTime.Parse(r.Cells["交货日期"].Value.ToString()).ToShortDateString() + "'"));
                        sqlcheck += r.Cells["InvID"].Value == null || r.Cells["InvID"].Value.ToString() == "" ? ",null" :",'"+ r.Cells["InvID"].Value.ToString() + "' ";
                        sqlcheck += "); ";
                        //}
                        //else
                        //{
                        //    sqlcheck += string.Format("UPDATE [ASP] SET [GNAME]={1} [GCZ]={2} [GXH]={3} [GDW1]={4} [GSL]={5} [GDJ1]={6} [GDW2]={7} [GJM]={8} [GDZ]={9} [GMEMO]={10}  [DJ2]={11} [JHRQ]={12},zz={13} zj={14} WHERE GCODE ={0}",
                        //        new object[] {
                        //                     r.Cells["gcode"].Value,
                        //                    (r.Cells["产品名称"].Value == null ? "'未知商品'," : "'" + r.Cells["产品名称"].Value.ToString() + "',"),
                        //                    (r.Cells["材质"].Value == null ? "null," : "'" + r.Cells["材质"].Value.ToString() + "',"),
                        //                    (r.Cells["规格型号"].Value == null ? "null," : "'" + r.Cells["规格型号"].Value.ToString() + "',"),
                        //                    (r.Cells["计量单位1"].Value == null ? "null," : "'" + r.Cells["计量单位1"].Value.ToString() + "',"),
                        //                    (r.Cells["数量"].Value == null || r.Cells["数量"].Value.ToString() == "" ? "NULL," : string.Format("'{0}',", r.Cells["数量"].Value.ToString())),
                        //                    (r.Cells["单价1"].Value == null || r.Cells["单价1"].Value.ToString() == "" ? "NULL," : string.Format("'{0}',", r.Cells["单价1"].Value.ToString())) ,   
                        //                    //(r.Cells["数量"].Value == null ? "0" : "'" + r.Cells["数量"].Value.ToString() + "',"),
                        //                    //(r.Cells["单价1"].Value == null ? "0" : "'" + r.Cells["单价1"].Value.ToString() + "',"),
                        //                    (r.Cells["计量单位2"].Value == null ? "null," : "'" + r.Cells["计量单位2"].Value.ToString() + "',"),
                        //                    (r.Cells["净毛"].Value == null ? "null," : "'" + r.Cells["净毛"].Value.ToString() + "',"),
                        //                    (r.Cells["单重"].Value == null ? "null," : (r.Cells["单重"].Value.ToString() == "" ? "null," : "'" + r.Cells["单重"].Value.ToString() + "',")),
                        //                    (r.Cells["备注"].Value == null ? "null," : "'" + r.Cells["备注"].Value.ToString() + "',"),
                        //                    (r.Cells["单价2"].Value == null ? "null," : (r.Cells["单价2"].Value.ToString() == "" ? "null," : "'" + r.Cells["单价2"].Value.ToString() + "',")),
                        //                    (r.Cells["交货日期"].Value == null ? "null" : (r.Cells["交货日期"].Value.ToString() == "" ? "null" : "'" + DateTime.Parse(r.Cells["交货日期"].Value.ToString()).ToShortDateString() + "'")),
                        //                    (r.Cells["总重"].Value == null || r.Cells["总重"].Value.ToString() == "" ? "NULL," : string.Format("'{0}',", r.Cells["总重"].Value.ToString())),
                        //                    (r.Cells["总价"].Value == null || r.Cells["总价"].Value.ToString() == "" ? "NULL " : string.Format("'{0}' ", r.Cells["总价"].Value.ToString()))    
                        //        });

                        //}
                    }




                    sql += sqlcheck;
                    foreach (DataRow r in dt_tk.Rows)
                    {
                        sql += " INSERT INTO ATK([TNAME],[ORDER],[HTH]) VALUES ('" + r[0].ToString() + "','" + r[1].ToString() + "','" + this.hCODETextBox.Text + "') ; ";
                    }

                    foreach (DataGridViewRow r in this.dataGridView4.Rows)
                    {
                        if (r.Cells[0].Value != null)
                        {
                            sql += " INSERT INTO AWX(WXHTH,XSHTH) VALUES ('" + this.hCODETextBox.Text + "','" + r.Cells[0].Value.ToString() + "'); ";
                            sql += string.Format("UPDATE ACONTRACT SET WXSTATE = '{0}' WHERE HCODE = '{1}'", r.Cells[1].Value.ToString(), r.Cells[0].Value.ToString());
                        }
                    }

                    foreach (int _index in this.LdeL)
                    {
                        sql += string.Format(" DELETE FROM ASP WHERE GCODE ={0}", _index.ToString());
                    }
                    //DBAdo.ExecuteNonQuerySql(sql);
                    //if (ht != null)
                    //{
                    //    ht.DataLoad();
                    //}
                    string sqlproduce = "jsje_insert";
                    OleDbParameter[] pars = new OleDbParameter[6];
                    pars[0] = new OleDbParameter("@hcode", this.hCODETextBox.Text);
                    pars[1] = new OleDbParameter("@jsjeOld", this.jsje);
                    pars[2] = new OleDbParameter("@jsjeNew", this.hJSJETextBox.Text);
                    pars[3] = new OleDbParameter("@sqlstr", sql);
                    pars[4] = new OleDbParameter("@userId", ClassConstant.USER_ID);
                    pars[5] = new OleDbParameter("@userId", ClassConstant.DW_ID);
                    DBAdo.ExecuteScalarProcedure(sqlproduce, pars);
                    MessageBox.Show("合同修改需要财务确认，请等待确认");
                    this.Close();

                }

                //foreach (DataGridViewRow r in this.dataGridView4.Rows)
                //{
                //    if (r.Cells[0].Value != null)
                //        sql += " INSERT INTO AWX(WXHTH,XSHTH) VALUES ('" + this.hCODETextBox.Text + "','" + r.Cells[0].Value.ToString() + "'); ";
                //}


                //}

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void QK()
        {
            foreach (var item in this.tableLayoutPanel1.Controls)
            {
                if (item is TextBox)
                    (item as TextBox).Text = "";
                if (item is ComboBox)
                    (item as ComboBox).Text = "";
            }
            foreach (var item in this.tabControl1.TabPages)
            {
                foreach (var c in (item as TabPage).Controls)
                {
                    if (c is TextBox)
                        (c as TextBox).Text = "";
                    if (c is ComboBox)
                        (c as ComboBox).Text = "";
                    if (c is RichTextBox)
                        (c as RichTextBox).Text = "";
                }
            }
            this.hUSERTextBox.Text = ClassConstant.USER_NAME;
            this.HZTComboBox.Text = "正式";
            this.hHSBLTextBox.Text = "17";

            this.hDWTextBox.Text = ClassConstant.DW_NAME;
            this.hDWTextBox.Tag = ClassConstant.DW_ID;

            this.customSpGrid1.Rows.Clear();
            dt_tk.Rows.Clear();
            dt_wx.Rows.Clear();
            foreach (DataGridViewRow r in this.dataGridView3.Rows)
            {
                r.Cells[1].Value = "";
                r.Cells[2].Value = "";
            }
            foreach (DataGridViewRow r in this.dataGridView4.Rows)
            {
                r.Cells[0].Value = "";
            }
            foreach (DataGridViewRow r in this.dataGridView5.Rows)
            {
                r.Cells[1].Value = "";
            }
            foreach (DataGridViewRow r in this.dataGridView6.Rows)
            {
                r.Cells[1].Value = "";
            }
            foreach (DataGridViewRow r in this.dataGridView7.Rows)
            {
                r.Cells[1].Value = "";
            }
        }

        private void btn_close(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region treeView_DoubleClick
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_DQ.SelectedNode.Nodes.Count != 0)
                    return;
                this.hAREATextBox.Text = this.FZ_DQ.SelectedNode.Text;
                this.hAREATextBox.Tag = this.FZ_DQ.SelectedNode.Tag;
                this.FZ_DQ.Visible = false;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void treeView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_HTLX.SelectedNode.Nodes.Count != 0)
                    return;
                this.hLXTextBox.Text = this.FZ_HTLX.SelectedNode.Text;
                this.hLXTextBox.Tag = this.FZ_HTLX.SelectedNode.Tag;
                this.FZ_HTLX.Visible = false;
                if (this.hLXTextBox.Tag.ToString().Substring(0, 2) != "03")
                {
                    this.tabControl1.TabPages.Remove(this.tabPage4);
                }
                else
                {
                    if (this.tabControl1.TabPages.IndexOf(this.tabPage4) < 0)
                    {

                        this.tabPage4.Controls.Add(this.button8);
                        this.tabPage4.Controls.Add(this.button9);
                        this.tabPage4.Controls.Add(this.dataGridView8);
                        this.tabPage4.Controls.Add(this.dataGridView4);
                        this.tabControl1.TabPages.Insert(3, this.tabPage4);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void treeView3_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_YWY.SelectedNode.Nodes.Count != 0)
                    return;
                this.hYWYTextBox.Text = this.FZ_YWY.SelectedNode.Text;
                this.hYWYTextBox.Tag = this.FZ_YWY.SelectedNode.Tag;
                this.FZ_YWY.Visible = false;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void treeView4_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.FZ_DW.SelectedNode.Nodes.Count != 0)
                    return;
                this.hDWTextBox.Text = this.FZ_DW.SelectedNode.Text;
                this.hDWTextBox.Tag = this.FZ_DW.SelectedNode.Tag;
                this.FZ_DW.Visible = false;
                string sql = "SELECT * FROM ACLIENTS WHERE CCODE ='" + this.hDWTextBox.Tag.ToString() + "'";
                DataTable dtsql = DBAdo.DtFillSql(sql);
                if (dtsql.Rows.Count == 1)
                {
                    this.dataGridView6.Rows[0].Cells[1].Value = dtsql.Rows[0]["CNAME"].ToString();
                    this.dataGridView6.Rows[1].Cells[1].Value = dtsql.Rows[0]["CADDRESS"].ToString();
                    this.dataGridView6.Rows[2].Cells[1].Value = dtsql.Rows[0]["CFR"].ToString();
                    //this.dataGridView5.Rows[3].Cells[1].Value = dtsql.Rows[0][""].ToString();
                    this.dataGridView6.Rows[4].Cells[1].Value = dtsql.Rows[0]["CPHONE"].ToString();
                    this.dataGridView6.Rows[5].Cells[1].Value = dtsql.Rows[0]["CFAXNO"].ToString();
                    this.dataGridView6.Rows[6].Cells[1].Value = dtsql.Rows[0]["CFPTEL"].ToString();
                    this.dataGridView6.Rows[7].Cells[1].Value = dtsql.Rows[0]["CBANKNAME"].ToString();
                    this.dataGridView6.Rows[8].Cells[1].Value = dtsql.Rows[0]["CACCOUNT"].ToString();
                    this.dataGridView6.Rows[9].Cells[1].Value = dtsql.Rows[0]["CSH"].ToString();
                    this.dataGridView6.Rows[10].Cells[1].Value = dtsql.Rows[0]["CPOSNO"].ToString();
                }
                this.comboBox1.Items.Clear();
                DataTable bank = dtsql = DBAdo.DtFillSql("SELECT DISTINCT YNAME FROM ABANK WHERE BCODE ='" + this.hDWTextBox.Tag.ToString() + "'");
                foreach (DataRow r in bank.Rows)
                {
                    if (r[0].ToString() != "")
                        this.comboBox1.Items.Add(r[0].ToString());
                }

                //this.tabControl1.SelectTab(5);
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void treeView5_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView5.SelectedNode.Nodes.Count != 0)
                    return;
                this.hKHTextBox.Text = this.treeView5.SelectedNode.Text;
                this.hKHTextBox.Tag = this.treeView5.SelectedNode.Tag;
                this.FZ_KH.Visible = false;
                string sql = "SELECT * FROM ACLIENTS WHERE CCODE ='" + this.hKHTextBox.Tag.ToString() + "'";
                DataTable dtsql = DBAdo.DtFillSql(sql);
                if (dtsql.Rows.Count == 1)
                {
                    this.dataGridView5.Rows[0].Cells[1].Value = dtsql.Rows[0]["CNAME"].ToString();
                    this.dataGridView5.Rows[1].Cells[1].Value = dtsql.Rows[0]["CADDRESS"].ToString();
                    this.dataGridView5.Rows[2].Cells[1].Value = dtsql.Rows[0]["CFR"].ToString();
                    //this.dataGridView5.Rows[3].Cells[1].Value = dtsql.Rows[0][""].ToString();
                    this.dataGridView5.Rows[4].Cells[1].Value = dtsql.Rows[0]["CTEL"].ToString();
                    this.dataGridView5.Rows[5].Cells[1].Value = dtsql.Rows[0]["CFAXNO"].ToString();
                    this.dataGridView5.Rows[6].Cells[1].Value = dtsql.Rows[0]["CFPTEL"].ToString();
                    this.dataGridView5.Rows[7].Cells[1].Value = dtsql.Rows[0]["CBANKNAME"].ToString();
                    this.dataGridView5.Rows[8].Cells[1].Value = dtsql.Rows[0]["CACCOUNT"].ToString();
                    this.dataGridView5.Rows[9].Cells[1].Value = dtsql.Rows[0]["CSH"].ToString();
                    this.dataGridView5.Rows[10].Cells[1].Value = dtsql.Rows[0]["CPOSNO"].ToString();
                }
                //this.tabControl1.SelectTab(4);
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
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

        #region IsetText 成员

        public void SetTextKH(string key, string value)
        {
            try
            {
                this.hKHTextBox.Text = value;
                this.hKHTextBox.Tag = key;
                string sql = "SELECT * FROM ACLIENTS WHERE CCODE ='" + this.hKHTextBox.Tag.ToString() + "'";
                DataTable dtsql = DBAdo.DtFillSql(sql);
                if (dtsql.Rows.Count == 1)
                {
                    this.dataGridView5.Rows[0].Cells[1].Value = dtsql.Rows[0]["CNAME"].ToString();
                    this.dataGridView5.Rows[1].Cells[1].Value = dtsql.Rows[0]["CADDRESS"].ToString();
                    this.dataGridView5.Rows[2].Cells[1].Value = dtsql.Rows[0]["CFR"].ToString();
                    //this.dataGridView5.Rows[3].Cells[1].Value = dtsql.Rows[0][""].ToString();
                    this.dataGridView5.Rows[4].Cells[1].Value = dtsql.Rows[0]["CPHONE"].ToString();
                    this.dataGridView5.Rows[5].Cells[1].Value = dtsql.Rows[0]["CFAXNO"].ToString();
                    this.dataGridView5.Rows[6].Cells[1].Value = dtsql.Rows[0]["CFPTEL"].ToString();
                    this.dataGridView5.Rows[7].Cells[1].Value = dtsql.Rows[0]["CBANKNAME"].ToString();
                    this.dataGridView5.Rows[8].Cells[1].Value = dtsql.Rows[0]["CACCOUNT"].ToString();
                    this.dataGridView5.Rows[9].Cells[1].Value = dtsql.Rows[0]["CSH"].ToString();
                    this.dataGridView5.Rows[10].Cells[1].Value = dtsql.Rows[0]["CPOSNO"].ToString();
                }
                //this.tabControl1.SelectTab(4);
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        #endregion

        #region 商品 条款 等信息 录入

        public void SP_ADD(object[] values)
        {
            try
            {
                values[0] = this.dt_sp.Rows.Count + 1;
                this.dt_sp.Rows.Add(values);
                decimal d = 0;
                foreach (DataRow dr in dt_sp.Rows)
                {
                    d += decimal.Parse(dr["总价"].ToString() == "" ? "0" : dr["总价"].ToString());
                }
                decimal je = decimal.Round(d, 4);
                this.hJSJETextBox.Text = d.ToString();
                this.JE.Text = d.ToString();
                this.RMB.Text = ClassCustom.UpMoney(je);
                //this.dataGridView1.DataSource = dt_sp;

                Dgv1CssSet();
                this.dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void Dgv1CssSet()
        {


            //this.dataGridView1.Columns["数量"].DefaultCellStyle.NullValue = 0;
            //this.dataGridView1.Columns["单价"].DefaultCellStyle.NullValue = 0;
            //this.dataGridView1.Columns["单重"].DefaultCellStyle.NullValue = 0;
            //this.dataGridView1.Columns["总重"].DefaultCellStyle.NullValue = 0;
            //this.dataGridView1.Columns["总价"].DefaultCellStyle.NullValue = 0;
            this.dataGridView1.Columns["数量"].DefaultCellStyle.Format = "N4";
            this.dataGridView1.Columns["总价"].DefaultCellStyle.Format = "N4";
            this.dataGridView1.Columns["单价"].DefaultCellStyle.Format = "N4";
            this.dataGridView1.Columns["单重"].DefaultCellStyle.Format = "N4";
            this.dataGridView1.Columns["总重"].DefaultCellStyle.Format = "N4";
            this.dataGridView1.Columns["数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["单价"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["总价"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["单重"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridView1.Columns["总重"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        }

        public void SP_UPD(object[] values)
        {
            try
            {
                foreach (DataRow drr in dt_sp.Rows)
                {
                    if (drr[0].ToString() == values[0].ToString())
                    {
                        //drr["序号", typeof(int));
                        drr["商品名称"] = values[1].ToString();
                        drr["材质"] = values[2].ToString();
                        drr["型号"] = values[3].ToString();
                        drr["数量"] = decimal.Parse(values[4].ToString());
                        drr["数量单位"] = values[5].ToString();
                        drr["单价"] = decimal.Parse(values[6].ToString());
                        //drr["总价", typeof(decimal), "数量*单价");
                        drr["重量单位"] = values[8].ToString();
                        drr[@"净/毛"] = values[9].ToString();
                        drr["单重"] = (values[10] == null ? 0 : decimal.Parse(values[10].ToString()));
                        //drr["总重", typeof(decimal), "单重*数量");
                        drr["备注"] = values[12].ToString();
                        drr["图号"] = values[13].ToString();
                        break;
                    }
                }

                decimal d = 0;
                foreach (DataRow dr in dt_sp.Rows)
                {
                    d += decimal.Parse(dr["总价"].ToString());
                }
                decimal je = decimal.Round(d, 4);
                this.hJSJETextBox.Text = d.ToString();
                this.JE.Text = d.ToString();
                this.RMB.Text = ClassCustom.UpMoney(je);
                //this.dataGridView1.DataSource = dt_sp;
                this.dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
            }

        }

        public void TK_ADD(List<string> L)
        {
            dt_tk.Clear();
            for (int i = 0; i < L.Count; i++)
            {
                this.dt_tk.Rows.Add(new object[] { L[i].ToString(), i + 1 });
            }
            //this.dataGridView2.DataSource = dt_tk;
            this.dataGridView2.AutoResizeColumns();
        }

        public void WX_ADD(List<string> L)
        {
            dt_wx.Clear();
            foreach (string s in L)
            {
                this.dt_wx.Rows.Add(s);
            }
            //this.dataGridView4.DataSource = dt_wx;
            this.dataGridView4.Columns[0].Width = 500;
            this.dataGridView4.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        #endregion

        #region 其它信息
        private void QAtextBox_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView7.Rows[0].Cells[1].Value = this.QAtextBox.Text;
        }

        private void QBtextBox_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView7.Rows[1].Cells[1].Value = this.QBtextBox.Text;
        }

        private void QCtextBox2_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView7.Rows[2].Cells[1].Value = this.QCtextBox.Text;
        }

        private void QDtextBox3_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView7.Rows[3].Cells[1].Value = this.QDtextBox.Text;
        }

        private void QEtextBox4_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView7.Rows[4].Cells[1].Value = this.QEtextBox.Text;
        }

        private void QFtextBox5_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView7.Rows[5].Cells[1].Value = this.QFtextBox.Text;
        }

        private void QGtextBox6_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView7.Rows[6].Cells[1].Value = this.QGtextBox.Text;
        }
        #endregion

        #region 合同审批
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[0].Cells[1].Value = this.comboBox3.Text;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[1].Cells[1].Value = this.comboBox4.Text;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[2].Cells[1].Value = this.comboBox5.Text;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[3].Cells[1].Value = this.comboBox6.Text;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[0].Cells[2].Value = this.richTextBox1.Text;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[1].Cells[2].Value = this.richTextBox2.Text;
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[2].Cells[2].Value = this.richTextBox3.Text;
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView3.Rows[3].Cells[2].Value = this.richTextBox4.Text;
        }

        #endregion

        #region 委托人
        private void WTRT1extBox_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView5.Rows[3].Cells[1].Value = this.WTRT1extBox.Text;
        }

        private void WTRT2extBox_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView6.Rows[3].Cells[1].Value = this.WTRT2extBox.Text;
        }
        #endregion

        #region 外协
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView4.Rows.Add(new object[] { this.dataGridView8.SelectedRows[0].Cells[0].Value.ToString() });
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView4.Rows.Remove(this.dataGridView4.SelectedRows[0]);
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }
        #endregion

        private void hCODETextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (op == 3 && dgvr.Cells["合同号"].Value.ToString() == this.hCODETextBox.Text)
                    return;
                int bj = int.Parse(DBAdo.ExecuteScalarSql("SELECT COUNT(*) FROM ACONTRACT WHERE HCODE ='" + this.hCODETextBox.Text + "'").ToString());
                if (bj == 0)
                    return;
                MessageBox.Show("该合同号已经存在");
                if (op == 3)
                {
                    this.hCODETextBox.Text = dgvr.Cells["合同号"].Value.ToString();
                }
                else
                {
                    this.hCODETextBox.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if ((this.MdiParent as MForm1).OpenChildForm(typeof(A_HT_TK)))
                    return;
                A_HT_TK tk = new A_HT_TK(this.hCODETextBox.Text, this, 3, this.dataGridView2.DataSource as DataTable);
                tk.MdiParent = this.MdiParent;
                tk.Show();
            }
            catch (Exception ex)
            {

                MessageView.MessageErrorShow(ex);
            }
        }

        private void A_HT_OP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (op == 1)
            {
                if (this.hCODETextBox.Text == "")
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

        private void hYWYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.hYWYTextBox.Text == "")
                return;
            //if (hYWYTextBox.Text.Substring(0, 2) == "03")
            //{
            //    if (this.tabControl1.TabPages.IndexOf(this.tabPage4) < 0)
            //    {
            //        this.tabControl1.TabPages.Insert(3, this.tabPage4);
            //    }
            //}
            //else
            //{
            //    if (this.tabControl1.TabPages.IndexOf(this.tabPage4) >= 0)
            //    {
            //        this.tabControl1.TabPages.Remove(this.tabPage4);
            //    }
            //}
        }

        private void 粘贴EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //检测数据是否是可以使用的格式,即文本格式　　
                IDataObject iData = Clipboard.GetDataObject();
                if (!iData.GetDataPresent(DataFormats.Text))
                {
                    MessageBox.Show("没有从剪切板中接收到数据！");
                    return;
                }
                string clb = (String)iData.GetData(DataFormats.Text);
                string[] rows = clb.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in rows)
                {
                    string[] cells = s.Split(new char[] { '\t' });

                    if (cells.Length != dt_sp.Columns.Count - 3 - 1)
                    {
                        MessageBox.Show("数据格式不正确！");
                        return;
                    }
                    else
                    {
                        string[] new_cells = new string[cells.Length + 3];
                        new_cells[0] = (dt_sp.Rows.Count + 1).ToString();
                        new_cells[1] = cells[0];
                        new_cells[2] = cells[1];
                        new_cells[3] = cells[2];
                        new_cells[4] = cells[3];
                        new_cells[5] = cells[4];
                        new_cells[6] = cells[5];
                        new_cells[7] = "0";
                        new_cells[8] = cells[6];
                        new_cells[9] = cells[7];
                        new_cells[10] = cells[8];
                        new_cells[11] = "0";
                        new_cells[12] = cells[9];
                        new_cells[13] = cells[10];
                        dt_sp.Rows.Add(new_cells);
                    }
                    decimal d = 0;
                    foreach (DataRow dr in dt_sp.Rows)
                    {
                        d += decimal.Parse(dr["总价"].ToString());
                    }
                    decimal je = decimal.Round(d, 4);
                    this.hJSJETextBox.Text = d.ToString();
                    this.JE.Text = d.ToString();
                    this.RMB.Text = ClassCustom.UpMoney(je);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void 删除全部商品信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("是否删除所有商品信息", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    foreach (DataRow r in dt_sp.Rows)
                    {
                        if (r["ID"].ToString() != "")
                        {
                            LdeL.Add(int.Parse(r["ID"].ToString()));
                        }
                    }
                    dt_sp.Rows.Clear();
                    decimal d = 0;
                    foreach (DataRow dr in dt_sp.Rows)
                    {
                        d += decimal.Parse(dr["总价"].ToString());
                    }
                    decimal je = decimal.Round(d, 4);
                    this.hJSJETextBox.Text = d.ToString();
                    this.JE.Text = d.ToString();
                    this.RMB.Text = ClassCustom.UpMoney(je);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void contextMenuStrip1_VisibleChanged(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                this.粘贴EXCELToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.粘贴EXCELToolStripMenuItem.Enabled = false;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.customSpGrid1.Rows.Add(new object[] { this.customSpGrid1.Rows.Count + 1, null, null, null, null, null, null, null, null, null, null, null, null, this.hJHDATEDateTimePicker.Value.ToShortDateString() });
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void customSpGrid1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("商品属性输入错误！");
        }

        private void customSpGrid1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.customSpGrid1.Rows.Count == 0)
                    return;

                //if (this.customSpGrid1["单价1", e.RowIndex].Value != null && this.customSpGrid1["数量", e.RowIndex].Value != null)
                //{
                //    if (this.customSpGrid1["总价", e.RowIndex].Value != null)
                //    {
                //        if (this.customSpGrid1["总价", e.RowIndex].Value.ToString() != "")
                //        {
                //            this.customSpGrid1["总价", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["单价1", e.RowIndex].Value.ToString()) * decimal.Parse(this.customSpGrid1["数量", e.RowIndex].Value.ToString()));
                //        }
                //    }
                //}
                //if (this.customSpGrid1["总价", e.RowIndex].Value != null && this.customSpGrid1["数量", e.RowIndex].Value != null)
                //{
                //    if (this.customSpGrid1["单价1", e.RowIndex].Value != null)
                //    {
                //        if (this.customSpGrid1["单价1", e.RowIndex].Value.ToString() != "")
                //        {
                //            this.customSpGrid1["单价1", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["总价", e.RowIndex].Value.ToString().ToString()) / decimal.Parse(this.customSpGrid1["数量", e.RowIndex].Value.ToString()));
                //        }
                //    }
                //}
                //if (this.customSpGrid1["总价", e.RowIndex].Value != null && this.customSpGrid1["单价1", e.RowIndex].Value != null)
                //{
                //    if (decimal.Parse(this.customSpGrid1["单价1", e.RowIndex].Value.ToString()) != 0)
                //    {
                //        if (this.customSpGrid1["数量", e.RowIndex].Value != null)
                //        {
                //            if (this.customSpGrid1["数量", e.RowIndex].Value.ToString() != "")
                //            {
                //                this.customSpGrid1["数量", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["总价", e.RowIndex].Value.ToString()) / decimal.Parse(this.customSpGrid1["单价1", e.RowIndex].Value.ToString()));
                //            }
                //        }
                //    }
                //}
                //if (this.customSpGrid1["单重", e.RowIndex].Value != null && this.customSpGrid1["数量", e.RowIndex].Value != null)
                //{
                //    if (this.customSpGrid1["单重", e.RowIndex].Value.ToString() != "" && this.customSpGrid1["数量", e.RowIndex].Value.ToString() != "")
                //    {
                //        if (this.customSpGrid1["总重", e.RowIndex].Value != null)
                //        {
                //            if (this.customSpGrid1["总重", e.RowIndex].Value.ToString() != "")
                //            {
                //                this.customSpGrid1["总重", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["单重", e.RowIndex].Value.ToString()) * decimal.Parse(this.customSpGrid1["数量", e.RowIndex].Value.ToString()));
                //            }
                //        }
                //    }
                //}
                //if (this.customSpGrid1["总重", e.RowIndex].Value != null && this.customSpGrid1["数量", e.RowIndex].Value != null)
                //{
                //    if (this.customSpGrid1["总重", e.RowIndex].Value.ToString() != "" && this.customSpGrid1["数量", e.RowIndex].Value.ToString() != "")
                //    {
                //        if (decimal.Parse(this.customSpGrid1["数量", e.RowIndex].Value.ToString()) != 0)
                //        {
                //            if (this.customSpGrid1["单重", e.RowIndex].Value != null)
                //            {
                //                if (this.customSpGrid1["单重", e.RowIndex].Value.ToString() != "")
                //                {
                //                    this.customSpGrid1["单重", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["总重", e.RowIndex].Value.ToString()) / decimal.Parse(this.customSpGrid1["数量", e.RowIndex].Value.ToString()));
                //                }
                //            }
                //        }
                //    }
                //}
                //if (this.customSpGrid1["单价2", e.RowIndex].Value != null && this.customSpGrid1["总重", e.RowIndex].Value != null)
                //{
                //    if (this.customSpGrid1["单价2", e.RowIndex].Value.ToString() != "" && this.customSpGrid1["总重", e.RowIndex].Value.ToString() != "")
                //    {
                //        if (this.customSpGrid1["总价", e.RowIndex].Value != null)
                //        {
                //            if (this.customSpGrid1["总价", e.RowIndex].Value.ToString() != "")
                //            {
                //                this.customSpGrid1["总价", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["单价2", e.RowIndex].Value.ToString()) * decimal.Parse(this.customSpGrid1["总重", e.RowIndex].Value.ToString()));
                //            }
                //        }
                //    }
                //}
                //if (this.customSpGrid1["总价", e.RowIndex].Value != null && this.customSpGrid1["总重", e.RowIndex].Value != null)
                //{
                //    if (this.customSpGrid1["总价", e.RowIndex].Value.ToString() != "" && this.customSpGrid1["总重", e.RowIndex].Value.ToString() != "")
                //    {
                //        if (decimal.Parse(this.customSpGrid1["总重", e.RowIndex].Value.ToString()) != 0)
                //        {
                //            if (this.customSpGrid1["单价2", e.RowIndex].Value != null)
                //            {
                //                if (this.customSpGrid1["单价2", e.RowIndex].Value.ToString() != "")
                //                {
                //                    this.customSpGrid1["单价2", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["总价", e.RowIndex].Value.ToString()) / decimal.Parse(this.customSpGrid1["总重", e.RowIndex].Value.ToString()));
                //                }
                //            }
                //        }
                //    }
                //}
                //if (this.customSpGrid1["总重", e.RowIndex].Value != null && this.customSpGrid1["单重", e.RowIndex].Value != null)
                //{
                //    if (this.customSpGrid1["总重", e.RowIndex].Value.ToString() != "" && this.customSpGrid1["单重", e.RowIndex].Value.ToString() != "")
                //    {
                //        if (decimal.Parse(this.customSpGrid1["单重", e.RowIndex].Value.ToString()) != 0)
                //        {
                //            if (this.customSpGrid1["数量", e.RowIndex].Value != null)
                //            {
                //                if (this.customSpGrid1["数量", e.RowIndex].Value.ToString() != "")
                //                {
                //                    this.customSpGrid1["数量", e.RowIndex].Value = (decimal.Parse(this.customSpGrid1["总重", e.RowIndex].Value.ToString()) / decimal.Parse(this.customSpGrid1["单重", e.RowIndex].Value.ToString()));
                //                }
                //            }
                //        }
                //    }
                //}
                SumJSJE();
                this.customSpGrid1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.customSpGrid1, this.hCODETextBox.Text + "商品明细");

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void customSpGrid1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                //if (e.FormattedValue.ToString() == "")
                //{
                //    return;
                //}
                //e.ColumnIndex == 9 || e.ColumnIndex == 10 || e.ColumnIndex == 11 ||
                //if (e.ColumnIndex == 4 || e.ColumnIndex == 6 || e.ColumnIndex == 12)
                //{
                //    if (decimal.Parse(e.FormattedValue.ToString()) == 0)
                //    {
                //        MessageBox.Show("该单元不能为0");
                //        e.Cancel = true;
                //    }
                //    else
                //    {
                //        e.Cancel = false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.customSpGrid1.Rows.Count == 0)
                {
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("是否要删除该商品信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //foreach (DataGridViewCell c in this.customSpGrid1.CurrentRow.Cells)
                    //{
                    //    c.Value = null;
                    //}
                    this.customSpGrid1.Rows.Remove(this.customSpGrid1.CurrentRow);
                    int _index = 1;
                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        r.Cells[0].Value = _index++;
                    }
                    SumJSJE();
                    if (op == 3)
                    {
                        if (this.customSpGrid1.CurrentRow.Cells["GCODE"].Value != null)
                        {
                            LdeL.Add(int.Parse(this.customSpGrid1.CurrentRow.Cells["GCODE"].Value.ToString()));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes != MessageBox.Show("是否要删除该合同有商品信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;
                //while (this.customSpGrid1.Rows.Count > 0)
                //{
                this.customSpGrid1.Rows.Clear();
                //}
                foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                {
                    if (r.Cells["GCODE"].Value != null)
                    {
                        LdeL.Add(int.Parse(this.customSpGrid1.CurrentRow.Cells["GCODE"].Value.ToString()));
                    }
                }

                SumJSJE();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                //检测数据是否是可以使用的格式,即文本格式　　
                IDataObject iData = Clipboard.GetDataObject();
                if (!iData.GetDataPresent(DataFormats.Text))
                {
                    MessageBox.Show("没有从剪切板中接收到数据！");
                    return;
                }
                string clb = (String)iData.GetData(DataFormats.Text);
                string[] rows = clb.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in rows)
                {
                    string[] cells = s.Split(new char[] { '\t' });

                    if (cells.Length != this.customSpGrid1.Columns.Count - 2)
                    {
                        MessageBox.Show("数据格式不正确！");
                        return;
                    }
                    else
                    {
                        object[] new_cells = new object[cells.Length];
                        new_cells[0] = (this.customSpGrid1.Rows.Count + 1).ToString();//序号;
                        new_cells[1] = (cells[1] == "" ? "未知商品" : cells[1]);            //产品名称;
                        new_cells[2] = (cells[2] == "" ? "" : cells[2]);            //规格型号;
                        new_cells[3] = (cells[3] == "" ? "" : cells[3]);            //材质;
                        new_cells[4] = (cells[4] == "" ? null : cells[4]);            //数量;
                        new_cells[5] = (cells[5] == "" ? "" : cells[5]);            //计量单位1;
                        new_cells[6] = (cells[6] == "" ? null : cells[6]);            //单价1;
                        new_cells[7] = (cells[7] == "" ? "" : cells[7]);            //计量单位2;
                        new_cells[8] = (cells[8] == "" ? "" : cells[8]);            //净重;
                        new_cells[9] = (cells[9] == "" ? null : cells[9]);            //单重;
                        new_cells[10] = (cells[10] == "" ? null : cells[10]);         //总重;
                        new_cells[11] = (cells[11] == "" ? null : cells[11]);         //单价2;
                        new_cells[12] = (cells[12] == "" ? null : cells[12]);         //总价;
                        new_cells[13] = (cells[13] == "" ? DateTime.Now.ToShortDateString() : cells[13]);         //交货日期;
                        new_cells[14] = (cells[14] == "" ? "" : cells[14]);         //备注;
                        this.customSpGrid1.Rows.Add(new_cells);
                    }
                    SumJSJE();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void SumJSJE()
        {
            decimal d = 0;
            foreach (DataGridViewRow dr in this.customSpGrid1.Rows)
            {
                if (dr.Cells["总价"].Value == null)
                {
                    continue;
                }
                if (dr.Cells["总价"].Value.ToString() == "")
                {
                    continue;
                }
                d += decimal.Parse(dr.Cells["总价"].Value.ToString());
            }
            decimal je = decimal.Round(d, 4);
            this.hJSJETextBox.Text = d.ToString();
            this.JE.Text = d.ToString();
            this.RMB.Text = ClassCustom.UpMoney(je);
        }

        private void A_HT_OP_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.FZ_KH.Location = new Point(this.hKHTextBox.Location.X, this.hKHTextBox.Location.Y + 35);
                this.FZ_DW.Location = new Point(this.hDWTextBox.Location.X, this.hDWTextBox.Location.Y + 35);
                this.FZ_YWY.Location = new Point(this.hYWYTextBox.Location.X, this.hYWYTextBox.Location.Y + 35);
                this.FZ_HTLX.Location = new Point(this.hLXTextBox.Location.X, this.hLXTextBox.Location.Y + 35);
                this.FZ_DQ.Location = new Point(this.hAREATextBox.Location.X, this.hAREATextBox.Location.Y + 35);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private decimal CountResult(bool isChu, object x, object y)
        {
            try
            {
                if (x != null && y != null)
                {
                    if (x.ToString() != "" && y.ToString() != "")
                    {
                        if (isChu)
                        {
                            if (decimal.Parse(x.ToString()) != 0)
                            {
                                return decimal.Divide(decimal.Parse(y.ToString()), decimal.Parse(x.ToString()));
                            }
                        }
                        else
                        {
                            return decimal.Multiply(decimal.Parse(x.ToString()), decimal.Parse(y.ToString()));
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private void toolStripButton_单价_Click(object sender, EventArgs e)
        {
            try
            {
                this.customSpGrid1.EndEdit();
                if (this.toolStripComboBox1.Text == "选中行")
                {
                    if (this.customSpGrid1.Rows.Count <= 0)
                        return;
                    int _RowIndex = this.customSpGrid1.CurrentCell.RowIndex;
                    this.customSpGrid1["单价1", _RowIndex].Value = CountResult(true, this.customSpGrid1["数量", _RowIndex].Value, this.customSpGrid1["总价", _RowIndex].Value);
                }
                else
                {
                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        int _RowIndex = r.Index;
                        r.Cells["单价1"].Value = CountResult(true, r.Cells["数量"].Value, r.Cells["总价"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton_数量_Click(object sender, EventArgs e)
        {
            try
            {
                this.customSpGrid1.EndEdit();
                if (this.toolStripComboBox1.Text == "选中行")
                {
                    if (this.customSpGrid1.Rows.Count <= 0)
                        return;
                    int _RowIndex = this.customSpGrid1.CurrentCell.RowIndex;
                    this.customSpGrid1["数量", _RowIndex].Value = CountResult(true, this.customSpGrid1["单价1", _RowIndex].Value, this.customSpGrid1["总价", _RowIndex].Value);
                }
                else
                {
                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        int _RowIndex = r.Index;
                        r.Cells["数量"].Value = CountResult(true, r.Cells["单价1"].Value, r.Cells["总价"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton_总价_Click(object sender, EventArgs e)
        {
            try
            {
                this.customSpGrid1.EndEdit();
                if (this.toolStripComboBox1.Text == "选中行")
                {
                    if (this.customSpGrid1.Rows.Count <= 0)
                        return;
                    int _RowIndex = this.customSpGrid1.CurrentCell.RowIndex;
                    this.customSpGrid1["总价", _RowIndex].Value = CountResult(false, this.customSpGrid1["单价1", _RowIndex].Value, this.customSpGrid1["数量", _RowIndex].Value);
                }
                else
                {
                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        int _RowIndex = r.Index;
                        r.Cells["总价"].Value = CountResult(false, r.Cells["单价1"].Value, r.Cells["数量"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton_总重_Click(object sender, EventArgs e)
        {
            try
            {
                this.customSpGrid1.EndEdit();
                if (this.toolStripComboBox1.Text == "选中行")
                {
                    if (this.customSpGrid1.Rows.Count <= 0)
                        return;
                    int _RowIndex = this.customSpGrid1.CurrentCell.RowIndex;
                    this.customSpGrid1["总重", _RowIndex].Value = CountResult(false, this.customSpGrid1["数量", _RowIndex].Value, this.customSpGrid1["单重", _RowIndex].Value);
                }
                else
                {
                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        int _RowIndex = r.Index;
                        r.Cells["总重"].Value = CountResult(false, r.Cells["数量"].Value, r.Cells["单重"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton_单重_Click(object sender, EventArgs e)
        {
            try
            {
                this.customSpGrid1.EndEdit();
                if (this.toolStripComboBox1.Text == "选中行")
                {
                    if (this.customSpGrid1.Rows.Count <= 0)
                        return;
                    int _RowIndex = this.customSpGrid1.CurrentCell.RowIndex;
                    this.customSpGrid1["单重", _RowIndex].Value = CountResult(true, this.customSpGrid1["数量", _RowIndex].Value, this.customSpGrid1["总重", _RowIndex].Value);
                }
                else
                {
                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        int _RowIndex = r.Index;
                        r.Cells["单重"].Value = CountResult(true, r.Cells["数量"].Value, r.Cells["总重"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripButton_吨价_Click(object sender, EventArgs e)
        {
            try
            {
                this.customSpGrid1.EndEdit();
                if (this.toolStripComboBox1.Text == "选中行")
                {
                    if (this.customSpGrid1.Rows.Count <= 0)
                        return;
                    int _RowIndex = this.customSpGrid1.CurrentCell.RowIndex;
                    this.customSpGrid1["单价2", _RowIndex].Value = CountResult(true, this.customSpGrid1["总重", _RowIndex].Value, this.customSpGrid1["总价", _RowIndex].Value);
                }
                else
                {
                    foreach (DataGridViewRow r in this.customSpGrid1.Rows)
                    {
                        int _RowIndex = r.Index;
                        r.Cells["单价2"].Value = CountResult(true, r.Cells["总重"].Value, r.Cells["总价"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void hHSCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.hHSCheckBox.Checked = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.hLXTextBox.Tag == null || this.hDWTextBox.Tag.ToString() == null || this.comboBox7.SelectedIndex < 0)
            {
                return;
            }
            A_FZ_Bid bid = new A_FZ_Bid(this, this.hLXTextBox.Tag.ToString(), this.hDWTextBox.Tag.ToString(), hDATEDateTimePicker.Value.Year.ToString(), this.comboBox7.SelectedItem.ToString());
            bid.ShowDialog();
        }

        public void SetBidCode(string code)
        {
            this.button10.Text = code;
        }


    }
}
