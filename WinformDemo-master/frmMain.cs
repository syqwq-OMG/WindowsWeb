using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myproject
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnHomework2_Click(object sender, EventArgs e)
        {
            frmAlbum fm2 = new frmAlbum();
            fm2.ShowDialog(); //显示模态窗体
        }

        private void btnHomework1_Click(object sender, EventArgs e)
        {
            //fm24就是frm24grade的实例
            frm24grade fm24 = new frm24grade();
            fm24.ShowDialog(); //显示模态窗体
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmSearchFile frmSearch = new frmSearchFile();
            frmSearch.ShowDialog(); //显示模态窗体
        }

        private void btnCrawer_Click(object sender, EventArgs e)
        {
            frmTiebaSpider frmSpider = new frmTiebaSpider();
            frmSpider.ShowDialog();
        }

        private void btnControls_Click(object sender, EventArgs e)
        {
            frm24grade fm24 = new frm24grade();
            fm24.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FrmOcr frmOcr = new FrmOcr();
            frmOcr.ShowDialog();
        }

        private void btnSegment_Click(object sender, EventArgs e)
        {
            frmSegment frmSegment = new frmSegment();
            frmSegment.ShowDialog();
        }

        private void btnSQL_Click(object sender, EventArgs e)
        {
            var frmSQL = new frmSQL();
            frmSQL.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            // Program.CurrentUserStudentNo 保存了登录时用户的学号
            if (!string.IsNullOrEmpty(Program.CurrentUserStudentNo))
            {
                // 创建修改密码窗体实例，并将当前用户的学号传递过去
                frmChangePassword changePasswordForm = new frmChangePassword(Program.CurrentUserStudentNo);
                changePasswordForm.ShowDialog(); // 以对话框模式显示
            }
            else
            {
                MessageBox.Show("错误：无法获取当前用户信息，请重新登录。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
