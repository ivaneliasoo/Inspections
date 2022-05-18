import AsyncStorage from "@react-native-async-storage/async-storage";
import { AuthContext } from "../contexts/AuthContext";
import { useContext } from "react";
import { API_CONFIG, API_HOST, API_KEY } from "../config/config";
import { AuthApi, Configuration, ConfigurationParameters } from "../services/api";
import { UsersApi } from '../services/api/apis';


export const useAuthorization = () => {
  const authApi = new AuthApi( new Configuration(API_CONFIG));


  const createToken = async (data: { user: string, password: string }) => {
    const resp = await authApi.login({ loginModel: {username: data.user, password: data.password }})
    return resp as unknown as string
  }

  const userInfo = async (token: string) => {
    try {
      const userToken = await AsyncStorage.getItem('userToken')

      const usersApi = new UsersApi((new Configuration({ accessToken: token ?? userToken, basePath: API_HOST, apiKey: API_KEY })));
      const resp = await usersApi.getActiveUser()
      return resp || {}
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

  return { createToken, userInfo, signOut }
}

