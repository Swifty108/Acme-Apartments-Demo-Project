namespace AcmeApartments.Web.BindingModels
{
    public class ApproveAppBindingModel
    {
        public string UserId { get; set; }
        public int ApplicationId { get; set; }
        public string SSN { get; set; }
        public string AptNumber { get; set; }
        public string AptPrice { get; set; }
    }
}