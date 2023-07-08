using HeatingGateway.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeatingGateway.Application.Persistence.Configurations; 

public class CompressorRecordConfiguration : IEntityTypeConfiguration<CompressorRecord> {
  
  public void Configure(EntityTypeBuilder<CompressorRecord> builder) {
    builder.ToTable("CompressorRecords");
    builder.HasKey(r => r.Time);
    builder.HasIndex(r => r.Time).IsUnique();
  }
}
