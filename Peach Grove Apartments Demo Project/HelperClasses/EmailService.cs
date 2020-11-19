using Peach_Grove_Apartments_Demo_Project.Interfaces;
using System;
using System.Collections.Generic;

namespace Peach_Grove_Apartments_Demo_Project.HelperClasses
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            throw new NotImplementedException();
        }

        public void Send(EmailMessage emailMessage)
        {
            throw new NotImplementedException();
        }
    }
}