import React, { useRef, useState } from 'react';
import { Button, Divider, TopNavigation, TopNavigationAction, ViewPager } from '@ui-kitten/components';
import { ReportForm } from '../containers/ReportForm'
import { Checklists } from '../containers/Checklists'
import { OperationalReading } from '../containers/OperationalReading'
import { BackIcon } from '../components/Icons'
import { Formik, useFormik, useFormikContext } from 'formik';


export interface MyValues {
  date: Date;
  address: string;
  licenseNumber: string;
  handleSubmit: ()=>{}
}

export const Details = ({ route, navigation }) => {

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
        <TopNavigation title={`Report ${title} ${reportId}`} alignment='center' accessoryLeft={BackAction} accessoryRight={() => <Button size='small' onPress={handleSubmit}>Save</Button>} />
        <Divider />
        <ViewPager selectedIndex={selectedIndex} shouldLoadComponent={shouldLoadComponent} onSelect={index => setSelectedIndex(index)}>
          <OperationalReading />
          <ReportForm />
          <Checklists />
        </ViewPager>
      </>
    </Formik>
  );
};