using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Providers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeApartments.Providers.DependencyInjection
{
    public static class ProvidersServiceCollectionExtension
    {
        public static IServiceCollection AddProvidersServiceCollection(this IServiceCollection servicCollection)
        {
            servicCollection.AddScoped<IApplicationService, ApplicationService>();
            servicCollection.AddScoped<IBillService, BillService>();
            servicCollection.AddScoped<IFloorPlanService, FloorPlanService>();
            servicCollection.AddScoped<IMaintenanceService, MaintenanceService>();
            servicCollection.AddScoped<IReviewService, ReviewService>();
            servicCollection.AddScoped<IUserService, UserService>();

            return servicCollection;
        }
    }
}
