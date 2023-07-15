'use client';

import ErrorAlert from "./components/error_alert";

const Error = () => {
  return (
    <div className="flex-1 flex justify-center items-center">
      <ErrorAlert message="Server connection failure" className=""/>
    </div>
  );
};

export default Error;
