import AsyncStorage from "@react-native-async-storage/async-storage";
import { API_CONFIG } from "../config/config";
import { AuthApi, Configuration } from "../services/api";
import { UsersApi } from '../services/api/api';

const authApi = new AuthApi(API_CONFIG as Configuration)
const usersApi = new UsersApi(API_CONFIG as Configuration)

export const useAuthorization = () => {
  const createToken = async (data: { user: string, password: string}) => {
    try {
      const resp = await authApi.login({ username: data.user, password: data.password })
      await AsyncStorage.mergeItem('userToken', resp.data as unknown as string)
      return resp.data as unknown as string
    } catch (error) {
      console.log({ error })
    }
  }

  const userInfo = async (token: string) => {
    try {
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

