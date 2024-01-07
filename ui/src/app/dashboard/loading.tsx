import React from 'react';
import Card from '@/app/components/card';
import Spinner from '@/app/components/spinner';
import HorizontalDivider from '@/app/components/horizontal_divider';

const DashboardLoading = () => {
  return (
    <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
      <Card className="col-span-full md:col-span-2 flex flex-col justify-evenly items-center">
        <Spinner className="flex-1 max-h-96" />
        <HorizontalDivider />
        <Spinner className="flex-1 max-h-96" />
      </Card>
      <Card className="col-span-full md:col-span-1 flex-col justify-evenly items-center">
        TODO...
      </Card>
    </div>
  );
};

export default DashboardLoading;
