import React, { useEffect, useRef, useState } from 'react';
import { Button, Divider, Spinner, TopNavigation, TopNavigationAction, ViewPager } from '@ui-kitten/components';
import { ReportForm } from '../components/reports/ReportForm'
import { OperationalReading } from '../components/reports/OperationalReading'
import { BackIcon } from '../components/Icons'
import { Formik } from 'formik';
import { Directions, FlingGestureHandler, State } from 'react-native-gesture-handler'
import { API_CONFIG } from '../config/config'
import { Configuration, Report, ReportsApi } from '../services/api'
import AsyncStorage from '@react-native-async-storage/async-storage';
import moment from 'moment';
import * as Yup from 'yup';

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
  const [reportData, setReportData] = useState<Report>({})

  const { reportId } = route.params

  const shouldLoadComponent = (index: number) => index === selectedIndex;

  const formRef = useRef()

  const handleSubmit = () => {
    if (formRef.current) {
      formRef.current!.handleSubmit()
    }
  }
  
  useEffect(() => {
    const getReportData = async () => {
      const userToken: string = await AsyncStorage.getItem('userToken') as string;
      const apiService =  new ReportsApi({accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey} as Configuration)
      const result = (await apiService.reportsIdGet(reportId)).data
      result.date = moment(result.date).toDate()
      console.log(result.date)
      setReportData(result as any)
    }
    getReportData()
  }, [reportId])

  const reportValidationSchema = Yup.object().shape({
    address: Yup.string().required('Required. Please Select an Address')
  })

  return (

    <Formik innerRef={formRef} validationSchema={reportValidationSchema} initialValues={reportData} enableReinitialize onSubmit={values => console.log({values})}>
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
            {reportData ? <ViewPager style={{ flex: 1 }} selectedIndex={selectedIndex} shouldLoadComponent={shouldLoadComponent} onSelect={index => setSelectedIndex(index)}>
              <OperationalReading />
              <ReportForm />
            </ViewPager>:<Spinner  />}
          </FlingGestureHandler>
        </FlingGestureHandler>
      </>
    </Formik >
  );
};