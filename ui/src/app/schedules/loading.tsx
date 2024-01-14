import React from 'react';
import Spinner from '@/app/components/spinner';

const SchedulesLoading = () => {
  return (
    <div className="flex-1 w-full p-8 flex justify-center items-center">
      <Spinner className="flex-1 max-h-96" />
    </div>
  );
};

export default SchedulesLoading;
