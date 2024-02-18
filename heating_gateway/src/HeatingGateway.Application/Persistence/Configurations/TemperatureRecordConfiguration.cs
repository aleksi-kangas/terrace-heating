using HeatingGateway.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeatingGateway.Application.Persistence.Configurations; 

public class TemperatureRecordConfiguration : IEntityTypeConfiguration<TemperatureRecord> {
  
  public void Configure(EntityTypeBuilder<TemperatureRecord> builder) {
    builder.ToTable("TemperatureRecords");
    builder.HasKey(r => r.Time);
    builder.HasIndex(r => r.Time).IsUnique();
  }
}
