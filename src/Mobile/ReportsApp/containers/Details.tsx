import React, { useRef, useState } from 'react';
import { Button, Divider, TopNavigation, TopNavigationAction, ViewPager } from '@ui-kitten/components';
import { ReportForm } from '../containers/ReportForm'
import { OperationalReading } from '../containers/OperationalReading'
import { BackIcon } from '../components/Icons'
import { Formik } from 'formik';
import { Directions, FlingGestureHandler, State } from 'react-native-gesture-handler'
import { Signatures } from './Signatures';


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

  const navigateBack = () => {
    navigation.goBack();
  };

  const BackAction = () => (
    <TopNavigationAction icon={BackIcon} onPress={navigateBack} />
  );

  const [selectedIndex, setSelectedIndex] = useState(1)

  const { reportId, title } = route.params

  const shouldLoadComponent = (index) => index === selectedIndex;

  const formRef = useRef<MyValues>()

  const handleSubmit = () => {
    if (formRef.current) {
      formRef.current.handleSubmit()
    }
  }

  return (

    <Formik innerRef={formRef} initialValues={{
      date: new Date(),
      address: '',
      licenseNumber: ''
    }} onSubmit={values => console.log(`Email: ${values.address}, Password: ${values.licenseNumber}, Date: ${values.date}`)}>
      <>
        <TopNavigation title={`Report  `} alignment='center' accessoryLeft={BackAction} accessoryRight={() => <Button size='small' onPress={handleSubmit}>Save</Button>} />
        <Divider />
        <FlingGestureHandler
          numberOfPointers={2}
          direction={Directions.DOWN | Directions.UP}
          onHandlerStateChange={({nativeEvent}) => {
            if (nativeEvent.state === State.ACTIVE) {
              navigateToCamera()
            }
          }}
        >
          <ViewPager style={{flex: 1}} selectedIndex={selectedIndex} shouldLoadComponent={shouldLoadComponent} onSelect={index => setSelectedIndex(index)}>
            <OperationalReading />
            <ReportForm />
            <Signatures />
          </ViewPager>
        </FlingGestureHandler>
      </>
    </Formik >
  );
};