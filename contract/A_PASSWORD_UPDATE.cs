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
    public partial class A_PASSWORD_UPDATE : Form
    {
        public A_PASSWORD_UPDATE()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    MessageBox.Show("密码不能为空");
                    return;
                }
                if (this.textBox1.Text != this.textBox2.Text)
                {
                    MessageBox.Show("两次输入密码不一致，请重新输入！");
                    return;
                }

                string sql = "UPDATE AUSERS set upassword ='" + this.textBox2.Text + "' where ucode = '" + ClassConstant.USER_ID + "'";
                DBAdo.ExecuteNonQuerySql(sql);
                MessageBox.Show("密码修改成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
