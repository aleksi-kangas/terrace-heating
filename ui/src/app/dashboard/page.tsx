import React from 'react';
import Card from '@/app/components/card';
import HorizontalDivider from '@/app/components/horizontal_divider';
import CompressorUsageGauge from '@/app/components/dashboard/compressor_usage_gauge';
import CompressorUsageGraph from '@/app/components/dashboard/compressor_usage_graph';
import HeatingControl from '@/app/components/dashboard/heating_control';
import OutsideTemperatureGraph from '@/app/components/dashboard/outside_temperature_graph';

const Dashboard = () => {
  return (
    <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
      <Card className="col-span-full md:col-span-2 flex flex-col justify-evenly items-center">
        <OutsideTemperatureGraph />
        <HorizontalDivider />
        <CompressorUsageGraph />
      </Card>
      <Card className="col-span-full md:col-span-1 flex flex-col justify-evenly items-center">
        <HeatingControl />
        <HorizontalDivider />
        <CompressorUsageGauge />
      </Card>
    </div>
  );
};

export default Dashboard;
export const dynamic = 'force-dynamic';
