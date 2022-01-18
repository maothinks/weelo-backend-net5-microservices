using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Domain.Interfaces.Repositories;
using Weelo.Microservices.Properties.Infrastructure.API.Controllers;
using Weelo.Microservices.Properties.Infrastructure.API.Models;
using Xunit;

namespace Weelo.Microservices.Properties.Infrastructure.APITests
{
    public  class PropertyImagesControllerTests
    {
        private readonly Mock<IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid>> propertyRepositoryStub = new();
        private readonly Mock<IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyImage, Guid>> propertyImageRepositoryStub = new();
        private readonly Mock<ILogger<PropertyImagesController>> loggerStub = new();

        /// <summary>
        /// GetById Without Existing Id And Returns Ok With Null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetById_WithoutExistingId_ReturnsOkWithNull()
        {
            // Arrange
            propertyImageRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((PropertyImage)null);

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.GetById(new Guid());

            // Asserts
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.Null(response.Message);
        }

        /// <summary>
        /// GetById With Existing Id Returns Ok With Property Image
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetById_WithExistingId_ReturnsOkWithPropertyImage()
        {
            // Arrange
            var expectedPropertyImage = CreateRandomPropertyImage();
            propertyImageRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedPropertyImage);

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.GetById(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<PropertyImage>(response.Message);

            var resultPropertyImage = response.Message as PropertyImage;
            Assert.Equal(expectedPropertyImage.PropertyImageId, resultPropertyImage.PropertyImageId);
            Assert.Equal(expectedPropertyImage.PropertyId, resultPropertyImage.PropertyId);
            Assert.Equal(expectedPropertyImage.Enabled, resultPropertyImage.Enabled);
            Assert.Equal(expectedPropertyImage.FilePath, resultPropertyImage.FilePath);
        }

        /// <summary>
        /// Get By Id Server Exception Returns Status 500 Internal Server Error
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetById_ServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            propertyImageRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.GetById(Guid.NewGuid());

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetAll_WithFilterAndPagination_ReturnsOkWithPropertyImageList()
        {
            // Arrange
            List<PropertyImage> propertyImages = new();
            propertyImages.Add(CreateRandomPropertyImage());

            propertyImageRepositoryStub.Setup(repo => repo.GetAllMetadataAsync(It.IsAny<ParamsDTO>()))
                .ReturnsAsync(new PaginationMetadataDTO(2, 1, 2));

            propertyImageRepositoryStub.Setup(repo => repo.GetAllAsync(It.IsAny<ParamsDTO>()))
                .ReturnsAsync(new List<PropertyImage>());

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.Get(new ParamsDTO());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<List<PropertyImage>>(response.Message);
        }

        [Fact]
        public async Task GetAll_ServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            propertyRepositoryStub.Setup(repo => repo.GetAllMetadataAsync(It.IsAny<ParamsDTO>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.Get(new ParamsDTO());

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task Post_WithPropertyImageToCreate_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            var propertyImageToCreate = CreateRandomPropertyImage();
            propertyImageRepositoryStub.Setup(repo => repo.AddAsync(propertyImageToCreate))
                .ReturnsAsync(propertyImageToCreate);

            propertyRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Property());

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.Post(propertyImageToCreate);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);
        }

        [Fact]
        public async Task Post_ServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            propertyImageRepositoryStub.Setup(repo => repo.AddAsync(It.IsAny<PropertyImage>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.Post(new PropertyImage());

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task Put_WithPropertyImageToUpdate_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            var propertyImageToUpdate = CreateRandomPropertyImage();
            propertyImageRepositoryStub.Setup(repo => repo.EditAsync(propertyImageToUpdate))
                .ReturnsAsync(true);

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.Put(new Guid(), propertyImageToUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);
        }

        [Fact]
        public async Task Put_WithPropertyImageToUpdate_ReturnsOkWithNoSuccessMessage()
        {
            // Arrange
            var propertyImageToUpdate = CreateRandomPropertyImage();
            propertyImageRepositoryStub.Setup(repo => repo.EditAsync(It.IsAny<PropertyImage>()))
                .ReturnsAsync(false);

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.Put(new Guid(), propertyImageToUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.False(response.Success);
            Assert.IsType<string>(response.Message);
        }

        [Fact]
        public async Task Put_WithNull_ReturnsStatus500InternalServerError()
        {
            // Arrange
            var propertyImageToUpdate = CreateRandomPropertyImage();
            propertyRepositoryStub.Setup(repo => repo.EditAsync(null))
                .ReturnsAsync(false);

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.Put(new Guid(), null);

            // Asserts
            Assert.IsType<StatusCodeResult>(result.Result);
            var response = ((StatusCodeResult)result.Result);

            Assert.Equal<int>(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task Delete_WithExistingId_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            propertyImageRepositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

            // Act
            var result = await controller.Delete(new Guid());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);
        }

        [Fact]
        public async Task Delete_WithServerException_ReturnsStatus500InternalServerError()
        {
            // Arrange
            var propertyImageToUpdate = CreateRandomPropertyImage();
            propertyImageRepositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception("InternalServerError"));

            var controller = new PropertyImagesController(loggerStub.Object, propertyImageRepositoryStub.Object, propertyRepositoryStub.Object);

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
        private PropertyImage CreateRandomPropertyImage()
        {
            return new PropertyImage()
            {
                PropertyImageId = Guid.NewGuid(),
                PropertyId = Guid.NewGuid(),
                Enabled = true,
                FilePath = ""
            };
        }
    }
}
