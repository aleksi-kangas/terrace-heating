import React from 'react';
import {Inter} from 'next/font/google';
import '@/app/globals.css';
import NavigationBar from '@/app/components/navigation_bar';

const inter = Inter({subsets: ['latin']});

export const metadata = {
  title: 'Terrace Heating',
  description: 'Terrace Heating',
};

const RootLayout = ({children}: {children: React.ReactNode}) => {
  return (
    <html lang="en">
      <body className={`${inter.className} flex flex-col min-h-screen`}>
        <NavigationBar />
        <div className="flex flex-1 justify-center p-1 sm:p-2 lg:p-8">
          {children}
        </div>
      </body>
    </html>
  );
};

export default RootLayout;
