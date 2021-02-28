import React, { useEffect, useRef, useState } from 'react';
import { Button, Divider, TopNavigation, TopNavigationAction, ViewPager } from '@ui-kitten/components';
import { ReportForm } from '../containers/ReportForm'
import { OperationalReading } from '../containers/OperationalReading'
import { BackIcon } from '../components/Icons'
import { Formik } from 'formik';
import { Directions, FlingGestureHandler, State } from 'react-native-gesture-handler'
import config, { API_CONFIG } from '../config/config'
import { ReportsApi } from '../services/api'
import AsyncStorage from '@react-native-async-storage/async-storage';


export interface MyValues {
  date: Date;
  address: string;
  licenseNumber: string;
  handleSubmit: () => {}
}

export const Details = ({ route, navigation }) => {


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
  const [reportData, setReportData] = useState({})

  const { reportId, title } = route.params

  const shouldLoadComponent = (index: number) => index === selectedIndex;

  const formRef = useRef<MyValues>()

  const handleSubmit = () => {
    if (formRef.current) {
      formRef.current.handleSubmit()
    }
  }

  const defaultValues = {
    date: new Date(),
    address: 'esrgesrgsergsedrgsdrg',
    licenseNumber: ''
  }
  async function getReportData() {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const apiService =  new ReportsApi({accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey})
    const respuesta = await apiService.reportsIdGet(reportId)
    console.log({respuesta})
  }
  useEffect(() => {
    getReportData()
  }, [reportId])

  return (

    <Formik innerRef={formRef} initialValues={defaultValues} onSubmit={values => console.log(`Email: ${values.address}, Password: ${values.licenseNumber}, Date: ${values.date}`)}>
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
            <ViewPager style={{ flex: 1 }} selectedIndex={selectedIndex} shouldLoadComponent={shouldLoadComponent} onSelect={index => setSelectedIndex(index)}>
              <OperationalReading />
              <ReportForm />
            </ViewPager>
          </FlingGestureHandler>
        </FlingGestureHandler>
      </>
    </Formik >
  );
};