using System.Data.SqlClient;
using SQL;

namespace myproject
{
    public partial class frmChangePassword : Form
    {
        private readonly string _studentNo;

        // 构造函数需要传入当前登录的学号
        public frmChangePassword(string studentNo)
        {
            InitializeComponent();
            _studentNo = studentNo;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("旧密码和新密码不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            SQLHelper sh = new SQLHelper();
            try
            {
                // 1. 验证旧密码是否正确
                string selectSql = "SELECT pwd, salt FROM tblTopStudents WHERE studentNo = @studentNo";
                SqlParameter[] selectParams = { new SqlParameter("@studentNo", _studentNo) };
                var ds = new System.Data.DataSet();
                sh.RunSQL(selectSql, selectParams, ref ds);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("发生内部错误：找不到当前用户。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string storedHash = ds.Tables[0].Rows[0]["pwd"].ToString();
                string storedSalt = ds.Tables[0].Rows[0]["salt"].ToString();

                if (!PasswordHelper.VerifyPassword(oldPassword, storedHash, storedSalt))
                {
                    MessageBox.Show("旧密码错误！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. 生成新的盐和哈希
                string newSalt = PasswordHelper.GenerateSalt();
                string newHash = PasswordHelper.HashPassword(newPassword, newSalt);

                // 3. 更新数据库
                string updateSql = "UPDATE tblTopStudents SET pwd = @pwd, salt = @salt WHERE studentNo = @studentNo";
                SqlParameter[] updateParams = {
                    new SqlParameter("@pwd", newHash),
                    new SqlParameter("@salt", newSalt),
                    new SqlParameter("@studentNo", _studentNo)
                };

                int rowsAffected = sh.RunSQL(updateSql, updateParams);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("密码修改成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("密码修改失败，请稍后重试。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，数据库错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}