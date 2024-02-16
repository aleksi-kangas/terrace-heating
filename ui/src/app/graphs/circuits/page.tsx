import React, {Suspense} from 'react';
import {DateTime} from 'luxon';
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
        series={[
          {
            color: 'rgb(78, 121, 167)',
            data: records.map(r => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.temperatures.circuit1,
            })),
            label: 'Circuit 1 °C',
          },
          {
            color: 'rgb(242, 142, 43)',
            data: records.map(r => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.temperatures.circuit2,
            })),
            label: 'Circuit 2 °C',
          },
          {
            color: 'rgb(225, 87, 89)',
            data: records.map(r => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.temperatures.circuit3,
            })),
            label: 'Circuit 3 °C',
          },
          {
            color: 'rgb(89, 161, 79)',
            data: records.map(r => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.temperatures.inside,
            })),
            label: 'Inside °C',
          },
        ]}
      />
    </Suspense>
  );
};

export default CircuitGraphsPage;
export const dynamic = 'force-dynamic';
