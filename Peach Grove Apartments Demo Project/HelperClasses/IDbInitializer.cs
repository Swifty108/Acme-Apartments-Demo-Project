using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.HelperClasses
{
    interface IDbInitializer
    {
        /* Applies any pending migrations for the context to the database.
         Will create the database if it does not already exist. */
        void Initialize();

        // add seed data to the database
        Task SeedData();
    }
}
