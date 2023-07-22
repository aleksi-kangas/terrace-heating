import {DateTime, Duration} from 'luxon';
import {fetchCompressorRecords} from '../../api/heating';
import DashboardGraph from './dashboard_graph';
import {Suspense} from 'react';
import Spinner from '../spinner';

const CompressorUsageGraph = async () => {
  const from = DateTime.utc().minus(Duration.fromObject({days: 2}));
  const to = DateTime.utc();
  const records = await fetchCompressorRecords(from, to);

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <DashboardGraph
        className="flex-1 max-h-[45%] w-full"
        dateTimes={records.map(r => r.time)}
        values={records
          .filter(r => r.usage !== undefined)
          .map(r => r.usage! * 100)}
        label="Compressor Usage %"
        xLimits={[from.toISO()!, to.toISO()!]}
        yLimits={[0, 100]}
      />
    </Suspense>
  );
};

export default CompressorUsageGraph;
