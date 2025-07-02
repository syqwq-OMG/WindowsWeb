using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hhhgallery
{
    public partial class PreviewForm : Form
    {
        private Picture _currentPicture;

        // 私有构造函数，防止无参数创建
        private PreviewForm()
        {
            InitializeComponent();
        }

        // 公共构造函数，接收一个Picture对象
        public PreviewForm(Picture picture)
        {
            InitializeComponent();
            _currentPicture = picture;
            LoadPicture();
        }

        private void LoadPicture()
        {
            if (_currentPicture != null)
            {
                this.Text = $"预览 - {_currentPicture.FileName}"; // 设置窗体标题
                pictureBoxPreview.ImageLocation = _currentPicture.FullPath;
            }
        }
    }
}
