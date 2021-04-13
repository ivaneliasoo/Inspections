import React, { useEffect, useState } from 'react'
import moment from 'moment'
import { useFormikContext } from 'formik'
import { Datepicker, Input, Layout, Select, SelectItem } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'
import { CalendarIcon } from '../Icons'
import { useOrientation } from '../../utils/helpers'
import { Checklists } from './Checklists'
import { AddressDTO, AddressesApi, Configuration, Report } from '../../services/api'
import AsyncStorage from '@react-native-async-storage/async-storage';
import { API_CONFIG } from '../../config/config'
import {useDebouncedCallback} from 'use-debounce'
import { ScrollView } from 'react-native-gesture-handler'

const getAddressList = async (filter?: string) => {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const apiService =  new AddressesApi({accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey} as Configuration)
    const result = await apiService.getAddresses(filter);

    return result.data
}

const ReportForm = () => {
  const { values, setFieldValue, errors } = useFormikContext<Report>()
  const { orientation } = useOrientation();
  const [addresses, setAddresses] = useState<AddressDTO[]>([])
  const flexType = orientation === 'landscape' ? 'row' : 'column'

  useEffect(() => {
    getAddressList().then((resp) => {
      setAddresses(resp)
    })
    return () => {
      setAddresses([]);
    }
  }, [])

  const reanderOption = (item: AddressDTO, index: number) => {
    return (<SelectItem
      key={index}
      title={`${item.formatedAddress} (${item.number} Expires on ${moment(item.validity?.end).format('DD/MM/YYYY')})`}
    ></SelectItem>
    )
  }

  return (
    <>
      <Layout style={[styles.container, { flexDirection: flexType }]}>
        <Datepicker
          style={[{ flex: flexType === 'row' ? 3 : undefined}, styles.inputMargin]}
          label='Date'
          caption='Select the report date'
          placeholder='Pick Date'
          date={values.date as Date|undefined}
          max={new Date()}
          onSelect={(e) => setFieldValue('date', e)}
          accessoryRight={CalendarIcon}
        />
        <Select
          placeholder='type to search an address'
          value={values.address!}
          style={[{ flex: flexType === 'row' ? 7 : undefined}, styles.inputMargin]} label='Address' 
          onSelect={(e) => { setFieldValue('address', addresses[e.row].formatedAddress); setFieldValue('license.number', addresses[e.row].number)}}
          status={errors.address ? 'danger':'basic'}
          caption={errors.address}
        >
          {addresses.map(reanderOption)}
        </Select>
        <Input style={[{ flex: flexType === 'row' ? 2 : undefined}, styles.inputMargin]}} disabled label='License' value={values.license?.number}
        caption='Selected Address License Number' />
      </Layout>
      <ScrollView>
        <Checklists />
      </ScrollView>
    </>
  )
}

export { ReportForm }

const styles = StyleSheet.create({
  container: {
    padding: 10
  },
  inputMargin: {
    marginHorizontal: 3
  }
})
