using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Windows.Forms;

namespace mailscreenshotl;

public class EmailClient
{
    private static string CaptureFullScreen()
    {
        try
        {
            Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss");
            screenshot.Save($"{filename}.png", ImageFormat.Png);
            return filename;
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.ToString());
            return "";
        }
    }
    public static bool SendEmail(string email, string title, string content)
    {
        string smtpServer = "smtp.qq.com"; // SMTP服务器
        string userName = "393025885@qq.com"; // 邮箱账号
        string password = "oxqexxzjljgecajh"; // 密码或授权码

        if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        using (System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage())
        {
            msg.To.Add(email); // 设置收件人
            msg.From = new MailAddress(userName, userName, System.Text.Encoding.UTF8);
            msg.Subject = title; // 邮件标题
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = content; // 邮件内容
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true; // 是否是HTML邮件
            msg.Priority = MailPriority.Normal;
            // 截屏
            string path = CaptureFullScreen();
            // 添加附件
            Attachment attachment = new Attachment($"{path}.png"); // 附件路径
            msg.Attachments.Add(attachment);

            SmtpClient client = new SmtpClient(smtpServer, 587); // 邮件服务器地址及端口号
            client.EnableSsl = true; // 使用SSL加密发送
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential(userName, password); // 邮箱账号和密码
            client.Timeout = 6000; // 超时时间

            try
            {
                client.Send(msg); // 发送邮件
                client.Dispose(); // 释放资源
                return true;
            }
            catch (System.Net.Mail.SmtpException)
            {
                return false;
            }
        }
    }

}
