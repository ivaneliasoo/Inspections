import { useReducer } from "react";
import { AuthState, initialState } from './AuthContext';

type AuthAction =
  | { type: 'SIGN_IN', payload: { userInfo: any; token: string; } }
  | { type: 'SIGN_OUT' }
  | { type: 'RESTORE_TOKEN', payload: { userInfo: any; token: string; } }

export const authReducer = (prevState: AuthState, action: AuthAction): AuthState => {
  switch (action.type) {
    case 'SIGN_IN':
      return {
        ...prevState,
        userToken: action.payload.token,
        userInfo: action.payload.userInfo,
        isSignOut: false,
      }
    case 'SIGN_OUT':
      return {
        ...prevState,
        userToken: undefined,
        isSignOut: true,
      }
    case 'RESTORE_TOKEN':
      return {
        ...prevState,
        userToken: action.payload.token,
        userInfo: undefined,
        isLoading: false,
      }
    default:
      return prevState
  }
}