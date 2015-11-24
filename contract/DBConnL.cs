using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;

namespace contract
{
    public partial class DBConnL : Form
    {


        public DBConnL()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)      //保存配置
        {
            try
            {
                return;
                MessageBox.Show("数据库路径保存成功！请从新登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                try
                {
                    Process myProcess = new Process();
                    myProcess.StartInfo.FileName = this.textBox1.Text.ToString().Trim() + ".exe";
                    myProcess.Start();
                    Application.Exit();
                }
                catch
                {
                    try
                    {
                        Process myProcess = new Process();
                        myProcess.StartInfo.FileName = "contract.exe";
                        myProcess.Start();
                        Application.Exit();
                    }
                    catch
                    {
                        MessageBox.Show("程序文件名称错误或已经被修改！请改回原来的程序文件名称！", "提示");
                    }
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show("保存失败！请从新设定数据库配置！" + ex1, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)      //退出
        {
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)      //默认连接类型
        {
            this.comboBox1.Text = "SQL Server";
            this.comboBox2.Text = "SQL Server Framework 2.0";
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)     //数据库类型与驱动联动
        {
            if (this.comboBox1.Text == "SQL Server")
            {
                this.comboBox2.Items.Clear();
                this.comboBox2.Items.AddRange(new object[] {
            "SQL Server Framework 2.0",
            "SQL Server Framework 3.5",
            "Microsoft OLEDB Driver",
            "Microsoft ODBC Data Source"});
                this.comboBox2.Text = "SQL Server Framework 2.0";
            }
            else if (this.comboBox1.Text == "Access")
            {
                this.comboBox2.Items.Clear();
                this.comboBox2.Items.AddRange(new object[] {
            "Microsoft Access Driver 4.0",
            "Microsoft Access Driver 12.0"});
                this.comboBox2.Text = "Microsoft Access Driver 4.0";
            }
            else if (this.comboBox1.Text == "Oracle")
            {
                this.comboBox2.Items.Clear();
                this.comboBox2.Items.AddRange(new object[] {
            "Microsoft OLEDB Driver Oracle"});
                this.comboBox2.Text = "Microsoft OLEDB Driver Oracle";
            }
            else if (this.comboBox1.Text == "Sybase")
            {
                this.comboBox2.Items.Clear();
                this.comboBox2.Items.AddRange(new object[] {
            "Microsoft OLEDB Driver Sybase"});
                this.comboBox2.Text = "Microsoft OLEDB Driver Sybase";
            }
            else if (this.comboBox1.Text == "DB2")
            {
                this.comboBox2.Items.Clear();
                this.comboBox2.Items.AddRange(new object[] {
            "Microsoft OLEDB Driver DB2"});
                this.comboBox2.Text = "Microsoft OLEDB Driver DB2";
            }
            else if (this.comboBox1.Text == "MySQL")
            {
                this.comboBox2.Items.Clear();
                this.comboBox2.Items.AddRange(new object[] {
            "Microsoft OLEDB Driver MySQL"});
                this.comboBox2.Text = "Microsoft OLEDB Driver MySQL";
            }
            else
            {
                MessageBox.Show("无法保存该连接", "提示");
            }
        }
    }
}