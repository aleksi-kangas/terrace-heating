using ErrorOr;
using HeatingGateway.API.Controllers;
using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Services;
using HeatingGateway.Contracts.Heating;
using HeatingGateway.Contracts.HeatPump;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HeatingGateway.API.Test;

public class HeatingControllerTest {
  [Fact]
  public async void GetHeatPumpRecords_WhenRequestValid_ShouldReturn200() {
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

  [Fact]
  public async void GetHeatPumpRecords_WhenRecordsFound_ShouldReturnRecords() {
    // Arrange
    var utcNow = DateTime.UtcNow;
    IEnumerable<HeatPumpRecord> heatPumpRecords = new List<HeatPumpRecord> {
      new() {
        TankLimits = new TankLimits(1, 2, 3, 4, 5, 6, 7, 8),
        Temperatures = new Temperatures(1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
        Time = utcNow
      }
    };
    var mockMapper = new Mock<IMapper>();
    mockMapper.Setup(x => x.Map<List<HeatPumpRecordResponse>>(heatPumpRecords))
      .Returns(new List<HeatPumpRecordResponse>() {
        new(new TankLimitsResponse(1, 2, 3, 4, 5, 6, 7, 8),
          new TemperaturesResponse(1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
          utcNow)
      });
    var mockHeatingService = new Mock<IHeatingService>();
    mockHeatingService.Setup(x =>
        x.GetHeatPumpRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(heatPumpRecords));
    var controller = new HeatingController(mockMapper.Object, mockHeatingService.Object);

    // Act
    var request = new GetHeatPumpRecordsRequest(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
    var result = await controller.GetHeatPumpRecords(request);

    // Assert
    var okObjectResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<List<HeatPumpRecordResponse>>(okObjectResult.Value);
    Assert.Single(response);
  }
}
