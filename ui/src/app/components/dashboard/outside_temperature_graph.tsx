import {DateTime, Duration} from 'luxon';
import {fetchRecords} from '../../api/heating';
import DashboardGraph from './dashboard_graph';
import {Suspense} from 'react';
import Spinner from '../spinner';

const OutsideTemperatureGraph = async () => {
  const from = DateTime.utc().minus(Duration.fromObject({days: 2}));
  const to = DateTime.utc();
  const records = await fetchRecords(from, to);

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <DashboardGraph
        className="flex-1 max-h-[45%] w-full"
        dateTimes={records.map(r => r.time)}
        values={records.map(r => r.temperatures.outside)}
        label="Outside °C"
      />
    </Suspense>
  );
};

export default OutsideTemperatureGraph;
