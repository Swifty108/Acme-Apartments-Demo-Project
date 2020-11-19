using Peach_Grove_Apartments_Demo_Project.HelperClasses;
using System.Collections.Generic;

namespace Peach_Grove_Apartments_Demo_Project.Interfaces
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);

        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}