using AcmeApartments.DAL.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class HomeTests
    {
        //TODO-tests: create a Home class with mocks in it. Then, implement a singleton for it.

        //sut = system under test
        private readonly HomeTests _sut;

        private readonly Mock<IUnitOfWork> _unitOfwork = new Mock<IUnitOfWork>();

        [Fact]
        public async Task Test1()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}