export interface BoostingSchedule {
  monday: WeekdayBoostingSchedule,
  tuesday: WeekdayBoostingSchedule,
  wednesday: WeekdayBoostingSchedule,
  thursday: WeekdayBoostingSchedule,
  friday: WeekdayBoostingSchedule,
  saturday: WeekdayBoostingSchedule,
  sunday: WeekdayBoostingSchedule,
}

export interface HeatPumpRecord {
  tankLimits: TankLimits;
  temperatures: Temperatures;
  time: Date;
}

export interface TankLimits {
  lowerTankMinimum: number;
  lowerTankMinimumAdjusted: number;
  lowerTankMaximum: number;
  lowerTankMaximumAdjusted: number;
  upperTankMinimum: number;
  upperTankMinimumAdjusted: number;
  upperTankMaximum: number;
  upperTankMaximumAdjusted: number;
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
