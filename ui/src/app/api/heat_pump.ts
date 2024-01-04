import {BoostingSchedule, BoostingScheduleVariable} from '@/app/api/types';

const baseUrl = 'http://host.docker.internal:8000/heat-pump';

export const fetchBoostingSchedule = async (
  boostingScheduleVariable: BoostingScheduleVariable
): Promise<BoostingSchedule> => {
  let boostingScheduleVariableUrl: string | undefined = undefined;
  switch (boostingScheduleVariable) {
    case BoostingScheduleVariable.Circuit3:
      boostingScheduleVariableUrl = 'circuit3';
      break;
    case BoostingScheduleVariable.LowerTank:
      boostingScheduleVariableUrl = 'lower-tank';
      break;
  }
  const url = `${baseUrl}/schedules/${boostingScheduleVariableUrl}`;
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error();
  }
  return response.json();
};
