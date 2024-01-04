import Card from '../components/card';
import CompressorUsageGraph from '../components/dashboard/compressor_usage_graph';
import OutsideTemperatureGraph from '../components/dashboard/outside_temperature_graph';
import HorizontalDivider from '../components/horizontal_divider';
import React from 'react';
import CompressorUsageGauge from '../components/dashboard/compressor_usage_gauge';
import HeatingControl from '../components/dashboard/heating_control';

const Dashboard = () => {
  return (
    <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
      <Card className="col-span-full md:col-span-2 flex flex-col justify-evenly items-center">
        {/* @ts-expect-error Async Server Component */}
        <OutsideTemperatureGraph />
        <HorizontalDivider />
        {/* @ts-expect-error Async Server Component */}
        <CompressorUsageGraph />
      </Card>
      <Card className="col-span-full md:col-span-1 flex flex-col justify-evenly items-center">
        {/* @ts-expect-error Async Server Component */}
        <HeatingControl />
        <HorizontalDivider />
        {/* @ts-expect-error Async Server Component */}
        <CompressorUsageGauge />
      </Card>
    </div>
  );
};

export default Dashboard;
export const dynamic = 'force-dynamic';
