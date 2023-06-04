import Card from '../components/card';
import BoostingScheduleTable from '../components/schedules/boosting_schedule_table';
import {BoostingScheduleVariable} from '../api/types';
import {fetchBoostingSchedule} from '../api/heat_pump';

const SchedulesPage = async () => {
  const circuit3BoostingSchedule = await fetchBoostingSchedule(
    BoostingScheduleVariable.Circuit3
  );
  const lowerTankBoostingSchedule = await fetchBoostingSchedule(
    BoostingScheduleVariable.LowerTank
  );
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-4">
        <Card className="col-span-full md:col-span-1 flex flex-col justify-center items-center">
          <BoostingScheduleTable
            boostingSchedule={circuit3BoostingSchedule}
            title="Circuit 3"
          />
        </Card>
        <Card className="col-span-full md:col-span-1 flex flex-col justify-center items-center">
          <BoostingScheduleTable
            boostingSchedule={lowerTankBoostingSchedule}
            title="Lower Tank"
          />
        </Card>
      </div>
    </div>
  );
};

export default SchedulesPage;
