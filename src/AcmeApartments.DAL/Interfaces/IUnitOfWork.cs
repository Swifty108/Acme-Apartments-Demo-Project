using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
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