import React from 'react';
import Link from 'next/link';
import Card from '@/app/components/card';

interface TabProps {
  label: string;
  href: string;
}

const Tab = ({label, href}: TabProps) => {
  return (
    <li className="w-full">
      <Link href={href}>
        <button>{label}</button>
      </Link>
    </li>
  );
};

interface TabsProps {
  tabItems: {
    href: string;
    label: string;
  }[];
}

const Tabs = ({tabItems}: TabsProps) => {
  return (
    <Card>
      <ul className="font-medium text-center text-gray-700 divide-y md:divide-x divide-gray-200 rounded-lg shadow sm:flex">
        {tabItems.map(tabItem => (
          <Tab
            key={tabItem.href}
            href={tabItem.href}
            label={tabItem.label}
          ></Tab>
        ))}
      </ul>
    </Card>
  );
};

export default Tabs;
