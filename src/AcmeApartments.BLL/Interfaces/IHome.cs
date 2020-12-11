using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IHome
    {
        public List<FloorPlan> GetFloorPlans();

        public Task<string> Apply(ApplyViewModelDTO applyViewModelDTO);
    }
}