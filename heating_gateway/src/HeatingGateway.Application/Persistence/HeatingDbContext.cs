using HeatingGateway.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace HeatingGateway.Application.Persistence; 

public class HeatingDbContext : DbContext {
  public HeatingDbContext(DbContextOptions<HeatingDbContext> options) : base(options) {}
  public DbSet<HeatPumpRecord> HeatPumpRecords { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(HeatingDbContext).Assembly);
  }
}
