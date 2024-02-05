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

interface GraphProps {
  className?: string;
  dateTimes: string[];
  values: number[];
  label: string;
  xLimits?: string[];
  yLimits?: number[];
  fill?: {
    target: string;
    above: string;
  };
  stepped?: 'before' | 'after' | 'middle' | boolean;
}

const Graph = ({
  className,
  dateTimes,
  values,
  label,
  xLimits,
  yLimits,
  fill,
  stepped,
}: GraphProps): React.JSX.Element => {
  const styles = 'p-2 md:p-8 flex justify-center items-center';
  className = className ? styles.concat(' ', className) : styles;

  const [isLoading, setIsLoading] = useState<boolean>(true);

  const data: ChartData<'line'> = useMemo(
    () => ({
      labels: dateTimes.map(dt => DateTime.fromISO(dt).toLocal()),
      datasets: [
        {
          backgroundColor: 'rgb(110, 160, 176)',
          borderColor: 'rgb(110, 160, 176)',
          data: values,
          label: label,
          pointHitRadius: 5,
          pointRadius: 2,
          showLine: true,
          spanGaps: false,
          fill: fill ? fill : false,
          stepped: stepped ? stepped : false,
        },
      ],
    }),
    [dateTimes, values, label, fill, stepped]
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
          // Disable legend interactivity
          onClick: () => {},
          position: 'bottom',
        },
      },
      responsive: true,
      scales: {
        x: {
          ...(xLimits && {
            min: DateTime.fromISO(xLimits[0]).toMillis(),
            max: DateTime.fromISO(xLimits[1]).toMillis(),
          }),
          ticks: {
            major: {enabled: true},
          },
          time: {
            displayFormats: {day: 'EEE dd', hour: 'HH:mm'},
            tooltipFormat: 'yyyy-MM-dd EEE HH:mm',
          },
          type: 'time',
        },
        y: {
          ...(yLimits && {
            min: yLimits[0],
            max: yLimits[1],
          }),
          grace: '2%',
          type: 'linear',
        },
      },
    }),
    [xLimits, yLimits]
  );

  return (
    <div className={className}>
      {isLoading && <Spinner className="absolute" />}
      <Line data={data} options={options} />
    </div>
  );
};
export default Graph;
