﻿using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IApplicantAccount
    {
        public Task<List<Application>> GetApplications();
    }
}