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
        public SxAppMailManager(string smtpUserName, string smtpUserPassword, string smtpHost, int smtpPort= 587)
        {
            _smtpUserName = smtpUserName;
            _smtpUserPassword = smtpUserPassword;
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
        }

        public virtual void SendMail(string mailFrom, string body, string[] mailsTo, string subject, bool isBodyHtml=false, bool enableSsl=false)
        {
            Task.Run(()=> {
                if (!mailsTo.Any()) return;

                var mail = new MailMessage();
                for (int i = 0; i < mailsTo.Length; i++)
                {
                    mail.To.Add(mailsTo[i]);
                }

                mail.From = new MailAddress(mailFrom);
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
                    smtp.Send(mail);
                }
            });
        }
    }
}
