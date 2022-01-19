using System;
using Xunit;
using Xunit.Abstractions;
using Moq;
using AcmeApartments.DAL.Identity;
using System.Threading.Tasks;
using AcmeApartments.Web.ViewModels;
using AcmeApartments.Web.BindingModels;
using Microsoft.AspNetCore.Mvc;
using AcmeApartments.BLL.DTOs;
using AcmeApartments.Tests.Fixtures;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;

namespace AcmeApartments.Tests.Controllers
{
    public class HomeControllerTests : IClassFixture<HomeControllerFixture>
    {
        private readonly HomeControllerFixture _homeControllerFixture;
        private readonly ITestOutputHelper _output;

        public HomeControllerTests(HomeControllerFixture homeControllerFixture, ITestOutputHelper output)
        {
            _homeControllerFixture = homeControllerFixture;
            _output = output;
        }

        [Fact]
        public void Index_ReturnsCorrectView()
        {
            //Act
            var result = _homeControllerFixture.State.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ShowAmenities_ReturnsCorrectView()
        {
            //Act
            var result = _homeControllerFixture.State.ShowAmenities();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ShowGallery_ReturnsCorrectView()
        {
            //Act
            var result = _homeControllerFixture.State.ShowGallery();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ShowFloorPlans_ReturnsCorrectView()
        {
            //Arrange

            var floorPlansViewModelDTO = new FloorPlansViewModelDTO
            {
                TwoBedPlans = new List<FloorPlan>()
                {
                    new FloorPlan()
                    {
                        FloorPlanType = "2bed",
                        AptNumber = "4173-325"
                    }

                }
            };

            var floorPlansViewModel = new FloorPlansViewModel
            {
                TwoBedPlans = new List<FloorPlanDTO>()
                {
                    new FloorPlanDTO()
                    {
                        FloorPlanType = "2bed",
                        AptNumber = "4173-325"
                    }

                }
            };

            _homeControllerFixture._mockFloorPlanService.Setup(x => x.GetFloorPlans()).ReturnsAsync(floorPlansViewModelDTO);
            _homeControllerFixture._mockMapper.Setup(x => x.Map<FloorPlansViewModel>(floorPlansViewModelDTO)).Returns(floorPlansViewModel);

            //Act
            var result = _homeControllerFixture.State.ShowFloorPlans();

            //Assert
            var viewResult = await result as ViewResult;

            var model = Assert.IsType<FloorPlansViewModel>(viewResult.Model);

            Assert.Equal(floorPlansViewModelDTO.TwoBedPlans[0].FloorPlanType, model.TwoBedPlans[0].FloorPlanType);
        }

        [Fact]
        public void ShowApplicationAlreadyExistsError_ReturnsCorrectView()
        {
            //Act
            var result = _homeControllerFixture.State.ShowApplicationAlreadyExistsError();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ShowApplicationAlreadyExistsError_HasCorrectViewBagValue()
        {
            //Act
            var result = _homeControllerFixture.State.ShowApplicationAlreadyExistsError();
            _output.WriteLine("Test Message.");

            //Assert
            var viewResult = result as ViewResult;
            Assert.True((bool)viewResult.ViewData["ApplicationFoundError"]);
        }

        [Fact]
        public void ContactUsGet_ReturnsCorrectView()
        {
            //Act
            _homeControllerFixture._mockTempData.Setup(x => x.Add("ContactUsSuccess", false));
            var result = _homeControllerFixture.State.ContactUs();
            _homeControllerFixture._mockTempData.Setup(x => x.Clear());

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ContactUsPost_InvalidModelState_ReturnsCorrectViewModel()
        {
            //Arrange
            _homeControllerFixture.State.ModelState.AddModelError("e", "Error");

            var appUserContactBindingModel = new AppUserContactBindingModel()
            {
                SenderName = null,
                FromEmailAddress = "raj@rajnarayanan.com",
                Subject = "Test message",
                Message = "Hi. This is only a test message."
            };

            var appUserContactViewModel = new AppUserContactViewModel()
            {
                SenderName = appUserContactBindingModel.SenderName,
                FromEmailAddress = appUserContactBindingModel.FromEmailAddress,
                Subject = appUserContactBindingModel.Subject,
                Message = appUserContactBindingModel.Message
            };

            _homeControllerFixture._mockMapper.Setup(x => x.Map<AppUserContactViewModel>(appUserContactBindingModel)).Returns(appUserContactViewModel);


            //Act
            var result = _homeControllerFixture.State.ContactUs(appUserContactBindingModel);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AppUserContactViewModel>(viewResult.Model);

            //Assert
            Assert.Equal(appUserContactBindingModel.SenderName, model.SenderName);
            Assert.Equal(appUserContactBindingModel.FromEmailAddress, model.FromEmailAddress);
            Assert.Equal(appUserContactBindingModel.Subject, model.Subject);
            Assert.Equal(appUserContactBindingModel.Message, model.Message);

            _homeControllerFixture.State.ModelState.Remove("e");
        }

        [Fact]
        public async Task ApplyGet_ValidBidingModel_ViewShouldReturnValidApplyViewModel()
        {
            // Arrange
            _homeControllerFixture._mockApplicationService.Setup(x => x.CheckifApplicationExists(It.IsAny<string>())).ReturnsAsync(false);

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

            _homeControllerFixture._mockUserService.Setup(x => x.GetUser()).ReturnsAsync(user);

            var applyReturnUrlBindingModel = new ApplyReturnUrlBindingModel()
            {
                AptNumber = "3546-415",
                Area = "1050",
                Price = "$1,050",
                FloorPlanType = "2bed"
            };

            // Act
            var result = await _homeControllerFixture.State.Apply(applyReturnUrlBindingModel);
            var viewResult = result as ViewResult;

            // Assert
            var model = viewResult.Model as ApplyViewModel;
            Assert.Equal(user.FirstName, model.FirstName);
        }

        [Fact]
        public async Task ApplyGet_ApplicationExists_ShouldReturnValidRedirectToActionResult()
        {
            // Arrange
            _homeControllerFixture._mockApplicationService.Setup(x => x.CheckifApplicationExists(It.IsAny<string>())).ReturnsAsync(true);

            var applyReturnUrlBindingModel = new ApplyReturnUrlBindingModel()
            {
                AptNumber = "3546-415",
                Area = "1050",
                Price = "$1,050",
                FloorPlanType = "2bed"
            };

            // Act
            var result = await _homeControllerFixture.State.Apply(applyReturnUrlBindingModel);
            var viewResult = result as RedirectToActionResult;

            // Assert
            Assert.Equal("ShowApplicationAlreadyExistsError", viewResult.ActionName);
        }

        [Fact]
        public async Task ApplyPost_ValidBidingModel_ReturnsValidRedirectToActionResult()
        {
            // Arrange
            var applyBindingModel = new ApplyBindingModel()
            {
                FirstName = "Raj",
                LastName = "Narayanan"
            };

            var applyViewModelDTO = new ApplyViewModelDTO()
            {
                FirstName = applyBindingModel.FirstName,
                LastName = applyBindingModel.LastName
            };

            _homeControllerFixture._mockMapper.Setup(x => x.Map<ApplyViewModelDTO>(applyBindingModel)).Returns(applyViewModelDTO);
            _homeControllerFixture._mockApplicationService.Setup(x => x.Apply(It.IsAny<ApplyViewModelDTO>())).ReturnsAsync("resident");

            // Act
            var result = await _homeControllerFixture.State.Apply(applyBindingModel);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            // Assert
            Assert.Equal("index", viewResult.ActionName);
            Assert.Equal("residentaccount", viewResult.ControllerName);
        }

        [Fact]
        public async Task ApplyPost_InValidBidingModel_ReturnsValidViewResult()
        {
            // Arrange
            _homeControllerFixture.State.ModelState.AddModelError("e", "Error");

            var applyBindingModel = new ApplyBindingModel()
            {
                FirstName = null,
                LastName = "Narayanan"
            };

            var applyViewModel = new ApplyViewModel()
            {
                FirstName = applyBindingModel.FirstName,
                LastName = applyBindingModel.LastName
            };

            _homeControllerFixture._mockMapper.Setup(x => x.Map<ApplyViewModel>(applyBindingModel)).Returns(applyViewModel);

            // Act
            var result = await _homeControllerFixture.State.Apply(applyBindingModel);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ApplyViewModel>(viewResult.Model);

            // Assert
            Assert.Null(model.FirstName);
            Assert.Equal("Narayanan", model.LastName);

            _homeControllerFixture.State.ModelState.Remove("e");
        }
    }
}
