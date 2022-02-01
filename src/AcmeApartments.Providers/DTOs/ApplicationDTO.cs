using AcmeApartments.Data.Provider.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Providers.DTOs
{
    public class ApplicationDto
    {
        public int ApplicationId { get; set; }
        public string AptUserId { get; set; }
        public DateTime DateApplied { get; set; }
        public string Occupation { get; set; }
        public int? Income { get; set; }
        public string ReasonForMoving { get; set; }
        public string SSN { get; set; }
        public string AptNumber { get; set; }
        public string Area { get; set; }
        public string Price { get; set; }
        public string Status { get; set; }
    }
}