'use client';
import React from 'react';
import Link from 'next/link';
import {usePathname, useSearchParams} from 'next/navigation';

const commonStyles = 'px-4 py-2 text-sm border-gray-200 shadow-md';
const inactiveStyles = ' font-medium text-gray-900 bg-white hover:bg-gray-100';
const activeStyles = ' font-semibold text-white bg-[rgb(31,41,55)]';

const isActive = (daysQuery: string | null, days: number) => {
  return daysQuery !== null ? daysQuery === days.toString() : days === 2;
};

const DaysSelector = () => {
  const activeUrl = usePathname();
  const searchParams = useSearchParams();
  const daysQuery = searchParams.get('days');
  return (
    <div className="inline-flex rounded-md justify-center">
      <Link href={`${activeUrl}?days=7`}>
        <button
          type="button"
          className={'border rounded-s-lg '
            .concat(commonStyles)
            .concat(isActive(daysQuery, 7) ? activeStyles : inactiveStyles)}
        >
          7 Days
        </button>
      </Link>
      <Link href={`${activeUrl}?days=2`}>
        <button
          type="button"
          className={'border-t border-b '
            .concat(commonStyles)
            .concat(isActive(daysQuery, 2) ? activeStyles : inactiveStyles)}
        >
          2 Days
        </button>
      </Link>
      <Link href={`${activeUrl}?days=1`}>
        <button
          type="button"
          className={'border rounded-e-lg '
            .concat(commonStyles)
            .concat(isActive(daysQuery, 1) ? activeStyles : inactiveStyles)}
        >
          1 Day
        </button>
      </Link>
    </div>
  );
};

export default DaysSelector;
