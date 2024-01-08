import React from 'react';
import Card from '@/app/components/card';
import DaysSelector from '@/app/components/graphs/days_selector';
import GraphSelector from '@/app/components/graphs/graph_selector';

const GraphsLayout = ({children}: {children: React.ReactNode}) => {
  return (
    <div className="flex flex-1 max-w-7xl flex-col gap-4">
      <GraphSelector
        tabItems={[
          {href: '/graphs/external', label: 'External'},
          {href: '/graphs/tanks', label: 'Tanks'},
          {href: '/graphs/circuits', label: 'Heat Distribution Circuits'},
        ]}
      />
      <Card className="flex-1 h-44 w-full p-8 flex justify-center items-center">
        {children}
      </Card>
      <DaysSelector
        tabItems={[
          {href: '?days=7', label: '7 Days'},
          {href: '?days=2', label: '2 Days'},
          {href: '?days=1', label: '1 Day'},
        ]}
      />
    </div>
  );
};

export default GraphsLayout;
