using HeatingGateway.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeatingGateway.Application.Persistence.Configurations; 

public class TankLimitRecordConfiguration : IEntityTypeConfiguration<TankLimitRecord> {
  
  public void Configure(EntityTypeBuilder<TankLimitRecord> builder) {
    builder.ToTable("TankLimitRecords");
    builder.HasKey(r => r.Time);
    builder.HasIndex(r => r.Time).IsUnique();
  }
}
