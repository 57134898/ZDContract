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
    public partial class ChangeUser : Form
    {
        public ChangeUser()
        {
            InitializeComponent();
        }

        private void ChangeUser_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT UCODE+':'+UNAME AS CN FROM AUSERS WHERE UCODE LIKE '01__'";
                this.comboBox1.DataSource = DBAdo.DtFillSql(sql);
                this.comboBox1.DisplayMember = "CN";
                this.comboBox4.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBox1.Text == "")
                {
                    return;
                }
                string sql = "SELECT UCODE+':'+UNAME AS CN FROM AUSERS WHERE UCODE NOT LIKE '010102%' AND UCODE LIKE '" + ClassCustom.codeSub(this.comboBox1.Text) + "__'";
                this.comboBox2.DataSource = DBAdo.DtFillSql(sql);
                this.comboBox2.DisplayMember = "CN";

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBox2.Text == "")
                {
                    return;
                }
                string sql = "SELECT UCODE+':'+UNAME AS CN FROM AUSERS WHERE UCODE LIKE '" + ClassCustom.codeSub(this.comboBox2.Text) + "____'";
                this.comboBox3.DataSource = DBAdo.DtFillSql(sql);
                this.comboBox3.DisplayMember = "CN";

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
            if (this.textBox1.Text != "1qaz")
            {
                MessageBox.Show("密码错误");
                return;
            }
            ClassConstant.DW_ID = ClassCustom.codeSub(this.comboBox1.Text);
            ClassConstant.DW_NAME = ClassCustom.codeSub1(this.comboBox1.Text);
            ClassConstant.BCODE = ClassCustom.codeSub(this.comboBox2.Text);
            ClassConstant.BNAME = ClassCustom.codeSub1(this.comboBox2.Text);
            ClassConstant.USER_ID = ClassCustom.codeSub(this.comboBox3.Text);
            ClassConstant.USER_NAME = ClassCustom.codeSub1(this.comboBox3.Text);
            ClassConstant.AccountingBook = ClassCustom.codeSub(this.comboBox4.Text);
            ClassConstant.AccountingBookName = ClassCustom.codeSub1(this.comboBox4.Text);
            ClassConstant.MF1.LogInApplication();
            this.Close();
        }
    }
}
