using ErrorOr;
using HeatingGateway.API.Controllers;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Services;
using HeatingGateway.Contracts.Heating;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HeatingGateway.API.Test;

public class HeatingControllerTest {
  [Fact]
  public async void GetHeatPumpRecords_WhenRecordsFound_ShouldReturn200() {
    // Arrange
    var mockMapper = new Mock<IMapper>();
    var mockHeatingService = new Mock<IHeatingService>();
    mockHeatingService.Setup(x =>
        x.GetHeatPumpRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(Enumerable.Empty<HeatPumpRecord>()));
    var controller = new HeatingController(mockMapper.Object, mockHeatingService.Object);

    // Act
    var request = new GetHeatPumpRecordsRequest(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
    var result = await controller.GetHeatPumpRecords(request);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(200, okResult.StatusCode);
  }
}
