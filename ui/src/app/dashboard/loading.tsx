import Card from '../components/card';
import Spinner from '../components/spinner';

const DashboardLoading = () => {
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card className="col-span-full md:col-span-2 flex flex-col justify-between items-center">
          <Spinner className="flex-1 max-h-96" />
          <Spinner className="flex-1 max-h-96" />
        </Card>
        <Card className="col-span-full md:col-span-1 flex-col justify-between items-center">
          TODO...
        </Card>
      </div>
    </div>
  );
};

export default DashboardLoading;
