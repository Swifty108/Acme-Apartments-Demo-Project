using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.ViewModels
{
    public class ApplicantContactViewModel
    {
        [Required]
        [MaxLength(10000)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(10000)]
        public string Message { get; set; }
    }
}
