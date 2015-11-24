using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace contract
{
    public partial class A_HT_PZ : Form
    {
        private static OleDbConnection conn;
        private DataTable dt;
        private decimal je;
        private A_HT_FX_XX xx;
        private int op;
        private DataGridViewRow dgvr;
        private ToolStripItem[] bts = null;
        public A_HT_PZ()
        {
            InitializeComponent();
        }
        public A_HT_PZ(int op, A_HT_FX_XX xx, DataGridViewRow dgvr)
        {
            InitializeComponent();
            this.xx = xx;
            this.op = op;
            this.dgvr = dgvr;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                je = decimal.Parse(dgvr.Cells["金额"].Value.ToString());
                string cname = dgvr.Cells["客户"].Value.ToString();
                string vtype = "";
                if (this.comboBox3.Text == "现金") { vtype = "1"; }
                if (this.comboBox3.Text == "银行") { vtype = "2"; }
                if (this.comboBox3.Text == "转账") { vtype = "3"; }
                int vdc;
                if (dgvr.Cells["类型"].Value.ToString() == "付款" || dgvr.Cells["类型"].Value.ToString() == "销项发票")
                {
                    vdc = 1;
                }
                else
                {
                    vdc = -1;
                }
                //string sql = "SELECT h.vexpl AS 摘要 FROM ivoucher i,hvoucher h "
                //          + "WHERE h.bcode='" + ClassConstant.DW_ID + "' AND h.id=i.hid AND i.vdc=1 AND h.year='" + this.comboBox1.Text + "' AND h.month='" + this.comboBox2.Text + "' AND h.vtype = '" + vtype + "' "
                //          + "group by h.vdate,h.vtype,h.vno,h.vexpl HAVING SUM(i.rmb)=" + je.ToString()
                //          + " order by h.vno";
                string sql = "SELECT h.vdate AS 日期,CASE WHEN h.vtype=1 THEN '现金' WHEN h.vtype=2 THEN '银行' ELSE '转账' END AS 凭证类型,h.vno AS 凭证号, i.rmb AS 金额,h.vexpl AS 摘要 FROM ivoucher i,hvoucher h ,ccode c "
                          + "WHERE i.ccode = c.ccode and h.bcode='" + ClassConstant.DW_ID + "' AND h.id=i.hid AND i.vdc= " + vdc.ToString() + " AND h.year='" + this.comboBox1.Text + "' AND h.month='" + this.comboBox2.Text + "' AND h.vtype = '" + vtype + "' "
                          + " AND i.ccode = '" + dgvr.Cells["CCODE"].Value.ToString() + "' AND i.rmb=" + je.ToString()
                    //+ "group by h.vdate,h.vtype,h.vno,cname,h.vexpl,i.rmb HAVING i.rmb=" + je.ToString()
                          + " order by h.vno";
                Console.WriteLine(sql);
                conn = new OleDbConnection(ClassConstant.CONNECT_STRING);
                //MessageBox.Show(ClassConstant.CONNECT_STRING);
                conn.Open();
                OleDbDataAdapter oledb = new OleDbDataAdapter(sql, conn);
                dt = new DataTable();
                oledb.Fill(dt);
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
            finally
            {
                conn.Close();
            }
        }


        private void Form_Load(object sender, EventArgs e)
        {
            try
            {

                this.comboBox1.Text = DateTime.Now.Year.ToString();
                this.comboBox2.Text = DateTime.Now.Month.ToString();


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
                if (this.dataGridView1.Rows.Count <= 0)
                {
                    return;
                }

                if (DateTime.Parse(this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString()).Year != DateTime.Parse(dgvr.Cells["日期"].Value.ToString()).Year ||
                    DateTime.Parse(this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString()).Month != DateTime.Parse(dgvr.Cells["日期"].Value.ToString()).Month)
                {
                    MessageBox.Show("业务日期与凭证日期不符，操作已终止");
                    return;
                }
                string Vouchertype = "";
                switch (this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString())
                {
                    case "现金":
                        Vouchertype = "1";
                        break;
                    case "银行":
                        Vouchertype = "2";
                        break;
                    case "转账":
                        Vouchertype = "3";
                        break;
                    default:
                        throw new Exception("未知凭证类型");
                }
                string sql = "UPDATE ACASH SET VoucherFlag =1,Voucheryear = '" + DateTime.Parse(this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString()).Year + "',Vouchermonth='" + DateTime.Parse(this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString()).Month + "',Vouchertype='" + Vouchertype + "',Voucherid='" + this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + "' WHERE CID=" + dgvr.Cells[0].Value.ToString();
                DBAdo.ExecuteNonQuerySql(sql);
                xx.reLoad();
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

    }
}
