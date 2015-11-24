using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace contract
{
    /// <summary>
    /// 点击按钮触发Click事件,Click事件委托
    /// </summary>    
    public delegate void OnButtonClick(object sender, EventArgs e);

    /// <summary>
    /// TooLStripButton 工厂
    /// </summary>
    public class Factory_ToolBtn
    {
        /// <summary>
        /// ToolStripButton
        /// </summary>
        private ToolStripButton _btn = null;

        public Factory_ToolBtn()
        {

        }

        /// <summary>
        /// 构造器重载
        /// </summary>
        /// <param name="caption">按钮标题</param>
        /// <param name="tooltiptext">鼠标移到按钮上的提示</param>
        /// <param name="image">按钮图片</param>
        /// <param name="onbuttonclick">按钮单击事件</param>
        public Factory_ToolBtn(string caption, string tooltiptext, Image image, OnButtonClick onbuttonclick, object tag, bool flag)
        {
            _btn = new ToolStripButton();
            this._btn.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            this._btn.ImageTransparentColor = Color.Magenta;
            this._btn.Size = new System.Drawing.Size(32, 32);
            this._btn.Text = caption;
            this._btn.Image = image;
            this._btn.ToolTipText = tooltiptext;
            this._btn.Click += new EventHandler(onbuttonclick);
            this._btn.Enabled = flag;
            this._btn.Tag = tag;
        }
        /// <summary>
        /// 生产按钮
        /// </summary>
        /// <returns>ToolStripButton生产按钮</returns>
        public ToolStripButton TBtnProduce()
        {
            return this._btn;
        }
    }
}
