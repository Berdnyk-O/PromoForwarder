using OpenPop.Mime;
using PromoForwarder;
using System.Configuration;

string? SenderMail = ConfigurationManager.AppSettings["SenderMail"];
string? SenderMailPass = ConfigurationManager.AppSettings["SenderMailPass"];
string? RecipientMail = ConfigurationManager.AppSettings["RecipientMail"];

if (string.IsNullOrEmpty(SenderMail) || string.IsNullOrEmpty(SenderMailPass) || string.IsNullOrEmpty(RecipientMail))
{
    Console.WriteLine("Error while receiving configuration data");
    return;
}

EmailReader reader = new EmailReader(SenderMail, SenderMailPass);
EmailForwarder forwarder = new EmailForwarder(SenderMail, SenderMailPass);

reader.FindUnreadEmailsMatchingRegex();
Dictionary<int, Message> messages = reader.Messages;

forwarder.ForwardEmails(messages.Values.ToList(), RecipientMail);