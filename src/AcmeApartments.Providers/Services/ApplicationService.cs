using AcmeApartments.Providers.DTOs;
using AcmeApartments.Providers.HelperClasses;
using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Interfaces;
using AcmeApartments.Data.Provider.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AptUser> _userManager;

        public ApplicationService(
            IUnitOfWork unitOfWork,
            IUserService userService,
            IMapper mapper,
            UserManager<AptUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CheckifApplicationExists(string aptNumber, string userId)
        {
            var apps = await GetApplicationsByAptNumber(aptNumber, userId);
            return apps.Count > 0;
        }

        public async Task Apply(ApplyModelDto applyViewModelDTO, AptUser user)
        {
            var app = new Application
            {
                AptUserId = user.Id,
                Income = applyViewModelDTO.Income,
                Occupation = applyViewModelDTO.Occupation,
                Price = applyViewModelDTO.Price,
                ReasonForMoving = applyViewModelDTO.ReasonForMoving,
                AptNumber = applyViewModelDTO.AptNumber,
                Area = applyViewModelDTO.Area,
                DateApplied = DateTime.Now,
                SSN = applyViewModelDTO.SSN,
                Status = ApplicationStatus.PENDINGAPPROVAL
            };
            await _unitOfWork.ApplicationRepository.Insert(app);
            await _unitOfWork.Save();
        }

        public async Task<Application> GetApplication(int applicationId) => await _unitOfWork.ApplicationRepository.GetByID(applicationId);

        public async Task<List<Application>> GetApplicationsByAptNumber(string aptNumber, string userId)
        {
            var apps = await _unitOfWork.ApplicationRepository.Get(
                filter: application => application.AptNumber == aptNumber
                && application.AptUserId == userId
                && (application.Status == null || application.Status == "Pending Approval" || application.Status == "Approved" || application.Status == "Denied")).ToListAsync();

            return apps;
        }

        public List<Application> GetApplications(string userId)
        {
            return _unitOfWork.ApplicationRepository.Get(filter: application => application.AptUserId == userId, includeProperties: "User").ToList();
        }

        public async Task<List<AptUser>> GetApplicationUsers()
        {
            var users = await (from userRecord in _unitOfWork.AptUserRepository.Get()
                               join applicationRecord in _unitOfWork.ApplicationRepository.Get() on userRecord.Id equals applicationRecord.AptUserId
                               select userRecord).Distinct().ToListAsync();

            return users;
        }

        public async Task<bool> ApproveApplication(
            AptUser user,
            int appId,
            string ssn,
            string aptNumber,
            string aptPrice,
            IList<string> roles
            )
        {
            try
            {
                var app = await _unitOfWork.ApplicationRepository.GetByID(appId);
                app.Status = ApplicationStatus.APPROVED;
                _unitOfWork.ApplicationRepository.Update(app);
                await _unitOfWork.Save();

                if (roles.Contains("Applicant"))
                {
                    user.SSN = ssn;
                    user.AptNumber = aptNumber;
                    user.AptPrice = aptPrice;

                    _unitOfWork.AptUserRepository.Update(user);

                    await _userManager.RemoveFromRoleAsync(user, "Applicant");
                    await _userManager.AddToRoleAsync(user, "Resident");
                    await _unitOfWork.Save();
                }
                else if (roles.Contains("Resident"))
                {
                    user.AptNumber = aptNumber;
                    user.AptPrice = aptPrice;

                    _unitOfWork.AptUserRepository.Update(user);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DenyApplication(string userId, string aptNumber, int appId)
        {
            try
            {
                var app = await _unitOfWork.ApplicationRepository.GetByID(appId);
                app.Status = ApplicationStatus.UNAPPROVED;
                _unitOfWork.ApplicationRepository.Update(app);
                await _unitOfWork.Save();

                var applicationUser = await _unitOfWork.AptUserRepository.GetByID(userId);
                var roles = await _userManager.GetRolesAsync(applicationUser);

                if (roles.Contains("Applicant"))
                {
                    applicationUser.AptNumber = null;
                    applicationUser.AptPrice = null;

                    _unitOfWork.AptUserRepository.Update(applicationUser);
                    await _unitOfWork.Save();
                }
                else if (roles.Contains("Resident"))
                {
                    applicationUser.AptNumber = aptNumber;
                    applicationUser.AptPrice = "853";

                    _unitOfWork.AptUserRepository.Update(applicationUser);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> EditApplication(ApplicationDto applicationDTO)
        {
            try
            {
                var applicationEntity = _mapper.Map<Application>(applicationDTO);
                _unitOfWork.ApplicationRepository.Update(applicationEntity);
                await _unitOfWork.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CancelApplication(int ApplicationId)
        {
            try
            {
                var application = await GetApplication(ApplicationId);
                application.Status = ApplicationStatus.CANCELED;

                _unitOfWork.ApplicationRepository.Update(application);
                await _unitOfWork.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}