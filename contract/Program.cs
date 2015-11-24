using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace contract
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new A_LOCK());
            Application.Run(new MForm1());
            //Application.Run(new ReportView());

        }
    }
}
