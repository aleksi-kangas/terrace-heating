import './globals.css';
import {Inter} from 'next/font/google';
import Link from "next/link";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faTemperatureHalf} from "@fortawesome/free-solid-svg-icons";

const inter = Inter({subsets: ['latin']});

export const metadata = {
  title: 'Terrace Heating',
  description: 'Terrace Heating',
};

const navigation = [
  {name: 'Dashboard', href: '/dashboard', active: true},
  {name: 'Graphs', href: '/graphs', active: false},
  {name: 'Schedules', href: '/schedules', active: false},
];

const RootLayout = ({children}: { children: React.ReactNode }) => {
  return (
    <html lang="en">
    <body className={`${inter.className} flex flex-col min-h-screen`}>
    <header className="bg-gray-800">
      <div className="mx-auto max-w-7xl px-4 sm:px6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <FontAwesomeIcon icon={faTemperatureHalf} className="h-8 w-8"/>
            </div>
            <nav>
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
            </nav>
          </div>
        </div>
      </div>
    </header>
    <div className="flex flex-1 justify-center p-4">{children}</div>
    </body>
    </html>
  );
};

export default RootLayout;
