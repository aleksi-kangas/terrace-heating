'use client';

import {BoostingSchedule} from '../../api/types';
import {useState} from 'react';

interface BoostingScheduleTableProps {
  boostingSchedule: BoostingSchedule;
  title: string;
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

const BoostingScheduleTable = ({
  boostingSchedule,
  title,
}: BoostingScheduleTableProps) => {
  const [editing, setEditing] = useState<boolean>(false);

  return (
    <div className="flex-1 w-full h-full flex flex-col p-6 gap-2">
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
            console.log(weekday);
            console.log(getWeekday(boostingSchedule, weekday));
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
                    className="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                    value={getWeekday(boostingSchedule, weekday).startHour}
                    disabled={!editing}
                  />
                </td>
                <td className="px-6 py-1">
                  <input
                    type="number"
                    min="0"
                    max="24"
                    className="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                    value={getWeekday(boostingSchedule, weekday).endHour}
                    disabled={!editing}
                  />
                </td>
                <td className="px-6 py-1">
                  <input
                    type="number"
                    min="0"
                    max="24"
                    className="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                    value={
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
    </div>
  );
};

export default BoostingScheduleTable;
