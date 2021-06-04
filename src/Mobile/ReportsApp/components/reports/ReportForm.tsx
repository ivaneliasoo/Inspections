import React, { useRef } from 'react'
import moment from 'moment'
import { Formik, FormikProps } from 'formik'
import { Datepicker, Layout } from '@ui-kitten/components'
import { Alert, StyleSheet, View } from 'react-native'
import { CalendarIcon } from '../Icons'
import { useOrientation } from '../../hooks/helpers'
import { Checklists } from './Checklists'
import { ScrollView } from 'react-native-gesture-handler'
import { AutoSave } from '../../components/AutoSave'
import * as Yup from 'yup';
import { useReports } from '../../hooks/useReports';
import { Report, UpdateReportCommand } from 'services/api'
import { AddressAutocomplete, AddressSelectedResult } from './AddressAutocomplete'

const ReportForm = () => {
  const formRef = useRef<FormikProps<Report>>(null)

  const { orientation } = useOrientation();
  const flexType = orientation === 'landscape' ? 'row' : 'column'

  const { workingReport: reportData, saveReport, updateCheckList, updateCheckListItem } = useReports()

  const handleSubmit = async () => {
    console.log('save called')
    if (formRef.current) {
      console.log(formRef.current)
      if (formRef.current.isValid && formRef.current.dirty) {
        console.log('jajaja')
        const updateCmd: UpdateReportCommand = {
          address: formRef.current.values.address ?? '',
          date: moment(formRef.current.values.date).format('YYYY-MM-DD'),
          name: formRef.current.values.name ?? '',
          id: formRef.current.values.id ?? 0,
          isClosed: formRef.current.values.isClosed ?? false,
          licenseNumber: formRef.current.values.license?.number ?? ''
        }
        await saveReport(updateCmd)
          .catch(error => {
            Alert.alert('Datos Inválidos', error.response.message)
          })
      } else {
        Alert.alert('Datos Inválidos', `report contains invalid fields: ${Object.keys(formRef.current.errors).map(field => field)}`)
      }
    }
  }

  const reportValidationSchema = Yup.object().shape({
    address: Yup.string().required('Required. Please Select an Address'),
    date: Yup.date().required('Required. Please Select a date')
  })

  return (
    <ScrollView>
      <Layout style={[styles.container, { flexDirection: flexType }]}>
        <Formik innerRef={formRef} validateOnMount validationSchema={reportValidationSchema} initialValues={reportData!} enableReinitialize onSubmit={handleSubmit}>
          {({ values, errors, setFieldValue }) => (
            <>
              <View style={{ alignSelf: 'center', flex: 1 }}>
                <AutoSave debounceMs={300} />
              </View>
              <Datepicker
                style={[{ flex: flexType === 'row' ? 3 : 1 }, styles.inputMargin]}
                label='Date'
                caption='Select the report date'
                placeholder='Pick Date'
                date={values.date as Date | undefined}
                max={new Date()}
                onSelect={(e) => setFieldValue('date', e)}
                accessoryRight={CalendarIcon} />
              <AddressAutocomplete values={values} errors={errors} flexType={flexType} onSelect={(e: AddressSelectedResult) => { console.log({e}); setFieldValue('address', e.formattedAddress); setFieldValue('license.number', e.licenseNumber) }} />
              <Checklists onCheckListUpdated={updateCheckList} onCheckListItemUpdated={updateCheckListItem} />
            </>
          )}
        </Formik >
      </Layout>
    </ScrollView>
  )
}

export { ReportForm }

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  inputMargin: {
    marginHorizontal: 3
  }
})
