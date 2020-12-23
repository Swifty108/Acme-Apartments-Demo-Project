using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.DTOs;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IHome
    {
        public Task<FloorPlansViewModelDTO> GetFloorPlans();

        public Task<string> Apply(ApplyViewModelDTO applyViewModelDTO);

        public bool CheckifApplicationExists(string aptNumber);
    }
}