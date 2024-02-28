using Quartz;

namespace PromoForwarder
{
    [DisallowConcurrentExecution]
    internal class ForwardPromoJob : IJob
    {
        private readonly EmailReader _emailReader;
        private readonly EmailForwarder _emailForwarder;

        private readonly string _recipientEmail;

        public ForwardPromoJob(EmailReader emailReader, EmailForwarder emailForwarder, string recipientEmail)
        {
            _emailReader = emailReader;
            _emailForwarder = emailForwarder;
            _recipientEmail = recipientEmail;

            if (_emailReader == null)
            {
                throw new ArgumentNullException("Email Reader cennnot be null");
            }
            if (_emailForwarder == null)
            {
                throw new ArgumentNullException("Email Forwarder cennnot be null");
            }
            if (string.IsNullOrEmpty(recipientEmail))
            {
                throw new ArgumentNullException("Recipient Email cennnot be null");
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            _emailReader.FindUnreadEmailsMatchingRegex();
            var messages = _emailReader.Messages;

            Console.WriteLine($"Founded {messages.Count} matching messages");

            _emailForwarder.ForwardEmails(messages.Values.ToList(), _recipientEmail);

            _emailReader.Dispose();
            _emailForwarder.Dispose();

            return Task.CompletedTask;
        }
    }
}
