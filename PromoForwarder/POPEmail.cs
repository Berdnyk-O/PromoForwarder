using OpenPop.Mime;
using OpenPop.Pop3;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace PromoForwarder
{
    internal class POPEmail
    {
        protected const int POP3PORT = 995;
        protected const string POP3HOST = "pop.gmail.com";
        protected bool useSsl;

        private readonly Pop3Client _client;

        protected string regEx;

        private Dictionary<int , Message> _messages;

        public POPEmail(string regex = @"\s*знижк.\s*")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            useSsl = true;
            regEx = regex;
            _messages = [];

            _client = new Pop3Client();
            _client.Connect(POP3HOST, POP3PORT, useSsl);
            _client.Authenticate("email", "password", AuthenticationMethod.UsernameAndPassword);

            Console.WriteLine("Connecting to POP3 server using SSL.");
        }

        public void FindEmailsMatchingRegex()
        {
            
            //int messageCount = _client.GetMessageCount();
            int messageCount = 10;
            Console.WriteLine($"Message count: {messageCount}");

            int j = 0;
            for (int i = messageCount; i > 0; i--)
            {
                Message message = _client.GetMessage(i);
                Match m = Regex.Match(message.Headers.Subject, regEx, RegexOptions.IgnoreCase);
                if(m.Success)
                {
                    _messages.Add(j, message);
                    j++;
                }
            }

            _client.Disconnect();
        }


        public void ForwardEmail()
        {
            try
            {
                SmtpClient mySmtpClient = new("smtp.gmail.com");

                mySmtpClient.UseDefaultCredentials = false;
                mySmtpClient.EnableSsl = true;
                NetworkCredential basicAuthenticationInfo = new("example@gmail.com", "password");
                mySmtpClient.Credentials = basicAuthenticationInfo;


                MailAddress from = new MailAddress("example@gmail.com");
                MailAddress to = new MailAddress("example2@gmail.com");
                MailMessage myMail = new MailMessage(from, to);

                Console.WriteLine(_messages.GetValueOrDefault(1)!.Headers.Subject);
                myMail.Subject = _messages.GetValueOrDefault(1)!.Headers.Subject;

                Console.WriteLine(  );
                Console.WriteLine(_messages.GetValueOrDefault(1)!.Headers.ContentDescription);
                myMail.Body = _messages.GetValueOrDefault(1)!.FindFirstHtmlVersion().GetBodyAsText();               

                Console.WriteLine(_messages.GetValueOrDefault(1)!.Headers.Date);
                Console.WriteLine(_messages.GetValueOrDefault(1)!.Headers.Keywords);


                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
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
    }
}
