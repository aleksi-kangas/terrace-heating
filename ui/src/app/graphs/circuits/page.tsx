import React, {Suspense} from 'react';
import {fetchHeatPumpRecordsDays} from '@/app/api/heating';
import Spinner from '@/app/components/spinner';
import Graph from '@/app/graphs/graph';

interface SearchParams {
  days?: number;
}

interface CircuitGraphsPageProps {
  searchParams: SearchParams;
}

const CircuitGraphsPage = async ({
  searchParams,
}: CircuitGraphsPageProps): Promise<JSX.Element> => {
  const days = searchParams.days ?? 2;
  if (days < 1 || days > 365) throw new Error('Invalid days');
  const records = await fetchHeatPumpRecordsDays(days);
  return (
    <Suspense fallback={<Spinner className="flex-1 h-full w-full" />}>
      <Graph
        dateTimes={records.map(r => r.time)}
        series={[
          {
            color: 'rgb(78, 121, 167)',
            label: 'Circuit 1 °C',
            values: records.map(r => r.temperatures.circuit1),
          },
          {
            color: 'rgb(242, 142, 43)',
            label: 'Circuit 2 °C',
            values: records.map(r => r.temperatures.circuit2),
          },
          {
            color: 'rgb(225, 87, 89)',
            label: 'Circuit 3 °C',
            values: records.map(r => r.temperatures.circuit3),
          },
          {
            color: 'rgb(89, 161, 79)',
            label: 'Inside °C',
            values: records.map(r => r.temperatures.inside),
          },
        ]}
      />
    </Suspense>
  );
};

export default CircuitGraphsPage;
export const dynamic = 'force-dynamic';
