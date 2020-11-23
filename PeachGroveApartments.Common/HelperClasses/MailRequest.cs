namespace PeachGroveApartments.Common.HelperClasses
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        // public List<IFormFile> Attachments { get; set; }
        //public EmailAddress ToAddresses { get; set; }
        //public EmailAddress FromAddresses { get; set; }
        //public string Subject { get; set; }
        //public string Content { get; set; }
    }
}