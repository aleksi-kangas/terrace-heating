import React, {Suspense} from 'react';
import {fetchHeatPumpRecordsDays} from '@/app/api/heating';
import Spinner from '@/app/components/spinner';
import Graph from '@/app/graphs/graph';

interface SearchParams {
  days?: number;
}

interface ExternalGraphsPageProps {
  searchParams: SearchParams;
}

const ExternalGraphsPage = async ({searchParams}: ExternalGraphsPageProps) => {
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
            label: 'Outside °C',
            values: records.map(r => r.temperatures.outside),
          },
          {
            color: 'rgb(242, 142, 43)',
            label: 'Ground-Loop Input °C',
            values: records.map(r => r.temperatures.groundInput),
          },
          {
            color: 'rgb(225, 87, 89)',
            label: 'Ground-Loop Output °C',
            values: records.map(r => r.temperatures.groundOutput),
          },
        ]}
      />
    </Suspense>
  );
};

export default ExternalGraphsPage;
export const dynamic = 'force-dynamic';
