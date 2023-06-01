'use client';

import {
  Chart as ChartJS,
  ChartData,
  ChartOptions,
  registerables,
} from 'chart.js';
import 'chartjs-adapter-luxon';
import {useMemo, useState} from 'react';
import {Line} from 'react-chartjs-2';
import {DateTime, Duration} from 'luxon';
import Spinner from '../spinner';

ChartJS.register(...registerables);

interface DashboardGraphProps {
  className?: string;
  dateTimes: string[];
  values: number[];
  label: string;
}

const DashboardGraph = ({
  className,
  dateTimes,
  values,
  label,
}: DashboardGraphProps): React.JSX.Element => {
  const styles = 'p-8';
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
        },
      ],
    }),
    [dateTimes, values, label]
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
          min: DateTime.now()
            .minus(Duration.fromObject({days: 2}))
            .toMillis(),
          max: DateTime.now().toMillis(),
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
    <div className={className}>
      <Line data={data} options={options} />
      {isLoading && <Spinner className="static" />}
    </div>
  );
};
export default DashboardGraph;
