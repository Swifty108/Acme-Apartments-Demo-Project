using AcmeApartments.BLL.HelperClasses;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IHome
    {
        public Task<FloorPlansViewModelDTO> GetFloorPlans();

        public Task<AptUser> GetUser();

        public Task<string> Apply(ApplicationDTO appDTO);
    }
}