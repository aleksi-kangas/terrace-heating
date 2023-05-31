import Card from "../components/card";
import DashboardGraph from "../components/dashboard/dashboard_graph";
import {fetchRecords} from "../api/heating";

const Dashboard = async () => {
  const to = new Date();
  const from = new Date(to.getFullYear(), to.getMonth(), to.getDate() - 7);
  const records = await fetchRecords(from, to);
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card className="flex-1 col-span-full md:col-span-2 flex flex-col justify-between items-center">
          <DashboardGraph className="max-h-72" x={records.map(r => r.time)} y={records.map(r => r.temperatures.outside)} yLabel="Outside"/>
          <DashboardGraph className="max-h-72" x={records.map(r => r.time)} y={records.map(r => r.temperatures.circuit3)} yLabel="Circuit 3"/>
        </Card>
        <Card className="flex-1 col-span-full md:col-span-1 flex-col justify-between items-center">
          <DashboardGraph className="max-h-72" x={records.map(r => r.time)} y={records.map(r => r.temperatures.circuit3)} yLabel="Circuit 1"/>
          <DashboardGraph className="max-h-72" x={records.map(r => r.time)} y={records.map(r => r.temperatures.circuit3)} yLabel="Circuit 2"/>
        </Card>
      </div>
    </div>
  )
};

export default Dashboard;
