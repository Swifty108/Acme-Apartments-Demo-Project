using AcmeApartments.Web.Controllers;
using Moq;
using AcmeApartments.Providers.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AcmeApartments.Tests.Fixtures
{
    public class HomeControllerFixture
    {
        public HomeController State { get; set; }

        public readonly Mock<IUserService> _mockUserService;
        public readonly Mock<IMapper> _mockMapper;
        public readonly Mock<ITempDataDictionary> _mockTempData;
        public readonly Mock<IApplicationService>  _mockApplicationService;
        public readonly Mock<IFloorPlanService>  _mockFloorPlanService;

        public HomeControllerFixture()
        {
            _mockUserService = new Mock<IUserService>();
            _mockMapper = new Mock<IMapper>();
            _mockTempData = new Mock<ITempDataDictionary>();
            _mockApplicationService = new Mock<IApplicationService>();
            _mockFloorPlanService = new Mock<IFloorPlanService>();

            State = new HomeController(_mockUserService.Object, _mockApplicationService.Object, _mockFloorPlanService.Object, _mockMapper.Object);
            State.TempData = _mockTempData.Object;
        }
    }
}
