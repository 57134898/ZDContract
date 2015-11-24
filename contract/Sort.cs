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
    public partial class Sort : Form
    {
        private A_CX_HT cx;
        private DataTable dt;

        public Sort()
        {
            InitializeComponent();
        }

        public Sort(A_CX_HT cx, DataTable dt)
        {
            InitializeComponent();
            this.cx = cx;
            this.dt = dt;
        }

        private void Sort_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (DataColumn c in dt.Columns)
                {
                    this.dataGridView1.Rows.Add(new object[] { c.ColumnName, "升序", "ASC" });
                    this.dataGridView1.Rows.Add(new object[] { c.ColumnName, "降序", "DESC" });
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Label lb = new Label();
                lb.AutoSize = true;
                lb.Margin = new Padding(3, 3, 3, 3);
                lb.BorderStyle = BorderStyle.FixedSingle;
                lb.Text = string.Format("{0} - {1}", this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                lb.Tag = string.Format("{0} {1}", this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), this.dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
                OnButtonClick lbClick = (object sender1, EventArgs e1) =>
                {
                    lb.Dispose();
                };
                lb.DoubleClick += new EventHandler(lbClick);

                this.flowLayoutPanel1.Controls.Add(lb);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cx != null)
            {
                string filter = "";
                foreach (Control lb in this.flowLayoutPanel1.Controls)
                {
                    filter += "," + lb.Tag.ToString();
                }
                
                cx.sort(filter.Substring(1));
            }
            this.Close();
        }

    }
}
