import React from 'react';
import axios from 'axios';
import {AuthContext} from './authentication-context'
import { AuthApi } from './services/api'
import {NavigationContainer} from '@react-navigation/native';
import {createStackNavigator} from '@react-navigation/stack';
import {HomeScreen} from './containers/Home';
import {SplashScreen} from './containers/SplashScreen';
import {Authentication} from './containers/Authentication';
import AsyncStorage from '@react-native-async-storage/async-storage';

const {Navigator, Screen} = createStackNavigator();

const authApi = new AuthApi({ basePath: 'http://192.168.88.250:5000', password: undefined, username: undefined, apiKey: 'falskjdufghasjdghfaskjdhgfa' })

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
        // In a production app, we need to send some data (usually username, password) to server and get a token
        // We will also need to handle errors if sign in failed
        // After getting token, we need to persist the token using `AsyncStorage`
        // In the example, we'll use a dummy token
        try {
          const resp = await authApi.login({username: data.user, password: data.password})
          await AsyncStorage.mergeItem('userToken', resp.data)
          dispatch({ type: 'SIGN_IN', token: 'dummy-auth-token' });
        } catch (error) {
          console.log({error})          
        }
      },
      signOut: () => dispatch({ type: 'SIGN_OUT', token: null }),
    }),
    []
  );

  return (
    <AuthContext.Provider value={authContext}>
      <Navigator headerMode="none">
        {
          state.isLoading ? <Screen name="SplashScreen" component={SplashScreen} /> 
          : state.userToken === null ? <Screen name="Authentication" component={Authentication} /> :
            <Screen name="My Reports" component={HomeScreen} />
        }
      </Navigator>
    </AuthContext.Provider>
  )};

export const AppNavigator = () => (
  <NavigationContainer>
    <HomeNavigator />
  </NavigationContainer>
);
