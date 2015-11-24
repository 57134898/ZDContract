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
    public partial class A_EXCEL_INPUT : Form
    {

        #region Form基本方法
        private DataTable dt_sp = new DataTable();
        private DataTable dt_sh = new DataTable();
        private ToolStripItem[] bts = null;
        private string hth;
        private A_HT_OP hop;
        public A_EXCEL_INPUT()
        {
            InitializeComponent();
        }
        public A_EXCEL_INPUT(string hth, A_HT_OP hop)
        {
            InitializeComponent();
            this.hth = hth;
            this.hop = hop;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = hth + "-商品EXCEL导入";

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

        }

        private void DataLoad()
        {
            try
            {
                dt_sp.Columns.Add("序号", typeof(int));
                dt_sp.Columns.Add("商品名称", typeof(string));
                dt_sp.Columns.Add("材质", typeof(string));
                dt_sp.Columns.Add("型号", typeof(string));
                dt_sp.Columns.Add("数量", typeof(decimal));
                dt_sp.Columns.Add("数量单位", typeof(string));
                dt_sp.Columns.Add("单价", typeof(decimal));
                dt_sp.Columns.Add("总价", typeof(decimal), "数量*单价");
                dt_sp.Columns.Add("重量单位", typeof(string));
                dt_sp.Columns.Add(@"净/毛", typeof(string));
                dt_sp.Columns.Add("单重", typeof(decimal));
                dt_sp.Columns.Add("总重", typeof(decimal), "单重*数量");
                dt_sp.Columns.Add("备注", typeof(string));
                dt_sp.Columns.Add("图号", typeof(string));
                this.dataGridView1.DataSource = dt_sp;

            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }

        }

        private void Reg()
        {
            try
            {
                bts = new ToolStripItem[]{
                    new Factory_ToolBtn(" 计算器 "," 计算器 ",ClassCustom.getImage("jsq.png"),(this.MdiParent as MForm1).jsq,null,true).TBtnProduce(),
                    new Factory_ToolBtn("关闭窗口","关闭窗口",ClassCustom.getImage("gb.png"),(this.MdiParent as MForm1).CloseAll,null,true).TBtnProduce(),
                    new Factory_ToolBtn("窗口层叠","窗口层叠",ClassCustom.getImage("cd.png"),(this.MdiParent as MForm1).cd,null,true).TBtnProduce(),
                    new Factory_ToolBtn("垂直平铺","垂直平铺",ClassCustom.getImage("cz.png"),(this.MdiParent as MForm1).cz,null,true).TBtnProduce(),
                    new Factory_ToolBtn("水平平铺","水平平铺",ClassCustom.getImage("spp.png"),(this.MdiParent as MForm1).sp,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  查找  ","明细条款",ClassCustom.getImage("sel.png"),this.btn_sel,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  导入  ", "商品明细",ClassCustom.getImage("t1.png"), this.btn_dr,null,true).TBtnProduce(),
                    new ToolStripSeparator(),
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce(),
                    };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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

        private void btn_sel(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void btn_dr(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
                return;
            string tabname = this.textBox1.Text;
            string eLink = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tabname + ";Persist Security Info=True;Extended Properties = Excel 8.0";
            OleDbConnection conn = new OleDbConnection(eLink);
            conn.Open();
            try
            {
                string sql = "Select * from [" + this.dataGridView2.SelectedRows[0].Cells[0].Value.ToString() + "] ";
                //MessageBox.Show(sql);
                OleDbDataAdapter oledb = new OleDbDataAdapter(sql, conn);
                DataTable dtt = new DataTable();
                oledb.Fill(dtt);
                int ii = 1;
                foreach (DataRow r in dtt.Rows)
                {
                    //MessageBox.Show(r[0].ToString());
                    dt_sp.Rows.Add(new object[] { 
                        ii++,               //序号           
                        r[0].ToString(),    //商品名称  
                        r[1].ToString(), //材质
                        r[2].ToString(), //型号
                        decimal.Parse(r[3].ToString()==""?"0":r[3].ToString()), //数量           
                        r[4].ToString(), //数量单位
                        decimal.Parse( r[5].ToString()==""?"0":r[5].ToString()), //单价
                        0, //总价
                        r[6].ToString(), //重量单位
                        r[7].ToString(), //"净/毛
                        decimal.Parse( r[8].ToString()==""?"0":r[8].ToString()), //单重             
                        0, //总重 
                        r[9].ToString(), //备注
                        r[10].ToString() //图号 
                    });
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
                return;
            }
        }


        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                this.progressBar1.Maximum = this.dt_sp.Rows.Count;
                foreach (DataRow r in dt_sp.Rows)
                {
                    hop.SP_ADD(new object[] { 
                         1,              //序号           
                         r[1].ToString(),   //商品名称  
                         r[2].ToString(),//材质
                         r[3].ToString(),//型号
                        decimal.Parse(r[4].ToString()==""?"0":r[4].ToString()), //数量           
                        r[5].ToString(), //数量单位
                        decimal.Parse( r[6].ToString()==""?"0":r[6].ToString()), //单价
                        0, //总价
                        r[8].ToString(), //重量单位
                        r[9].ToString(), //"净/毛
                        decimal.Parse( r[10].ToString()==""?"0":r[10].ToString()), //单重             
                        0, //总重 
                        r[12].ToString(), //备注
                        r[13].ToString() //图号 
                    });
                    this.progressBar1.Value++;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.textBox1.Text = this.openFileDialog1.FileName;
        }
        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
                return;
            string tabname = this.textBox1.Text;
            string eLink = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tabname + ";Persist Security Info=True;Extended Properties = Excel 8.0";
            OleDbConnection conn = new OleDbConnection(eLink);
            conn.Open();
            try
            {
                dt_sh = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach (DataRow r in dt_sh.Rows)
                {
                    string sql = "Select COUNT(*) from [" + r["TABLE_NAME"].ToString() + "] ";
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    this.dataGridView2.Rows.Add(new object[] { r["TABLE_NAME"], cmd.ExecuteScalar() });
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
                return;
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView2.Rows.Count == 0)
                return;
            this.btn_dr(null, null);
        }
    }
}
