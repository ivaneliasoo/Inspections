import AsyncStorage from "@react-native-async-storage/async-storage";
import { AuthContext } from "../contexts/AuthContext";
import { useContext } from "react";
import { API_CONFIG, API_HOST, API_KEY } from "../config/config";
import { AuthApi, Configuration } from "../services/api";
import { UsersApi } from '../services/api/api';


export const useAuthorization = () => {
  const authApi = new AuthApi(API_CONFIG as Configuration)
  
  
  const createToken = async (data: { user: string, password: string}) => {
    try {
      const resp = await authApi.login({ username: data.user, password: data.password })
      return resp.data as unknown as string
    } catch (error) {
      console.log({ error })
    }
  }
  
  const userInfo = async (token: string) => {
    try {
      const userToken = await AsyncStorage.getItem('userToken')
      
      const usersApi = new UsersApi({ accessToken: token ?? userToken, basePath: API_HOST, apiKey: API_KEY } as Configuration)
      const resp = await usersApi.getActiveUser()
      return resp.data || {}
    } catch (error) {
      console.log({ error })
    }
  }

  const signOut = async () => {
    try {
      await AsyncStorage.removeItem('userToken')
    } catch (error) {
      console.warn(error)
    }
  }

  return {createToken, userInfo, signOut}
}

