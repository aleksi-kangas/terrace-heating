using HeatingService.Application.Persistence;
using HeatingService.Domain.HeatPump;

namespace HeatingService.Infrastructure.Persistence.Repositories; 

public class HeatPumpRecordRepository : RepositoryBase<HeatPumpRecord>, IHeatPumpRecordRepository {
  public HeatPumpRecordRepository(HeatingDbContext context) : base(context) {}
  
  public async Task<IEnumerable<HeatPumpRecord>> FindByDateTimeRangeAsync(DateTime from, DateTime to, bool trackChanges)
  {
    return await FindByConditionAsync(r => from <= r.TimeStamp && r.TimeStamp <= to, trackChanges);
  }
}
