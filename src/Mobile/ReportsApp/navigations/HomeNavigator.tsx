import React, { useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { createStackNavigator } from '@react-navigation/stack';
import { MyReports } from '../containers/MyReports';
import { Details } from '../containers/Details';
import { Authentication } from '../containers/Authentication';
import { Home } from '../containers/Home';

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
