import React from 'react'
import moment from 'moment'
import { useFormik, useFormikContext } from 'formik'
import { Button, Datepicker, Input, Layout } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'
import { CalendarIcon } from '../components/Icons'

export interface MyValues {
  date: Date;
  address: string;
  licenseNumber: string;
}

const ReportForm = () => {
  const { values, handleChange, setFieldValue, validationSchema } = useFormikContext<MyValues>()
  
  console.log({ validationSchema })

  return (
    <>
      <Layout style={styles.container}>
        <Datepicker
          style={{ flex: 3, marginHorizontal: 3 }}
          label='Label'
          caption='Caption'
          placeholder='Pick Date'
          date={values.date}
          min={new Date()}
          onSelect={nextDate => setFieldValue('date', nextDate)}
          accessoryRight={CalendarIcon}
        />
        <Input style={{ flex: 7, marginHorizontal: 3 }} label='Address' value={values.address} onChangeText={handleChange('address')} />
        <Input style={{ flex: 2, marginHorizontal: 3 }} label='License' value={values.licenseNumber} onChangeText={handleChange('licenseNumber')} />
      </Layout>
    </>
  )
}

export { ReportForm }

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    padding: 10
  }
})
