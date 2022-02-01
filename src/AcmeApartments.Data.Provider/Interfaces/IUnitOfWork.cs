using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Entities;
using System.Threading.Tasks;

namespace AcmeApartments.Data.Provider.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<AptUser> AptUserRepository { get; }
        IRepository<Application> ApplicationRepository { get; }
        IRepository<FloorPlan> FloorPlanRepository { get; }
        IRepository<Review> ReviewRepository { get; }
        IRepository<WaterBill> WaterBillRepository { get; }
        IRepository<ElectricBill> ElectricBillRepository { get; }
        IRepository<MaintenanceRequest> MaintenanceRequestRepository { get; }

        Task Save();
    }
}