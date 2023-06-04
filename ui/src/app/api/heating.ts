import {HeatPumpRecord} from '@/app/api/types';
import {DateTime} from 'luxon';
import {URLSearchParams} from 'url';

const baseUrl = 'http://localhost:8000/heating';

export const fetchRecords = async (
  from: DateTime,
  to: DateTime
): Promise<HeatPumpRecord[]> => {
  const url =
    `${baseUrl}/records?` +
    new URLSearchParams({
      from: from.toISO()!,
      to: to.toISO()!,
    });
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error();
  }
  return response.json();
};
