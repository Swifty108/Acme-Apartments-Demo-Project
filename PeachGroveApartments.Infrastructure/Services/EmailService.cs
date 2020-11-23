using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Infrastructure.Inerfaces;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        //private readonly IEmailConfiguration _emailConfiguration;

        //public EmailService(IEmailConfiguration emailConfiguration)
        //{
        //    _emailConfiguration = emailConfiguration;
        //}

        //public void Send(EmailMessage emailMessage)
        //{
        //    var message = new MimeMessage();
        //    message.To.Add(new MailboxAddress(emailMessage.ToAddresses.Name, emailMessage.ToAddresses.Address));
        //    message.From.Add(new MailboxAddress(emailMessage.FromAddresses.Name, emailMessage.FromAddresses.Address));

        //    message.Subject = emailMessage.Subject;
        //    //We will say we are sending HTML. But there are options for plaintext etc.
        //    message.Body = new TextPart(TextFormat.Html)
        //    {
        //        Text = emailMessage.Content
        //    };

        //    //Be careful that the SmtpClient class is the one from Mailkit not the framework!
        //    using (var emailClient = new SmtpClient())
        //    {
        //        emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
        //        //The last parameter here is to use SSL (Which you should!)
        //        emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

        //        //Remove any OAuth functionality as we won't be using it.
        //        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

        //        emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

        //        emailClient.Send(message);

        //        emailClient.Disconnect(true);
        //    }
        //}
    }
}