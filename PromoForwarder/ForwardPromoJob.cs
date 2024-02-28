using Quartz;
using System.Globalization;


namespace PromoForwarder
{
    [DisallowConcurrentExecution]
    internal class ForwardPromoJob : IJob
    {
        private EmailReader _emailReader;
        private EmailForwarder _emailForwarder;

        private string _recipientEmail;

        public ForwardPromoJob(EmailReader emailReader, EmailForwarder emailForwarder, string recipientEmail)
        {
            _emailReader = emailReader;
            _emailForwarder = emailForwarder;
            _recipientEmail = recipientEmail;

            if (_emailReader == null)
            {
                throw new ArgumentNullException("EmailReader cennnot be null");
            }
            if (_emailForwarder == null)
            {
                throw new ArgumentNullException("EmailForwarder cennnot be null");
            }
            if (recipientEmail == null)
            {
                throw new ArgumentNullException("recipientEmail cennnot be null");
            }
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _emailReader.FindUnreadEmailsMatchingRegex();
            var messages = _emailReader.Messages;

            await Console.Out.WriteLineAsync($"Founded {messages.Count} messages");

            _emailForwarder.ForwardEmails(messages.Values.ToList(), _recipientEmail);
        }
    }
}
