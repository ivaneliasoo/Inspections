import React, { createRef, useEffect, useRef, useState } from 'react';
import { Button, Divider, Spinner, TopNavigation, TopNavigationAction, ViewPager } from '@ui-kitten/components';
import { ReportForm } from '../components/reports/ReportForm'
import { OperationalReading } from '../components/reports/OperationalReading'
import { BackIcon } from '../components/Icons'
import { Formik, FormikProps } from 'formik';
import { StackNavigationProp } from '@react-navigation/stack';
import { Directions, FlingGestureHandler, State } from 'react-native-gesture-handler'
import { API_CONFIG } from '../config/config'
import { Configuration, Report, ReportsApi, Signature, UpdateReportCommand } from '../services/api'
import AsyncStorage from '@react-native-async-storage/async-storage';
import moment from 'moment';
import * as Yup from 'yup';
import { Alert, StyleSheet, TouchableOpacity, View } from 'react-native';
import { Signatures } from '../components/reports/Signatures';
import { SignaturePad } from '../components/reports/SignaturePad';

type DetailsScreenNavigationProp = StackNavigationProp<any, any>

type Props = {
  route: any,
  navigation: DetailsScreenNavigationProp
}
export const Details = ({ route, navigation }: Props) => {


  const navigateToCamera = () => {
    navigation.navigate('Camera')
  }

  const navigateToSignatures = () => {
    navigation.navigate('Signatures')
  }

  const navigateBack = () => {
    navigation.goBack();
  };

  const BackAction = () => (
    <TopNavigationAction icon={BackIcon} onPress={navigateBack} />
  );

  const [selectedIndex, setSelectedIndex] = useState(1)
  const [reportData, setReportData] = useState<Report>({})
  const mountedRef = useRef(true)

  const { reportId } = route.params

  const shouldLoadComponent = (index: number) => index === selectedIndex;

  const formRef = useRef<FormikProps<Report>>(null)

  const handleSubmit = async () => {
    if (formRef.current) {
      if (formRef.current.isValid) {
        const userToken: string = await AsyncStorage.getItem('userToken') as string;
        const apiService = new ReportsApi({ accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey } as Configuration)
        const updateCmd: UpdateReportCommand = {
          address: formRef.current.values.address,
          date: moment(formRef.current.values.date).format('YYYY-MM-DD'),
          name: formRef.current.values.name,
          id: formRef.current.values.id,
          isClosed: formRef.current.values.isClosed,
          licenseNumber: formRef.current.values.license?.number
        }
        await apiService.reportsIdPut(reportId, updateCmd)
        .catch(error => {
          Alert.alert('Datos Inválidos', error.response.message)
        })
        formRef.current!.handleSubmit()
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

    <Formik innerRef={formRef} validateOnMount validationSchema={reportValidationSchema} initialValues={reportData} enableReinitialize onSubmit={() => console.log('saved')}>
      <>
        <TopNavigation title={`Report  `} alignment='center' accessoryLeft={BackAction} accessoryRight={() => <Button size='small' onPress={handleSubmit}>Save</Button>} />
        <Divider />
        <FlingGestureHandler
          numberOfPointers={2}
          direction={Directions.LEFT}
          onHandlerStateChange={({ nativeEvent }) => {
            if (nativeEvent.state === State.ACTIVE) {
              navigateToCamera()
            }
          }}
        >
          <FlingGestureHandler
            numberOfPointers={2}
            direction={Directions.UP}
            onHandlerStateChange={({ nativeEvent }) => {
              if (nativeEvent.state === State.ACTIVE) {
                navigateToSignatures()
              }
            }}
          >
            {reportData ? <ViewPager style={styles.viewPagerLayout} selectedIndex={selectedIndex} shouldLoadComponent={shouldLoadComponent} onSelect={index => setSelectedIndex(index)}>
              <OperationalReading reportData={reportData} />
              <ReportForm />
              <Signatures report={reportData} />
            </ViewPager> : <Spinner />}
          </FlingGestureHandler>
        </FlingGestureHandler>
      </>
    </Formik >
  );
};

const styles = StyleSheet.create({
  viewPagerLayout: {
    flex: 1
  }
})