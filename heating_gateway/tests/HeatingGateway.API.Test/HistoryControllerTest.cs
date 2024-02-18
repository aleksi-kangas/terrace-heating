using ErrorOr;
using HeatingGateway.API.Controllers;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Services.Heating;
using HeatingGateway.Contracts.History;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HeatingGateway.API.Test;

public class HistoryControllerTest {
  [Fact]
  public async void GetCompressorRecords_WhenRequestValid_ShouldReturn200() {
    // Arrange
    var mockMapper = new Mock<IMapper>();
    var mockHeatingService = new Mock<IHeatingHistoryService>();
    mockHeatingService.Setup(x =>
        x.GetCompressorRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(new List<Compressor>()));
    var controller = new HistoryController(mockMapper.Object, mockHeatingService.Object);

    // Act
    var request =
      new GetCompressorRecordsRequest(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
    var result = await controller.GetCompressorRecords(request);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(200, okResult.StatusCode);
  }

  [Fact]
  public async void GetCompressorRecords_WhenRecordsFound_ShouldReturnRecords() {
    // Arrange
    var utcNow = DateTime.UtcNow;
    List<HeatPumpRecord> heatPumpRecords = new List<HeatPumpRecord>() {
      new HeatPumpRecord() {
        Compressor = new Compressor(true, 0.2),
        TankLimits = new TankLimits(1, 2, 3, 4, 5, 6, 7, 8),
        Temperatures = new Temperatures(9, 10, 11, 12, 13, 14, 15, 16, 17, 18),
        Time = utcNow
      }
    };
    var mockMapper = new Mock<IMapper>();
    mockMapper.Setup(x => x.Map<List<CompressorRecordResponse>>(heatPumpRecords))
      .Returns(new List<CompressorRecordResponse> { new(Active: true, Time: utcNow, Usage: 0.2) });
    var mockHeatingService = new Mock<IHeatingHistoryService>();
    mockHeatingService.Setup(x =>
        x.GetHeatPumpRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(heatPumpRecords));
    var controller = new HistoryController(mockMapper.Object, mockHeatingService.Object);

    // Act
    var request =
      new GetCompressorRecordsRequest(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
    var result = await controller.GetCompressorRecords(request);

    // Assert
    var okObjectResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<List<CompressorRecordResponse>>(okObjectResult.Value);
    Assert.Single(response);
  }

  [Fact]
  public async void GetHeatPumpRecords_WhenRequestValid_ShouldReturn200() {
    // Arrange
    var mockMapper = new Mock<IMapper>();
    var mockHeatingService = new Mock<IHeatingHistoryService>();
    mockHeatingService.Setup(x =>
        x.GetHeatPumpRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(new List<HeatPumpRecord>()));
    var controller = new HistoryController(mockMapper.Object, mockHeatingService.Object);

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
    List<HeatPumpRecord> heatPumpRecords = new List<HeatPumpRecord> {
      new() {
        TankLimits = new TankLimits(1, 2, 3, 4, 5, 6, 7, 8),
        Temperatures = new Temperatures(1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
        Time = utcNow
      }
    };
    var mockMapper = new Mock<IMapper>();
    mockMapper.Setup(x => x.Map<List<HeatPumpRecordResponse>>(heatPumpRecords))
      .Returns(new List<HeatPumpRecordResponse>() {
        new(new CompressorResponse(true, 0.2),
          new TankLimitsResponse(1, 2, 3, 4, 5, 6, 7, 8),
          new TemperaturesResponse(1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
          utcNow)
      });
    var mockHeatingService = new Mock<IHeatingHistoryService>();
    mockHeatingService.Setup(x =>
        x.GetHeatPumpRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(heatPumpRecords));
    var controller = new HistoryController(mockMapper.Object, mockHeatingService.Object);

    // Act
    var request = new GetHeatPumpRecordsRequest(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
    var result = await controller.GetHeatPumpRecords(request);

    // Assert
    var okObjectResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<List<HeatPumpRecordResponse>>(okObjectResult.Value);
    Assert.Single(response);
  }
}
