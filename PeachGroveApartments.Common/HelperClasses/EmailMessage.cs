namespace PeachGroveApartments.Common.HelperClasses
{
    public class EmailMessage
    {
        public EmailAddress ToAddresses { get; set; }
        public EmailAddress FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}