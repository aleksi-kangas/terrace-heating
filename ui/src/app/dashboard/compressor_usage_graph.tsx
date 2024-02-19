import React, {Suspense} from 'react';
import {DateTime} from 'luxon';
import {fetchCompressorRecordsDaysRange} from '@/app/api/history';
import {CompressorRecord} from '@/app/api/types';
import Graph from '@/app/dashboard/graph';
import Spinner from '@/app/components/spinner';

const CompressorUsageGraph = async () => {
  const records: CompressorRecord[] = await fetchCompressorRecordsDaysRange(2);
  const compressorUsageRecords = records.filter(
    (r: CompressorRecord) => r.usage !== null
  );
  const xLimits =
    records.length > 0
      ? [records[0].time, records[records.length - 1].time]
      : undefined;
  const yLimits = [0, 100];

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <Graph
        className="flex-1 max-h-[45%] w-full"
        data={compressorUsageRecords.map(r => ({
          x: DateTime.fromISO(r.time).toLocal().toMillis(),
          y: r.usage! * 100,
        }))}
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
