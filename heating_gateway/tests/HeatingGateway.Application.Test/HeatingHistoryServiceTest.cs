using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services.Heating;
using Moq;

namespace HeatingGateway.Application.Test;

public class HeatingHistoryServiceTest {
  [Fact]
  public async void GetCompressorRecordDateTimeRangeAsync_WhenFromToValid_ReturnsRecordsInRange() {
    // Arrange
    var fromDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var toDatetime = DateTime.UtcNow;
    var heatPumpRecords = new List<HeatPumpRecord> {
      new() {
        Time = fromDatetime,
        Compressor = new Compressor(true, 0.1),
        TankLimits = new TankLimits(1, 1, 1, 1, 1, 1, 1, 1),
        Temperatures = new Temperatures(1, 1, 1, 1, 1, 1, 1, 1, 1, 1)
      },
      new() {
        Time = fromDatetime,
        Compressor = new Compressor(false, 0.2),
        TankLimits = new TankLimits(1, 1, 1, 1, 1, 1, 1, 1),
        Temperatures = new Temperatures(1, 1, 1, 1, 1, 1, 1, 1, 1, 1)
      },
    };

    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();

    mockHeatPumpRecordRepository
      .Setup(x => x.FindByDateTimeRangeAsync(fromDatetime, toDatetime, It.IsAny<bool>()))
      .ReturnsAsync(heatPumpRecords);

    var service = new HeatingHistoryService(mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetCompressorRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.False(result.IsError);
    Assert.Equal(
      new List<Compressor> { new(true, 0.1), new(false, 0.2) }, result.Value);
  }

  [Fact]
  public async void
    GetCompressorRecordDateTimeRangeAsync_WhenFromToNotUTC_ReturnsValidationError() {
    var fromDatetime = DateTime.Now - TimeSpan.FromDays(1);
    var toDatetime = DateTime.Now;
    Assert.NotEqual(DateTimeKind.Utc, fromDatetime.Kind);
    Assert.NotEqual(DateTimeKind.Utc, toDatetime.Kind);
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingHistoryService(mockHeatPumpRecordRepository.Object);

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
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingHistoryService(mockHeatPumpRecordRepository.Object);

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
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    mockHeatPumpRecordRepository
      .Setup(x => x.FindByDateTimeRangeAsync(fromDatetime, toDatetime, false))
      .ReturnsAsync(heatPumpRecords);
    var service = new HeatingHistoryService(mockHeatPumpRecordRepository.Object);

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
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingHistoryService(mockHeatPumpRecordRepository.Object);

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
    var mockHeatPumpRecordRepository = new Mock<IHeatPumpRecordRepository>();
    var service = new HeatingHistoryService(mockHeatPumpRecordRepository.Object);

    // Act
    var result = await service.GetHeatPumpRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.FromIsAfterTo, result.FirstError);
  }
}
