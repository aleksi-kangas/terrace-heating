'use server';
import {DateTime} from 'luxon';
import {revalidateTag} from 'next/cache';
import {URLSearchParams} from 'url';
import {
  CacheTags,
  CompressorRecord,
  HeatingState,
  HeatPumpRecord,
} from '@/app/api/types';

const baseUrl = 'http://host.docker.internal:8000/heating';

export const fetchCompressorRecordsRange = async (
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

export const fetchCompressorRecordsDays = async (
  days: number
): Promise<CompressorRecord[]> => {
  const to = DateTime.utc().set({second: 0, millisecond: 0});
  const from = to.minus({days: days});
  return fetchCompressorRecordsRange(from, to);
};

export const fetchHeatPumpRecordsRange = async (
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

export const fetchHeatPumpRecordsDays = async (
    days: number
): Promise<HeatPumpRecord[]> => {
  const to = DateTime.utc().set({second: 0, millisecond: 0});
  const from = to.minus({days: days});
  console.log(from.toISO(), to.toISO());
  return fetchHeatPumpRecordsRange(from, to);
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
  const response = await fetch(url, {next: {tags: [CacheTags.HeatinState]}});
  if (!response.ok) {
    throw new Error();
  }
  const state = await response.json();
  return parseHeatingState(state);
};

export const startHeating = async (): Promise<HeatingState> => {
  const url = `${baseUrl}/start`;
  const response = await fetch(url, {method: 'POST'});
  if (!response.ok) {
    throw new Error();
  }
  const state = await response.json();
  revalidateTag(CacheTags.HeatinState);
  return parseHeatingState(state);
};

export const stopHeating = async (): Promise<HeatingState> => {
  const url = `${baseUrl}/stop`;
  const response = await fetch(url, {method: 'POST'});
  if (!response.ok) {
    throw new Error();
  }
  const state = await response.json();
  revalidateTag(CacheTags.HeatinState);
  return parseHeatingState(state);
};
