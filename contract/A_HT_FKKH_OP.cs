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
    public partial class A_HT_FKKH_OP : Form
    {
        private int cid;
        private A_HT_KHFK fk;
        private string type;
        private bool mark;
        public A_HT_FKKH_OP()
        {
            InitializeComponent();
        }

        public A_HT_FKKH_OP(int cid, A_HT_KHFK fk, string type,bool mark)
        {
            InitializeComponent();
            this.cid = cid;
            this.fk = fk;
            this.type = type;
            this.mark = mark;
        }

        private void A_HT_FKKH_OP_Load(object sender, EventArgs e)
        {

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

        }

        private void DataLoad()
        {
            try
            {
                foreach (string s in ClassConstant.fklx)
                {
                    (this.dataGridView1.Columns[2] as DataGridViewComboBoxColumn).Items.Add(s);
                }
                foreach (string s in ClassConstant.fkfs)
                {
                    (this.dataGridView1.Columns[3] as DataGridViewComboBoxColumn).Items.Add(s);
                }
                DataTable dt = DBAdo.DtFillSql("SELECT * FROM ACASH WHERE CID ='" + cid + "'");
                this.textBox9.Text = DBAdo.ExecuteScalarSql("SELECT CNAME FROM ACLIENTS WHERE CCODE = '" + dt.Rows[0]["Ccode"].ToString() + "'").ToString();
                this.textBox1.Text = dt.Rows[0]["cash"].ToString();
                this.textBox2.Text = dt.Rows[0]["note"].ToString();
                this.textBox3.Text = dt.Rows[0]["mz"].ToString();
                this.textBox7.Text = (decimal.Parse(this.textBox1.Text) + decimal.Parse(this.textBox2.Text) + decimal.Parse(this.textBox3.Text)).ToString();
                this.textBox4.Text = dt.Rows[0]["zjbl1"].ToString();
                this.textBox5.Text = dt.Rows[0]["zjbl2"].ToString();
                this.textBox6.Text = dt.Rows[0]["zjbl3"].ToString();
                this.textBox8.Text = "0";
                this.dateTimePicker1.Value = DateTime.Parse(dt.Rows[0]["exchangedate"].ToString());
                DataTable souce = DBAdo.DtFillSql("SELECT * FROM AFKXX WHERE CID = '" + cid + "'");

                if (this.type == "销项发票" || this.type == "进项发票" || this.type == "估验")
                {
                    this.panel2.Visible = false;
                    this.panel3.Visible = false;
                }
                else
                {
                    this.panel3.Visible = false;
                    this.panel2.Visible = true;
                }
                if (type=="付款")
                {
                    this.panel3.Visible = true;
                }
                foreach (DataRow r in souce.Rows)
                {
                    this.dataGridView1.Rows.Add(new object[] { r["HTH"].ToString(), r["RMB"].ToString(), r["FKLX"].ToString(), r["FKFS"].ToString(), r["XSHTH"].ToString(), r["ID"].ToString() });
                }


                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    foreach (DataRow row in DBAdo.DtFillSql("SELECT xshth from awx where wxhth='" + this.dataGridView1[0, i].Value.ToString() + "'").Rows)
                    {
                        (this.dataGridView1[4, i] as DataGridViewComboBoxCell).Items.Add(row[0].ToString());
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
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
                    new Factory_ToolBtn("  保存  ", "  保存  ",ClassCustom.getImage("sav.png"), this.btn_sav,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  删除  ", "  删除  ",ClassCustom.getImage("del.png"), this.btn_del,null,true).TBtnProduce(),
                    new Factory_ToolBtn("  退出  ", "  退出  ",ClassCustom.getImage("tc.png"),this.btn_close,null,true).TBtnProduce()
                    };

            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivate);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
        }

        private void QK()
        {

        }




        #region 按钮事件



        private void btn_sav(object sender, EventArgs e)
        {
            try
            {
                #region 判断是否该月结账
                string tsql = string.Format("SELECT flag FROM AMONTH WHERE [YEAR] ={0} AND [MONTH]={1} AND HDW = {2}", this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), ClassConstant.DW_ID);
                object result = DBAdo.ExecuteScalarSql(tsql);
                if (bool.Parse(result == null ? false.ToString() : result.ToString()))
                {
                    MessageBox.Show("本月已结账不能添加进度信息", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                #endregion
                #region 判断是否连接凭证
                if (mark)
                {
                     MessageBox.Show("进度信息已连接凭证禁止操作", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                #endregion

                if (this.type == "销项发票" || this.type == "进项发票" || this.type == "估验")
                {

                }
                else
                {
                    if (decimal.Parse(this.textBox7.Text) != (decimal.Parse(this.textBox1.Text) + decimal.Parse(this.textBox2.Text) + decimal.Parse(this.textBox3.Text)))
                    {
                        return;
                    }
                }
                if (decimal.Parse(this.textBox8.Text) != 0)
                {
                    return;
                }
                string sql = "";
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    sql += " UPDATE AFKXX SET RMB= '" + r.Cells[1].Value.ToString() + "',fkfs='" + r.Cells[3].Value.ToString() + "', fklx='" + r.Cells[2].Value.ToString() + "', xshth = '" + r.Cells[4].Value.ToString() + "', date = '" + this.dateTimePicker1.Value.ToShortDateString() + "'  WHERE ID ='" + r.Cells["ID"].Value.ToString() + "'; ";
                }

                if (this.type == "销项发票" || this.type == "进项发票" || this.type == "估验")
                {
                    sql += " UPDATE [ACash] SET [ExchangeDate]='" + this.dateTimePicker1.Value.ToShortDateString() + "', [Cash]='" + this.textBox1.Text + "', [Note]='" + this.textBox7.Text + "', [Mz]='" + this.textBox3.Text + "', [ZJBL1]='" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "', [ZJBL2]='" + (this.textBox5.Text == "" ? "0" : this.textBox5.Text) + "', [ZJBL3]='" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "' WHERE [CID]=" + cid;

                }
                else
                {
                    sql += " UPDATE [ACash] SET [ExchangeDate]='" + this.dateTimePicker1.Value.ToShortDateString() + "', [Cash]='" + this.textBox1.Text + "', [Note]='" + this.textBox2.Text + "', [Mz]='" + this.textBox3.Text + "', [ZJBL1]='" + (this.textBox4.Text == "" ? "0" : this.textBox4.Text) + "', [ZJBL2]='" + (this.textBox5.Text == "" ? "0" : this.textBox5.Text) + "', [ZJBL3]='" + (this.textBox6.Text == "" ? "0" : this.textBox6.Text) + "' WHERE [CID]=" + cid;

                }

                DBAdo.ExecuteNonQuerySql(sql);
                MessageBox.Show("修改成功！");
                fk.reLoad();
                this.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageView.MessageErrorShow(ex);
                return;
            }
        }

        private void btn_del(object sender, EventArgs e)
        {
            try
            {
                #region 判断是否该月结账
                string tsql = string.Format("SELECT flag FROM AMONTH WHERE [YEAR] ={0} AND [MONTH]={1} AND HDW = {2}", this.dateTimePicker1.Value.Year.ToString(), this.dateTimePicker1.Value.Month.ToString(), ClassConstant.DW_ID);
                object result = DBAdo.ExecuteScalarSql(tsql);
                if (bool.Parse(result == null ? false.ToString() : result.ToString()))
                {
                    MessageBox.Show("本月已结账不能添加进度信息", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                #region 判断是否连接凭证
                if (mark)
                {
                    MessageBox.Show("进度信息已连接凭证禁止操作", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                #endregion
                #endregion
                if (DialogResult.Yes != MessageBox.Show("是否删除信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;
                string sql = "";
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    sql += " DELETE FROM  AFKXX WHERE ID ='" + r.Cells["ID"].Value.ToString() + "'; ";
                }
                sql += " DELETE FROM  [ACash]  WHERE [CID]=" + cid;
                DBAdo.ExecuteNonQuerySql(sql);
                MessageBox.Show("删除成功！");
                fk.reLoad();
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


        #endregion

        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    decimal sum = 0;
                    foreach (DataGridViewRow r in this.dataGridView1.Rows)
                    {
                        sum += decimal.Parse(r.Cells[1].Value.ToString());
                    }
                    this.textBox8.Text = (decimal.Parse(this.textBox7.Text) - sum).ToString();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal sum = 0;
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    if (r.Cells["Column3"].Value != null)
                    {
                        sum += decimal.Parse(r.Cells["Column3"].Value.ToString());
                    }
                }
                this.textBox8.Text = (decimal.Parse(this.textBox7.Text == "" ? "0" : this.textBox7.Text) - sum).ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
