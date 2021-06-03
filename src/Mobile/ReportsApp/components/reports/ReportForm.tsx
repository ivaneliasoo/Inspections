import React, { useEffect, useRef, useState } from 'react'
import moment from 'moment'
import { useFormikContext } from 'formik'
import { Datepicker, Input, Layout, Select, SelectItem } from '@ui-kitten/components'
import { StyleSheet, View } from 'react-native'
import { CalendarIcon } from '../Icons'
import { useOrientation } from '../../hooks/helpers'
import { Checklists } from './Checklists'
import { AddressDTO, AddressesApi, ReportsApi, CheckListsApi, Configuration, Report, UpdateCheckListItemCommand, CheckListItem } from '../../services/api'
import AsyncStorage from '@react-native-async-storage/async-storage';
import { API_CONFIG } from '../../config/config'
import { ScrollView } from 'react-native-gesture-handler'
import { AutoSave } from '../../components/AutoSave'


const userToken = AsyncStorage.getItem('userToken');
const getAddressList = async (filter?: string) => {
    const apiService =  new AddressesApi({accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey} as Configuration)
    const result = await apiService.getAddresses(filter);
    return result.data
}

const updateCheckList = async (payload: { reportId: number; checkListId: number; newValue: number | undefined }) => {
  const api = new ReportsApi({accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey} as Configuration)
  api.bulkUpdateChecks(payload.reportId, payload.checkListId, payload.newValue)
}

const updateCheckListItem = async (payload: CheckListItem) => {
  const api = new CheckListsApi({accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey} as Configuration)
  const command: UpdateCheckListItemCommand = {
    checkListId: payload.checkListId,
    checked: payload.checked,
    editable: payload.editable,
    id: payload.id,
    remarks: payload.remarks,
    required: payload.required,
    text: payload.text
  }
  api.updateChecklistItem(payload.checkListId ?? -1, payload.id ?? -1, command)
}

const ReportForm = () => {
  const { values, setFieldValue, errors } = useFormikContext<Report>()
  const mountedRef = useRef(true)

  const { orientation } = useOrientation();
  const [addresses, setAddresses] = useState<AddressDTO[]>([])
  const flexType = orientation === 'landscape' ? 'row' : 'column'

  useEffect(() => {
    if(mountedRef.current) {
      getAddressList().then((resp) => {
        setAddresses(resp)
      })
    }
    return () => {
      mountedRef.current = false
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
      <View style={{alignSelf: 'center', flex: 1 }}>
        <AutoSave debounceMs={300}/>
      </View>
      <Layout style={[styles.container, { flexDirection: flexType }]}>
        
        <Datepicker
          style={[{ flex: flexType === 'row' ? 3 : undefined }, styles.inputMargin]}
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
        <Input style={[{ flex: flexType === 'row' ? 2 : undefined}, styles.inputMargin]} disabled label='License' value={values.license?.number!}
        caption='Selected Address License Number' />
      </Layout>
      <ScrollView>
        <Checklists onCheckListUpdated={updateCheckList} onCheckListItemUpdated={updateCheckListItem} />
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
