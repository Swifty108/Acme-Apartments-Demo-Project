using PeachGroveApartments.Common.HelperClasses;

namespace PeachGroveApartments.Infrastructure.Inerfaces
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}