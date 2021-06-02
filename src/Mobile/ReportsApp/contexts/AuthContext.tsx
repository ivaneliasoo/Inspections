import AsyncStorage from '@react-native-async-storage/async-storage';
import React, { createContext, useEffect, useMemo, useReducer } from 'react';
import { useAuthorization } from '../hooks/useAuthorization';
import { authReducer } from './authReducer';

export interface AuthState {
  isLoading: boolean;
  isSignOut: boolean;
  userToken?: string;
  userInfo?: any;
}

export const initialState: AuthState = {
  isLoading: false,
  isSignOut: true,
  userToken: undefined,
  userInfo: undefined
}

export interface AuthContextProps {
  authState: AuthState,
  signIn: (data: { user: string, password: string }) => Promise<void>,
  signOut: () => void;
}

export const AuthContext = createContext({} as AuthContextProps);

export const AuthProvider = ({ children }: any) => {
  const { createToken, userInfo, signOut: logout } = useAuthorization()

  const [authState, dispatch] = useReducer(authReducer, initialState)

  const signIn = async (data: { user: string, password: string }) => {
    const token = await createToken(data)
    if (token) {
      const userData = await userInfo(token)
      console.log(userData)
      dispatch({ type: 'SIGN_IN', payload: { userInfo: userData, token } });
    }
  }

  const signOut = async () => {
    await logout()
    dispatch({ type: 'SIGN_OUT' })
  }

  useEffect(() => {
    const tryRecoverToken = async () => {

      try {
        let userToken = await AsyncStorage.getItem('userToken');
        if(userToken) {
          const userData = await userInfo(userToken)
          console.log(userData)
          dispatch({ type: 'RESTORE_TOKEN', payload: { userInfo: userData, token: userToken } });
        }
      } catch (e) {
        console.warn('error trying to recover persisted user token')
        dispatch({ type: 'SIGN_OUT' })
      }
    }

    tryRecoverToken()
  }, [])

  const value = useMemo(() => {
    return {
      authState,
      signIn,
      signOut
    }
  }, [authState])

  return (
    <AuthContext.Provider
      value={value}
    >
      {children}
    </AuthContext.Provider>
  )

}