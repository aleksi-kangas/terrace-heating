import {HeatPumpRecord} from '@/app/api/types';

const baseUrl = 'http://192.168.221.148:8000/heating';

export const fetchRecords = async (
  from: Date,
  to: Date
): Promise<HeatPumpRecord[]> => {
  const url =
    `${baseUrl}/records?` +
    new URLSearchParams({
      from: from.toISOString(),
      to: to.toISOString(),
    });
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error();
  }
  return response.json();
};
