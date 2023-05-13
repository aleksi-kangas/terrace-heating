namespace HeatingService.Domain.HeatPump; 

public class HeatPumpRecord {
  public Guid Id { get; set; }
  public DateTime TimeStamp { get; set; }
  public TankLimits TankLimits { get; set; } = null!;
  public Temperatures Temperatures { get; set; } = null!;
}