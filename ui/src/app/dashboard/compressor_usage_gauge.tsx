import React, {Suspense} from 'react';
import {fetchCompressorRecordsDaysRange} from '@/app/api/history';
import {CompressorRecord} from '@/app/api/types';
import {Gauge} from '@/app/components/gauge';
import Spinner from '@/app/components/spinner';

const CompressorUsageGauge = async () => {
  const records: CompressorRecord[] = await fetchCompressorRecordsDaysRange(2);

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
