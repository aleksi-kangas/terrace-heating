'use server';
import {revalidateTag} from 'next/cache';
import {CacheTags, HeatingState} from '@/app/api/types';

interface NextRequestInit extends RequestInit {
  next: {
    tags: string[];
  };
}

const baseUrl = 'http://heating-gateway:80/heating';

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
