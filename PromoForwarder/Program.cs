using PromoForwarder;

POPEmail popClient = new();
popClient.FindEmailsMatchingRegex();
popClient.ForwardEmail();