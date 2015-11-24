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
    public partial class A_HT_AUTO : Form
    {
        private A_HT_FKFPGY fk;

        public A_HT_AUTO()
        {
            InitializeComponent();
        }

        public A_HT_AUTO(A_HT_FKFPGY fk)
        {
            InitializeComponent();
            this.fk = fk;
        }

        private void A_HT_AUTO_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.label1.Visible = this.radioButton1.Checked;
        }


        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
            {
                e.Handled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Enabled = !this.radioButton3.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!this.radioButton3.Checked) && this.textBox1.Text == "")
                {
                    return;
                }
                if ((!this.radioButton3.Checked) && decimal.Parse(this.textBox1.Text) == 0)
                {
                    return;
                }

                int op = 0;
                if (this.radioButton1.Checked) { op = 0; }
                else if (this.radioButton2.Checked) { op = 1; }
                else { op = 2; }
                this.fk.atuo(op, this.radioButton3.Checked ? 0 : decimal.Parse(this.textBox1.Text));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageView.MessageErrorShow(ex);
                return;
            }
        }
    }
}
