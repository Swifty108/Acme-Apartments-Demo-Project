using AcmeApartments.BLL.HelperClasses;
using AcmeApartments.Common.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.BLL.HelperClasses.Singletons
{
    class HomeSingleton
    {
        public readonly Home Home;
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

        private HomeSingleton()
        {
        }

        private static HomeSingleton _instance;

        public static HomeSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HomeSingleton();
            }
            return _instance;
        }
    }
}