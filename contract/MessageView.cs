using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace contract
{
    public class MessageView
    {
        public static void MessageErrorShow(Exception ex)
        {
            MessageBox.Show(ex.Message, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult MessageYesNoShow(string _message)
        {
            return MessageBox.Show(_message, "是(Y)/否(N)", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

    }
}
