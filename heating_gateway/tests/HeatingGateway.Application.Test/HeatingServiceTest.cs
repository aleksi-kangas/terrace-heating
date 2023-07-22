using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services;
using Moq;

namespace HeatingGateway.Application.Test;

public class HeatingServiceTest {
  [Fact]
  public async void GetCompressorRecordDateTimeRangeAsync_WhenFromToValid_ReturnsRecordsInRange() {
    // Arrange
    var fromDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var toDatetime = DateTime.UtcNow;
    var compressorRecords = new List<CompressorRecord> {
      new() { Active = true, Time = fromDatetime, Usage = 0.1 },
      new() { Active = true, Time = toDatetime, Usage = 0.2 }
    };
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    mockCompressorRecordRepository
      .Setup(x => x.FindByDateTimeRangeAsync(fromDatetime, toDatetime, false))
      .ReturnsAsync(compressorRecords);
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingService(mockCompressorRecordRepository.Object,
      mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetCompressorRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.False(result.IsError);
    Assert.Equal(compressorRecords, result.Value);
  }

  [Fact]
  public async void
    GetCompressorRecordDateTimeRangeAsync_WhenFromToNotUTC_ReturnsValidationError() {
    var fromDatetime = DateTime.Now - TimeSpan.FromDays(1);
    var toDatetime = DateTime.Now;
    Assert.NotEqual(DateTimeKind.Utc, fromDatetime.Kind);
    Assert.NotEqual(DateTimeKind.Utc, toDatetime.Kind);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingService(mockCompressorRecordRepository.Object,
      mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetCompressorRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.DatetimeNotUTC, result.FirstError);
  }

  [Fact]
  public async void GetCompressorRecordDateTimeRangeAsync_WhenFromAfterTo_ReturnsValidationError() {
    var fromDatetime = DateTime.UtcNow;
    var toDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingService(mockCompressorRecordRepository.Object,
      mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetCompressorRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.FromIsAfterTo, result.FirstError);
  }
  
  [Fact]
  public async void GetHeatPumpRecordDateTimeRangeAsync_WhenFromToValid_ReturnsRecordsInRange() {
    // Arrange
    var fromDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var toDatetime = DateTime.UtcNow;
    var heatPumpRecords = new List<HeatPumpRecord> {
      new() {
        TankLimits = new TankLimits(1, 2, 3, 4, 5, 6, 7, 8),
        Temperatures = new Temperatures(1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
        Time = toDatetime
      }
    };
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    mockHeatPumpRecordRepository
      .Setup(x => x.FindByDateTimeRangeAsync(fromDatetime, toDatetime, false))
      .ReturnsAsync(heatPumpRecords);
    var service = new HeatingService(mockCompressorRecordRepository.Object,
      mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetHeatPumpRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.False(result.IsError);
    Assert.Equal(heatPumpRecords, result.Value);
  }

  [Fact]
  public async void GetHeatPumpRecordDateTimeRangeAsync_WhenFromToNotUTC_ReturnsValidationError() {
    var fromDatetime = DateTime.Now - TimeSpan.FromDays(1);
    var toDatetime = DateTime.Now;
    Assert.NotEqual(DateTimeKind.Utc, fromDatetime.Kind);
    Assert.NotEqual(DateTimeKind.Utc, toDatetime.Kind);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingService(mockCompressorRecordRepository.Object,
      mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetHeatPumpRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.DatetimeNotUTC, result.FirstError);
  }

  [Fact]
  public async void GetHeatPumpRecordDateTimeRangeAsync_WhenFromAfterTo_ReturnsValidationError() {
    var fromDatetime = DateTime.UtcNow;
    var toDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingService(mockCompressorRecordRepository.Object,
      mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetHeatPumpRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.FromIsAfterTo, result.FirstError);
  }
}
