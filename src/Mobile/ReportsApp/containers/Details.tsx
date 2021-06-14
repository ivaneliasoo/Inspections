import React, { useEffect, useState } from 'react';
import { Divider, Icon, Layout, TopNavigation, TopNavigationAction, useTheme } from '@ui-kitten/components';
import { BackIcon } from '../components/Icons';
import { StackNavigationProp } from '@react-navigation/stack';
import { ActivityIndicator, StyleSheet, View } from 'react-native';
import { ReportForm } from '../components/reports/ReportForm';
import { OperationalReading } from '../components/reports/OperationalReading';
import { CameraScreen } from '../containers/CameraScreen';
import { useReports } from '../hooks/useReports';
import { Signatures } from '../components/reports/Signatures';
import { ReportsContext } from '../contexts/ReportsContext';
import { useContext } from 'react';
import { createMaterialTopTabNavigator } from '@react-navigation/material-top-tabs';
import { ParticullarOfInstallation } from '../components/reports/ParticullarOfInstallation';

type DetailsScreenNavigationProp = StackNavigationProp<any, any>

const { Navigator, Screen } = createMaterialTopTabNavigator();

interface Props {
  route: any;
  navigation: DetailsScreenNavigationProp;
  state: any;
}
export const Details = ({ route, navigation }: Props) => {
  const { clearWorkingReport } = useContext(ReportsContext)
  const [loading, setLoading] = useState(true)
  const theme = useTheme()

  const navigateBack = () => {
    clearWorkingReport()
    navigation.goBack();
  };

  const BackAction = () => (
    <TopNavigationAction icon={(props) => <Icon name='arrow-back-outline' style={{ width: 50, height: 50 }} {...props} />} onPress={navigateBack} />
  );

  const { reportId } = route.params
  const { getReportById, workingReport } = useReports()

  useEffect(() => {
    getReportById(reportId).finally(() => { setLoading(false) })
  }, [])


  

  return (
    <View style={{backgroundColor:'white', flex: 1}}>
      <TopNavigation title={`Report  `} alignment='center' accessoryLeft={BackAction} />
      <Divider />
      {workingReport && !loading ?
        <View style={styles.container}>
          <ParticullarOfInstallation />
          <Navigator tabBarPosition='bottom' initialRouteName="Report" lazy={true} title='Editing Report' options={{ showIcon: true }}>
            <Screen name='Camera' component={CameraScreen} options={{ tabBarIcon: () => <Icon name="camera-outline" size={26} /> }} />
            <Screen name='OperationalReadings' component={OperationalReading} options={{ title: 'Readings' }} />
            <Screen name='Report' component={ReportForm} />
            <Screen name='Signs' component={Signatures} />
          </Navigator>
        </View>

        :
        <View style={styles.spinnerLayout}>
          <ActivityIndicator size='large' color={theme['color-primary-500']} />
        </View>
      }
    </View>
  );

  
};

const styles = StyleSheet.create({
  spinnerLayout: {
    flex: 1, justifyContent: 'space-evenly', alignContent: 'center', alignSelf: 'center',
    backgroundColor: 'white'
  },
  container: {
    flex: 1,
    backgroundColor: 'white'
  },
})