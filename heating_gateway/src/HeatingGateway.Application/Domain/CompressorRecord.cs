using Microsoft.EntityFrameworkCore;

namespace HeatingGateway.Application.Domain;

[Index(nameof(Time), IsUnique = true)]
public class CompressorRecord {
  public DateTime Time { get; set; }
  public bool Active { get; set; }
  public double? Usage { get; set; }
}
