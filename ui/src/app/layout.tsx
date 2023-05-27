import './globals.css';
import {Inter} from 'next/font/google';
import Header from '../components/header';
import Footer from '../components/footer';

const inter = Inter({subsets: ['latin']});

export const metadata = {
  title: 'Terrace Heating',
  description: 'Terrace Heating',
};

const RootLayout = ({children}: {children: React.ReactNode}) => {
  return (
    <html lang="en">
      <body className={inter.className}>
        <Header />
        <div className="content">{children}</div>
        <Footer />
      </body>
    </html>
  );
};

export default RootLayout;
