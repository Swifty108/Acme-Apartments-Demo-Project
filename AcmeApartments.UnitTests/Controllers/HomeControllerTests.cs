using System;
using Xunit;
using Moq;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using System.Threading.Tasks;
using AcmeApartments.Web.Controllers;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AcmeApartments.Web.ViewModels;
using AcmeApartments.Web.BindingModels;
using Microsoft.AspNetCore.Mvc;

namespace AcmeApartments.UnitTests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> mockLogger;
        private readonly Mock<IUserService> mockUserService;
        private readonly Mock<IHome> mockHomeLogic;
        private readonly Mock<IMapper> mockMapper;
        private readonly HomeController _sut;

        public HomeControllerTests()
        {
            mockLogger = new Mock<ILogger<HomeController>>();
            mockUserService = new Mock<IUserService>();
            mockHomeLogic = new Mock<IHome>();
            mockMapper = new Mock<IMapper>();

            _sut = new HomeController(mockLogger.Object, mockHomeLogic.Object, mockUserService.Object, mockMapper.Object);
        }

        [Fact]
        public void Index_ReturnsCorrectView()
        {
            //Act
            var result = _sut.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ShowAmenities_ReturnsCorrectView()
        {
            //Act
            var result = _sut.ShowAmenities();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ShowGallery_ReturnsCorrectView()
        {
            //Act
            var result = _sut.ShowGallery();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ShowApplicationAlreadyExistsError_ReturnsCorrectView()
        {
            //Act
            var result = _sut.ShowApplicationAlreadyExistsError();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ShowApplicationAlreadyExistsError_HasCorrectViewBagValue()
        {
            //Act
            var result = _sut.ShowApplicationAlreadyExistsError();


            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True((bool) viewResult.ViewData["ApplicationFoundError"]);
        }

       /* [Fact]
        public void ContactUs_ReturnsCorrectView()
        {
            //Act
            var result = _sut.ContactUs(true);

            //Assert
            Assert.IsType<ViewResult>(result);
        }
*/


        [Fact]
        public void ShowFloorPlans_ReturnsCorrectView()
        {
            //Act
            var result = _sut.ShowFloorPlans();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ApplyGetMethod_ValidBidingModel_ViewShouldReturnValidApplyViewModel()
        {
            // Arrange


            mockHomeLogic.Setup(x => x.CheckifApplicationExists(It.IsAny<string>())).Returns(false);

            var user = new AptUser
            {
                FirstName = "Raj",
                LastName = "Narayanan",
                DateOfBirth = new DateTime(),
                DateRegistered = new DateTime(),
                StreetAddress = "1234 Some Drive",
                City = "Pittsburgh",
                State = "PA",
                Zipcode = "15222",
                SSN = "123456789"
            };

            mockUserService.Setup(x => x.GetUser()).ReturnsAsync(user);



            var applyReturnUrlBindingModel = new ApplyReturnUrlBindingModel()
            {
                AptNumber = "3546-415",
                Area = "1050",
                Price = "$1,050",
                FloorPlanType = "2bed"
            };

            // Act
            var result = await _sut.Apply(applyReturnUrlBindingModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<ApplyViewModel>(viewResult.Model);

            Assert.Equal(user.FirstName, model.FirstName);
        }
    }
}
