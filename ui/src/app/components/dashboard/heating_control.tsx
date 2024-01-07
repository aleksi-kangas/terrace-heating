import React, {Suspense} from 'react';
import {fetchHeatingState} from '@/app/api/heating';
import {HeatingState} from '@/app/api/types';
import Spinner from '@/app/components/spinner';
import Toggle from '@/app/components/toggle';

const HeatingControl = async () => {
  const heatingState = await fetchHeatingState();
  let statusText = '';
  let extraStatusText = '';
  switch (heatingState) {
    case HeatingState.Inactive:
      {
        statusText = 'Heating Inactive';
        extraStatusText = '';
      }
      break;
    case HeatingState.SoftStarting:
      {
        statusText = 'Heating Active';
        extraStatusText = 'Soft-Starting';
      }
      break;
    case HeatingState.Active:
      {
        statusText = 'Heating Active';
        extraStatusText = '';
      }
      break;
    case HeatingState.Boosting:
      {
        statusText = 'Heating Active';
        extraStatusText = 'Boosting';
      }
      break;
    default:
      throw new Error('Unknown heating state');
  }

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <div className="flex-1 max-h-[45%] w-full p-8 flex flex-col items-center justify-center">
        <div className="text-lg font-semibold p-4 pb-1">{statusText}</div>
        <div className="text-sm font-semibold p-4 pt-1">{extraStatusText}</div>
        <Toggle />
        {heatingState === HeatingState.SoftStarting && (
          <button
            type="button"
            className="m-4 p-2 inline-flex items-center gap-x-2 text-sm font-medium rounded-xl border border-transparent bg-[rgb(31,41,55)] text-white hover:bg-[rgb(110,160,176)] hover:rounded-md"
          >
            Skip Soft-Start
          </button>
        )}
      </div>
    </Suspense>
  );
};

export default HeatingControl;
