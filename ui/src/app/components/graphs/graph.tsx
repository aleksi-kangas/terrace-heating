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
import Card from '@/app/components/card';
import Spinner from '@/app/components/spinner';

ChartJS.register(...registerables);

interface DashboardGraphProps {
  className?: string;
  dateTimes: string[];
  series: {
    color: string;
    label: string;
    values: number[];
  }[];
}

const Graph = ({
  className,
  dateTimes,
  series,
}: DashboardGraphProps): React.JSX.Element => {
  const styles = 'p-8 flex justify-center items-center';
  className = className ? styles.concat(' ', className) : styles;

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
    <Card className="flex-1 h-full w-full p-8 flex justify-evenly items-center bg-amber-100">
      {isLoading && <Spinner className="absolute" />}
      <Line data={data} options={options} />
    </Card>
  );
};
export default Graph;
