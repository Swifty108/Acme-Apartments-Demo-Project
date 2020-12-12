using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.DTOs;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IHome
    {
        public FloorPlansViewModelDTO GetFloorPlans();

        public Task<string> Apply(ApplyViewModelDTO applyViewModelDTO);
    }
}