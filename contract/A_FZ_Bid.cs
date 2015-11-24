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
    public partial class A_FZ_Bid : Form
    {
        private string hlx;
        private string bcode;
        private string year;
        private string btype;
        private IBidCode ht;
        public A_FZ_Bid()
        {
            InitializeComponent();
        }

        public A_FZ_Bid(IBidCode ht, string hlx, string bcode, string year, string btype)
        {
            InitializeComponent();
            this.hlx = hlx.Substring(0, 2);
            this.bcode = bcode;
            this.year = year;
            this.btype = btype;
            this.ht = ht;
        }

        private void A_FZ_Bid_Load(object sender, EventArgs e)
        {
            DataLoad();

        }

        public void DataLoad()
        {
            try
            {
                string sql = string.Format(@"SELECT [TID] FROM [Bids] T0
                                                INNER JOIN ALX T1 ON T0.HLx=T1.LID where  T0.TID NOT IN(SELECT BIDCODE FROM ACONTRACT N1 WHERE N1.BIDCODE=T0.TID)  AND bcode='{0}' and (tyear = '{1}' or tyear = '{1}'- 1 ) and ttype = '{2}' and hlx = '{3}'", new string[] { bcode, year, btype, hlx });
                DataTable dt = DBAdo.DtFillSql(sql);
                this.dataGridView1.DataSource = dt;

                this.dataGridView1.Columns["TID"].HeaderText = "标号";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            this.ht.SetBidCode(this.dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ht.SetBidCode(this.button1.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ht.SetBidCode(this.button2.Text);
            this.Close();
        }
    }
}
