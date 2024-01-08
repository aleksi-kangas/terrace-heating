'use client';
import React from 'react';
import {startHeating, stopHeating} from '@/app/api/heating';
import {HeatingState} from '@/app/api/types';

interface HeatingToggleProps {
  heatingState: HeatingState;
}

const HeatingToggle = ({heatingState}: HeatingToggleProps) => {
  const toggleHeating = async () => {
    switch (heatingState) {
      case HeatingState.Inactive:
        {
          await startHeating();
        }
        break;
      case HeatingState.SoftStarting:
      case HeatingState.Active:
      case HeatingState.Boosting:
        {
          await stopHeating();
        }
        break;
      default:
        throw new Error('Unknown heating state');
    }
  };

  const handleCheckboxChange = () => {
    toggleHeating();
  };

  return (
    <label className="themeSwitcherTwo relative inline-flex cursor-pointer select-none items-center">
      <input
        type="checkbox"
        checked={heatingState !== HeatingState.Inactive}
        onChange={handleCheckboxChange}
        className="sr-only"
      />
      <span
        className={`slider mx-4 flex h-8 w-[60px] items-center rounded-full p-1 duration-200 ${
          heatingState !== HeatingState.Inactive
            ? 'bg-[rgb(31,41,55)]'
            : 'bg-[#CCCCCE]'
        }`}
      >
        <span
          className={`dot h-6 w-6 rounded-full bg-white duration-200 ${
            heatingState !== HeatingState.Inactive ? 'translate-x-[28px]' : ''
          }`}
        ></span>
      </span>
    </label>
  );
};

export default HeatingToggle;
