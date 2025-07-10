using SQL;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace myproject
{
    public partial class frmForgotPassword : Form
    {
        private string? _verificationCode = null;
        private string? _studentNoToReset = null;
        private DateTime _codeSentTime;

        public frmForgotPassword()
        {
            InitializeComponent();
            // 初始化UI状态
            pnlStep2.Enabled = false;
        }

        private void btnSendCode_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (!email.Contains("@"))
            {
                MessageBox.Show("请输入有效的邮箱地址。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SQLHelper sh = new SQLHelper();
            try
            {
                string sql = "SELECT studentNo FROM tblTopStudents WHERE Email = @Email";
                SqlParameter[] p = { new SqlParameter("@Email", email) };
                DataSet ds = new DataSet();
                sh.RunSQL(sql, p, ref ds);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("该邮箱未注册，请确认。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _studentNoToReset = ds.Tables[0].Rows[0]["studentNo"].ToString();

                // 生成6位随机验证码
                _verificationCode = new Random().Next(100000, 999999).ToString();
                _codeSentTime = DateTime.Now;

                // 发送邮件
                bool success = EmailHelper.SendVerificationEmail(email, _verificationCode);
                if (success)
                {
                    MessageBox.Show("验证码已发送到您的邮箱，请注意查收。\n验证码5分钟内有效。", "发送成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pnlStep1.Enabled = false;
                    pnlStep2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("邮件发送失败，请检查您的网络或联系管理员。", "发送失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，数据库错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // 验证码5分钟有效期
            if (DateTime.Now - _codeSentTime > TimeSpan.FromMinutes(5))
            {
                MessageBox.Show("验证码已过期，请重新发送。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pnlStep1.Enabled = true;
                pnlStep2.Enabled = false;
                return;
            }

            if (code != _verificationCode)
            {
                MessageBox.Show("验证码错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("新密码和确认密码不一致。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (newPassword.Length < 6)
            {
                MessageBox.Show("为了安全，新密码长度不能少于6位。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // 更新密码
            SQLHelper sh = new SQLHelper();
            try
            {
                string newSalt = PasswordHelper.GenerateSalt();
                string newHash = PasswordHelper.HashPassword(newPassword, newSalt);

                string updateSql = "UPDATE tblTopStudents SET pwd = @pwd, salt = @salt WHERE studentNo = @studentNo";
                SqlParameter[] updateParams = {
                    new SqlParameter("@pwd", newHash),
                    new SqlParameter("@salt", newSalt),
                    new SqlParameter("@studentNo", _studentNoToReset)
                };

                int rowsAffected = sh.RunSQL(updateSql, updateParams);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("密码重置成功，您现在可以使用新密码登录了。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("密码重置失败，请稍后重试。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，数据库错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}