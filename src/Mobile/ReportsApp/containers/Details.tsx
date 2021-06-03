import React, { useEffect, useRef, useState, useContext } from 'react';
import { BottomNavigation, BottomNavigationTab, Divider, Spinner, TabView, TopNavigation, TopNavigationAction } from '@ui-kitten/components';
import { BackIcon } from '../components/Icons'
import { Formik, FormikProps } from 'formik';
import { StackNavigationProp } from '@react-navigation/stack';
import { API_CONFIG } from '../config/config'
import { Configuration, Report, ReportsApi, UpdateReportCommand } from '../services/api'
import AsyncStorage from '@react-native-async-storage/async-storage';
import moment from 'moment';
import * as Yup from 'yup';
import { Alert, StyleSheet, View } from 'react-native';
import { ReportsContext } from '../contexts/ReportsContext';
import { ReportForm } from '../components/reports/ReportForm';
import { OperationalReading } from '../components/reports/OperationalReading';
import { CameraScreen } from '../containers/CameraScreen';
import { Signatures } from '../components/reports/Signatures';

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

  const [selectedIndex, setSelectedIndex] = useState(1)
  const [reportData, setReportData] = useState<Report>({})
  const mountedRef = useRef(true)

  const { reportsState: { workingReport: reportId } } = useContext(ReportsContext)

  // const shouldLoadComponent = (index: number) => index === selectedIndex;

  const formRef = useRef<FormikProps<Report>>(null)

  const handleSubmit = async () => {
    console.log('save called')
    if (formRef.current) {
      if (formRef.current.isValid) {
        const userToken: string = await AsyncStorage.getItem('userToken') as string;
        const apiService = new ReportsApi({ accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey } as Configuration)
        const updateCmd: UpdateReportCommand = {
          address: formRef.current.values.address ?? '',
          date: moment(formRef.current.values.date).format('YYYY-MM-DD'),
          name: formRef.current.values.name ?? '',
          id: formRef.current.values.id ?? 0,
          isClosed: formRef.current.values.isClosed ?? false,
          licenseNumber: formRef.current.values.license?.number ?? ''
        }
        await apiService.reportsIdPut(reportId, updateCmd)
          .catch(error => {
            Alert.alert('Datos Inválidos', error.response.message)
          })
      } else {
        Alert.alert('Datos Inválidos', `report contains invalid fields: ${Object.keys(formRef.current.errors).map(field => field)}`)
      }
    }
  }

  useEffect(() => {
    const getReportData = async () => {
      const userToken: string = await AsyncStorage.getItem('userToken') as string;
      const apiService = new ReportsApi({ accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey } as Configuration)
      const result: Report = (await apiService.reportsIdGet(reportId)).data as unknown as Report
      result.date = moment(result.date).toDate()
      setReportData(result as any)
    }
    if (mountedRef.current)
      getReportData()
    return () => {
      mountedRef.current = false
    }
  }, [])

  const reportValidationSchema = Yup.object().shape({
    address: Yup.string().required('Required. Please Select an Address'),
    date: Yup.date().required('Required. Please Select a date')
  })

  return (

    <Formik innerRef={formRef} validateOnMount validationSchema={reportValidationSchema} initialValues={reportData} enableReinitialize onSubmit={handleSubmit}>
      <>
        <TopNavigation title={`Report  `} alignment='center' accessoryLeft={BackAction} />
        <Divider />
        {reportData ?
          <View style={styles.viewPagerLayout}>
            <TabView selectedIndex={selectedIndex}
              onSelect={index => navigation.navigate(state.routeNames[index])}>
              <CameraScreen />
              <OperationalReading reportData={reportData} />
              <ReportForm />
              <Signatures report={reportData} />
            </TabView>
            <BottomNavigation selectedIndex={selectedIndex}
              onSelect={index => navigation.navigate(state.routeNames[index])}>
              <BottomNavigationTab title="Report Detail" />
              <BottomNavigationTab title="Camera and Photos" />
              <BottomNavigationTab title="Operational Readings" />
              <BottomNavigationTab title="Signatures" />
            </BottomNavigation>
          </View> : <Spinner />}
      </>
    </Formik >
  );
};

const styles = StyleSheet.create({
  viewPagerLayout: {
    flex: 1
  }
})