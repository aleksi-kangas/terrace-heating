import React from 'react';
import {fetchBoostingSchedule} from '@/app/api/heat_pump';
import {BoostingScheduleVariable} from '@/app/api/types';
import Card from '@/app/components/card';
import BoostingScheduleTable from '@/app/components/schedules/boosting_schedule_table';

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
            boostingScheduleVariable={BoostingScheduleVariable.Circuit3}
            boostingSchedule={circuit3BoostingSchedule}
          />
        </Card>
        <Card className="col-span-full md:col-span-1 flex flex-col justify-center items-center">
          <BoostingScheduleTable
            boostingScheduleVariable={BoostingScheduleVariable.LowerTank}
            boostingSchedule={lowerTankBoostingSchedule}
          />
        </Card>
      </div>
    </div>
  );
};

export default SchedulesPage;
export const dynamic = 'force-dynamic';
