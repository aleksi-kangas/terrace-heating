using HeatingGateway.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeatingGateway.Application.Persistence.Configurations; 

public class HeatPumpRecordConfiguration : IEntityTypeConfiguration<HeatPumpRecord> {
  
  public void Configure(EntityTypeBuilder<HeatPumpRecord> builder) {
    builder.ToTable("HeatPumpRecords");
    builder.HasKey(r => r.Time);
    builder.HasIndex(r => r.Time).IsUnique();
    builder.OwnsOne(r => r.TankLimits);
    builder.OwnsOne(r => r.Temperatures);
  }
}
