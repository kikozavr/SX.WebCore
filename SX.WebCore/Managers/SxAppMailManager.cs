using SX.WebCore.MvcApplication;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SX.WebCore.Managers
{
    public class SxAppMailManager
    {
        private readonly string _smtpUserName;
        private readonly string _smtpUserPassword;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        public SxAppMailManager(string smtpUserName, string smtpUserPassword, string smtpHost, int smtpPort = 587)
        {
            _smtpUserName = smtpUserName;
            _smtpUserPassword = smtpUserPassword;
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
        }

        public virtual async Task<bool> SendMail(string mailFrom, string body, string[] mailsTo, string subject, bool isBodyHtml = false, bool enableSsl = false)
        {
            return await Task.Run(() =>
            {
                var result = false;

                if (!mailsTo.Any()) return result;

                var mail = new MailMessage();
                for (int i = 0; i < mailsTo.Length; i++)
                {
                    mail.To.Add(mailsTo[i]);
                }

                mail.From = new MailAddress(_smtpUserName);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isBodyHtml;

                
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = _smtpUserName,
                        Password = _smtpUserPassword
                    };
                    smtp.Credentials = credential;
                    smtp.Host = _smtpHost;
                    smtp.Port = _smtpPort;
                    smtp.EnableSsl = enableSsl;
                    try
                    {
                        smtp.Send(mail);
                        result= true;
                    }
                    catch
                    {
                        result= false;
                    }
                }

                return result;
            });
        }
    }
}
