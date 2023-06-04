import {BoostingSchedule, BoostingScheduleVariable} from '@/app/api/types';

const baseUrl = 'http://localhost:8000/heat_pump';

export const fetchBoostingSchedule = async (
  boostingScheduleVariable: BoostingScheduleVariable
): Promise<BoostingSchedule> => {
  const url = `${baseUrl}/schedules/` + boostingScheduleVariable;
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error();
  }
  return response.json();
};
