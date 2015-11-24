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
                this.comboBox1.Text = ClassConstant.DW_ID + ":" + ClassConstant.DW_NAME;
                string rowFilter = string.Format("公司码 = '{0}'", ClassConstant.DW_ID);
                if (ClassConstant.USER_NAME == "于萍")
                {
                    this.comboBox1.Enabled = true;
                    rowFilter = "";
                }
                //CASE  WHEN '1' THEN '已结账' ELSE '未结账' END 
                string sql = string.Format("SELECT  M.HDW 公司码,C.CNAME 公司名,M.[YEAR] 年, M.[MONTH] 月,M.[FLAG]是否结账, [USER] 结账人, [DATE] 结账时间 FROM [AMONTH] M,ACLIENTS C WHERE C.CCODE = M.HDW ORDER BY M.HDW ,M.[YEAR], M.[MONTH]");
                dt = DBAdo.DtFillSql(sql);
                DataView dv = dt.DefaultView;
                dv.RowFilter = rowFilter;
                this.dataGridView1.DataSource = dv;
                this.dataGridView1.AutoResizeColumns();
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
    }
}
