import React, {Suspense} from 'react';
import {DateTime} from 'luxon';
import {fetchHeatPumpRecordsDays} from '@/app/api/heating';
import Spinner from '@/app/components/spinner';
import Graph from '@/app/dashboard/graph';

const OutsideTemperatureGraph = async () => {
  const records = await fetchHeatPumpRecordsDays(2);
  const xLimits =
    records.length > 0
      ? [records[0].time, records[records.length - 1].time]
      : undefined;

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <Graph
        className="flex-1 max-h-[45%] w-full"
        data={records.map(r => ({
          x: DateTime.fromISO(r.time).toLocal().toMillis(),
          y: r.temperatures.outside,
        }))}
        label="Outside °C"
        xLimits={xLimits}
      />
    </Suspense>
  );
};

export default OutsideTemperatureGraph;
