import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import { MyReports } from '../containers/MyReports';
import { Home } from '../containers/Home';
import { ReportDetailsNavigator } from './ReportDetailsNavigator';
import { Details } from '../containers/Details';

const MainStack = createStackNavigator();
export const HomeNavigator = () => {

  return (
    <MainStack.Navigator headerMode="none">
      <MainStack.Screen name="Home" component={Home} />
      <MainStack.Screen name="MyReports" component={MyReports} />
      <MainStack.Screen name="Details" component={Details} />
    </MainStack.Navigator>
  );
};
