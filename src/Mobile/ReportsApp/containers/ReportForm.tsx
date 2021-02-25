import React from 'react'
import moment from 'moment'
import { useFormik, useFormikContext } from 'formik'
import { Button, Datepicker, Input, Layout } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'
import { CalendarIcon } from '../components/Icons'
import { useOrientation } from '../utils/helpers'
import { Checklists } from './Checklists'

export interface MyValues {
  date: Date;
  address: string;
  licenseNumber: string;
}

const ReportForm = () => {
  const { values, handleChange, setFieldValue, validationSchema } = useFormikContext<MyValues>()
  const { orientation } = useOrientation();

  const flexType = orientation === 'landscape' ? 'row' : 'column'


  return (
    <>
      <Layout style={[styles.container, { flexDirection: flexType }]}>
        <Datepicker
          style={{ flex: flexType === 'row' ? 3 : undefined, marginHorizontal: 3, }}
          label='Label'
          caption=''
          placeholder='Pick Date'
          date={values.date}
          max={new Date()}
          onSelect={nextDate => setFieldValue('date', nextDate)}
          accessoryRight={CalendarIcon}
        />
        <Input style={{ flex: flexType === 'row' ? 7 : undefined, marginHorizontal: 3 }} multiline label='Address' value={values.address} onChangeText={handleChange('address')} />
        <Input style={{ flex: flexType === 'row' ? 2 : undefined, marginHorizontal: 3 }} disabled label='License' value={values.licenseNumber} onChangeText={handleChange('licenseNumber')} />
      </Layout>
      <Checklists />
    </>
  )
}

export { ReportForm }

const styles = StyleSheet.create({
  container: {
    padding: 10
  }
})
