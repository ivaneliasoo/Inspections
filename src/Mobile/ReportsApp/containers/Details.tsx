import React, { useEffect, useState } from 'react';
import { Divider, Spinner, Tab, TabView, TopNavigation, TopNavigationAction, ViewPager } from '@ui-kitten/components';
import { BackIcon } from '../components/Icons'
import { StackNavigationProp } from '@react-navigation/stack';
import { StyleSheet, Text, View } from 'react-native';
import { ReportForm } from '../components/reports/ReportForm';
import { OperationalReading } from '../components/reports/OperationalReading';
import { CameraScreen } from '../containers/CameraScreen';
import { useReports } from '../hooks/useReports';

type DetailsScreenNavigationProp = StackNavigationProp<any, any>


interface Props {
  route: any;
  navigation: DetailsScreenNavigationProp;
  state: any;
}
export const Details = ({ route, navigation, state }: Props) => {

  const navigateBack = () => {
    navigation.goBack();
  };

  const BackAction = () => (
    <TopNavigationAction icon={BackIcon} onPress={navigateBack} />
  );

  const [selectedIndex, setSelectedIndex] = useState(3)

  const { reportId } = route.params
  const { getReportById, workingReport } = useReports()


  const shouldLoadComponent = (index: number) => index === selectedIndex;

  useEffect(() => {
    getReportById(reportId)
  }, [])

  return (
    <>
      <TopNavigation title={`Report  `} alignment='center' accessoryLeft={BackAction} />
      <Divider />
      {workingReport ?
        <TabView style={styles.viewPagerLayout} selectedIndex={selectedIndex} shouldLoadComponent={shouldLoadComponent}
          onSelect={index => setSelectedIndex(index)}>
          <Tab title="Camera and Photos">
            <CameraScreen />
          </Tab>
          <Tab title="Operational Readings">
            <OperationalReading />
          </Tab>
          <Tab title="Report Detail">
            <ReportForm />
          </Tab>
          {/* <Tab title="Signatures">
              <Signatures />
            </Tab> */}
        </TabView>
        : <Spinner size='large' />}
    </>
  );
};

const styles = StyleSheet.create({
  viewPagerLayout: {
    flex: 1
  }
})