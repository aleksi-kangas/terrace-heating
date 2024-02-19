export enum BoostingScheduleVariable {
  Circuit3 = 'circuit3',
  LowerTank = 'lowerTank',
}

export interface BoostingSchedule {
  monday: WeekdayBoostingSchedule;
  tuesday: WeekdayBoostingSchedule;
  wednesday: WeekdayBoostingSchedule;
  thursday: WeekdayBoostingSchedule;
  friday: WeekdayBoostingSchedule;
  saturday: WeekdayBoostingSchedule;
  sunday: WeekdayBoostingSchedule;
}

export enum CacheTags {
  BoostingSchedules = 'boostingSchedules',
  HeatingState = 'heatingState',
}

export interface CompressorRecord {
  active: boolean;
  time: string;
  usage?: number;
}

export enum HeatingState {
  Inactive = 0,
  SoftStarting = 1,
  Active = 2,
  Boosting = 3,
}

export interface TankLimitRecord {
  time: string;
  lowerTankMinimum: number;
  lowerTankMinimumAdjusted: number;
  lowerTankMaximum: number;
  lowerTankMaximumAdjusted: number;
  upperTankMinimum: number;
  upperTankMinimumAdjusted: number;
  upperTankMaximum: number;
  upperTankMaximumAdjusted: number;
}

export interface TemperatureRecord {
  time: string;
  circuit1: number;
  circuit2: number;
  circuit3: number;
  groundInput: number;
  groundOutput: number;
  hotGas: number;
  inside: number;
  lowerTank: number;
  outside: number;
  upperTank: number;
}

export interface Temperatures {
  circuit1: number;
  circuit2: number;
  circuit3: number;
  groundInput: number;
  groundOutput: number;
  hotGas: number;
  inside: number;
  lowerTank: number;
  outside: number;
  upperTank: number;
}

export interface WeekdayBoostingSchedule {
  startHour: number;
  endHour: number;
  temperatureDelta: number;
}
