using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Weelo.Microservices.Notifications.SignalrAPI.Controllers;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs.Clients;
using Weelo.Microservices.Notifications.SignalrAPI.Models;
using Xunit;

namespace Weelo.Microservices.Notifications.SignalrAPITests
{
    public class MessageControllerTests
    {
        private readonly Mock<IHubContext<WeeloHub, IWeeloClient>> weeloHubStub = new();
        private readonly Mock<ILogger<MessageController>> loggerStub = new();

        [Fact]
        public async Task Post_SendSignalRMessageToClients_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            weeloHubStub.Setup(hub => hub.Clients.All.ReceiveMessage(It.IsAny<WeeloMessage>()))
                .Returns(Task.FromResult(true));

            var controller = new MessageController(loggerStub.Object, weeloHubStub.Object);

            // Act
            var result = await controller.Post("TestMessage");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var response = ((ObjectResult)result.Result).Value as JsonResponse;

            Assert.True(response.Success);
            Assert.IsType<string>(response.Message);

        }
    }
}
