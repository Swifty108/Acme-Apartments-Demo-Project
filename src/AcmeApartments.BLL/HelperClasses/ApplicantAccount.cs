using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Data;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class ApplicantAccount : IApplicantAccount
    {
        private readonly IUserService _userService;
        private readonly IApplicationService _appService;

        public ApplicantAccount(
            IUserService userService,
            IApplicationService appService)
        {
            _userService = userService;
            _appService = appService;
        }

        public List<Application> GetApplications()
        {
            var userId = _userService.GetUserId();
            var applications = _appService.GetApplications(userId);
            return applications;
        }
    }
}