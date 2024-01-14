'use client';
import React from 'react';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faTemperatureHalf} from '@fortawesome/free-solid-svg-icons';
import Link from 'next/link';
import {usePathname} from 'next/navigation';

const navigation = [
  {label: 'Dashboard', href: '/dashboard', hrefMatch: '/dashboard'},
  {label: 'Graphs', href: '/graphs/external', hrefMatch: '/graphs'},
  {label: 'Schedules', href: '/schedules', hrefMatch: '/schedules'},
];

interface NavigationBarItemProps {
  active: boolean;
  href: string;
  label: string;
}

const NavigationBarItem = ({active, href, label}: NavigationBarItemProps) => {
  return (
    <Link
      key={label}
      href={href}
      className={'rounded-md px-3 py-2 text-sm font-semibold '.concat(
        active
          ? 'bg-gray-900 text-white'
          : 'text-gray-300 hover:bg-gray-700 hover:text-white'
      )}
    >
      {label}
    </Link>
  );
};

const NavigationBar = () => {
  const activeUrl = usePathname();
  return (
    <header className="bg-gray-800">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <FontAwesomeIcon
                icon={faTemperatureHalf}
                className="h-8 w-8 text-gray-200"
              />
            </div>
            <nav>
              <div className="flex space-x-4 ml-6">
                {navigation.map(item => (
                  <NavigationBarItem
                    key={item.label}
                    active={activeUrl.startsWith(item.hrefMatch)}
                    href={item.href}
                    label={item.label}
                  />
                ))}
              </div>
            </nav>
          </div>
        </div>
      </div>
    </header>
  );
};

export default NavigationBar;
