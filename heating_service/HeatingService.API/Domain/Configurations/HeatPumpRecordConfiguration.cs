using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeatingService.API.Domain.Configurations; 

public class HeatPumpRecordConfiguration : IEntityTypeConfiguration<HeatPumpRecord> {
  
  public void Configure(EntityTypeBuilder<HeatPumpRecord> builder) {
    builder.ToTable("HeatPumpRecords");
    builder.HasKey(r => r.Id);
    builder.Property(r => r.Id).ValueGeneratedOnAdd();
    builder.OwnsOne(r => r.TankLimits);
    builder.OwnsOne(r => r.Temperatures);

    InitialSampleData(builder);
  }

  private void InitialSampleData(EntityTypeBuilder<HeatPumpRecord> builder)
  {
    builder.HasData(new HeatPumpRecord
    {
      Id = new Guid("d743aa73-829f-4743-a699-f1ca7d578cc7"),
      TimeStamp = DateTime.UtcNow
    });
    builder.OwnsOne(r => r.TankLimits).HasData(new
    {
      HeatPumpRecordId = new Guid("d743aa73-829f-4743-a699-f1ca7d578cc7"),
      LowerTankMinimum = 40U,
      LowerTankMaximum = 50U,
      UpperTankMinimum = 41U,
      UpperTankMaximum = 51U
    });
    builder.OwnsOne(r => r.Temperatures).HasData(new
    {
      HeatPumpRecordId = new Guid("d743aa73-829f-4743-a699-f1ca7d578cc7"),
      Circuit1 = 10.0f,
      Circuit2 = 20.0f,
      Circuit3 = 30.0f,
      GroundInput = 40.0f,
      GroundOutput = 50.0f,
      HotGas = 60.0f,
      Inside = 70.0f,
      LowerTank = 80.0f,
      Outside = 90.0f,
      UpperTank = 100.0f
    });
  }
}