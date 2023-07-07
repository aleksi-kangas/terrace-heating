import Tabs from '../components/tabs';
import Card from '../components/card';

const GraphsLayout = ({children}: {children: React.ReactNode}) => {
  return (
    <div className="flex flex-1 w-full h-full max-w-7xl flex-col gap-4">
      <Tabs
        tabItems={[
          {href: '/graphs/external', label: 'External'},
          {
            href: '/graphs/tanks',
            label: 'Tanks',
          },
          {href: '/graphs/circuits', label: 'Heat Distribution Circuits'},
        ]}
      />
      <Card className="flex-1 h-full flex justify-evenly items-center">
        {children}
      </Card>
    </div>
  );
};

export default GraphsLayout;
