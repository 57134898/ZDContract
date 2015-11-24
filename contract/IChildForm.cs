using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace contract
{
    /// <summary>
    /// 子窗体接口
    /// </summary>
    public interface IChildForm
    {
        void FormActivated(object sender, EventArgs e);
        void FormDeactivate(object sender, EventArgs e);
        void Form_Closing(object sender, EventArgs e);
    }
}
