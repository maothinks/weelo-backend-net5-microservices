using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Domain.Interfaces.Repositories;
using Weelo.Microservices.Properties.Infrastructure.API.Controllers;
using Weelo.Microservices.Properties.Infrastructure.API.Models;
using Xunit;

namespace Weelo.Microservices.Properties.Infrastructure.APITests
{
    public class PropertiesControllerTests
    {
        private readonly Mock<IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid>>  propertyRepositoryStub = new();
        private readonly Mock<IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyView, Guid>>  propertyViewRepositoryStub = new();
        private readonly Mock<ILogger<PropertiesController>> loggerStub = new ();

        /// <summary>
        /// Get Without Existing Id and Returns Ok With Null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_WithoutExistingId_ReturnsOkWithNull()
        {
            // Arrange
            propertyRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Property)null);

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Get(Guid.NewGuid());

            // Asserts
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;
            
            Assert.True(response.Success);
            Assert.Null(response.Message);
        }

        /// <summary>
        /// Get With Existing Id And Returns Ok With Property
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_WithExistingId_ReturnsOkWithProperty()
        {
            // Arrange
            var expectedProperty = CreateRandomProperty();
            propertyRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProperty);

            propertyViewRepositoryStub.Setup(repo => repo.GetAllByParentIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<PropertyView>());

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<Property>(response.Message);

            var resultProperty = response.Message as Property;
            Assert.Equal(expectedProperty.PropertyId, resultProperty.PropertyId);
            Assert.Equal(expectedProperty.Name, resultProperty.Name);
            Assert.Equal(expectedProperty.Address, resultProperty.Address);
            Assert.Equal(expectedProperty.CoverPath, resultProperty.CoverPath);
            Assert.Equal(expectedProperty.OwnerId, resultProperty.OwnerId);
            Assert.Equal(expectedProperty.CodeInternal, resultProperty.CodeInternal);
            Assert.Equal(expectedProperty.Price, resultProperty.Price);
            Assert.Equal(expectedProperty.Year, resultProperty.Year);
        }

        /// <summary>
        /// Get Server Exception And Returns Status 500 Internal Server Error
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_ServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            propertyRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Get(Guid.NewGuid());

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Get All With Filter And Pagination Returns Ok With Property List
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAll_WithFilterAndPagination_ReturnsOkWithPropertyList()
        {
            // Arrange
            List<Property> properties = new();
            properties.Add(CreateRandomProperty());

            propertyRepositoryStub.Setup(repo => repo.GetAllMetadataAsync(It.IsAny<ParamsDTO>()))
                .ReturnsAsync(new PaginationMetadataDTO(2,1,2));

            propertyRepositoryStub.Setup(repo => repo.GetAllAsync(It.IsAny<ParamsDTO>()))
                .ReturnsAsync(properties);

            propertyViewRepositoryStub.Setup(repo => repo.GetAllByParentIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<PropertyView>());

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.Get(new ParamsDTO());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<List<Property>>(response.Message);
        }

        /// <summary>
        /// GetAll ServerException ANd Returns Status 500 Internal ServerError
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAll_ServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            propertyRepositoryStub.Setup(repo => repo.GetAllMetadataAsync(It.IsAny<ParamsDTO>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Get(new ParamsDTO());

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }


        /// <summary>
        /// Post With Property To Create Returns Ok With Success Message
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Post_WithPropertyToCreate_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            var propertyToCreate = CreateRandomProperty();
            propertyRepositoryStub.Setup(repo => repo.AddAsync(propertyToCreate))
                .ReturnsAsync(propertyToCreate);

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Post(propertyToCreate);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);
        }

        /// <summary>
        /// Post Server Exception And Returns Status 500 Internal Server Error
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Post_ServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            propertyRepositoryStub.Setup(repo => repo.AddAsync(It.IsAny<Property>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Post(new Property());

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Put With Property To Update Returns Ok With Success Message
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Put_WithPropertyToUpdate_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            var propertyToUpdate = CreateRandomProperty();
            propertyRepositoryStub.Setup(repo => repo.EditAsync(propertyToUpdate))
                .ReturnsAsync(true);

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Put(new Guid(), propertyToUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);
        }

        /// <summary>
        /// Put With Property To Update Returns Ok With No Success Message
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Put_WithPropertyToUpdate_ReturnsOkWithNoSuccessMessage()
        {
            // Arrange
            var propertyToUpdate = CreateRandomProperty();
            propertyRepositoryStub.Setup(repo => repo.EditAsync(It.IsAny<Property>()))
                .ReturnsAsync(false);

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Put(new Guid(), propertyToUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.False(response.Success);
            Assert.IsType<string>(response.Message);
        }

        /// <summary>
        /// Put With Null And Returns Status 500 Internal Server Error
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Put_WithNull_ReturnsStatus500InternalServerError()
        {
            // Arrange
            var propertyToUpdate = CreateRandomProperty();
            propertyRepositoryStub.Setup(repo => repo.EditAsync(null))
                .ReturnsAsync(false);

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Put(new Guid(), null);

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete With Existing Id Returns Ok With Success Message
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Delete_WithExistingId_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            propertyRepositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Delete(new Guid());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);
        }

        /// <summary>
        /// Delete With No Existing Id Returns Ok With No Success Message
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Delete_WithServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            var propertyToUpdate = CreateRandomProperty();
            propertyRepositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertiesController(loggerStub.Object, propertyRepositoryStub.Object, propertyViewRepositoryStub.Object);

            // Act
            var result = await controller.Delete(new Guid());

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }


        /// <summary>
        /// Dummy Dto to Simulate results in the test functions
        /// </summary>
        /// <returns></returns>
        private Property CreateRandomProperty() {
            return new Property()
            {
                PropertyId = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                CoverPath = Guid.NewGuid().ToString(),
                OwnerId = 0,
                CodeInternal = Guid.NewGuid().ToString(),
                Price = 50000,
                Year = 2022
            };
        }
    }
}
