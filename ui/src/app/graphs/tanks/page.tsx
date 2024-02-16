import React, {Suspense} from 'react';
import {DateTime} from 'luxon';
import {fetchHeatPumpRecordsDays} from '@/app/api/heating';
import Spinner from '@/app/components/spinner';
import Graph from '@/app/graphs/graph';

interface SearchParams {
  days?: number;
}

interface TankGraphsPageProps {
  searchParams: SearchParams;
}

const TankGraphsPage = async ({searchParams}: TankGraphsPageProps) => {
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
              y: r.temperatures.lowerTank,
            })),
            label: 'Lower Tank °C',
          },
          {
            color: 'rgb(242, 142, 43)',
            data: records.map(r => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.temperatures.upperTank,
            })),
            label: 'Upper Tank °C',
          },
          {
            color: 'rgb(225, 87, 89)',
            data: records.map(r => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.temperatures.hotGas,
            })),
            label: 'Hot Gas °C',
          },
        ]}
      />
    </Suspense>
  );
};

export default TankGraphsPage;
export const dynamic = 'force-dynamic';
