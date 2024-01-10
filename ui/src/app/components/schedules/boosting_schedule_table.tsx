'use client';
import React, {FormEvent, useMemo, useState} from 'react';
import {BoostingSchedule, BoostingScheduleVariable} from '@/app/api/types';
import {updateBoostingSchedule} from '../../api/heat_pump';
import Spinner from "../spinner";

interface FormElements extends HTMLFormControlsCollection {
  mondayStartHour: HTMLInputElement;
  mondayEndHour: HTMLInputElement;
  mondayTemperatureDelta: HTMLInputElement;
  tuesdayStartHour: HTMLInputElement;
  tuesdayEndHour: HTMLInputElement;
  tuesdayTemperatureDelta: HTMLInputElement;
  wednesdayStartHour: HTMLInputElement;
  wednesdayEndHour: HTMLInputElement;
  wednesdayTemperatureDelta: HTMLInputElement;
  thursdayStartHour: HTMLInputElement;
  thursdayEndHour: HTMLInputElement;
  thursdayTemperatureDelta: HTMLInputElement;
  fridayStartHour: HTMLInputElement;
  fridayEndHour: HTMLInputElement;
  fridayTemperatureDelta: HTMLInputElement;
  saturdayStartHour: HTMLInputElement;
  saturdayEndHour: HTMLInputElement;
  saturdayTemperatureDelta: HTMLInputElement;
  sundayStartHour: HTMLInputElement;
  sundayEndHour: HTMLInputElement;
  sundayTemperatureDelta: HTMLInputElement;
}

interface FormElement extends HTMLFormElement {
  readonly elements: FormElements;
}

enum Weekday {
  Monday = 'Monday',
  Tuesday = 'Tuesday',
  Wednesday = 'Wednesday',
  Thursday = 'Thursday',
  Friday = 'Friday',
  Saturday = 'Saturday',
  Sunday = 'Sunday',
}

const getWeekday = (boostingSchedule: BoostingSchedule, weekday: Weekday) => {
  switch (weekday) {
    case Weekday.Monday:
      return boostingSchedule.monday;
    case Weekday.Tuesday:
      return boostingSchedule.tuesday;
    case Weekday.Wednesday:
      return boostingSchedule.wednesday;
    case Weekday.Thursday:
      return boostingSchedule.thursday;
    case Weekday.Friday:
      return boostingSchedule.friday;
    case Weekday.Saturday:
      return boostingSchedule.saturday;
    case Weekday.Sunday:
      return boostingSchedule.sunday;
  }
};

interface BoostingScheduleTableProps {
  boostingScheduleVariable: BoostingScheduleVariable;
  boostingSchedule: BoostingSchedule;
}

const BoostingScheduleTable = ({
  boostingScheduleVariable,
  boostingSchedule,
}: BoostingScheduleTableProps) => {
  const [editing, setEditing] = useState<boolean>(false);
  const [updating, setUpdating] = useState<boolean>(false);

  const title = useMemo(() => {
    switch (boostingScheduleVariable) {
      case BoostingScheduleVariable.Circuit3:
        return 'Circuit 3';
      case BoostingScheduleVariable.LowerTank:
        return 'Lower Tank';
      default:
        throw new Error('Unknown boosting schedule variable');
    }
  }, [boostingScheduleVariable]);

  const inputStyles = () => {
    const common =
      'border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5';
    const inactive = ' bg-gray-100';
    const active = ' bg-white';
    return common.concat(editing ? active : inactive);
  };

  const handleSave = async (event: FormEvent<FormElement>) => {
    event.preventDefault();
    const form = event.currentTarget;
    const newSchedule: BoostingSchedule = {
      monday: {
        startHour: parseInt(form.elements.mondayStartHour.value),
        endHour: parseInt(form.elements.mondayEndHour.value),
        temperatureDelta: parseInt(form.elements.mondayTemperatureDelta.value),
      },
      tuesday: {
        startHour: parseInt(form.elements.tuesdayStartHour.value),
        endHour: parseInt(form.elements.tuesdayEndHour.value),
        temperatureDelta: parseInt(form.elements.tuesdayTemperatureDelta.value),
      },
      wednesday: {
        startHour: parseInt(form.elements.wednesdayStartHour.value),
        endHour: parseInt(form.elements.wednesdayEndHour.value),
        temperatureDelta: parseInt(
          form.elements.wednesdayTemperatureDelta.value
        ),
      },
      thursday: {
        startHour: parseInt(form.elements.thursdayStartHour.value),
        endHour: parseInt(form.elements.thursdayEndHour.value),
        temperatureDelta: parseInt(
          form.elements.thursdayTemperatureDelta.value
        ),
      },
      friday: {
        startHour: parseInt(form.elements.fridayStartHour.value),
        endHour: parseInt(form.elements.fridayEndHour.value),
        temperatureDelta: parseInt(form.elements.fridayTemperatureDelta.value),
      },
      saturday: {
        startHour: parseInt(form.elements.saturdayStartHour.value),
        endHour: parseInt(form.elements.saturdayEndHour.value),
        temperatureDelta: parseInt(
          form.elements.saturdayTemperatureDelta.value
        ),
      },
      sunday: {
        startHour: parseInt(form.elements.sundayStartHour.value),
        endHour: parseInt(form.elements.sundayEndHour.value),
        temperatureDelta: parseInt(form.elements.sundayTemperatureDelta.value),
      },
    };
    setUpdating(true);
    await updateBoostingSchedule(boostingScheduleVariable, newSchedule);
    setUpdating(false);
    setEditing(false);
  };

  return (
    <form
      onSubmit={handleSave}
      className="flex-1 w-full h-full flex flex-col p-6 gap-2"
    >
      <h1 className="font-light text-2xl text-center">{title}</h1>
      <table className="table-fixed flex-1 w-full text-sm text-center text-gray-500">
        <thead className="text-xs text-gray-700 uppercase border-b">
          <tr>
            <th scope="col" className="px-6 py-3 text-center">
              Weekday
            </th>
            <th scope="col" className="px-6 py-3">
              Start Hour
            </th>
            <th scope="col" className="px-6 py-3">
              End Hour
            </th>
            <th scope="col" className="px-6 py-3">
              Δ °C
            </th>
          </tr>
        </thead>
        <tbody>
          {(Object.keys(Weekday) as Array<Weekday>).map((weekday: Weekday) => {
            return (
              <tr key={weekday} className="bg-white border-b">
                <th
                  scope="row"
                  className="px-6 py-1 font-light text-gray-900 whitespace-nowrap"
                >
                  {weekday}
                </th>
                <td className="px-6 py-1">
                  <input
                    type="number"
                    min="0"
                    max="24"
                    id={`${weekday.toLowerCase()}StartHour`}
                    className={inputStyles()}
                    defaultValue={
                      getWeekday(boostingSchedule, weekday).startHour
                    }
                    disabled={!editing}
                  />
                </td>
                <td className="px-6 py-1">
                  <input
                    type="number"
                    min="0"
                    max="24"
                    id={`${weekday.toLowerCase()}EndHour`}
                    className={inputStyles()}
                    defaultValue={getWeekday(boostingSchedule, weekday).endHour}
                    disabled={!editing}
                  />
                </td>
                <td className="px-6 py-1">
                  <input
                    type="number"
                    min="-10"
                    max="10"
                    id={`${weekday.toLowerCase()}TemperatureDelta`}
                    className={inputStyles()}
                    defaultValue={
                      getWeekday(boostingSchedule, weekday).temperatureDelta
                    }
                    disabled={!editing}
                  />
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
      <div className="flex justify-start items-center p-2 pb-0">
        <span>Edit</span>
        <label className={"relative inline-flex cursor-pointer select-none items-center".concat(updating ? " opacity-50 cursor-not-allowed" : "")}>
          <input
            type="checkbox"
            checked={editing}
            onChange={() => setEditing(!editing)}
            className="sr-only"
          />
          <span
            className={`slider mx-4 flex h-8 w-[60px] items-center rounded-full p-1 duration-200 ${
              editing ? 'bg-[rgb(31,41,55)]' : 'bg-[#CCCCCE]'
            }`}
          >
            <span
              className={`dot h-6 w-6 rounded-full bg-white duration-200 ${
                editing ? 'translate-x-[28px]' : ''
              }`}
            ></span>
          </span>
        </label>
        {editing && (
          <button
            type="submit"
            className={"bg-[rgb(31,41,55)] text-white font-bold py-1 px-3 rounded".concat(updating ? " opacity-50 cursor-not-allowed" : "")}
          >
            Save
          </button>
        )}
        {updating && (
          <Spinner className="px-3" />
        )}
      </div>
    </form>
  );
};

export default BoostingScheduleTable;
