import Card from "../components/card";

const Dashboard = () => {
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card className="flex-1 col-span-full md:col-span-2">
          TESTING
        </Card>
        <Card className="flex-1 col-span-full md:col-span-1">
          TESTING
        </Card>
      </div>
    </div>
  )
};

export default Dashboard;
