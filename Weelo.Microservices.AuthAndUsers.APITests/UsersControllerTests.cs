using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weelo.Microservices.AuthAndUsers.API.Controllers;
using Weelo.Microservices.AuthAndUsers.API.Entities;
using Weelo.Microservices.AuthAndUsers.API.Helpers;
using Weelo.Microservices.AuthAndUsers.API.Services;
using Xunit;

namespace Weelo.Microservices.AuthAndUsers.APITests
{
    public class UsersControllerTests
    {
        private Mock<IUserService> userServiceStub = new();
        private Mock<IMapper> mapperStub = new();
        private readonly AppSettings appSettings = new();
        private readonly Mock<ILogger<UsersController>> loggerStub = new();


        [Fact]
        public void GetById_WithoutExistingId_ReturnsOkWithNull()
        {
            // Arrange
            userServiceStub.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns((User)null);

            var controller = new UsersController(userServiceStub.Object, mapperStub.Object, loggerStub.Object);

            // Act
            var result = controller.GetById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.Null(response.Message);
        }

        [Fact]
        public void Get_WithExistingId_ReturnsOkWithUser()
        {
            // Arrange
            var user = CreateRandomUser();

            userServiceStub.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(user);

            var controller = new UsersController(userServiceStub.Object, mapperStub.Object, loggerStub.Object);

            // Act
            var result = controller.GetById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<User>(response.Message);

            var resultUser = response.Message as User;
            Assert.Equal(user.Id, resultUser.Id);
            Assert.Equal(user.FirstName, resultUser.FirstName);
            Assert.Equal(user.Address, resultUser.Address);
            Assert.Equal(user.PhotoPath, resultUser.PhotoPath);
            Assert.Equal(user.Username, resultUser.Username);
            Assert.Equal(user.Role, resultUser.Role);

        }

        [Fact]
        public void Get_ServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            userServiceStub.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new UsersController(userServiceStub.Object, mapperStub.Object, loggerStub.Object);

            // Act
            var result = controller.GetById(1);

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Dummy Dto to Simulate results in the test functions
        /// </summary>
        /// <returns></returns>
        private User CreateRandomUser()
        {
            return new User()
            {
                Id = 1,
                FirstName = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                PhotoPath= Guid.NewGuid().ToString(),
                Username= Guid.NewGuid().ToString(),
                Role = Guid.NewGuid().ToString()
            };
        }
    }
}
