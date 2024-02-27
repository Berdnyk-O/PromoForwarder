using OpenPop.Mime;
using OpenPop.Pop3;
using System.Text;
using System.Text.RegularExpressions;

namespace PromoForwarder
{
    internal class EmailReader
    {
        protected const int POP3PORT = 995;
        protected const string POP3HOST = "pop.gmail.com";
        protected bool useSsl;

        private readonly Pop3Client _client;

        protected string regEx;

        private Dictionary<int, Message> _messages;
        public Dictionary<int, Message> Messages { get =>  _messages; }

        public EmailReader(string email, string password, string regex = @"\s*знижк.\s*")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            useSsl = true;
            regEx = regex;
            _messages = [];

            _client = new Pop3Client();
            _client.Connect(POP3HOST, POP3PORT, useSsl);
            _client.Authenticate(email, password, AuthenticationMethod.UsernameAndPassword);

            Console.WriteLine("Connecting to POP3 server using SSL.");
        }

        public void FindUnreadEmailsMatchingRegex()
        {
            int messageCount = _client.GetMessageCount();
            
            messageCount = 10;

            Console.WriteLine($"Message count: {messageCount}");
            Message message1 = _client.GetMessage(1);
            int j = 0;
            for (int i = messageCount; i>0; i--)
            {
                Message message = _client.GetMessage(i);
                Match m = Regex.Match(message.Headers.Subject, regEx, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    _messages.Add(j, message);
                    j++;
                }
            }

            _client.Disconnect();
        }
    }
}
