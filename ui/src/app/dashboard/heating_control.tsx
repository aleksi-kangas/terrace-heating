import React, {Suspense} from 'react';
import {fetchCurrentHeatPumpTemperatures} from '@/app/api/heat_pump';
import {fetchHeatingState} from '@/app/api/heating';
import {HeatingState} from '@/app/api/types';
import Spinner from '@/app/components/spinner';
import HeatingToggle from '@/app/dashboard/heating_toggle';

const HeatingControl = async () => {
  const temperatures = await fetchCurrentHeatPumpTemperatures();
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

  const outsideTemperatureColor = (): string => {
    if (temperatures.outside < 0.0) return 'text-red-500';
    if (temperatures.outside < 10.0) return 'text-yellow-500';
    return 'text-green-500';
  };

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <div className="flex-1 max-h-[45%] w-full p-8 flex flex-col items-center justify-center">
        <div className="text-lg font-semibold p-4 pb-1">{statusText}</div>
        <div className="text-sm font-semibold p-4 pt-1">{extraStatusText}</div>
        <HeatingToggle
          heatingState={heatingState}
          outsideTemperature={temperatures.outside}
        />
        <div className="text-md font-semibold p-6">
          Outside |{' '}
          <span
            className={outsideTemperatureColor()}
          >{`${temperatures.outside} °C`}</span>
        </div>
      </div>
    </Suspense>
  );
};

export default HeatingControl;
