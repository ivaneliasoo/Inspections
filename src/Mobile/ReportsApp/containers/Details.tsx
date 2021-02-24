import React, { useState, useEffect } from 'react';
import { Divider, TopNavigation, TopNavigationAction, ViewPager } from '@ui-kitten/components';
import { ReportForm } from '../containers/ReportForm'
import { Checklists } from '../containers/Checklists'
import { OperationalReading } from '../containers/OperationalReading'
import { BackIcon } from '../components/Icons'

export const Details = ({ route, navigation }) => {

  const navigateBack = () => {
    navigation.goBack();
  };

  const BackAction = () => (
    <TopNavigationAction icon={BackIcon} onPress={navigateBack}/>
  );

  const [selectedIndex, setSelectedIndex] = useState(1)

  const { reportId, title } = route.params

  const shouldLoadComponent = (index) => index === selectedIndex;

  // useEffect(() => {
  //   effect
  //   return () => {
  //     cleanup
  //   }
  // }, [input])

  return (
    <>
      <TopNavigation title={`Report ${title} ${reportId}`} alignment='center' accessoryLeft={BackAction} />
      <Divider />
      <ViewPager selectedIndex={selectedIndex} shouldLoadComponent={shouldLoadComponent} onSelect={index => setSelectedIndex(index)}>
        <OperationalReading />
        <ReportForm />
        <Checklists />
      </ViewPager>
    </>
  );
};