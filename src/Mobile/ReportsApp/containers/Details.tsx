import React, { useEffect, useState } from 'react';
import { Divider, Text, Icon, TopNavigation, TopNavigationAction, useTheme } from '@ui-kitten/components';
import { StackNavigationProp } from '@react-navigation/stack';
import { ActivityIndicator, Alert, StyleSheet, View } from 'react-native';
import { ReportForm } from '../components/reports/ReportForm';
import { OperationalReading } from '../components/reports/OperationalReading';
import { CameraScreen } from '../containers/CameraScreen';
import { useReports } from '../hooks/useReports';
import { Signatures } from '../components/reports/Signatures';
import { ReportsContext } from '../contexts/ReportsContext';
import { useContext } from 'react';
import { createMaterialTopTabNavigator } from '@react-navigation/material-top-tabs';

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

  const CompleteAction = () => (
    <TopNavigationAction icon={(props) => <View style={{flexDirection: 'row', alignItems: 'center', justifyContent: 'space-evenly'}}><Icon fill='green' name='checkmark-circle-2-outline' style={{ width: 50, height: 50 }} {...props} /><Text category='s1'>Complete Report</Text></View>} onPress={() => {
      if (workingReport?.isClosed) return;
        Alert.alert('Complete / Close Report', `You are about to complete the report ${workingReport?.name} (${reportId}). Are you sure?`,
          [
            {
              text: 'Yes',
              onPress: () => { completeReport(reportId) }
            },
            {
              text: 'No',
            }
          ]
        );
    }} />
  );

  const { reportId } = route.params
  const { getReportById, workingReport, completeReport } = useReports()

  useEffect(() => {
    getReportById(reportId).finally(() => { setLoading(false) })
  }, [])


  

  return (
    <View style={{backgroundColor:'white', flex: 1}}>
      <TopNavigation title={`Report  `} alignment='center' accessoryRight={CompleteAction} accessoryLeft={BackAction} />
      <Divider />
      {workingReport && !loading ?
        <View style={styles.container}>
          <Navigator tabBarPosition='bottom' initialRouteName="Report" lazy={true} title='Editing Report' options={{ showIcon: true }}>
            <Screen name='Camera' component={CameraScreen} options={{ tabBarIcon: () => <Icon name="camera-outline" size={26} /> }} />
            <Screen name='OperationalReadings' component={OperationalReading} options={{ title: 'Readings' }} />
            <Screen name='Report' component={ReportForm} />
            <Screen name='Signature' component={Signatures} />
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