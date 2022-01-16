using AcmeApartments.Web.Controllers;
using System;
using Moq;
using Microsoft.Extensions.Logging;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.BLL.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AcmeApartments.Tests.Fixtures
{
    public class HomeControllerFixture
    {
        public HomeController State { get; set; }

        public readonly Mock<ILogger<HomeController>> _mockLogger;
        public readonly Mock<IUserService> _mockUserService;
        public readonly Mock<IHome> _mockHomeLogic;
        public readonly Mock<IMapper> _mockMapper;
        public readonly Mock<ITempDataDictionary> _mockTempData;

        public HomeControllerFixture()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockUserService = new Mock<IUserService>();
            _mockHomeLogic = new Mock<IHome>();
            _mockMapper = new Mock<IMapper>();
            _mockTempData = new Mock<ITempDataDictionary>();

            State = new HomeController(_mockLogger.Object, _mockHomeLogic.Object, _mockUserService.Object, _mockMapper.Object);
            State.TempData = _mockTempData.Object;
        }
    }
}
