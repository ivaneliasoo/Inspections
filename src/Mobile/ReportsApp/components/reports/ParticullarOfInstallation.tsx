import { Formik, FormikProps } from 'formik';
import { AddressAutocomplete, AddressSelectedResult } from './AddressAutocomplete';
import { AutoSave } from '../AutoSave';
import React, { useRef } from 'react';
import { CalendarIcon } from '../Icons';
import moment from 'moment';
import { ReportQueryResult, UpdateReportCommand } from '../../services/api';
import * as Yup from 'yup';
import { Datepicker, Text } from '@ui-kitten/components';
import { useReports } from '../../hooks/useReports';
import { useOrientation } from '../../hooks/helpers';
import { Alert, View } from 'react-native';

export const ParticullarOfInstallation = () => {
  const { orientation } = useOrientation();
  const flexType = orientation === 'landscape' ? 'row' : 'column'

  const formRef = useRef<FormikProps<ReportQueryResult>>(null)
  const { workingReport: reportData, saveReport, updateCheckList, updateCheckListItem } = useReports()

  const handleSubmit = async () => {
    if (formRef.current) {
      if (formRef.current.isValid && formRef.current.dirty) {
        const updateCmd: UpdateReportCommand = {
          address: formRef.current.values.address ?? '',
          date: moment(formRef.current.values.date).format('YYYY-MM-DD'),
          name: formRef.current.values.name ?? '',
          id: formRef.current.values.id ?? 0,
          isClosed: false, // TODO: Mirar Como pasar isClosed
          licenseNumber: formRef.current.values.licenseNumber ?? ''
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

  return <Formik innerRef={formRef} validateOnMount validationSchema={reportValidationSchema} initialValues={reportData!} enableReinitialize onSubmit={handleSubmit}>
    {({ values, errors, setFieldValue }) => (
      <>
        <View style={{ alignSelf: 'center' }}>
          <AutoSave debounceMs={300} />
        </View>
        <View style={{ marginHorizontal: 10 }}>
          <Text category='h6'>Particular of Installation: </Text>
          <View>
            <Datepicker
              style={{ flex: 6 }}
              label='Report Date'
              placeholder='Pick Date'
              date={values.date as Date | undefined}
              max={new Date()}
              onSelect={(e) => setFieldValue('date', e)}
              accessoryRight={CalendarIcon} />
            <AddressAutocomplete
              style={{ flex: 6 }}
              values={values} errors={errors} flexType={flexType} onSelect={(e: AddressSelectedResult) => { setFieldValue('address', e.formattedAddress); setFieldValue('licenseNumber', e.licenseNumber); }} />
          </View>
          <View style={{ marginHorizontal: 10 }}>
            <Text category='h6'>License</Text>
            <Text category='s1'>Company:
              <Text category='s2'> {values.licenseName!}</Text>
            </Text>
            <Text category='s1'>
              Validity
              <Text category='s2'> {moment(values.licenseValidity?.start!).format('DD-MM-YYYY')} - {moment(values.licenseValidity?.end!).format('DD-MM-YYYY')}</Text>
              <Text category='s1'>  Number:
                <Text category='s2'> {values.licenseNumber!}</Text>
              </Text>
            </Text>
            <Text category='s1'>Appoved Load:
              <Text category='s1'> Amp:
                <Text category='s2'> {values.licenseAmp!}</Text>
                <Text category='s1'>Kva:
                  <Text category='s2'> {values.licenseKVA!}</Text>
                  <Text category='s1'>  Volt:
                    <Text category='s2'> {values.licenseVolt!}</Text>
                  </Text>
                </Text>
              </Text>
            </Text>
          </View>
        </View>
      </>
    )}
  </Formik>;
}