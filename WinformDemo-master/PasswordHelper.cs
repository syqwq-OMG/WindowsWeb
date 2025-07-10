using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// PasswordHelper.cs
using System;
using System.Security.Cryptography;
namespace myproject
{


    public static class PasswordHelper
    {
        // 生成一个加密安全的盐
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // 使用指定的盐对密码进行哈希处理
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // 将密码和盐组合成一个字节数组
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password + salt);
                // 计算哈希值
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                // 将哈希值转换为Base64字符串以便存储
                return Convert.ToBase64String(hashBytes);
            }
        }

        // 验证密码是否正确
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            // 使用相同的盐对用户输入的密码进行哈希
            string hashOfEnteredPassword = HashPassword(enteredPassword, storedSalt);
            // 比较新生成的哈希和数据库中存储的哈希
            return string.Equals(hashOfEnteredPassword, storedHash);
        }
    }
}
