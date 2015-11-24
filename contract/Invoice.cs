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
    public partial class Invoice : Form
    {
        public DataGridViewRow Dgvr { get; set; }
        private Warehouse wh;
        public Invoice()
        {
            InitializeComponent();
        }

        public Invoice(DataGridViewRow dgrv, Warehouse wh)
        {
            InitializeComponent();
            this.Dgvr = dgrv;
            this.wh = wh;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ZjTB.Text.Length <= 0 || this.invTB.Text.Length <= 0 || this.comboBox1.Text.Length <= 0)
                {
                    MessageBox.Show("数据填写不完整！");
                    return;
                }

                if (decimal.Parse(this.invTB.Text) > decimal.Parse(Dgvr.Cells["zj"].Value.ToString()))
                {
                    MessageBox.Show("本次金额大于余额！");
                    return;
                }

                this.Dgvr.Cells["本次数量"].Value = decimal.Parse(this.quaTB.Text);
                this.Dgvr.Cells["本次单价"].Value = decimal.Parse(this.Dj1TB.Text);
                this.Dgvr.Cells["本次总价"].Value = decimal.Parse(this.ZjTB.Text);
                this.Dgvr.Cells["EtcCost"].Value = (this.etcTB.Text.Length <= 0 ? 0 : decimal.Parse(this.etcTB.Text));
                this.Dgvr.Cells["税额"].Value = decimal.Parse(this.taxTB.Text);
                this.Dgvr.Cells["备注"].Value = this.todoTB.Text;
                this.Dgvr.Cells["单位"].Value = this.comboBox1.Text;
                this.Dgvr.DefaultCellStyle.BackColor = SystemColors.ControlLight;
                this.Dgvr.Cells["本次发票金额"].Value = decimal.Parse(this.invTB.Text);
                //this.Dgvr.DefaultCellStyle.Font.Bold = SystemColors.ControlLight;
                this.wh.GenerateTotal();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void ClearForm()
        {
            this.Dj1TB.Text = string.Empty;
            this.quaTB.Text = string.Empty;
            this.ZjTB.Text = string.Empty;

        }

        private void ENTER_KeyPress(object sender, KeyPressEventArgs e)//限制输入数字"." "-"
        {
            if (!(((e.KeyChar >= 46) && (e.KeyChar <= 57)) || (e.KeyChar == 45) || (e.KeyChar == 46) || (e.KeyChar == 8)))
            {
                e.Handled = true;
            }
        }

        private void Invoice_Load(object sender, EventArgs e)
        {
            this.radioButton3.Checked = true;
            this.ZjTB.Enabled = false;
        }

        private void quaTB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.radioButton1.Checked)
                {
                    if (this.Dj1TB.Text.Length > 0 && this.ZjTB.Text.Length > 0)
                    {
                        this.quaTB.Text = (decimal.Parse(this.ZjTB.Text) / decimal.Parse(this.Dj1TB.Text)).ToString();
                    }
                    else
                    {
                        this.quaTB.Text = string.Empty;
                    }
                }
                else if (this.radioButton2.Checked)
                {
                    if (this.quaTB.Text.Length > 0 && this.ZjTB.Text.Length > 0)
                    {
                        this.Dj1TB.Text = (decimal.Parse(this.ZjTB.Text) / decimal.Parse(this.quaTB.Text)).ToString();
                    }
                    else
                    {
                        this.Dj1TB.Text = string.Empty;
                    }
                }
                else
                {
                    if (this.Dj1TB.Text.Length > 0 && this.quaTB.Text.Length > 0)
                    {
                        this.ZjTB.Text = (decimal.Parse(this.quaTB.Text) * decimal.Parse(this.Dj1TB.Text)).ToString();
                    }
                    else
                    {
                        this.ZjTB.Text = string.Empty;
                    }
                }

                if (this.ZjTB.Text.Length > 0 && this.taxTB.Text.Length > 0)
                {
                    this.invTB.Text = (decimal.Parse(this.ZjTB.Text) + decimal.Parse(this.taxTB.Text)).ToString();
                }
                else
                {
                    this.invTB.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Dj1TB.Text = string.Empty;
                this.quaTB.Text = string.Empty;
                this.ZjTB.Text = string.Empty;
            }
        }

        private void taxTB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ZjTB.Text.Length > 0 && this.taxTB.Text.Length > 0)
                {
                    this.invTB.Text = (decimal.Parse(this.ZjTB.Text) + decimal.Parse(this.taxTB.Text)).ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.taxTB.Text = string.Empty;
                this.invTB.Text = string.Empty;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.quaTB.Enabled = false;
                this.Dj1TB.Enabled = true;
                this.ZjTB.Enabled = true;
            }
            else if (this.radioButton2.Checked)
            {
                this.quaTB.Enabled = true;
                this.Dj1TB.Enabled = false;
                this.ZjTB.Enabled = true;
            }
            else
            {
                this.quaTB.Enabled = true;
                this.Dj1TB.Enabled = true;
                this.ZjTB.Enabled = false;
            }
        }
    }
}
