import React, { useContext, useEffect, useState } from 'react'
import { AddressDTO, AddressesApi, Configuration } from '../services/api';
import { API_HOST, API_KEY } from '../config/config';
import { AuthContext } from '../contexts/AuthContext';

export const useAddresses = () => {
  const { authState: { userToken } } = useContext(AuthContext)
  const [addresses, setAddresses] = useState<AddressDTO[]>([])

  const addressApi = new AddressesApi(new Configuration({ accessToken: userToken!, basePath: API_HOST, apiKey: API_KEY }))

  const getAddressList = async (filter?: string) => {
    const result = await addressApi.getAddresses(filter);
    setAddresses(result.data)
    return result.data
  }

  useEffect(() => {
    getAddressList()
  }, [])

  return {
    getAddressList,
    addresses
  }
}
