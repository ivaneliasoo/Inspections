import React from 'react';
import { AuthContext } from './authentication-context'
import { AuthApi, Configuration } from './services/api'
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { MyReports } from './containers/MyReports';
import { Details } from './containers/Details';
import { SplashScreen } from './containers/SplashScreen';
import { Authentication } from './containers/Authentication';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { API_CONFIG } from './config/config'
import { CameraScreen } from './containers/CameraScreen';
import { Signatures } from './components/reports/Signatures';
import { ReportsProvider } from './reports-contexts';
import { SignaturePad } from './components/reports/SignaturePad';
import {Home} from './containers/Home';

const MainStack = createStackNavigator();
const RootStack = createStackNavigator();

const authApi = new AuthApi(API_CONFIG as Configuration)

const HomeNavigator = () => {
  const [state, dispatch] = React.useReducer(
    (prevState: any, action: { type: any; token: any; }) => {
      switch (action.type) {
        case 'SIGN_IN':
          return {
            ...prevState,
            userToken: action.token,
            isSignOut: false,
          }
        case 'SIGN_OUT':
          return {
            ...prevState,
            userToken: null,
            isSignOut: true,
          }
        case 'RESTORE_TOKEN':
          return {
            ...prevState,
            userToken: action.token,
            isLoading: false,
          }
      }
    },
    {
      isLoading: true,
      isSignOut: false,
      userToken: null,
    }
  );

  React.useEffect(() => {
    // Fetch the token from storage then navigate to our appropriate place
    const bootstrapAsync = async () => {
      let userToken;

      try {
        userToken = await AsyncStorage.getItem('userToken');
      } catch (e) {
        // Restoring token failed
      }

      // After restoring token, we may need to validate it in production apps

      // This will switch to the App screen or Auth screen and this loading
      // screen will be unmounted and thrown away.
      dispatch({ type: 'RESTORE_TOKEN', token: userToken });
    };

    bootstrapAsync();
  }, []);
  const authContext = React.useMemo(
    () => ({
      signIn: async (data: any) => {
        try {
          const resp = await authApi.login({ username: data.user, password: data.password })
          await AsyncStorage.mergeItem('userToken', resp.data as unknown as string)
          dispatch({ type: 'SIGN_IN', token: 'dummy-auth-token' });
        } catch (error) {
          console.log({ error })
        }
      },
      signOut: () => dispatch({ type: 'SIGN_OUT', token: null }),
    }),
    []
  );

  return (
    <AuthContext.Provider value={authContext}>
      <MainStack.Navigator headerMode="none">
        {
          state.isLoading ? <MainStack.Screen name="SplashScreen" component={SplashScreen} />
            : state.userToken === null ? <MainStack.Screen name="Authentication" component={Authentication} /> :
              <>
                <MainStack.Screen name="Home" component={Home} />
                <MainStack.Screen name="MyReports" component={MyReports} />
                <MainStack.Screen name="Details" component={Details} />
                <MainStack.Screen name="Camera" component={CameraScreen} />
              </>
        }
      </MainStack.Navigator>
    </AuthContext.Provider >
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
        <RootStack.Screen name='ModalSignatures' component={SignaturePad} options={{ title: 'Please Sign' }}/>
      </RootStack.Navigator>
    </ReportsProvider>
  </NavigationContainer>
);
