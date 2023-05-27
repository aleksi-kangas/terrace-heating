import Link from 'next/link';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faTemperatureHalf} from '@fortawesome/free-solid-svg-icons';

const navigation = [
  {name: 'Dashboard', href: '/dashboard', active: true},
  {name: 'Graphs', href: '/graphs', active: false},
  {name: 'Schedules', href: '/schedules', active: false},
];

const Header = () => {
  return (
    <nav className="bg-gray-800">
      <div className="mx-auto max-w-7xl px-4 sm:px6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <FontAwesomeIcon icon={faTemperatureHalf} className="h-8 w-8" />
            </div>
            <div>
              <div className="flex space-x-4 ml-6">
                {navigation.map(item => (
                  <Link
                    key={item.name}
                    href={item.href}
                    className={'rounded-md px-3 py-2 text-sm font-medium '.concat(
                      item.active
                        ? 'bg-gray-900 text-white'
                        : 'text-gray-300 hover:bg-gray-700 hover:text-white'
                    )}
                  >
                    {item.name}
                  </Link>
                ))}
              </div>
            </div>
          </div>
          <div className="flex items-center hidden md:ml-6">
            <button
              type="button"
              className="rounded-md bg-gray-900 px-3 py-2 text-sm font-medium"
            >
              TODO Dark Mode
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Header;
