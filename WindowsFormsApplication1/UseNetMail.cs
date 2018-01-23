using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class UseNetMail : ISendMail
    {
        private MailMessage Mail { get; set; }
        private SmtpClient Host { get; set; }

        public void CreateHost(ConfigHost host)
        {
            Host = new SmtpClient(host.Server, host.Port);
            Host.Credentials = new System.Net.NetworkCredential(host.Username, host.Password);
            Host.EnableSsl = host.EnableSsl;
        }

        public void CreateMail(ConfigMail mail)
        {
            Mail = new MailMessage();
            Mail.From = new MailAddress(mail.From);

            foreach (var t in mail.To)
                Mail.To.Add(t);

            Mail.Subject = mail.Subject;
            Mail.Body = mail.Body;
            Mail.IsBodyHtml = true;
            Mail.BodyEncoding = System.Text.Encoding.UTF8;
        }

        public void CreateMultiMail(ConfigMail mail)
        {
            CreateMail(mail);

            Mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("如果你看到此信息,说明你的邮箱客户端不支持html.", Encoding.UTF8, "text/plain"));
                       
            List<LinkedResource> resList = new List<LinkedResource>();
            if (mail.Resources != null)
            {
                int index = 0;
                foreach (string resource in mail.Resources)
                {
                    var image = new LinkedResource(resource, "image/jpeg");
                    image.ContentId =  "pic"+index;
                    mail.Body += "<br><img src=\"cid:" + image.ContentId + "\">";
                    resList.Add(image);
                }
            }
            var html = AlternateView.CreateAlternateViewFromString(mail.Body, Encoding.UTF8, "text/html");
            foreach (LinkedResource resource in resList)
            {
                html.LinkedResources.Add(resource);
            }
           
            Mail.AlternateViews.Add(html);

            if (mail.Attachments != null)
            {
                foreach (var attachment in mail.Attachments)
                {
                    Mail.Attachments.Add(new Attachment(attachment));
                }
            }           
        }

        public void SendMail()
        {
            if (Host != null && Mail != null)
                Host.Send(Mail);
            else
                throw new Exception("These is not a host to send mail or there is not a mail need to be sent.");
        }
    }
}
