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
    public partial class A_Users : Form
    {
        private DataTable dt_com1;
        private DataTable dt_com2;
        public A_Users()
        {
            InitializeComponent();
        }

        private void A_Users_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;
            this.radioButton2.Enabled = false;
            this.radioButton3.Enabled = true;
            DataLoad();
            ComboxListLoad();
        }

        private void ComboxListLoad()
        {
            string sql1 = string.Format(@"SELECT YCODE BCODE,YNAME BNAME FROM [AYWY] WHERE LEN(YCODE)=4");
            dt_com1 = DBAdo.DtFillSql(sql1);
            string sql2 = string.Format(@"SELECT YCODE BCODE,YNAME BNAME FROM [AYWY] WHERE LEN(YCODE)=6");
            dt_com2 = DBAdo.DtFillSql(sql2);
            foreach (DataRow r in dt_com1.Rows)
            {
                this.comboBox1.Items.Add(r["bcode"].ToString() + ':' + r["bname"].ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2.Items.Clear();
            foreach (DataRow r in dt_com2.Rows)
            {
                if (r["bcode"].ToString().StartsWith(ClassCustom.codeSub(this.comboBox1.Text)))
                {
                    this.comboBox2.Items.Add(r["bcode"].ToString() + ':' + r["bname"].ToString());
                }
            }
            if (this.radioButton2.Checked)
            {
                bool b1 = true;

                foreach (object item in this.comboBox2.Items)
                {
                    if (ClassCustom.codeSub(item.ToString()) == ClassCustom.codeSub(this.dataGridView1.SelectedRows[0].Cells[4].Value.ToString()))
                    {
                        this.comboBox2.SelectedItem = item;
                        b1 = false;

                    }
                }
                if (b1)
                {
                    this.comboBox2.SelectedIndex = -1;
                }
            }
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


        private void DataLoad()
        {
            string sql = string.Format(@"SELECT [UCODE],[UNAME],[UPASSWORD],[UDW],[UBM] FROM AUSERS");
            DataTable dt = DBAdo.DtFillSql(sql);
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].HeaderText = "编号";
            this.dataGridView1.Columns[1].HeaderText = "名字";
            this.dataGridView1.Columns[2].HeaderText = "密码";
            this.dataGridView1.Columns[3].HeaderText = "单位";
            this.dataGridView1.Columns[4].HeaderText = "部门";
            this.dataGridView1.AutoResizeColumns();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                if (string.IsNullOrEmpty(this.textBox1.Text)
                      || string.IsNullOrEmpty(this.textBox2.Text)
                      || string.IsNullOrEmpty(this.textBox3.Text)
                      || string.IsNullOrEmpty(this.comboBox1.Text)
                      || string.IsNullOrEmpty(this.comboBox2.Text))
                {
                    MessageBox.Show("用户信息输入不完整！");
                    return;
                }
                string sql_check = string.Format(@"SELECT TOP 2 * FROM AUSERS WHERE UCODE = '{0}' ", this.textBox1.Text);
                DataTable dt_check = DBAdo.DtFillSql(sql_check);
                if (dt_check.Rows.Count > 0)
                {
                    MessageBox.Show("用户编号或者用户名已经存在！");
                    return;
                }
                string sql = string.Format(@"INSERT INTO AUSERS ([UCODE],[UNAME],[UPASSWORD],[UDW],[UBM]) VALUES ('{0}','{1}','{2}','{3}','{4}')",
                    new string[] { this.textBox1.Text, this.textBox2.Text, this.textBox3.Text, this.comboBox1.Text, this.comboBox2.Text });
                DBAdo.ExecuteNonQuerySql(sql);
                DataLoad();
                FormClear();
            }
            else if (this.radioButton2.Checked)
            {
                if (string.IsNullOrEmpty(this.textBox1.Text)
      || string.IsNullOrEmpty(this.textBox2.Text)
      || string.IsNullOrEmpty(this.textBox3.Text)
      || string.IsNullOrEmpty(this.comboBox1.Text)
      || string.IsNullOrEmpty(this.comboBox2.Text))
                {
                    MessageBox.Show("用户信息输入不完整！");
                    return;
                }
                if (DialogResult.Yes !=
                    MessageBox.Show(string.Format("确定要把编号为{1}[{0}]{1}的用户?", this.textBox1.Text, Environment.NewLine), "提示",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                string sql = string.Format(@"UPDATE AUSERS SET UNAME= '{0}',UPASSWORD='{1}',UDW='{2}',UBM='{3}' WHERE UCODE='{4}'",
                    new string[] { this.textBox2.Text, this.textBox3.Text, this.comboBox1.Text, this.comboBox2.Text, this.textBox1.Text });
                DBAdo.ExecuteNonQuerySql(sql);
                DataLoad();
                FormClear();
            }
            else
            {
                if (DialogResult.Yes !=
    MessageBox.Show(string.Format("确定删除编号为{1}[{0}]{1}的用户?",
    this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString(),
    Environment.NewLine), "提示",
    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }

                string sql = string.Format(@"DELETE FROM AUSERS WHERE UCODE='{0}'", this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                DBAdo.ExecuteNonQuerySql(sql);
                DataLoad();
                FormClear();
            }
        }

        private void FormClear()
        {
            this.textBox1.Text = string.Empty;
            this.textBox2.Text = string.Empty;
            this.textBox3.Text = string.Empty;
            this.comboBox1.SelectedIndex = -1;
            this.comboBox2.SelectedIndex = -1;
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
            this.textBox3.Text = this.dataGridView1[2, e.RowIndex].Value.ToString();
            bool b = true;
            foreach (object item in this.comboBox1.Items)
            {
                if (ClassCustom.codeSub(item.ToString()) == ClassCustom.codeSub(this.dataGridView1[3, e.RowIndex].Value.ToString()))
                {
                    this.comboBox1.SelectedItem = item;
                    b = false;

                }
            }
            if (b)
            {
                this.comboBox1.SelectedIndex = -1;
            }

        }


    }
}
