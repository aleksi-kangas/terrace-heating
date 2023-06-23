import Card from '../components/card';
import OutsideTemperatureGraph from '../components/dashboard/outside_temperature_graph';
import HorizontalDivider from "../components/horizontal_divider";

const Dashboard = () => {
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card className="col-span-full md:col-span-2 flex flex-col justify-between items-center">
          {/* @ts-expect-error Async Server Component */}
          <OutsideTemperatureGraph />
          <HorizontalDivider />
          {/* @ts-expect-error Async Server Component */}
          <OutsideTemperatureGraph />
        </Card>
        <Card className="col-span-full md:col-span-1 flex-col justify-between items-center">
          TODO...
        </Card>
      </div>
    </div>
  );
};

export default Dashboard;
