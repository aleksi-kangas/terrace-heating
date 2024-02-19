import React, {Suspense} from 'react';
import {DateTime} from 'luxon';
import {fetchTemperatureRecordsDaysRange} from '@/app/api/history';
import {TemperatureRecord} from '@/app/api/types';
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
  const records: TemperatureRecord[] =
    await fetchTemperatureRecordsDaysRange(days);
  return (
    <Suspense fallback={<Spinner className="flex-1 h-full w-full" />}>
      <Graph
        series={[
          {
            color: 'rgb(78, 121, 167)',
            data: records.map((r: TemperatureRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.outside,
            })),
            label: 'Outside °C',
          },
          {
            color: 'rgb(242, 142, 43)',
            data: records.map((r: TemperatureRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.groundInput,
            })),
            label: 'Ground-Loop Input °C',
          },
          {
            color: 'rgb(225, 87, 89)',
            data: records.map((r: TemperatureRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.groundOutput,
            })),
            label: 'Ground-Loop Output °C',
          },
        ]}
      />
    </Suspense>
  );
};

export default ExternalGraphsPage;
export const dynamic = 'force-dynamic';
