using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Interfaces
{
    internal interface IDbInitializer
    {
        /* Applies any pending migrations for the context to the database.
         Will create the database if it does not already exist. */

        Task Initialize();

        // add seed data to the database
        Task SeedData();
    }
}