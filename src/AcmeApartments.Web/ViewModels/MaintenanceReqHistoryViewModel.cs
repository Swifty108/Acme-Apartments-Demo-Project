using AcmeApartments.BLL.DTOs;
using System.Collections.Generic;

namespace AcmeApartments.Web.ViewModels
{
    public class MaintenanceReqHistoryViewModel
    {
        public List<MaintenanceRequestDTO> Requests { get; set; }
    }
}