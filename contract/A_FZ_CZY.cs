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
    public partial class A_FZ_CZY : Form
    {
        private DataTable souce;
        private DataTable souce1;
        private DataTable souce2;
        public A_FZ_CZY()
        {
            InitializeComponent();
        }

        private void A_FZ_CZY_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT [UID], [UCODE], [UNAME], [UPASSWORD], [UDW], [UBM], [POS], [AUT], [FLAG] FROM AUSERS";
                souce = DBAdo.DtFillSql(sql);
                this.dataGridView1.DataSource = souce;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].HeaderText = "编号";
                this.dataGridView1.Columns[2].HeaderText = "用户名";
                this.dataGridView1.Columns[3].Visible = false;
                this.dataGridView1.Columns[4].HeaderText = "单位";
                this.dataGridView1.Columns[5].HeaderText = "部门";
                this.dataGridView1.Columns[6].HeaderText = "岗位";
                this.dataGridView1.Columns[7].Visible = false;
                this.dataGridView1.Columns[8].Visible = false;
                this.dataGridView1.AutoResizeColumns();

                souce1 = DBAdo.DtFillSql("SELECT UCODE,UNAME FROM AUSERS WHERE LEN(UCODE) = '4'");
                this.comboBox1.DataSource = souce1;
                this.comboBox1.DisplayMember = "UNAME";
                this.comboBox1.ValueMember = "UCODE";
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                souce2 = DBAdo.DtFillSql("SELECT UCODE,UNAME FROM AUSERS WHERE UCODE LIKE '" + this.comboBox1.SelectedValue.ToString() + "__'");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
