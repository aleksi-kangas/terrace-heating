import Card from '../components/card';
import DashboardGraph from '../components/dashboard/dashboard_graph';
import {fetchRecords} from '../api/heating';
import {DateTime, Duration} from 'luxon';

const Dashboard = async () => {
  const from = DateTime.utc().minus(Duration.fromObject({days: 2}));
  const to = DateTime.utc();
  const records = await fetchRecords(from, to);
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card className="flex-1 col-span-full md:col-span-2 flex flex-col justify-between items-center">
          <DashboardGraph
            className="max-h-72"
            dateTimes={records.map(r => r.time)}
            values={records.map(r => r.temperatures.outside)}
            label="Outside °C"
          />
          <DashboardGraph
            className="max-h-72"
            dateTimes={records.map(r => r.time)}
            values={records.map(r => r.temperatures.circuit3)}
            label="Circuit 3"
          />
        </Card>
        <Card className="flex-1 col-span-full md:col-span-1 flex-col justify-between items-center">
          TODO...
        </Card>
      </div>
    </div>
  );
};

export default Dashboard;
