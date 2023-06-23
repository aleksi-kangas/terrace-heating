'use client'

import Card from '../components/card';
import ErrorAlert from "../components/error_alert";
import HorizontalDivider from "../components/horizontal_divider";

const DashboardError = () => {
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card className="col-span-full md:col-span-2 flex flex-col justify-evenly items-center">
          <ErrorAlert message="Server connection failure" className="" />
          <HorizontalDivider />
          <ErrorAlert message="Server connection failure" className="" />
        </Card>
        <Card className="col-span-full md:col-span-1 flex-col justify-between items-center">
          TODO...
        </Card>
      </div>
    </div>
  );
};

export default DashboardError;
