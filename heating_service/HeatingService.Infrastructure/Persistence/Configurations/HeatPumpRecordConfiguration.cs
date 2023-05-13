using HeatingService.Domain.HeatPump;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeatingService.Infrastructure.Persistence.Configurations; 

public class HeatPumpRecordConfiguration : IEntityTypeConfiguration<HeatPumpRecord> {
  
  public void Configure(EntityTypeBuilder<HeatPumpRecord> builder) {
    builder.ToTable("HeatPumpRecords");
    builder.HasKey(r => r.Id);
    builder.Property(r => r.Id).ValueGeneratedOnAdd();
    builder.OwnsOne(r => r.TankLimits);
    builder.OwnsOne(r => r.Temperatures);
  }
}