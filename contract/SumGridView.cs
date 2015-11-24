using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace contract
{
    public partial class SumGridView : UserControl
    {
        private List<SumCellInfo> Lcs = new List<SumCellInfo>();
        private DataTable _DataSouce;

        public DataGridView Grid { get; set; }

        public List<string> SumColumnNames { get; set; }

        public SumGridView()
        {
            InitializeComponent();
            this.Grid = this.dataGridView1;
        }

        public DataTable DataSouce
        {
            get { return _DataSouce; }
            set
            {
                _DataSouce = value;
                if (value != null)
                {
                    this.dataGridView1.DataSource = value.DefaultView;
                    foreach (DataGridViewColumn c in this.dataGridView1.Columns)
                    {
                        if (c.Index == 1)
                        {
                            c.Width = 300;
                        }
                        else if (c.Index == 0)
                        {
                            c.Width = 50;
                        }
                        else
                        {
                            c.Width = 120;
                        }
                        if (c.ValueType == typeof(decimal))
                        {
                            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            c.DefaultCellStyle.Format = "N2";
                        }
                    }

                    SethHScrollBarMaxValue();
                    //this.SumColumnNames = new List<string>() { "RMB", "Age1" };
                    ComputerTotals();
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                }
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            try
            {
                //    DataTable dt = new DataTable();
                //    dt.Columns.Add("ID", typeof(int));
                //    dt.Columns.Add("Name", typeof(string));
                //    dt.Columns.Add("RMB", typeof(int));
                //    dt.Columns.Add("Age", typeof(decimal));
                //    dt.Columns.Add("Sex", typeof(string));
                //    dt.Columns.Add("Count", typeof(decimal));
                //    dt.Columns.Add("RMB1", typeof(int));
                //    dt.Columns.Add("Age1", typeof(decimal));
                //    dt.Columns.Add("Sex1", typeof(string));
                //    dt.Columns.Add("Count1", typeof(decimal));
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男男男男男男男男男男男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "LUCY", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "ROSE", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "ALICE", 800, 28, "男", 100000, 800, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "LYLdsfsfsdfsdfsdfsdfsdfY", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "ROSE", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "ALICE", 800, 28, "男", 100000, 800, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "LYLY", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    dt.Rows.Add(new object[] { 1, "JACK", 1000, 28, "男", 100000, 1000, 28, "男" });
                //    this.dataGridView1.DataSource = dt.DefaultView;
                //    this.dataGridView1.AutoResizeColumns();
                //    SethHScrollBarMaxValue();

                //    this.SumColumnNames = new List<string>() { "RMB", "Age1" };
                //    ComputerTotals();

                //    sf.Alignment = StringAlignment.Far;
                //    sf.LineAlignment = StringAlignment.Center;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }

        }


        private void SethHScrollBarMaxValue()
        {
            int tempWidth = 0;
            foreach (DataGridViewColumn c in this.dataGridView1.Columns)
            {
                tempWidth += c.Width;
            }
            this.hScrollBar1.Maximum = ((int)((tempWidth - this.dataGridView1.DisplayRectangle.Width + this.dataGridView1.RowHeadersWidth) / 10) + 1) + 9;
        }

        private void ComputerTotals()
        {
            try
            {
                if (this.dataGridView1.Rows.Count == 0)
                    return;
                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    //Console.WriteLine(SumColumnNames.FindIndex(delegate(string s) { return this.dataGridView1.Columns[i].Name == s; }) < 0);
                    if (SumColumnNames.FindIndex(delegate(string s) { return this.dataGridView1.Columns[i].Name == s; }) < 0)
                        continue;

                    decimal sum = 0;
                    for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
                    {
                        sum += decimal.Parse(this.dataGridView1[i, j].Value == DBNull.Value ? "0" : this.dataGridView1[i, j].Value.ToString());
                    }

                    Lcs.Add(new SumCellInfo(i, sum));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageBox.Show(ex.Message);
                return;
            }
        }


        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count == 0)
                {
                    return;
                }
                this.dataGridView1.HorizontalScrollingOffset = this.hScrollBar1.Value * 10;
                this.Invalidate(false);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }

        }




        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {


        }

        StringFormat sf = new StringFormat(StringFormatFlags.LineLimit);


        private void SumGridView_Paint(object sender, PaintEventArgs e)
        {
            try
            {

                //Graphics ghi = this.CreateGraphics();
                e.Graphics.DrawString("合计:", new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))), new SolidBrush(SystemColors.ControlText), new Rectangle(5, this.dataGridView1.Height, this.dataGridView1.RowHeadersWidth, 28));
                foreach (SumCellInfo sci in Lcs)
                {
                    if (sci.ColumnIndex < this.dataGridView1.FirstDisplayedScrollingColumnIndex || sci.ColumnIndex > this.dataGridView1.FirstDisplayedScrollingColumnIndex + this.dataGridView1.DisplayedColumnCount(false))
                    {
                        continue;

                    }
                    Rectangle rtlTemp = this.dataGridView1.GetColumnDisplayRectangle(sci.ColumnIndex, false);
                    float w = e.Graphics.MeasureString(sci.Value.ToString("N2"), new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)))).Width;


                    e.Graphics.DrawString(w <= rtlTemp.Width ? sci.Value.ToString("N2") : "...", new Font("微软雅黑", 10.5F, w <= rtlTemp.Width ? FontStyle.Underline : FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))), new SolidBrush(SystemColors.ControlText), new Rectangle(rtlTemp.X, this.dataGridView1.Height, rtlTemp.Width, 28), sf);
                    //Console.WriteLine(1);
                }
                //ghi.Dispose();
                //this.Invalidate(false);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SethHScrollBarMaxValue();
            this.Invalidate(false);
        }

        private void SumGridView_SizeChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
                return;
            if (this.dataGridView1.DisplayedColumnCount(false) < this.dataGridView1.Columns.Count)
            {

                if (!this.hScrollBar1.Visible)
                {
                    this.dataGridView1.Height -= 17;
                    this.hScrollBar1.Visible = true;
                }

            }
            else
            {

                if (this.hScrollBar1.Visible)
                {
                    this.dataGridView1.Height += 17;
                    this.hScrollBar1.Visible = false;
                }


            }
            this.Invalidate(false);
        }
    }

    public struct SumCellInfo
    {
        public int ColumnIndex;
        public decimal Value;
        public SumCellInfo(int columnIndex, decimal value)
        {
            ColumnIndex = columnIndex;
            Value = value;
        }

    }
}
