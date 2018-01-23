using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    /// <summary>
    ///     整站邮件服务类
    /// </summary>
    public class EmailService
    {
        static string Host = "211.150.65.22";
        static string Port = "25";
        static string UserName = "pd@maxinsight.cn";
        static string Password = "Max2017";
        static string MailFrom = "pd@maxinsight.cn";
        

        /// <summary>
        ///     发送邮件
        /// </summary>
        /// <param name="mailTo">收件人
        /// <param name="mailCcArray">抄送
        /// <param name="subject">主题
        /// <param name="body">内容
        /// <param name="isBodyHtml">是否Html
        /// <param name="attachmentsPath">附件
        /// <returns></returns>
        public static bool Send(string[] mailTo, string[] mailCcArray, string subject, string body, bool isBodyHtml,
                                string[] attachmentsPath,string png)
        {
            try
            {
                if (string.IsNullOrEmpty(Host) || string.IsNullOrEmpty(UserName) ||
                    string.IsNullOrEmpty(Port) || string.IsNullOrEmpty(Password))
                {
                    //todo:记录日志
                    return false;
                }
                var @from = new MailAddress(MailFrom); //使用指定的邮件地址初始化MailAddress实例
                var message = new MailMessage(); //初始化MailMessage实例
                //向收件人地址集合添加邮件地址
                if (mailTo != null)
                {
                    foreach (string t in mailTo)
                    {
                        message.To.Add(t);
                    }
                }

                //向抄送收件人地址集合添加邮件地址
                if (mailCcArray != null)
                {
                    foreach (string t in mailCcArray)
                    {
                        if (string.IsNullOrEmpty(t)) continue;
                        message.CC.Add(t);
                    }
                }
                //发件人地址
                message.From = @from;

                //电子邮件的标题
                message.Subject = subject;

                //电子邮件的主题内容使用的编码
                message.SubjectEncoding = Encoding.UTF8;

                //电子邮件正文
                message.Body = body;

                //电子邮件正文的编码
                message.BodyEncoding = Encoding.Default;
                message.Priority = MailPriority.High;
                //message.IsBodyHtml = isBodyHtml;

                //string htmlBody = string.Format("<html><body>{0}<br>{1}</body></html>",
                //body,"<img src=\"cid:Pic1\">");
                //AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                //  (htmlBody, null, MediaTypeNames.Text.Html);
                body += "<br>" + "<img src=\"cid:Pic1\">";
                var html = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
                var image = new LinkedResource(png, "image/jpeg");
                image.ContentId = "Pic1";// Convert.ToBase64String(Encoding.Default.GetBytes(Path.GetFileName(png)));
                html.LinkedResources.Add(image);
                // Create an alternate view for unsupported clients
                string textBody = "You must use an e-mail client that supports HTML messages";
                AlternateView avText = AlternateView.CreateAlternateViewFromString
                  (textBody, null, MediaTypeNames.Text.Plain);
                message.AlternateViews.Add(html);
                message.AlternateViews.Add(avText);

                //在有附件的情况下添加附件
                if (attachmentsPath != null && attachmentsPath.Length > 0)
                {
                    foreach (string path in attachmentsPath)
                    {
                        if (path == "") continue;
                        var attachFile = new Attachment(path);
                        message.Attachments.Add(attachFile);
                    }
                }
                var smtp = new SmtpClient
                {
                    Credentials = new NetworkCredential(UserName, Password),
                    Host = Host,
                    Port = Convert.ToInt32(Port)
                };

                //将邮件发送到SMTP邮件服务器
                smtp.Send(message);
                //todo:记录日志
                return true;
            }
            catch (SmtpException ex)
            {
                //todo:记录日志
                return false;
            }
        }
    }
}
