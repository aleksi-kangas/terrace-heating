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
      .ReturnsAsync(ErrorOrFactory.From(new List<CompressorRecord>()));
    var controller = new HistoryController(mockMapper.Object, mockHeatingService.Object);

    // Act
    var request = new DateTimeRangeRequest(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
    var result = await controller.GetCompressorRecords(request);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(200, okResult.StatusCode);
  }
  
  [Fact]
  public async void GetCompressorRecords_WhenRequestInvalidDatetimeRange_ShouldReturnProblem() {
    // Arrange
    var mockMapper = new Mock<IMapper>();
    var mockHeatingService = new Mock<IHeatingHistoryService>();
    mockHeatingService.Setup(x =>
        x.GetCompressorRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(new List<CompressorRecord>()));
    var controller = new HistoryController(mockMapper.Object, mockHeatingService.Object);

    // Act
    var request = new DateTimeRangeRequest(DateTime.Now, DateTime.Now - TimeSpan.FromDays(1));
    var result = await controller.GetCompressorRecords(request);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    Assert.Equal(200, okResult.StatusCode);
  }

  [Fact]
  public async void GetCompressorRecords_WhenRecordsFound_ShouldReturnRecords() {
    // Arrange
    var utcNow = DateTime.UtcNow;
    var compressorRecords = new List<CompressorRecord> { new(Time: utcNow, Active: true, Usage: 0.2) };
    var mockMapper = new Mock<IMapper>();
    mockMapper.Setup(x => x.Map<List<CompressorRecordResponse>>(compressorRecords))
      .Returns(new List<CompressorRecordResponse> { new(Active: true, Time: utcNow, Usage: 0.2) });
    var mockHeatingHistoryService = new Mock<IHeatingHistoryService>();
    mockHeatingHistoryService.Setup(x =>
        x.GetCompressorRecordsDateTimeRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
      .ReturnsAsync(ErrorOrFactory.From(compressorRecords));
    var controller = new HistoryController(mockMapper.Object, mockHeatingHistoryService.Object);

    // Act
    var request = new DateTimeRangeRequest(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
    var result = await controller.GetCompressorRecords(request);

    // Assert
    var okObjectResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<List<CompressorRecordResponse>>(okObjectResult.Value);
    Assert.Single(response);
  }
}
