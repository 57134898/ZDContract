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
    public partial class BitForm : Form
    {
        public BitForm()
        {
            InitializeComponent();
        }

        private void BitForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.textBox3.Text = DateTime.Now.Year.ToString();
                DataTable dt = DBAdo.DtFillSql("SELECT * FROM ALX WHERE LEN(lid) = 2");
                this.comboBox3.DataSource = dt;
                this.comboBox3.ValueMember = "lid";
                this.comboBox3.DisplayMember = "lname";
                DataTable dt1 = DBAdo.DtFillSql("SELECT * FROM bcode");
                this.comboBox1.DataSource = dt1;
                this.comboBox1.ValueMember = "bcode";
                this.comboBox1.DisplayMember = "bname";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex >= 0 && this.textBox3.Text.Length > 0 && this.comboBox4.SelectedIndex >= 0)
            {
                DataLoad();

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex >= 0 && this.textBox3.Text.Length > 0 && this.comboBox4.SelectedIndex >= 0)
            {
                DataLoad();

            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex >= 0 && this.textBox3.Text.Length > 0 && this.comboBox3.SelectedIndex >= 0)
            {
                DataLoad();
            }
        }

        public void DataLoad()
        {
            try
            {
                string sql = string.Format(@"SELECT [ID],[TYear],[Bcode],[HLx],T1.LNAME ,[TType],[TID] FROM [Bids] T0
                                                INNER JOIN ALX T1 ON T0.HLx=T1.LID WHERE 1=1  AND bcode='{0}' and tyear = '{1}' and ttype = '{2}' and hlx = '{3}'", new string[] { this.comboBox1.SelectedValue.ToString(), this.textBox3.Text, this.comboBox4.Text, this.comboBox3.SelectedValue.ToString() });
                DataTable dt = DBAdo.DtFillSql(sql);
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns["ID"].Visible = false;
                this.dataGridView1.Columns["HLx"].Visible = false;
                this.dataGridView1.Columns["Bcode"].Visible = false;
                this.dataGridView1.Columns["TYear"].HeaderText = "年";
                this.dataGridView1.Columns["TType"].HeaderText = "中标方式";
                this.dataGridView1.Columns["TID"].HeaderText = "标号";
                this.dataGridView1.Columns["LNAME"].HeaderText = "合同类型";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex < 0 || this.comboBox3.SelectedIndex < 0 || this.comboBox4.SelectedIndex < 0)
            {
                return;
            }
            string sql = string.Format(@"
                                        DECLARE @Year NVARCHAR(20)
                                        DECLARE @Bcode NVARCHAR(20)
                                        DECLARE @Hlx NVARCHAR(20)
                                        DECLARE @Ttype NVARCHAR(20)
                                        SET @Year	=	'{0}'
                                        SET @Bcode	=	'{1}'
                                        SET @Hlx	=	'{2}'
                                        SET @Ttype	=	'{3}'

                                        IF EXISTS(SELECT * FROM dbo.Bids
                                        WHERE	TYEAR	=		@Year 
                                        AND		BCODE	=		@Bcode
                                        AND		HLX		LIKE	@Hlx+'%'
                                        AND		TTYPE	=		@Ttype)
                                        SELECT (SELECT shortname FROM bcode WHERE bcode = @Bcode)+'-'
                                        +(SELECT LNAME FROM ALX WHERE LID = @Hlx)+'-'+@Ttype+'-'+@Year+'-'+ 
                                        (SELECT TOP 1  dbo.PadLeft(SUBSTRING(TID,LEN(TID)-4,5)+1, '0', 6)  FROM dbo.Bids 
                                        WHERE	TYEAR	=		@Year 
                                        AND		BCODE	=		@Bcode
                                        AND		HLX		LIKE	@Hlx+'%'
                                        AND		TTYPE	=		@Ttype
                                        ORDER BY 1 DESC )
                                        ELSE
                                        SELECT (SELECT shortname FROM bcode WHERE bcode = @Bcode)+'-'
                                        +(SELECT LNAME FROM ALX WHERE LID = @Hlx)+'-'+@Ttype+'-'+@Year+'-'+'00001'"
                , new string[] { DateTime.Now.Year.ToString(), this.comboBox1.SelectedValue.ToString(), this.comboBox3.SelectedValue.ToString(), this.comboBox4.SelectedItem.ToString() });
            string result = DBAdo.ExecuteScalarSql(sql).ToString();
            this.textBox1.Text = result;
            MessageBox.Show(result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                return;
            }
            string sql = string.Format(@"INSERT INTO [Bids]([TYear],[Bcode],[HLx],[TType],[TID]) VALUES('{0}','{1}','{2}','{3}','{4}')"
                , new string[] { 
                 DateTime.Now.Year.ToString(), 
                 this.comboBox1.SelectedValue.ToString(),
                 this.comboBox3.SelectedValue.ToString(), 
                 this.comboBox4.SelectedItem.ToString(),
                 this.textBox1.Text
                });
            try
            {
                DBAdo.ExecuteNonQuerySql(sql);
                MessageBox.Show("添加成功");
                this.textBox1.Text = "";
                this.DataLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }


        }


    }
}
