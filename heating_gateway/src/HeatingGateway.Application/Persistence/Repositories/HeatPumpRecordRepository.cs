using HeatingGateway.Application.Domain;

namespace HeatingGateway.Application.Persistence.Repositories; 

public class HeatPumpRecordRepository : RepositoryBase<HeatPumpRecord>, IHeatPumpRecordRepository {
  public HeatPumpRecordRepository(HeatingDbContext context) : base(context) {}
  
  public async Task<IEnumerable<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to, bool trackChanges)
  {
    return await FindByConditionAsync(r => from <= r.Time && r.Time <= to, trackChanges);
  }
}
