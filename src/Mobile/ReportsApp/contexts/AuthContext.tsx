import AsyncStorage from '@react-native-async-storage/async-storage';
import { createContext, useEffect, useMemo, useReducer } from 'react';
import { useAuthorization } from '../hooks/useAuthorization';
import { authReducer } from './authReducer';

export interface AuthState {
  isLoading: boolean;
  isSignOut: boolean;
  userToken?: string;
  userInfo?: any;
}

export const initialState: AuthState = {
  isLoading: true,
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
  const { createToken, userInfo } = useAuthorization()

  const [authState, dispatch] = useReducer(authReducer, initialState)

  const signIn = async (data: { user: string, password: string }) => {
    const token = await createToken(data)
    const userData = await userInfo()
    if (token) dispatch({ type: 'SIGN_IN', payload: { userInfo: userData, token } });
  }

  const signOut = () => {
    dispatch({ type: 'SIGN_OUT' })
  }

  useEffect(() => {
    const tryRecoverToken = async () => {

      try {
        let userToken = await AsyncStorage.getItem('userToken');
        if(userToken) {
          const userData = await userInfo()
          dispatch({ type: 'RESTORE_TOKEN', payload: { userInfo: userData, token: userToken } });
        }
      } catch (e) {
        console.warn('erro try to recover persisten user token')
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