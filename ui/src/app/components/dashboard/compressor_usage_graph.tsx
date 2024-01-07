import {fetchCompressorRecordsDays} from '../../api/heating';
import DashboardGraph from './dashboard_graph';
import {Suspense} from 'react';
import Spinner from '../spinner';

const CompressorUsageGraph = async () => {
  const records = await fetchCompressorRecordsDays(2);
  const xLimits =
    records.length > 0
      ? [records[0].time, records[records.length - 1].time]
      : undefined;
  const yLimits = [0, 100];

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <DashboardGraph
        className="flex-1 max-h-[45%] w-full"
        dateTimes={records.map(r => r.time)}
        values={records
          .filter(r => r.usage !== undefined)
          .map(r => r.usage! * 100)}
        label="Compressor Usage %"
        xLimits={xLimits}
        yLimits={yLimits}
      />
    </Suspense>
  );
};

export default CompressorUsageGraph;
