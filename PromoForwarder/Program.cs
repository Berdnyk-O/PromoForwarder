using OpenPop.Mime;
using PromoForwarder;

EmailReader reader = new EmailReader();
EmailForwarder forwarder = new EmailForwarder();

reader.FindUnreadEmailsMatchingRegex();
Dictionary<int, Message> messages = reader.Messages;

forwarder.ForwardEmails(messages.Values.ToList());