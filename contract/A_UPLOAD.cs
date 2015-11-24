using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace contract
{
    public partial class A_UPLOAD : Form
    {
        private int bj = 3;
        private string fileName;
        private SqlConnection conn = new SqlConnection("Data Source=192.168.7.70;Initial Catalog=contract1;User ID=sa;");
        public A_UPLOAD()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.richTextBox1.Text = this.openFileDialog1.FileName;
            fileName = this.richTextBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.progressBar1.Value = 0;
                while (this.progressBar1.Value < this.progressBar1.Maximum / 5 * 4) { Thread.Sleep(10); this.progressBar1.Value++; }
                FileInfo finfo = new FileInfo(this.richTextBox1.Text);  //绝对路径 
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE ASYSCONFIGS SET sysexe = @Content WHERE SYSID = 1";
                cmd.Parameters.Add("@Content", SqlDbType.Image, (int)finfo.Length);  //此处参数Size为写入的字节数 
                //读取文件内容，写入byte数组 
                byte[] content = new byte[finfo.Length];
                FileStream stream = finfo.OpenRead();
                stream.Read(content, 0, content.Length);
                stream.Flush();
                stream.Close();
                cmd.Parameters["@Content"].Value = content;  //为参数赋值 
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                while (this.progressBar1.Value < this.progressBar1.Maximum) { Thread.Sleep(100); this.progressBar1.Value++; }
                MessageBox.Show("上传成功");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text == "")
            {
                this.button2.Enabled = false;
            }
            else
            {
                this.button2.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bj == 0)
            {
                Application.Exit();
            }
            if (this.textBox1.Text == "snsoft123")
            {
                this.groupBox2.Enabled = true;
            }
            else
            {
                this.groupBox2.Enabled = false;
                bj -= 1;
            }
        }

        private void A_UPLOAD_Load(object sender, EventArgs e)
        {

        }

    }
}
