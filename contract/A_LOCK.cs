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
    public partial class A_LOCK : Form
    {
        private DataTable dt;

        public A_LOCK()
        {
            InitializeComponent();
        }

        private void A_LOCK_Load(object sender, EventArgs e)
        {
            try
            {
                this.numericUpDown1.Value = DateTime.Now.Year;
                this.numericUpDown2.Value = DateTime.Now.Month;
                this.comboBox1.Enabled = true;
                this.comboBox1.Items.Clear();
                var sql1 = "SELECT * FROM BCODE WHERE LEN(BCODE)=4";
                var souce = DBAdo.DtFillSql(sql1);
                foreach (DataRow row in souce.Rows)
                {
                    if (row[0].ToString().Substring(2, 2) == ClassConstant.DW_ID.Substring(2, 2))
                    {
                        this.comboBox1.Items.Add(string.Format("{0}:{1}", row[0].ToString(), row[1].ToString()));
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.radioButton1.Checked)
                {
                    if (DialogResult.Yes == MessageBox.Show(string.Format("是否确定把[{0}年{1}月]设为[{2}]",
                         this.numericUpDown1.Value.ToString(),
                         this.numericUpDown2.Value.ToString(),
                        this.radioButton1.Text),
                        "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        string sql = string.Format("IF EXISTS(SELECT * FROM AMONTH WHERE [YEAR]={0} AND [MONTH]={1} AND HDW = '{2}') "
                                                + "BEGIN "
                                                + "UPDATE AMONTH SET FLAG = 1 WHERE [YEAR]={0} AND [MONTH]={1} AND HDW = '{2}' "
                                                + "END "
                                                + "ELSE "
                                                + "BEGIN "
                                                + "INSERT INTO AMONTH([year],[month],[hdw],[flag],[user]) VALUES({0},{1},'{2}',1,'{3}') "
                                                + "END",
                                                new string[] { 
                                                this.numericUpDown1.Value.ToString(),
                                                this.numericUpDown2.Value.ToString(),
                                                ClassCustom.codeSub(this.comboBox1.Text),
                                                ClassConstant.USER_NAME
                                            });
                        DBAdo.ExecuteNonQuerySql(sql);
                        this.A_LOCK_Load(null, null);
                    }
                }
                else
                {
                    if (this.dataGridView1.Rows.Count == 0)
                    {
                        return;
                    }
                    if (DialogResult.Yes == MessageBox.Show(string.Format("是否确定把[{0}年{1}月]设为[{2}]",
                        this.dataGridView1.SelectedRows[0].Cells["年"].Value.ToString(),
                        this.dataGridView1.SelectedRows[0].Cells["月"].Value.ToString(),
                        this.radioButton2.Text),
                        "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        string sql = string.Format("update AMONTH set flag=0,[USER] = '{3}' WHERE [YEAR]={0} AND [MONTH]={1} AND HDW = '{2}'",
                            new object[]{
                                   this.dataGridView1.SelectedRows[0].Cells["年"].Value.ToString(),
                                   this.dataGridView1.SelectedRows[0].Cells["月"].Value.ToString(),
                                   this.dataGridView1.SelectedRows[0].Cells["公司码"].Value.ToString(),
                                   ClassConstant.USER_NAME
                            });
                        DBAdo.ExecuteNonQuerySql(sql);
                        this.A_LOCK_Load(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "")
            {
                return;
            }
            string where = string.Empty;
            if (ClassConstant.USER_NAME == "于萍")
            {
                where = " and 1=1 ";
            }
            else
            {
                where = string.Format("  AND  M.HDW ='{0}' ", ClassCustom.codeSub(this.comboBox1.Text));
            }
            //CASE  WHEN '1' THEN '已结账' ELSE '未结账' END 
            string sql = string.Format("SELECT  M.HDW 公司码,C.CNAME 公司名,M.[YEAR] 年, M.[MONTH] 月,M.[FLAG]是否结账, [USER] 结账人, [DATE] 结账时间 FROM [AMONTH] M,ACLIENTS C WHERE C.CCODE = M.HDW {0} ORDER BY M.HDW ,M.[YEAR], M.[MONTH]", where);
            dt = DBAdo.DtFillSql(sql);
            DataView dv = dt.DefaultView;
            this.dataGridView1.DataSource = dv;
            this.dataGridView1.AutoResizeColumns();
        }
    }
}
