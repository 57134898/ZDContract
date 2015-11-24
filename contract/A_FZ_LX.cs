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
    public partial class A_FZ_LX : Form
    {
        private string lxName;

        public A_FZ_LX()
        {
            InitializeComponent();
        }

        public A_FZ_LX(string lxName)
        {
            InitializeComponent();
            this.lxName = lxName;
        }

        private void A_FZ_LX_Load(object sender, EventArgs e)
        {
            this.Text = ClassCustom.codeSub(lxName);
            DataTable dt = DBAdo.DtFillSql("SELECT LID,LNAME FROM ALX WHERE LID LIKE '__'");



            Button b = new Button();
            b.Size = new System.Drawing.Size(140, 49);

            b.Text = "button1";
            b.UseVisualStyleBackColor = true;
            //b.Click += new System.EventHandler(this.button1_Click);
        }

        private void button_Click(object sender, EventArgs e)
        {

        }
    }
}
