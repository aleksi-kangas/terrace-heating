import {CompressorRecord, HeatPumpRecord} from '@/app/api/types';
import {DateTime} from 'luxon';
import {URLSearchParams} from 'url';

const baseUrl = 'http://localhost:8000/heating';

export const fetchCompressorRecords = async (
  from: DateTime,
  to: DateTime
): Promise<CompressorRecord[]> => {
  const url =
    `${baseUrl}/records/compressor?` +
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

export const fetchHeatPumpRecords = async (
  from: DateTime,
  to: DateTime
): Promise<HeatPumpRecord[]> => {
  const url =
    `${baseUrl}/records/heat-pump?` +
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
