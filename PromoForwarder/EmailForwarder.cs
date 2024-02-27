using System.Net.Mail;
using System.Net;
using OpenPop.Mime;

namespace PromoForwarder
{
    internal class EmailForwarder
    {
        protected const int SMTPPORT = 587;
        protected const string SMTPHOST = "smtp.gmail.com";
        protected bool useSsl;

        private readonly SmtpClient _SmtpClient;
        private readonly string _senderMail;

        public EmailForwarder(string senderMail, string senderMailPass)
        {
            useSsl = true;

            _SmtpClient = new(SMTPHOST, SMTPPORT);
            _SmtpClient.UseDefaultCredentials = false;
            _SmtpClient.EnableSsl = useSsl;

            _senderMail = senderMail;
            
            NetworkCredential basicAuthenticationInfo = new(senderMail, senderMailPass);
            _SmtpClient.Credentials = basicAuthenticationInfo;
        }

        public void ForwardEmails(List<Message> messages, string recipientMail)
        {
            try
            {
                foreach (var message in messages)
                {
                    MailMessage mail = GetMailMessage(_senderMail, recipientMail);
                    SetContent(
                        mail,
                        message.Headers.Subject,
                        message.FindFirstHtmlVersion().GetBodyAsText());

                    _SmtpClient.Send(mail);
                }
               
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MailMessage GetMailMessage(string addressFrom, string addressTo)
        {
            MailAddress from = new(addressFrom);
            MailAddress to = new(addressTo);
            MailMessage mail = new(from, to);

            return mail;
        }

        public void SetContent(MailMessage mail, string subject, string body)
        {
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
        }
    }
}
