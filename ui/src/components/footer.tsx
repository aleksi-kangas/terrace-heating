import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faGithub} from '@fortawesome/free-brands-svg-icons';
import Link from 'next/link';

const Footer = () => {
  return (
    <footer className="bg-gray-800">
      <div className="mx-auto max-w-7xl px-4 sm:px6 lg:px-8">
        <div className="flex h-12 items-center justify-center">
          <div className="flex items-center">
            <Link
              href="https://github.com/aleksi-kangas/terrace-heating"
              className="flex flex-shrink-0 font-medium items-center px-2 py-1 rounded-md space-x-4 text-sm hover:bg-gray-700 hover:text-white"
            >
              <FontAwesomeIcon icon={faGithub} className="h-6 w-6" />
              <span className="text-sm font-medium">aleksi-kangas</span>
            </Link>
            <div className="flex items-center space-x-4 flex-shrink-0"></div>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
