import Card from './card';
import Link from 'next/link';

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
          <Tab href={tabItem.href} label={tabItem.label}></Tab>
        ))}
      </ul>
    </Card>
  );
};

export default Tabs;
