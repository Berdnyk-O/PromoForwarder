using System.Net.Mail;
using System.Net;
using OpenPop.Mime;

namespace PromoForwarder
{
    internal class EmailForwarder
    {
        protected const int POP3PORT = 995;
        protected const string SMTPHOST = "smtp.gmail.com";
        protected bool useSsl = true;

        private readonly SmtpClient _SmtpClient;

        public EmailForwarder()
        {
            _SmtpClient = new(SMTPHOST);
            _SmtpClient.UseDefaultCredentials = false;
            _SmtpClient.EnableSsl = useSsl;
            NetworkCredential basicAuthenticationInfo = new("example@gmail.com", "password");
            _SmtpClient.Credentials = basicAuthenticationInfo;
        }

        public void ForwardEmail(Message message)
        {
            try
            {
                MailMessage mail = GetMailMessage("addressFrom", "addressTo");
                SetContent(
                    mail,
                    message.Headers.Subject,
                    message.FindFirstHtmlVersion().GetBodyAsText());

                _SmtpClient.Send(mail);
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
            MailAddress from = new MailAddress(addressFrom);
            MailAddress to = new MailAddress(addressTo);
            MailMessage mail = new MailMessage(from, to);

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
