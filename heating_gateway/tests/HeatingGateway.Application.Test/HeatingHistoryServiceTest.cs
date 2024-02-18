using HeatingGateway.Application.Common.Errors;
using HeatingGateway.Application.Domain;
using HeatingGateway.Application.Persistence.Repositories;
using HeatingGateway.Application.Services.Heating;
using Moq;

namespace HeatingGateway.Application.Test;

public class HeatingHistoryServiceTest {
  [Fact]
  public async void GetCompressorRecordsDateTimeRangeAsync_WhenFromToValid_ReturnsRecordsInRange() {
    // Arrange
    var fromDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var toDatetime = DateTime.UtcNow;
    var compressorRecords = new List<CompressorRecord> {
      new(
        fromDatetime,
        true,
        0.1),
      new(
        toDatetime,
        false,
        0.2)
    };
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    mockCompressorRecordRepository
      .Setup(x => x.FindByDateTimeRangeAsync(fromDatetime, toDatetime))
      .ReturnsAsync(compressorRecords);
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetCompressorRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.False(result.IsError);
    Assert.Equal(
      new List<CompressorRecord> { new(fromDatetime, true, 0.1), new(toDatetime, false, 0.2) }, result.Value);
  }

  [Fact]
  public async void
    GetCompressorRecordsDateTimeRangeAsync_WhenFromToNotUTC_ReturnsValidationError() {
    // Arrange
    var fromDatetime = DateTime.Now - TimeSpan.FromDays(1);
    var toDatetime = DateTime.Now;
    Assert.NotEqual(DateTimeKind.Utc, fromDatetime.Kind);
    Assert.NotEqual(DateTimeKind.Utc, toDatetime.Kind);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetCompressorRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.DatetimeNotUTC, result.FirstError);
  }

  [Fact]
  public async void GetCompressorRecordsDateTimeRangeAsync_WhenFromAfterTo_ReturnsValidationError() {
    // Arrange
    var fromDatetime = DateTime.UtcNow;
    var toDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetCompressorRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.FromIsAfterTo, result.FirstError);
  }

  [Fact]
  public async void GetTankLimitRecordsDateTimeRangeAsync_WhenFromToValid_ReturnsRecordsInRange() {
    // Arrange
    var fromDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var toDatetime = DateTime.UtcNow;
    var tankLimitRecords = new List<TankLimitRecord> {
      new(
        fromDatetime,
        40,
        44,
        48,
        52,
        60,
        65,
        70,
        75),
      new(
        toDatetime,
        30,
        34,
        38,
        42,
        50,
        55,
        60,
        65)
    };
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    mockTankLimitRecordRepository
      .Setup(x => x.FindByDateTimeRangeAsync(fromDatetime, toDatetime))
      .ReturnsAsync(tankLimitRecords);
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetTankLimitRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.False(result.IsError);
    Assert.Equal(
      new List<TankLimitRecord> {
        new(
          fromDatetime,
          40,
          44,
          48,
          52,
          60,
          65,
          70,
          75),
        new(
          toDatetime,
          30,
          34,
          38,
          42,
          50,
          55,
          60,
          65)
      }, result.Value);
  }
  
  [Fact]
  public async void
    GetTankLimitRecordsDateTimeRangeAsync_WhenFromToNotUTC_ReturnsValidationError() {
    // Arrange
    var fromDatetime = DateTime.Now - TimeSpan.FromDays(1);
    var toDatetime = DateTime.Now;
    Assert.NotEqual(DateTimeKind.Utc, fromDatetime.Kind);
    Assert.NotEqual(DateTimeKind.Utc, toDatetime.Kind);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetTankLimitRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.DatetimeNotUTC, result.FirstError);
  }
  
  [Fact]
  public async void GetTankLimitRecordsDateTimeRangeAsync_WhenFromAfterTo_ReturnsValidationError() {
    // Arrange
    var fromDatetime = DateTime.UtcNow;
    var toDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetTankLimitRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.FromIsAfterTo, result.FirstError);
  }
  
  [Fact]
  public async void GetTemperatureRecordsDateTimeRangeAsync_WhenFromToValid_ReturnsRecordsInRange() {
    // Arrange
    var fromDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var toDatetime = DateTime.UtcNow;
    var temperatureRecords = new List<TemperatureRecord> {
      new(
        fromDatetime,
        1.0f,
        2.0f,
        3.0f,
        4.0f,
        5.0f,
        6.0f,
        7.0f,
        8.0f,
        9.0f,
        10.0f),
      new(
        toDatetime,
        -1.0f,
        -2.0f,
        -3.0f,
        -4.0f,
        -5.0f,
        -6.0f,
        -7.0f,
        -8.0f,
        -9.0f,
        -10.0f)
    };
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    mockTemperatureRecordRepository
      .Setup(x => x.FindByDateTimeRangeAsync(fromDatetime, toDatetime))
      .ReturnsAsync(temperatureRecords);
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetTemperatureRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.False(result.IsError);
    Assert.Equal(
      new List<TemperatureRecord> {
        new(
          fromDatetime,
          1.0f,
          2.0f,
          3.0f,
          4.0f,
          5.0f,
          6.0f,
          7.0f,
          8.0f,
          9.0f,
          10.0f),
        new(
          toDatetime,
          -1.0f,
          -2.0f,
          -3.0f,
          -4.0f,
          -5.0f,
          -6.0f,
          -7.0f,
          -8.0f,
          -9.0f,
          -10.0f)
      }, result.Value);
  }
  
  [Fact]
  public async void
    GetTemperatureRecordsDateTimeRangeAsync_WhenFromToNotUTC_ReturnsValidationError() {
    // Arrange
    var fromDatetime = DateTime.Now - TimeSpan.FromDays(1);
    var toDatetime = DateTime.Now;
    Assert.NotEqual(DateTimeKind.Utc, fromDatetime.Kind);
    Assert.NotEqual(DateTimeKind.Utc, toDatetime.Kind);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetTemperatureRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.DatetimeNotUTC, result.FirstError);
  }
  
  [Fact]
  public async void GetTemperatureRecordsDateTimeRangeAsync_WhenFromAfterTo_ReturnsValidationError() {
    // Arrange
    var fromDatetime = DateTime.UtcNow;
    var toDatetime = DateTime.UtcNow - TimeSpan.FromDays(1);
    var mockCompressorRecordRepository = new Mock<ICompressorRecordRepository>();
    var mockTankLimitRecordRepository = new Mock<ITankLimitRecordRepository>();
    var mockTemperatureRecordRepository = new Mock<ITemperatureRecordRepository>();
    var service = new HeatingHistoryService(mockCompressorRecordRepository.Object, mockTankLimitRecordRepository.Object,
      mockTemperatureRecordRepository.Object);

    // Act
    var result = await service.GetTemperatureRecordsDateTimeRangeAsync(fromDatetime, toDatetime);

    // Assert
    Assert.True(result.IsError);
    Assert.Equal(Errors.DateTimeRange.FromIsAfterTo, result.FirstError);
  }
}
