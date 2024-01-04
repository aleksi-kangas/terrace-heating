import {CompressorRecord, HeatingState, HeatPumpRecord} from '@/app/api/types';
import {DateTime} from 'luxon';
import {URLSearchParams} from 'url';

const baseUrl = 'http://host.docker.internal:8000/heating';

export const fetchCompressorRecords = async (
  from: DateTime,
  to: DateTime
): Promise<CompressorRecord[]> => {
  const url =
    `${baseUrl}/history/compressor?` +
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
    `${baseUrl}/history?` +
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

export const fetchHeatingState = async (): Promise<HeatingState> => {
  const url = `${baseUrl}/state`;
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error();
  }
  const state = await response.json();
  switch (state) {
    case HeatingState.Inactive:
      return HeatingState.Inactive;
    case HeatingState.SoftStarting:
      return HeatingState.SoftStarting;
    case HeatingState.Active:
      return HeatingState.Active;
    case HeatingState.Boosting:
      return HeatingState.Boosting;
    default:
      throw new Error('Unknown heating state');
  }
};
