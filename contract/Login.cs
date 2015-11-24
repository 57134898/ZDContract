using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace contract
{
    public partial class Login : Form
    {
        private DataTable dt1;//公司
        private DataTable dt2;//部门
        private DataTable dt3;//用户

        public Login()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.textBox1.Text == "" || this.textBox2.Text == "")
                {
                    MessageBox.Show("用户名或密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string sql = string.Format("SELECT * FROM AUSERS");
                DataTable users = DBAdo.DtFillSql(sql);
                bool flag = false;
                foreach (DataRow r in users.Rows)
                {
                    if (this.textBox1.Text == r["uname"].ToString() && this.textBox2.Text == r["upassword"].ToString())
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load("info.xml");
                        XmlNode xn = xmlDoc.SelectSingleNode("system/username");
                        xn.InnerText = this.textBox1.Text;
                        xmlDoc.Save("info.xml");

                        ClassConstant.DW_ID = ClassCustom.codeSub(r["udw"].ToString());
                        ClassConstant.DW_NAME = ClassCustom.codeSub1(r["udw"].ToString());
                        ClassConstant.BCODE = ClassCustom.codeSub(r["ubm"].ToString());
                        ClassConstant.BNAME = ClassCustom.codeSub1(r["ubm"].ToString());
                        ClassConstant.USER_ID = r["ucode"].ToString();
                        ClassConstant.USER_NAME = r["uname"].ToString();


                        ClassConstant.QX = ClassConstant.QXCOLLECTIONS[r["pcode"].ToString() == "" ? "00" : r["pcode"].ToString()];
                        ((MForm1)this.MdiParent).LogInApplication();
                        flag = true;

                        //string s = string.Format("UPDATE AUSERS SET LASTIP = '{0}' WHERE UNAME = '{1}'", ClassCustom.GetIP(), this.textBox1.Text);
                        //DBAdo.ExecuteNonQuerySql(s);
                        break;
                    }
                }
                if (flag)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名或密码不正确\r请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
                return;
            }
            //if (this.textBox1.Text == "" || this.comboBox3.Text == "")
            //{
            //    MessageBox.Show("请输入用户名和密码！");
            //    return;
            //}
            //object flag = DBAdo.ExecuteScalarSql("select * from ausers where ucode  ='" + this.comboBox3.SelectedValue.ToString() + "' and UPASSWORD = '" + this.textBox1.Text + "'");
            //if (flag == null)
            //{
            //    MessageBox.Show("密码错误！");
            //    return;
            //}
            //ClassConstant.USER_ID = this.comboBox3.SelectedValue.ToString();
            //ClassConstant.USER_NAME = this.comboBox3.Text;
            //ClassConstant.DW_ID = this.comboBox1.SelectedValue.ToString();
            //ClassConstant.DW_NAME = this.comboBox1.Text;
            //ClassConstant.BCODE = this.comboBox2.SelectedValue.ToString();
            //ClassConstant.BNAME = this.comboBox2.Text;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                
                //MessageBox.Show(System.Environment.GetCommandLineArgs().Length.ToString());
                if (System.Environment.GetCommandLineArgs().Length > 1)
                {
                    this.textBox2.Text = System.Environment.GetCommandLineArgs()[1];
                }
                
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("info.xml");
                XmlNode xn = xmlDoc.SelectSingleNode("system/username");
                this.textBox1.Text = xn.InnerText;
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);
            }
        }

    }
}
