using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace contract
{
    public partial class WhPrint : Form
    {
        public WhPrint()
        {
            InitializeComponent();
        }

        public string InvGuid { get; set; }

        public string Customer { get; set; }

        public string Hcode { get; set; }

        public string InvCode { get; set; }

        public string IDate { get; set; }

        public string ITitle { get; set; }

        public string Head1Title { get; set; }

        public decimal Total { get; set; }

        public WhPrint(string InvGuid, string Customer, string Hcode, string InvCode, string IDate, string ITitle, string Head1Title, decimal Total)
        {
            InitializeComponent();
            this.Customer = Customer;
            this.Hcode = Hcode;
            this.InvCode = InvCode;
            this.IDate = IDate;
            this.ITitle = ITitle;
            this.Head1Title = Head1Title;
            this.InvGuid = InvGuid;
            this.Total = Total;
        }

        private void WhPrint_Load(object sender, EventArgs e)
        {
            try
            {
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "contract.ReportInvoice.rdlc";
                string sql = string.Format(@"SELECT T2.GNAME,T2.GCZ,T2.GXH,T2.GDW1,T2.GDW2,T2.GJM,T2.HTH,T1.*
                                            FROM[Invioce] T0 INNER JOIN  [InvoiceRows] T1 ON T0.InvID=T1.InvID
                                            INNER JOIN ASP T2 ON T1.SpID=T2.InvID WHERE T1.INVID='{0}'", this.InvGuid);
                DataTable dt = DBAdo.DtFillSql(sql);
                ReportDataSource reportDataSource = new ReportDataSource("DB_InvoiceDataSouce", dt);
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter rp1 = new ReportParameter("p1", this.Customer);
                ReportParameter rp2 = new ReportParameter("p2", this.Hcode);
                ReportParameter rp3 = new ReportParameter("p3", this.InvCode);
                ReportParameter rp4 = new ReportParameter("p4", this.IDate);
                ReportParameter rp5 = new ReportParameter("p5", this.ITitle);
                ReportParameter rp6 = new ReportParameter("p6", this.Head1Title);
                ReportParameter rp7 = new ReportParameter("p7", this.Total.ToString());
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7 });

                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 100;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
