import React, { useState } from 'react';
import { Divider, TopNavigation, Tab, TabView } from '@ui-kitten/components';
import { ReportForm } from '../containers/ReportForm'
import { Checklists } from '../containers/Checklists'
import { OperationalReading } from '../containers/OperationalReading'

export const Details = () => {
  const [selectedIndex, setSelectedIndex] = useState(1)
  return (
    <>
      <TopNavigation title='Report Detail' alignment='center' />
      <Divider />
      <TabView selectedIndex={selectedIndex} onSelect={index => setSelectedIndex(index)}>
        <Tab title='Operational Readings'>
          <OperationalReading />
        </Tab>
        <Tab title='Report Details'>
          <ReportForm />
        </Tab>
        <Tab title='Checklist'>
          <Checklists />
        </Tab>
      </TabView>
    </>
  );
};