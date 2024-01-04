import React, {Suspense} from 'react';
import Spinner from '../spinner';
import {Gauge} from '../gauge';
import {DateTime, Duration} from 'luxon';
import {fetchCompressorRecords} from '../../api/heating';
import {CompressorRecord} from '../../api/types';

const CompressorUsageGauge = async () => {
  const from = DateTime.utc().minus(Duration.fromObject({days: 2}));
  const to = DateTime.utc();
  const records = await fetchCompressorRecords(from, to);

  let currentUsage: number | undefined = undefined;
  const lastWithUsage = records
    .filter((compressorRecord: CompressorRecord) => compressorRecord.usage)
    .pop();
  if (lastWithUsage) {
    currentUsage = Math.round(lastWithUsage.usage! * 100);
  }

  return (
    <Suspense fallback={<Spinner className="flex-1 max-h-[45%] w-full" />}>
      <div className="flex-1 max-h-[45%] w-full p-8 flex justify-center items-center flex-col">
        <Gauge
          className="p-4"
          value={currentUsage ?? 0}
          size="large"
          showValue={true}
          textColor="text-[#333]"
        />
        <div className="p-4 font-semibold">Compressor Usage %</div>
      </div>
    </Suspense>
  );
};

export default CompressorUsageGauge;
