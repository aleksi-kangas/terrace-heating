import {fetchHeatPumpRecordsDays} from '../../api/heating';
import DashboardGraph from './dashboard_graph';
import {Suspense} from 'react';
import Spinner from '../spinner';

const OutsideTemperatureGraph = async () => {
  const records = await fetchHeatPumpRecordsDays(2);
  const xLimits =
    records.length > 0
      ? [records[0].time, records[records.length - 1].time]
      : undefined;

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <DashboardGraph
        className="flex-1 max-h-[45%] w-full"
        dateTimes={records.map(r => r.time)}
        values={records.map(r => r.temperatures.outside)}
        label="Outside °C"
        xLimits={xLimits}
      />
    </Suspense>
  );
};

export default OutsideTemperatureGraph;
