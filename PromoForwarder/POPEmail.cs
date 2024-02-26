using OpenPop.Pop3;

namespace PromoForwarder
{
    internal class POPEmail
    {
        protected const int POP3PORT = 995;
        protected const string POP3HOST = "pop.gmail.com";
        protected bool useSsl = true;

        private readonly Pop3Client _client;

        public POPEmail()
        {
            _client = new Pop3Client();
            _client.Connect(POP3HOST, POP3PORT, useSsl);
            _client.Authenticate("email", "password", AuthenticationMethod.UsernameAndPassword);


            Console.WriteLine("\nConnecting to POP3 server using SSL.");
            int messageCount = _client.GetMessageCount();
            Console.WriteLine("Total Messages: " + messageCount);
        }

        
    }
}
