using Microsoft.EntityFrameworkCore;

namespace HeatingService.API.Domain; 

public class HeatingDbContext : DbContext {
  public HeatingDbContext(DbContextOptions<HeatingDbContext> options) : base(options) {}
  
  public DbSet<HeatPumpRecord> HeatPumpRecords { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(HeatingDbContext).Assembly);
  }
}
