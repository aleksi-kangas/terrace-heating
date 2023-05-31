'use client';

import {Chart as ChartJS, registerables} from "chart.js";
import {useMemo} from "react";
import {Line} from "react-chartjs-2";

ChartJS.register(...registerables);

interface DashboardGraphProps {
  className?: string;
  x: Date[];
  y: number[];
  yLabel: string;
}

const DashboardGraph = ({className, x, y, yLabel}: DashboardGraphProps): React.JSX.Element => {
  const styles = 'm-2';
  className = className ? styles.concat(' ', className) : styles;
  const data = useMemo(() => ({
    labels: x,
    datasets: [
      {
        label: yLabel,
        data: y,
      },
    ],
  }), [x, y, yLabel]);
  const options = useMemo(() => ({}), []);
  return <Line className={className} data={data} options={options}/>;
}

export default DashboardGraph;
