using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace myproject
{
    public static class CredentialManager
    {
        // 定义一个固定的“熵”值，增加加密的复杂性
        private static readonly byte[] s_entropy = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static readonly string CredentialFile = Path.Combine(Application.LocalUserAppDataPath, "user.dat");

        public static void SaveCredentials(string username, string password)
        {
            try
            {
                // 将用户名和密码组合
                string credentials = $"{username}\n{password}";
                byte[] dataToProtect = Encoding.UTF8.GetBytes(credentials);

                // 使用Windows数据保护API加密数据
                byte[] protectedData = ProtectedData.Protect(dataToProtect, s_entropy, DataProtectionScope.CurrentUser);

                File.WriteAllBytes(CredentialFile, protectedData);
            }
            catch (Exception ex)
            {
                // 可以选择性地记录错误，但不向用户显示
                Console.WriteLine("Failed to save credentials: " + ex.Message);
            }
        }

        public static (string? Username, string? Password) LoadCredentials()
        {
            if (!File.Exists(CredentialFile))
            {
                return (null, null);
            }

            try
            {
                byte[] protectedData = File.ReadAllBytes(CredentialFile);
                // 解密数据
                byte[] data = ProtectedData.Unprotect(protectedData, s_entropy, DataProtectionScope.CurrentUser);
                string credentials = Encoding.UTF8.GetString(data);

                var parts = credentials.Split('\n');
                if (parts.Length == 2)
                {
                    return (parts[0], parts[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load credentials: " + ex.Message);
                DeleteCredentials(); // 如果解密失败，删除坏文件
            }

            return (null, null);
        }

        public static void DeleteCredentials()
        {
            if (File.Exists(CredentialFile))
            {
                File.Delete(CredentialFile);
            }
        }
    }
}
