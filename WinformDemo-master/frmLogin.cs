// frmLogin.cs
using System.Data;
using System.Data.SqlClient;
using SQL; // 确保引用了您的SQLHelper命名空间

namespace myproject
{
    public partial class frmLogin : Form
    {
        // 用于传递登录成功的用户学号给主窗体
        public string LoggedInStudentNo { get; private set; }

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // 窗体加载时，尝试读取本地保存的凭据
            var (username, password) = CredentialManager.LoadCredentials();
            if (username != null && password != null)
            {
                txtUsername.Text = username;
                txtPassword.Text = password;
                chkRememberMe.Checked = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string studentNo = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(studentNo) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("学号和密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SQLHelper sh = new SQLHelper();
            // 查询语句，同时获取哈希和盐
            string sql = "SELECT pwd, salt FROM tblTopStudents WHERE studentNo = @studentNo";
            SqlParameter[] prams = { new SqlParameter("@studentNo", studentNo) };

            try
            {
                DataSet ds = new DataSet();
                sh.RunSQL(sql, prams, ref ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string storedHash = ds.Tables[0].Rows[0]["pwd"]?.ToString() ?? "";
                    string storedSalt = ds.Tables[0].Rows[0]["salt"]?.ToString() ?? "";

                    // 使用 PasswordHelper 进行验证
                    if (PasswordHelper.VerifyPassword(password, storedHash, storedSalt))
                    {
                        // 登录成功
                        if (chkRememberMe.Checked)
                        {
                            CredentialManager.SaveCredentials(studentNo, password);
                        }
                        else
                        {
                            CredentialManager.DeleteCredentials();
                        }

                        LoggedInStudentNo = studentNo; // 保存学号
                        this.DialogResult = DialogResult.OK; // 设置对话框结果
                        this.Close(); // 关闭登录窗体
                    }
                    else
                    {
                        MessageBox.Show("学号或密码错误！", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("学号或密码错误！", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录时发生数据库错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llblChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 打开修改密码窗体
            // 注意：修改密码需要知道当前是哪个用户，所以这里不适合直接打开
            // 这个功能更适合在主窗体登录后操作
            MessageBox.Show("请登录后在主界面修改密码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void llblForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 打开找回密码窗体
            frmForgotPassword forgotPwdForm = new frmForgotPassword();
            forgotPwdForm.ShowDialog();
        }
    }
}