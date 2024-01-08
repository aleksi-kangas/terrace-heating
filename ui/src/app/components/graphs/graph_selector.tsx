'use client';
import React from 'react';
import Link from 'next/link';
import {usePathname} from 'next/navigation';

interface TabProps {
  active: boolean;
  label: string;
  href: string;
}

const Tab = ({active, label, href}: TabProps) => {
  let styles =
    'flex items-center justify-center gap-2 rounded-lg px-3 py-2 font-semibold';
  const inactiveStyles =
    ' text-gray-400 hover:shadow hover:bg-gray-700 hover:text-white';
  const activeStyles = ' text-white bg-gray-900';
  styles = active ? styles.concat(activeStyles) : styles.concat(inactiveStyles);
  return (
    <li className="flex-1">
      <Link href={href} className={styles}>
        {label}
      </Link>
    </li>
  );
};

interface GraphSelectorProps {
  tabItems: {
    href: string;
    label: string;
  }[];
}

const GraphSelector = ({tabItems}: GraphSelectorProps) => {
  const activeUrl = usePathname();
  console.log(activeUrl);
  return (
    <div className="overflow-hidden rounded-xl border border-gray-100 bg-[rgb(31,41,55)] p-1.5">
      <ul className="flex items-center gap-2">
        {tabItems.map((tabItem, i) => (
          <Tab
            key={tabItem.href}
            active={activeUrl.endsWith(tabItem.href)}
            href={tabItem.href}
            label={tabItem.label}
          ></Tab>
        ))}
      </ul>
    </div>
  );
};

export default GraphSelector;
