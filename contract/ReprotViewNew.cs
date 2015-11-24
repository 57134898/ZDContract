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
    public partial class ReprotViewNew : Form
    {
        private string reportName;
        public ReprotViewNew()
        {
            InitializeComponent();
        }

        public ReprotViewNew(string reportName)
        {
            InitializeComponent();
            this.reportName = reportName;
        }

        private void ReprotViewNew_Load(object sender, EventArgs e)
        {
            if (this.reportName == "集团合同类型汇总表(新)")
            {
                this.textBox1.Text = "集团合同类型汇总表";
            }
            if (this.reportName == "各单位签订合同情况表(新)")
            {
                this.textBox1.Text = "各单位签订合同情况表";
            }
            if (this.reportName == "各单位货款同期对比表(新)")
            {
                this.textBox1.Text = "各单位货款同期对比表";
            }
            if (this.reportName == "各单位货款回收汇总表(新)")
            {
                this.textBox1.Text = "各单位货款回收汇总表";
            }
            if (this.reportName == "铸锻公司全部采购外协合同汇总表(新)")
            {
                this.textBox1.Text = "铸锻公司全部采购外协合同汇总表";
            }
            if (this.reportName == "合同类型总览表(新)")
            {
                this.textBox1.Text = "合同类型总览表";
            }
            this.textBox1.Text = @"/集团报表/" + this.textBox1.Text;
            ///集团报表/集团合同类型汇总表(新)
            ///集团报表/各单位签订合同情况表(新)
            ///集团报表/各单位货款同期对比表(新)
            ///集团报表/各单位货款回收汇总表(新)
            ///集团报表/铸锻公司全部采购外协合同汇总表(新)
            ///集团报表/合同类型总览表新)
            ///集团报表/集团毛利(新)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel1Collapsed = true;
            this.reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            this.reportViewer1.ServerReport.ReportServerUrl = new System.Uri("http://192.168.1.105/reportserver");
            this.reportViewer1.ServerReport.ReportPath = string.Format(this.textBox1.Text);
            this.reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = new System.Net.NetworkCredential("administrator", "abcd_1234", "");
            this.reportViewer1.RefreshReport();
        }
    }
}
