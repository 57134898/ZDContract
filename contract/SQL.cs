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
    public partial class SQL : Form
    {
        public SQL()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DBAdo.DtFillSql(this.textBox1.Text);
                this.dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClassCustom.ExportDataGridview1(this.dataGridView1,"查询结果");
        }

        private void SQL_Load(object sender, EventArgs e)
        {
            if (ClassConstant.USER_NAME=="于萍")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }

        }
    }
}
