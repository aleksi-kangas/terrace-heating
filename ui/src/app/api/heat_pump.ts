'use server';
import {
  BoostingSchedule,
  BoostingScheduleVariable,
  CacheTags,
  Temperatures,
} from '@/app/api/types';
import {revalidateTag} from 'next/cache';

interface NextRequestInit extends RequestInit {
  next: {
    tags: string[];
  };
}

const baseUrl = 'http://host.docker.internal:8000/heat-pump';

export const fetchBoostingSchedule = async (
  boostingScheduleVariable: BoostingScheduleVariable
): Promise<BoostingSchedule> => {
  let boostingScheduleVariableUrl: string | undefined = undefined;
  switch (boostingScheduleVariable) {
    case BoostingScheduleVariable.Circuit3:
      {
        boostingScheduleVariableUrl = 'circuit3';
      }
      break;
    case BoostingScheduleVariable.LowerTank:
      {
        boostingScheduleVariableUrl = 'lower-tank';
      }
      break;
    default:
      throw new Error('Unknown boosting schedule variable');
  }
  const url = `${baseUrl}/schedules/${boostingScheduleVariableUrl}`;
  const options: NextRequestInit = {
    next: {tags: [CacheTags.BoostingSchedules]},
  };
  const response = await fetch(url, options);
  if (!response.ok) {
    throw new Error();
  }
  return response.json();
};

export const updateBoostingSchedule = async (
  boostingScheduleVariable: BoostingScheduleVariable,
  boostingSchedule: BoostingSchedule
): Promise<void> => {
  let boostingScheduleVariableUrl: string | undefined = undefined;
  switch (boostingScheduleVariable) {
    case BoostingScheduleVariable.Circuit3:
      {
        boostingScheduleVariableUrl = 'circuit3';
      }
      break;
    case BoostingScheduleVariable.LowerTank:
      {
        boostingScheduleVariableUrl = 'lower-tank';
      }
      break;
    default:
      throw new Error('Unknown boosting schedule variable');
  }
  const url = `${baseUrl}/schedules/${boostingScheduleVariableUrl}`;
  const response = await fetch(url, {
    method: 'PUT',
    headers: {
      'Content-type': 'application/json',
    },
    body: JSON.stringify(boostingSchedule),
  });
  if (!response.ok) {
    throw new Error();
  }
  revalidateTag(CacheTags.BoostingSchedules);
};

export const fetchCurrentHeatPumpTemperatures =
  async (): Promise<Temperatures> => {
    const url = `${baseUrl}/temperatures`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error();
    }
    return response.json();
  };
