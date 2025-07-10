using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace myproject
{
    public static class EmailHelper
    {
        /// <summary>
        /// 发送包含验证码的邮件。
        /// </summary>
        /// <param name="toEmail">收件人邮箱地址。</param>
        /// <param name="verificationCode">6位验证码。</param>
        /// <returns>发送成功返回true，否则返回false。</returns>
        public static bool SendVerificationEmail(string toEmail, string verificationCode)
        {
            // ======================= !! 重要配置 !! =======================
            string fromMail = "3422403944@qq.com";                 // 您的发件邮箱地址
            string fromPassword = "lfgondvppslecjhi";   // 您的邮箱SMTP授权码 (不是登录密码!)
            string smtpServer = "smtp.qq.com";                      // SMTP服务器地址 (QQ邮箱)
            int smtpPort = 587;                                     // SMTP端口 (通常是587或465)
                                                                    // =============================================================

            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail, "学生管理系统"); // 可以设置发件人名称
                message.Subject = "密码重置验证码";
                message.To.Add(new MailAddress(toEmail));
                message.Body = $"您好，\n\n您的密码重置验证码是： {verificationCode}\n\n该验证码5分钟内有效。请勿泄露给他人。\n\n此致\n学生管理系统";
                message.IsBodyHtml = false;

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true, // 启用SSL加密
                };

                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                // 在实际应用中，这里应该记录详细的错误日志
                Console.WriteLine("Email sending failed: " + ex.ToString());
                return false;
            }
        }
    }
}
