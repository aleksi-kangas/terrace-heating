'use client';
import React, {useMemo, useState} from 'react';
import {
  Chart as ChartJS,
  ChartData,
  ChartOptions,
  registerables,
} from 'chart.js';
import 'chartjs-adapter-luxon';
import {Line} from 'react-chartjs-2';
import {DateTime} from 'luxon';
import Spinner from '@/app/components/spinner';

ChartJS.register(...registerables);

interface DashboardGraphProps {
  dateTimes: string[];
  series: {
    color: string;
    label: string;
    values: number[];
  }[];
}

const Graph = ({dateTimes, series}: DashboardGraphProps): React.JSX.Element => {
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const data: ChartData<'line'> = useMemo(
    () => ({
      labels: dateTimes.map(dt => DateTime.fromISO(dt).toLocal()),
      datasets: series.map(s => ({
        backgroundColor: s.color,
        borderColor: s.color,
        data: s.values,
        label: s.label,
        pointHitRadius: 5,
        pointRadius: 2,
        showLine: true,
        spanGaps: false,
      })),
    }),
    [dateTimes, series]
  );
  const options: ChartOptions<'line'> = useMemo(
    () => ({
      animation: {
        onProgress: context => {
          if (context.initial) {
            setIsLoading(false);
          }
        },
      },
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'top',
        },
      },
      responsive: true,
      scales: {
        x: {
          // min: DateTime.now()
          //   .minus(Duration.fromObject({days: 2}))
          //   .toMillis(),
          // max: DateTime.now().toMillis(),
          ticks: {
            major: {enabled: true},
          },
          time: {
            displayFormats: {day: 'EEE dd', hour: 'HH:mm'},
            tooltipFormat: 'yyyy-MM-dd EEE HH:mm',
          },
          type: 'time',
        },
      },
    }),
    []
  );
  return (
    <>
      {isLoading && <Spinner className="absolute" />}
      <Line data={data} options={options} />
    </>
  );
};
export default Graph;
