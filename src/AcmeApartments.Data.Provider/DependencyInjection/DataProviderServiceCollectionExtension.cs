using AcmeApartments.Data.Provider.Data;
using AcmeApartments.Data.Provider.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeApartments.Data.Provider.DependencyInjection
{
    public static class DataProviderServiceCollectionExtension
    {
        public static IServiceCollection AddDataProviderServiceCollection(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDbInitializer, DbInitializer>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            return serviceCollection;
        }
    }
}
