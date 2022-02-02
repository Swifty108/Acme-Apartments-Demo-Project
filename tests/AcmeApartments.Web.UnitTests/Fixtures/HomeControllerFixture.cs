using AcmeApartments.Web.Controllers;
using Moq;
using AcmeApartments.Providers.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity;
using AcmeApartments.Data.Provider.Identity;

namespace AcmeApartments.Tests.Fixtures
{
    public class HomeControllerFixture
    {
        public HomeController State { get; set; }

        public readonly Mock<IWebUserService> _mockUserService;
        public readonly Mock<IMapper> _mockMapper;
        public readonly Mock<ITempDataDictionary> _mockTempData;
        public readonly Mock<IApplicationService>  _mockApplicationService;
        public readonly Mock<IFloorPlanService> _mockFloorPlanService;
        public readonly Mock<UserManager<AptUser>> _mockUserManager;

        public HomeControllerFixture()
        {
            var store = new Mock<IUserStore<AptUser>>();
            var mgr = new Mock<UserManager<AptUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<AptUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<AptUser>());

            _mockUserService = new Mock<IWebUserService>();
            _mockMapper = new Mock<IMapper>();
            _mockTempData = new Mock<ITempDataDictionary>();
            _mockApplicationService = new Mock<IApplicationService>();
            _mockFloorPlanService = new Mock<IFloorPlanService>();
            _mockUserManager = mgr;

            State = new HomeController(_mockUserService.Object, _mockApplicationService.Object, _mockFloorPlanService.Object, _mockUserManager.Object, _mockMapper.Object);
            State.TempData = _mockTempData.Object;
        }
    }
}
