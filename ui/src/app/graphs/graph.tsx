'use client';
import React, {useMemo, useState} from 'react';
import {
  Chart as ChartJS,
  ChartData,
  ChartOptions,
  Point,
  registerables,
} from 'chart.js';
import 'chartjs-adapter-luxon';
import {Line} from 'react-chartjs-2';
import Spinner from '@/app/components/spinner';

ChartJS.register(...registerables);

interface DashboardGraphProps {
  series: {
    color: string;
    data: Point[];
    label: string;
  }[];
}

const Graph = ({series}: DashboardGraphProps): React.JSX.Element => {
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const chartData: ChartData<'line'> = useMemo(
    () => ({
      datasets: series.map(s => ({
        animation: false,
        backgroundColor: s.color,
        borderColor: s.color,
        data: s.data,
        label: s.label,
        pointHitRadius: 5,
        pointRadius: 1,
        showLine: true,
        spanGaps: false,
      })),
    }),
    [series]
  );
  const chartOptions: ChartOptions<'line'> = useMemo(
    () => ({
      animation: {
        onComplete: () => {
          setIsLoading(false);
        },
      },
      maintainAspectRatio: false,
      parsing: false,
      plugins: {
        decimation: {
          enabled: true,
          algorithm: 'lttb',
        },
        legend: {
          position: 'top',
        },
        tooltip: {
          animation: false,
        },
      },
      responsive: true,
      scales: {
        x: {
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
      <Line data={chartData} options={chartOptions} />
    </>
  );
};
export default Graph;
