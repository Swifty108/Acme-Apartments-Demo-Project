using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<AptUser> AptUserRepository { get; }
        IRepository<Application> ApplicationRepository { get; }
        IRepository<FloorPlan> FloorPlanRepository { get; }
        IRepository<Review> ReviewRepository { get; }
        IRepository<MaintenanceRequest> MaintenanceRequestRepository { get; }

        void Save();
    }
}