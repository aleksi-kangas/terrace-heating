import React, {Suspense} from 'react';
import {fetchCompressorRecordsDays} from '@/app/api/heating';
import Graph from '@/app/dashboard/graph';
import Spinner from '@/app/components/spinner';

const CompressorUsageGraph = async () => {
  const records = await fetchCompressorRecordsDays(2);
  const xLimits =
    records.length > 0
      ? [records[0].time, records[records.length - 1].time]
      : undefined;
  const yLimits = [0, 100];

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <Graph
        className="flex-1 max-h-[45%] w-full"
        dateTimes={records.map(r => r.time)}
        values={records.filter(r => r.usage !== null).map(r => r.usage! * 100)}
        label="Compressor Usage %"
        xLimits={xLimits}
        yLimits={yLimits}
        fill={{
          target: 'origin',
          above: 'rgba(110, 160, 176, 0.4)',
        }}
        stepped="after"
      />
    </Suspense>
  );
};

export default CompressorUsageGraph;
