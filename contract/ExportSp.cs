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
    public partial class ExportSp : Form
    {
        public ExportSp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DBAdo.DtFillSql(string.Format("SELECT HDW 公司名,合同号,客户名,签定日期,商品,型号,材质,数量单位,数量,单价,数量*单价 总价,gdz 单重,ZZ 总重 FROM VCX1 where 1=1 and hdw like '" + ClassCustom.codeSub(this.comboBox3.Text.ToString()) + "%' and 商品!='' and 商品 is not null and hlx like '{0}%' {1} ORDER BY 商品,材质,型号,HDW,客户名,数量,单价", ClassCustom.codeSub(this.comboBox2.Text), string.Format((this.checkBox1.Checked ? " and 签定日期 >= '{0}'  and 签定日期 <= '{1}' " : ""), this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString())));
                //foreach (DataRow r in dt.Rows)
                //{
                //    r[0] = DBAdo.ExecuteScalarSql(string.Format("select cname from aclients where ccode = '{0}'", r[0].ToString())).ToString();
                //}
                this.dataGridView1.DataSource = dt.DefaultView;
                this.dataGridView1.AutoResizeColumns();
                this.dataGridView1.Columns[3].Frozen = true;
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
            ClassCustom.ExportDataGridview1(this.dataGridView1, "商品明细");
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    if (this.textBox1.Text == "zdgs")
        //    {
        //        this.button1.Enabled = true;
        //        this.button2.Enabled = true;
        //        this.button3.Enabled = false;
        //        this.textBox1.Enabled = false;
        //    }
        //    else
        //    {
        //        MessageBox.Show("密码错误！");
        //        this.textBox1.Text = "";
        //        this.button1.Enabled = false;
        //        this.button2.Enabled = false;
        //    }
        //}

        private void ExportSp_Load(object sender, EventArgs e)
        {
            try
            {
                this.comboBox1.DataSource = DBAdo.DtFillSql("select lid+':'+lname lx from alx where len(lid) = 2 ");
                this.comboBox1.DisplayMember = "lx";
                DataTable dt = DBAdo.DtFillSql("SELECT CCODE+':'+CNAME cname FROM ACLIENTS WHERE CCODE LIKE '01%'");
                foreach (DataRow r in dt.Rows)
                {
                    if (ClassCustom.codeSub(r[0].ToString()) == "01")
                    {
                        if (ClassConstant.USER_ID == "0101999999" || ClassConstant.USER_ID == "0101010001")
                        {
                            r[0] = "01:全部";
                        }
                        else
                        {
                            this.comboBox3.Enabled = false;
                        }
                        break;
                    }
                }

                this.comboBox3.DataSource = dt;
                this.comboBox3.DisplayMember = "cname";
                //this.comboBox3.ValueMember = "ccode";

                this.comboBox3.Text = ClassConstant.DW_ID + ":" + ClassConstant.DW_NAME;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comboBox2.DataSource = DBAdo.DtFillSql("select lid+':'+lname lx from alx where lid like  '" + ClassCustom.codeSub(this.comboBox1.Text) + "__' ");
                this.comboBox2.DisplayMember = "lx";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
