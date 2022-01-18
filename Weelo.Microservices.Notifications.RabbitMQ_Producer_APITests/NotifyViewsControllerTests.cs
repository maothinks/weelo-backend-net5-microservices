using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Weelo.Microservices.Notifications.RabbitMQ_Producer_API.Controllers;
using Xunit;

namespace Weelo.Microservices.Notifications.RabbitMQ_Producer_APITests
{
    public class NotifyViewsControllerTests
    {
        private readonly Mock<IBusControl> busStub = new();
        private readonly Mock<ILogger<NotifyViewsController>> loggerStub = new();

        [Fact]
        public async Task Post_SendMessageToEndPoints_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            var mockSendEndpoint = new Mock<ISendEndpoint>();

            busStub.Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>()))
                .ReturnsAsync(mockSendEndpoint.Object);

            var controller = new NotifyViewsController(busStub.Object, loggerStub.Object);

            // Act
            var result = await controller.Post(new PropertyView());

            // Assert

            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);

        }
    }
}
