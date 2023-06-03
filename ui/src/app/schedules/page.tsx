import Card from '../components/card';
import BoostingScheduleTable from '../components/schedules/boosting_schedule_table';
import {BoostingSchedule} from '../api/types';

// TODO Temporary
const temporaryBoostingSchedule: BoostingSchedule = {
  monday: {
    startHour: 0,
    endHour: 0,
    temperatureDelta: 0,
  },
  tuesday: {
    startHour: 0,
    endHour: 0,
    temperatureDelta: 0,
  },
  wednesday: {
    startHour: 0,
    endHour: 0,
    temperatureDelta: 0,
  },
  thursday: {
    startHour: 0,
    endHour: 0,
    temperatureDelta: 0,
  },
  friday: {
    startHour: 0,
    endHour: 0,
    temperatureDelta: 0,
  },
  saturday: {
    startHour: 0,
    endHour: 0,
    temperatureDelta: 0,
  },
  sunday: {
    startHour: 0,
    endHour: 0,
    temperatureDelta: 0,
  },
};

const SchedulesPage = () => {
  return (
    <div className="flex flex-1 max-w-7xl">
      <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-4">
        <Card className="col-span-full md:col-span-1 flex flex-col justify-center items-center">
          <BoostingScheduleTable
            boostingSchedule={temporaryBoostingSchedule}
            title="Circuit 3"
          />
        </Card>
        <Card className="col-span-full md:col-span-1 flex flex-col justify-center items-center">
          <BoostingScheduleTable
            boostingSchedule={temporaryBoostingSchedule}
            title="Lower Tank"
          />
        </Card>
      </div>
    </div>
  );
};

export default SchedulesPage;
