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
    public partial class Form1 : Form
    {
        private DataTable dt1;
        private DataTable dt2;
        private DataView dv;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.numericUpDown1.Value = DateTime.Now.Year;
                this.numericUpDown2.Value = DateTime.Now.Month;
                this.comboBox1.Text = "在建工程";

                if (ClassConstant.USER_NAME == "于萍" || ClassConstant.USER_ID == "0101999999")
                {
                    this.comboBox2.Enabled = true;
                    DataTable csouce2 = DBAdo.DtFillSql("SELECT CCODE+':'+CNAME CODENAME FROM N7_铸锻公司..CCODE WHERE CCODE LIKE '01__' OR CCODE LIKE '11__'");
                    //DataTable csouce1 = DBHelper.DtFillSql("SELECT ACODE+':'+ANAME CODENAME FROM N7_铸锻公司..ACODE WHERE ACODE IN ('1122','112201','1221','122103','122104','1604','160401','2202','220201','220202','2241','224104')");
                    //DataTable csouce3 = DBHelper.DtFillSql("SELECT LID+':'+LNAME CODENAME FROM ALX WHERE LEN(LID)=2 and ( lid like '01%' OR lid like '02%' OR lid like '03%' OR lid like '04%')");
                    //this.comboBox1.DataSource = csouce1;
                    this.comboBox2.DataSource = csouce2;
                    //this.comboBox3.DataSource = csouce3;
                    //this.comboBox1.DisplayMember = "CODENAME";
                    this.comboBox2.DisplayMember = "CODENAME";
                    //this.comboBox3.DisplayMember = "CODENAME";
                }
                else
                {
                    string sql = string.Format("SELECT BCODE +':'+BNAME,SUBSTRING(BCODE,3,2) from N7_铸锻公司..Bcode where LEN(BCODE)=4 and BCODE <300 AND SUBSTRING(BCODE,3,2)='{0}'", ClassConstant.DW_ID.Substring(2, 2));
                    DataTable dt = DBAdo.DtFillSql(sql);
                    foreach (DataRow item in dt.Rows)
                    {
                        this.comboBox2.Items.Add(item[0].ToString());
                    }
                    //this.comboBox2.Text = ClassConstant.DW_ID + ":" + ClassConstant.DW_NAME;
                    //this.comboBox2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ClassCustom.ExportDataGridview1(this.dataGridView1, "未对上");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string dw = this.codeSub(this.comboBox2.Text);
                string lx = "";
                string acode = "";
                string year = this.numericUpDown1.Value.ToString();
                string month = this.numericUpDown2.Value.ToString();
                string nb = "";
                string sql = "";
                if (this.comboBox1.Text == "在建工程")
                {
                    lx = string.Format(" AND (HLX LIKE '04%' OR HLX LIKE '07%') ");
                    acode = " AND (acode like '160401%') ";
                    nb = "   AND ACODE LIKE  '122103%' AND DCODE LIKE '122103026%'    ";//新加
                    sql = string.Format("SELECT CCODE 客户码,CNAME 客户名 FROM ACLIENTS WHERE 1=1 AND (CCODE LIKE '01%' OR CCODE LIKE '11%' OR CCODE LIKE '05%' OR CCODE LIKE '03{0}%') ORDER BY CCODE", dw.Substring(2));
                }
                else if (this.comboBox1.Text == "销售合同")
                {
                    lx = string.Format(" AND (HLX LIKE '02%') ");
                    acode = " AND (acode like '112201%') ";
                    nb = "   AND ACODE LIKE  '122103%' AND DCODE LIKE '122103004%'    ";
                    sql = string.Format("SELECT CCODE 客户码,CNAME 客户名 FROM ACLIENTS WHERE 1=1 AND (CCODE LIKE '01%' OR CCODE LIKE '11%'  OR CCODE LIKE '05%' OR CCODE LIKE '02{0}%') ORDER BY CCODE", dw.Substring(2));
                }
                else if (this.comboBox1.Text == "采购合同")
                {
                    nb = "   AND ACODE LIKE  '122103%' AND DCODE LIKE '122103001%'    ";
                    lx = string.Format(" AND (HLX LIKE '01%') ");
                    acode = " AND (acode like '220201%') ";
                    sql = string.Format("SELECT CCODE 客户码,CNAME 客户名 FROM ACLIENTS WHERE 1=1 AND (CCODE LIKE '01%' OR CCODE LIKE '11%'  OR CCODE LIKE '05%' OR CCODE LIKE '02{0}%') ORDER BY CCODE", dw.Substring(2));
                }
                else if (this.comboBox1.Text == "外协合同")
                {
                    nb = "   AND ACODE LIKE  '122103%' AND DCODE LIKE '122103002%'    ";
                    lx = string.Format(" AND (HLX LIKE '03%') ");
                    acode = " AND (acode like '220202%') ";
                    sql = string.Format("SELECT CCODE 客户码,CNAME 客户名 FROM ACLIENTS WHERE 1=1 AND (CCODE LIKE '01%' OR CCODE LIKE '11%'  OR CCODE LIKE '05%' OR CCODE LIKE '02{0}%') ORDER BY CCODE", dw.Substring(2));

                }
                else
                {
                    throw new Exception("未知操作");
                }

                //sql = "SELECT CCODE 客户码,CNAME 客户名 FROM ACLIENTS WHERE 1=1 AND " + (this.codeSub(this.comboBox3.Text) == "04" ? "" : " CCODE LIKE '01%' OR CCODE LIKE '05%' OR ") + "CCODE LIKE '" + (this.codeSub(this.comboBox3.Text) == "04" ? "03" : "02") + this.codeSub(this.comboBox2.Text).Substring(2) + "%'";
                dt1 = DBAdo.DtFillSql(sql);
                dt1.Columns.Add("合同余额", typeof(decimal));
                dt1.Columns.Add("财务余额", typeof(decimal));
                dt1.Columns.Add("差", typeof(decimal), "合同余额 - 财务余额");

                string datecon = string.Format(" and (year(DATE) < {0} or (year(DATE) ={1} and month(DATE) <={2})) ", year, year, month);
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = dt1.Rows.Count;
                this.dataGridView1.Visible = false;
                this.progressBar1.Visible = true;

                foreach (DataRow r in dt1.Rows)
                {
                    Application.DoEvents();
                    this.groupBox1.Text = string.Format("处理进度{0}/{1}", this.progressBar1.Value.ToString(), this.progressBar1.Maximum.ToString());
                    //合同余额
                    string hths = string.Format("SELECT HCODE FROM ACONTRACT WHERE HDW ='{0}' AND HKH like '{1}%' {2}", new object[] { dw, r[0].ToString(), lx });
                    string sql1 = string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 and type like '_款' AND HTH IN ({0}) " + datecon + " GROUP BY TYPE", hths);
                    string sql2 = string.Format("SELECT SUM(RMB) FROM AFKXX WHERE 1=1 and type like '__发票' AND HTH IN ({0}) " + datecon + "  GROUP BY TYPE", hths);
                    object o1 = DBAdo.ExecuteScalarSql(sql1);
                    object o2 = DBAdo.ExecuteScalarSql(sql2);
                    if (o1 == null) { o1 = "0"; }
                    if (o2 == null) { o2 = "0"; }
                    decimal result = 0;
                    if (this.comboBox1.Text == "销售合同" && r[0].ToString().Substring(0, 2) == "01")
                    {
                        result = -1 * (decimal.Parse(o2.ToString() == "" ? "0" : o2.ToString()) - decimal.Parse(o1.ToString() == "" ? "0" : o1.ToString()));
                    }
                    else
                    {
                        result = decimal.Parse(o2.ToString() == "" ? "0" : o2.ToString()) - decimal.Parse(o1.ToString() == "" ? "0" : o1.ToString());
                    }

                    r[2] = result;
                    //财务余额
                    string sqlcw = "";
                    if (r[0].ToString().Substring(0, 2) == "01" || r[0].ToString().Substring(0, 2) == "11")
                    {
                        sqlcw = string.Format("SELECT SUM(RMBbalance) FROM N7_铸锻公司..BALANCE WHERE CCODE like '{0}%' {1} and bcode like '{2}%' and year = " + year + " and month = " + month, r[0].ToString(), nb, dw);
                    }
                    else
                    {
                        sqlcw = string.Format("SELECT SUM(RMBbalance) FROM N7_铸锻公司..BALANCE WHERE CCODE like '{0}%' {1} and bcode like '{2}%' and year = " + year + " and month = " + month, r[0].ToString(), acode, dw);
                    }
                    object result1 = DBAdo.ExecuteScalarSql(sqlcw);
                    if (this.comboBox1.Text == "在建工程" || r[0].ToString().Substring(0, 2) == "01")
                    {
                        r[3] = decimal.Parse((result1.ToString() == "" ? "0" : result1.ToString())) * (-1);

                    }
                    else
                    {
                        r[3] = (result1.ToString() == "" ? "0" : result1.ToString());

                    }
                    this.progressBar1.Value++;
                }

                this.progressBar1.Visible = false;
                this.dataGridView1.Visible = true;
                dv = dt1.DefaultView;
                this.dataGridView1.DataSource = dv;
                this.dataGridView1.Columns[0].FillWeight = 100;
                this.dataGridView1.Columns[1].FillWeight = 300;
                this.dataGridView1.Columns[2].FillWeight = 150;
                this.dataGridView1.Columns[3].FillWeight = 150;
                this.dataGridView1.Columns[4].FillWeight = 100;
                this.dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dataGridView1.Columns[2].DefaultCellStyle.Format = "N2";
                this.dataGridView1.Columns[3].DefaultCellStyle.Format = "N2";
                this.dataGridView1.Columns[4].DefaultCellStyle.Format = "N2";

                int count = 0;
                foreach (DataGridViewRow r in this.dataGridView1.Rows)
                {
                    if (decimal.Parse(r.Cells[4].Value.ToString()) != 0)
                    {
                        r.DefaultCellStyle.BackColor = Color.LightGreen;
                        count++;
                    }
                }
                this.radioButton3.Checked = true;
                this.groupBox1.Text = string.Format("共[{1}]条记录其中[{0}]条记录未核对", count.ToString(), this.dataGridView1.Rows.Count.ToString());


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
            try
            {

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public string codeSub(String code)
        {
            try
            {
                if (code == "")
                    return "";
                if (code.IndexOf(":") < 0)
                {
                    return code;
                }
                else
                {
                    return code.Substring(0, code.IndexOf(":"));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return ""; }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                dv.RowFilter = "差 = 0";
            }
            if (this.radioButton2.Checked)
            {
                dv.RowFilter = "差<>0";
            }
            if (this.radioButton3.Checked)
            {
                dv.RowFilter = "";
            }

            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                if (decimal.Parse(r.Cells[4].Value.ToString()) != 0)
                {
                    r.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    r.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
