using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Interfaces;
using AcmeApartments.Data.Provider.Entities;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.Data.Provider.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext _dbContext;
        private GenericRepository<AptUser> _aptUsers;
        private GenericRepository<Application> _applications;
        private GenericRepository<FloorPlan> _floorPlans;
        private GenericRepository<Review> _reviews;
        private GenericRepository<WaterBill> _waterBills;
        private GenericRepository<ElectricBill> _electricBills;

        private GenericRepository<MaintenanceRequest> _maintenanceRequests;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<AptUser> AptUserRepository
        {
            get
            {
                return _aptUsers ??
                    (_aptUsers = new GenericRepository<AptUser>(_dbContext));
            }
        }

        public IRepository<Application> ApplicationRepository
        {
            get
            {
                return _applications ??
                    (_applications = new GenericRepository<Application>(_dbContext));
            }
        }

        public IRepository<MaintenanceRequest> MaintenanceRequestRepository
        {
            get
            {
                return _maintenanceRequests ??
                    (_maintenanceRequests = new GenericRepository<MaintenanceRequest>(_dbContext));
            }
        }

        public IRepository<FloorPlan> FloorPlanRepository
        {
            get
            {
                return _floorPlans ??
                    (_floorPlans = new GenericRepository<FloorPlan>(_dbContext));
            }
        }

        public IRepository<Review> ReviewRepository
        {
            get
            {
                return _reviews ??
                    (_reviews = new GenericRepository<Review>(_dbContext));
            }
        }

        public IRepository<WaterBill> WaterBillRepository
        {
            get
            {
                return _waterBills ??
                    (_waterBills = new GenericRepository<WaterBill>(_dbContext));
            }
        }

        public IRepository<ElectricBill> ElectricBillRepository
        {
            get
            {
                return _electricBills ??
                    (_electricBills = new GenericRepository<ElectricBill>(_dbContext));
            }
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}