using OpenPop.Mime;
using OpenPop.Pop3;
using System.Text.RegularExpressions;

namespace PromoForwarder
{
    internal class POPEmail
    {
        protected const int POP3PORT = 995;
        protected const string POP3HOST = "pop.gmail.com";
        protected bool useSsl;

        private readonly Pop3Client _client;

        protected string _regex;

        public POPEmail(string regex = @"\s*знижк.\s*")
        {
            useSsl = true;
            _regex=regex;

            _client = new Pop3Client();
            _client.Connect(POP3HOST, POP3PORT, useSsl);
            _client.Authenticate("email", "password", AuthenticationMethod.UsernameAndPassword);

            Console.WriteLine("Connecting to POP3 server using SSL.");
        }

        public void GoThroughEmails()
        {
            int messageCount = _client.GetMessageCount();
            Console.WriteLine($"Message count: {messageCount}");

            for (int i = messageCount; i > 0; i--)
            {
                Message message = _client.GetMessage(i);
                Match m = Regex.Match(message.Headers.Subject, _regex, RegexOptions.IgnoreCase);
                Console.WriteLine(message.Headers.Subject + " " + message.Headers.Date + " "+ m.Success + "\n");
            }
        }
    }
}
