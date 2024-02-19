import React, {Suspense} from 'react';
import {DateTime} from 'luxon';
import {fetchTemperatureRecordsDaysRange} from '@/app/api/history';
import {TemperatureRecord} from '@/app/api/types';
import Spinner from '@/app/components/spinner';
import Graph from '@/app/dashboard/graph';

const OutsideTemperatureGraph = async () => {
  const records: TemperatureRecord[] =
    await fetchTemperatureRecordsDaysRange(2);
  const xLimits =
    records.length > 0
      ? [records[0].time, records[records.length - 1].time]
      : undefined;

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <Graph
        className="flex-1 max-h-[45%] w-full"
        data={records.map((r: TemperatureRecord) => ({
          x: DateTime.fromISO(r.time).toLocal().toMillis(),
          y: r.outside,
        }))}
        label="Outside °C"
        xLimits={xLimits}
      />
    </Suspense>
  );
};

export default OutsideTemperatureGraph;
