using System;
using System.Linq;
using System.Linq.Expressions;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetByID(object id);

        void Insert(TEntity entity);

        void Update(TEntity entityToUpdate);

        void Delete(TEntity entityToDelete);

        void Delete(object id);
    }

    //public interface IRepository
    //{
    //    public Task<AptUser> GetApplicationUserByID(string userId);

    //    public Task UpdateUser(AptUser user);

    //    public Task<Application> GetApplicationByID(int appId);

    //    public Task UpdateApplication(Application app);

    //    public Task<List<Application>> GetApplications(string userId);

    //    public void UpdateMaintenaceRequest(MaintenanceRequest mRequest);
    //}
}