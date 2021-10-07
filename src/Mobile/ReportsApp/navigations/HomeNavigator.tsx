import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import { MyReports } from '../containers/MyReports';
import { Home } from '../containers/Home';
import { Details } from '../containers/Details';
import { SignaturePad } from '../components/reports/SignaturePad';
import PhotoRecordGallery from '../components/photorecords/PhotoRecordGallery';

const MainStack = createStackNavigator();
export const HomeNavigator = () => {

  return (
    <MainStack.Navigator headerMode="none">
      <MainStack.Screen name="Home" component={Home} />
      <MainStack.Screen name="MyReports" component={MyReports} />
      <MainStack.Screen name="Details" component={Details} />
      <MainStack.Screen name='ModalSignatures' component={SignaturePad} options={{ title: 'Please Sign' }} />
      <MainStack.Screen name="PhotoRecordGallery" component={PhotoRecordGallery} />
    </MainStack.Navigator>
  );
};
