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
    public partial class CustomDataGridView : DataGridView
    {
        public CustomDataGridView()
        {
            InitializeComponent();
            this.EnableHeadersVisualStyles = false;
            //this.RowHeadersVisible = false;
            this.Scroll += new ScrollEventHandler(CustomDataGridView_Scroll);
            this.SizeChanged += new EventHandler(CustomDataGridView_SizeChanged);
            this.ColumnWidthChanged += new DataGridViewColumnEventHandler(CustomDataGridView_ColumnWidthChanged);
            this.AllowUserToResizeRows = false;
            this.ColumnHeadersHeight = 50;
            sf.Alignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.LineLimit;
            sf.LineAlignment = StringAlignment.Center;
        }

        private string colName1 = "类型";


        public string ColName1
        {
            get { return colName1; }
            set
            {
                colName1 = value;
                this.Invalidate();
            }
        }

        private string colName2 = "估验";


        public string ColName2
        {
            get { return colName2; }
            set
            {
                colName2 = value;
                this.Invalidate();
            }
        }



        void CustomDataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.Invalidate();
        }

        void CustomDataGridView_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void CustomDataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.FillRectangle(sBack, 1, 1, this.Width, 49);
            pe.Graphics.DrawLine(pLine, 0, 50, this.RowHeadersWidth, 50);
            base.OnPaint(pe);
        }

        private SolidBrush sBack = new SolidBrush(Color.LightSkyBlue);
        private SolidBrush sText = new SolidBrush(SystemColors.ControlText);
        private Font f = new Font("宋体", 10);
        private StringFormat sf = new StringFormat();
        private Pen pText = new Pen(SystemColors.ControlDark);
        private Pen pLine = new Pen(SystemColors.ControlDarkDark);
        public List<SpanInfo> Ls { get; set; }


        public struct SpanInfo //表头信息
        {
            public SpanInfo(string Text, int ColumnsIndex1, int ColumnsIndex2)
            {
                this.Text = Text;
                this.ColumnsIndex1 = ColumnsIndex1;
                this.ColumnsIndex2 = ColumnsIndex2;
            }
            public string Text; //列主标题
            public int ColumnsIndex1;
            public int ColumnsIndex2;
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == -1)
            //if (e.ColumnIndex == 2 || e.ColumnIndex == 4 || e.ColumnIndex == 3 || e.ColumnIndex == 5 || e.ColumnIndex == -1)
            {
                base.OnCellMouseDown(e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!(e.KeyValue == 37 || e.KeyValue == 39))
                base.OnKeyDown(e);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {

            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            base.OnCellPainting(e);
            if (e.RowIndex == -1)
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (this.GetCellDisplayRectangle(i, -1, false).X != 0)
                    {


                        //Rectangle r = this.GetCellDisplayRectangle(i, -1, false);

                        //if (i == 4 || i == 5 || i == 6)
                        //{

                        //    if (i == 4)
                        //    {
                        //        int _width = this.GetCellDisplayRectangle(i, -1, false).Width + this.GetCellDisplayRectangle(i + 1, -1, false).Width + this.GetCellDisplayRectangle(i + 2, -1, false).Width;
                        //        e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1, _width, r.Height / 2);
                        //        e.Graphics.DrawString(ColName1, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y, _width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                        //    }
                        //    e.Graphics.DrawString(this.Columns[i].HeaderText, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y + this.GetCellDisplayRectangle(i, -1, false).Height / 2, this.GetCellDisplayRectangle(i, -1, false).Width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                        //    e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1 + r.Height / 2, r.Width, r.Height / 2);
                        //    continue;
                        //}

                        //if (i == 7 || i == 8 || i == 9 || i == 10 || i == 11)
                        //{

                        //    if (i == 7)
                        //    {
                        //        int _width = this.GetCellDisplayRectangle(i, -1, false).Width + this.GetCellDisplayRectangle(i + 1, -1, false).Width + this.GetCellDisplayRectangle(i + 2, -1, false).Width + this.GetCellDisplayRectangle(i + 3, -1, false).Width;
                        //        e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1, this.GetCellDisplayRectangle(i, -1, false).Width + _width, r.Height / 2);
                        //        e.Graphics.DrawString(ColName2, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y, _width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                        //    }
                        //    e.Graphics.DrawString(this.Columns[i].HeaderText, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y + this.GetCellDisplayRectangle(i, -1, false).Height / 2, this.GetCellDisplayRectangle(i, -1, false).Width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                        //    e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1 + r.Height / 2, r.Width, r.Height / 2);
                        //    continue;
                        //}
                        //e.Graphics.DrawString(this.Columns[i].HeaderText, f, sText, this.GetCellDisplayRectangle(i, -1, false), sf);
                        //e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1, r.Width, r.Height);


                        Rectangle r = this.GetCellDisplayRectangle(i, -1, false);
                        if (i == 1 || i == 2)
                        {
                            if (i == 1)
                            {
                                e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1, this.GetCellDisplayRectangle(i, -1, false).Width + this.GetCellDisplayRectangle(i + 1, -1, false).Width, r.Height / 2);
                                e.Graphics.DrawString(ColName1, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y, this.GetCellDisplayRectangle(i, -1, false).Width + this.GetCellDisplayRectangle(i + 1, -1, false).Width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                            }
                            e.Graphics.DrawString(this.Columns[i].HeaderText, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y + this.GetCellDisplayRectangle(i, -1, false).Height / 2, this.GetCellDisplayRectangle(i, -1, false).Width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                            e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1 + r.Height / 2, r.Width, r.Height / 2);
                            continue;
                        }
                        if (i == 3 || i == 4)
                        {
                            if (i == 3)
                            {
                                e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1, this.GetCellDisplayRectangle(i, -1, false).Width + this.GetCellDisplayRectangle(i + 1, -1, false).Width, r.Height / 2);
                                e.Graphics.DrawString(ColName2, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y, this.GetCellDisplayRectangle(i, -1, false).Width + this.GetCellDisplayRectangle(i + 1, -1, false).Width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                            }
                            e.Graphics.DrawString(this.Columns[i].HeaderText, f, sText, new Rectangle(this.GetCellDisplayRectangle(i, -1, false).X, this.GetCellDisplayRectangle(i, -1, false).Y + this.GetCellDisplayRectangle(i, -1, false).Height / 2, this.GetCellDisplayRectangle(i, -1, false).Width, this.GetCellDisplayRectangle(i, -1, false).Height / 2), sf);
                            e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1 + r.Height / 2, r.Width, r.Height / 2);
                            continue;
                        }
                        e.Graphics.DrawString(this.Columns[i].HeaderText, f, sText, this.GetCellDisplayRectangle(i, -1, false), sf);
                        e.Graphics.DrawRectangle(pLine, r.X - 1, r.Y - 1, r.Width, r.Height);




                    }

                }
                e.Handled = true;
            }
            else
            {
                base.OnCellPainting(e);
            }

        }
        // public MergeColumns
    }
}
