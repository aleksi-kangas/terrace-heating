import React, {Suspense} from 'react';
import {DateTime} from 'luxon';
import {
  fetchTankLimitRecordsDaysRange,
  fetchTemperatureRecordsDaysRange,
} from '@/app/api/history';
import {TankLimitRecord, TemperatureRecord} from '@/app/api/types';
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
  const temperatureRecords: TemperatureRecord[] =
    await fetchTemperatureRecordsDaysRange(days);
  const tankLimitRecords: TankLimitRecord[] =
    await fetchTankLimitRecordsDaysRange(days);
  return (
    <Suspense fallback={<Spinner className="flex-1 h-full w-full" />}>
      <Graph
        series={[
          {
            color: 'rgb(78, 121, 167)',
            data: temperatureRecords.map((r: TemperatureRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.lowerTank,
            })),
            label: 'Lower Tank °C',
          },
          {
            color: 'rgb(242, 142, 43)',
            data: temperatureRecords.map((r: TemperatureRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.upperTank,
            })),
            label: 'Upper Tank °C',
          },
          {
            color: 'rgb(225, 87, 89)',
            data: temperatureRecords.map((r: TemperatureRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.hotGas,
            })),
            label: 'Hot Gas °C',
          },
          {
            color: 'rgba(78, 121, 167, 0.35)',
            data: tankLimitRecords.map((r: TankLimitRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.lowerTankMinimumAdjusted,
            })),
            pointHitRadius: 0,
            pointRadius: 0,
          },
          {
            color: 'rgba(78, 121, 167, 0.35)',
            data: tankLimitRecords.map((r: TankLimitRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.lowerTankMaximumAdjusted,
            })),
            pointHitRadius: 0,
            pointRadius: 0,
          },
          {
            color: 'rgba(242, 142, 43, 0.35)',
            data: tankLimitRecords.map((r: TankLimitRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.upperTankMinimumAdjusted,
            })),
            pointHitRadius: 0,
            pointRadius: 0,
          },
          {
            color: 'rgba(242, 142, 43, 0.35)',
            data: tankLimitRecords.map((r: TankLimitRecord) => ({
              x: DateTime.fromISO(r.time).toLocal().toMillis(),
              y: r.upperTankMaximumAdjusted,
            })),
            pointHitRadius: 0,
            pointRadius: 0,
          },
        ]}
      />
    </Suspense>
  );
};

export default TankGraphsPage;
export const dynamic = 'force-dynamic';
