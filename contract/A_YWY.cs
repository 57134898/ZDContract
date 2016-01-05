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
    public partial class A_YWY : Form
    {
        public A_YWY()
        {
            InitializeComponent();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.button1.Text = "添加";
                this.FormClear();
            }
            else if (this.radioButton2.Checked)
            {
                this.button1.Text = "修改";
            }
            else
            {
                this.button1.Text = "删除";
            }
            this.textBox1.ReadOnly = (this.radioButton1.Checked ? false : true);
        }

        private void A_YWY_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;
            this.radioButton2.Enabled = false;
            this.radioButton3.Enabled = true;
            DataLoad();
        }

        private void DataLoad()
        {
            string sql = string.Format(@"SELECT * FROM AYWY");
            DataTable dt = DBAdo.DtFillSql(sql);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderText = "编号";
            this.dataGridView1.Columns[1].HeaderText = "名字";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                if (string.IsNullOrEmpty(this.textBox1.Text)
                    || string.IsNullOrEmpty(this.textBox2.Text))
                {
                    MessageBox.Show("信息输入不完整！");
                    return;
                }
                string sql_check = string.Format(@"SELECT TOP 2 * FROM AYWY WHERE YCODE = '{0}'", this.textBox1.Text);
                DataTable dt_check = DBAdo.DtFillSql(sql_check);
                if (dt_check.Rows.Count > 0)
                {
                    MessageBox.Show("业务员编号已经存在！");
                    return;
                }
                string sql = string.Format(@"INSERT INTO AYWY VALUES ('{0}','{1}')", this.textBox1.Text, this.textBox2.Text);
                DBAdo.ExecuteNonQuerySql(sql);
                DataLoad();
                FormClear();
            }
            else if (this.radioButton2.Checked)
            {
                if (string.IsNullOrEmpty(this.textBox1.Text)
                    || string.IsNullOrEmpty(this.textBox2.Text))
                {
                    MessageBox.Show("信息输入不完整！");
                    return;
                }
                if (DialogResult.Yes !=
                    MessageBox.Show(string.Format("确定要把编号为{2}[{0}]{2}的业务员修改为{2}[{1}]", this.textBox1.Text, this.textBox2.Text, Environment.NewLine), "提示",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                string sql = string.Format(@"UPDATE AYWY SET YNAME= '{0}' WHERE YCODE='{1}'", this.textBox2.Text, this.textBox1.Text);
                DBAdo.ExecuteNonQuerySql(sql);
                DataLoad();
                FormClear();
            }
            else
            {
                if (DialogResult.Yes !=
    MessageBox.Show(string.Format("确定删除{2}编号为[{0}]{2}名字为[{1}]{2}的业务员?",
    this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString(),
    this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString(),
    Environment.NewLine), "提示",
    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                string sql_check = string.Format(@"SELECT TOP 2 * FROM ACONTRACT WHERE HYWY = '{0}'", this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                DataTable dt_check = DBAdo.DtFillSql(sql_check);
                if (dt_check.Rows.Count > 0)
                {
                    MessageBox.Show("业务员业务员已经使用无法删除！");
                    return;
                }
                string sql = string.Format(@"DELETE FROM AYWY WHERE YCODE='{0}'", this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                DBAdo.ExecuteNonQuerySql(sql);
                DataLoad();
                FormClear();
            }
        }

        private void FormClear()
        {
            this.textBox1.Text = string.Empty;
            this.textBox2.Text = string.Empty;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            this.textBox1.ReadOnly = true;
            this.radioButton2.Checked = true;
            this.textBox1.Text = this.dataGridView1[0, e.RowIndex].Value.ToString();
            this.textBox2.Text = this.dataGridView1[1, e.RowIndex].Value.ToString();
        }
    }
}
