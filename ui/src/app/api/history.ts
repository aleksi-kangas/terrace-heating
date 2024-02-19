'use server';
import {DateTime} from 'luxon';
import {revalidateTag} from 'next/cache';
import {URLSearchParams} from 'url';
import {
  CacheTags,
  CompressorRecord,
  HeatingState,
  TankLimitRecord,
  TemperatureRecord,
} from '@/app/api/types';

interface NextRequestInit extends RequestInit {
  next: {
    tags: string[];
  };
}

const baseUrl = 'http://heating-gateway:80/history';

export const fetchCompressorRecordsDateTimeRange = async (
  from: DateTime,
  to: DateTime
): Promise<CompressorRecord[]> => {
  const url =
    `${baseUrl}/compressor?` +
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

export const fetchCompressorRecordsDaysRange = async (
  days: number
): Promise<CompressorRecord[]> => {
  const to = DateTime.utc().set({second: 0, millisecond: 0});
  const from = to.minus({days: days});
  return fetchCompressorRecordsDateTimeRange(from, to);
};

export const fetchTankLimitRecordsDateTimeRange = async (
  from: DateTime,
  to: DateTime
): Promise<TankLimitRecord[]> => {
  const url =
    `${baseUrl}/tank-limits?` +
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

export const fetchTankLimitRecordsDaysRange = async (
  days: number
): Promise<TankLimitRecord[]> => {
  const to = DateTime.utc().set({second: 0, millisecond: 0});
  const from = to.minus({days: days});
  return fetchTankLimitRecordsDateTimeRange(from, to);
};

export const fetchTemperatureRecordsDateTimeRange = async (
  from: DateTime,
  to: DateTime
): Promise<TemperatureRecord[]> => {
  const url =
    `${baseUrl}/temperatures?` +
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

export const fetchTemperatureRecordsDaysRange = async (
  days: number
): Promise<TemperatureRecord[]> => {
  const to = DateTime.utc().set({second: 0, millisecond: 0});
  const from = to.minus({days: days});
  return fetchTemperatureRecordsDateTimeRange(from, to);
};

const parseHeatingState = (state: number): HeatingState => {
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

export const fetchHeatingState = async (): Promise<HeatingState> => {
  const url = `${baseUrl}/state`;
  const options: NextRequestInit = {
    next: {tags: [CacheTags.HeatingState]},
  };
  const response = await fetch(url, options);
  if (!response.ok) {
    throw new Error();
  }
  const state = await response.json();
  return parseHeatingState(state);
};

export const startHeating = async (
  softStart: boolean
): Promise<HeatingState> => {
  const url = `${baseUrl}/start?soft-start=${softStart}`;
  const response = await fetch(url, {method: 'POST'});
  if (!response.ok) {
    throw new Error();
  }
  const state = await response.json();
  revalidateTag(CacheTags.HeatingState);
  return parseHeatingState(state);
};

export const stopHeating = async (): Promise<HeatingState> => {
  const url = `${baseUrl}/stop`;
  const response = await fetch(url, {method: 'POST'});
  if (!response.ok) {
    throw new Error();
  }
  const state = await response.json();
  revalidateTag(CacheTags.HeatingState);
  return parseHeatingState(state);
};
