import React, { useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext'
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { MyReports } from '../containers/MyReports';
import { Details } from '../containers/Details';
import { SplashScreen } from '../containers/SplashScreen';
import { Authentication } from '../containers/Authentication';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { CameraScreen } from '../containers/CameraScreen';
import { ReportsProvider } from '../contexts/ReportsContext';
import { SignaturePad } from '../components/reports/SignaturePad';
import { Home } from '../containers/Home';

const MainStack = createStackNavigator();
const RootStack = createStackNavigator();

const HomeNavigator = () => {

  const { authState } = useContext(AuthContext)

  return (
    <MainStack.Navigator headerMode="none">
      {
        authState.isLoading ? <MainStack.Screen name="SplashScreen" component={SplashScreen} />
          : authState.userToken === null ? <MainStack.Screen name="Authentication" component={Authentication} /> :
            <>
              <MainStack.Screen name="Home" component={Home} />
              <MainStack.Screen name="MyReports" component={MyReports} />
              <MainStack.Screen name="Details" component={Details} />
            </>
      }
    </MainStack.Navigator>
  )
};

export const AppNavigator = () => (
  <NavigationContainer>
    <ReportsProvider>
      <RootStack.Navigator mode='modal'>
        <RootStack.Screen
          name="Main"
          component={HomeNavigator}
          options={{ headerShown: false }}
        />
        <RootStack.Screen name='ModalSignatures' component={SignaturePad} options={{ title: 'Please Sign' }} />
      </RootStack.Navigator>
    </ReportsProvider>
  </NavigationContainer>
);
