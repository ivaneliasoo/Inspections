import React, { useRef } from 'react'
import moment from 'moment'
import { Formik, FormikProps } from 'formik'
import { Datepicker, Text, Layout, Card } from '@ui-kitten/components'
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
    if (formRef.current) {
      if (formRef.current.isValid && formRef.current.dirty) {
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
              <View style={{ flexDirection: 'row', flex: 1 }}>
                <View style={{ flex: 1 }}>
                  <Datepicker
                    style={{ flex: 1 }}
                    label='Report Date'
                    placeholder='Pick Date'
                    date={values.date as Date | undefined}
                    max={new Date()}
                    onSelect={(e) => setFieldValue('date', e)}
                    accessoryRight={CalendarIcon} />
                  <AddressAutocomplete values={values} errors={errors} flexType={flexType} onSelect={(e: AddressSelectedResult) => { console.log({ e }); setFieldValue('address', e.formattedAddress); setFieldValue('license.number', e.licenseNumber) }} />
                </View>
                {values.license &&
                  <View style={{ flex: 1, marginHorizontal: 10 }}>
                    <Text category='h5'>License</Text>
                    <Text category='h6' style={{ flex: 1 }}>Company:
                      <Text category='s1'> {values.license?.name!}</Text>
                    </Text>
                    <Text category='h6'>
                      Validity
                      <Text category='s1'> {moment(values.license?.validity?.start!).format('DD-MM-YYYY')} - {moment(values.license?.validity?.end!).format('DD-MM-YYYY')}</Text>
                    </Text>
                    <Text category='h6'>Number:
                      <Text category='s1'> {values.license?.number!}</Text>
                    </Text>
                    <Text category='h6'>Amp:
                      <Text category='s1'> {values.license?.amp!}</Text>
                      <Text category='h6'>Kva:
                        <Text category='s1'> {values.license?.kva!}</Text>
                        <Text category='h6'>  Volt:
                          <Text category='s1'> {values.license?.volt!}</Text>
                        </Text>
                      </Text>
                    </Text>
                  </View>
                }
              </View>
            </>
          )}
        </Formik >
      </Layout>
      <Card
        style={{ margin: 3 }}
        header={() => <Text style={{ marginHorizontal: 10, marginTop: 10 }} appearance="hint" category="h6">Checks Legend {reportData?.checkList![0].annotation!}</Text>}
      >
        <Checklists checkLists={reportData?.checkList} onCheckListUpdated={updateCheckList} onCheckListItemUpdated={updateCheckListItem} />
      </Card>
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
