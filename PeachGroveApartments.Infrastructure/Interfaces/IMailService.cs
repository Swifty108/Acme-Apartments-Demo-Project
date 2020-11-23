using PeachGroveApartments.Common.HelperClasses;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Inerfaces
{
    public interface IMailService
    {
        public Task SendEmailAsync(MailRequest mailRequest);
    }
}