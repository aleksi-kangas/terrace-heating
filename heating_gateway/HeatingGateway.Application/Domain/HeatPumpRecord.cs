using Microsoft.EntityFrameworkCore;

namespace HeatingGateway.Application.Domain; 

[Index(nameof(Time), IsUnique = true)]
public class HeatPumpRecord {
  public DateTime Time { get; set; }
  public TankLimits TankLimits { get; set; } = null!;
  public Temperatures Temperatures { get; set; } = null!;
}