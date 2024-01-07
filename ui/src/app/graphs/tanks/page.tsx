import React, {Suspense} from 'react';
import {fetchHeatPumpRecordsDays} from '@/app/api/heating';
import Spinner from '@/app/components/spinner';
import Graph from '@/app/components/graphs/graph';

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
        dateTimes={records.map(r => r.time)}
        series={[
          {
            color: 'rgb(78, 121, 167)',
            label: 'Lower Tank °C',
            values: records.map(r => r.temperatures.lowerTank),
          },
          {
            color: 'rgb(242, 142, 43)',
            label: 'Upper Tank °C',
            values: records.map(r => r.temperatures.upperTank),
          },
          {
            color: 'rgb(225, 87, 89)',
            label: 'Hot Gas °C',
            values: records.map(r => r.temperatures.hotGas),
          },
        ]}
      />
    </Suspense>
  );
};

export default TankGraphsPage;
export const dynamic = 'force-dynamic';
