import { Card, Datepicker, Input, Select, SelectItem, Text, Button, useTheme } from '@ui-kitten/components'
import { CalendarIcon, CrossIcon, EditSignatureIcon } from '../Icons'
import { Formik, FormikProps } from 'formik'
import React, { useContext, createRef } from 'react'
import { Alert, Image, StyleSheet, View } from 'react-native'
import { Configuration, Report, SignaturesApi } from '../../services/api'
import { ScrollView } from 'react-native-gesture-handler'
import { useNavigation } from '@react-navigation/native'
import { API_CONFIG } from '../../config/config'
import AsyncStorage from '@react-native-async-storage/async-storage'
import { AutoSave } from '../../components/AutoSave'
import { ReportsContext } from '../../contexts/ReportsContext'
import { showMessage } from 'react-native-flash-message';
import { useReports } from '../../hooks/useReports';


const Signatures = () => {
  const navigation = useNavigation()
  const theme = useTheme()

  const { workingReport: report, saveSignature } = useReports()
  const formRef = createRef<FormikProps<Report>>()

  const save = async (values: Report) => {
    if (formRef.current) {
      if (formRef.current.isValid && formRef.current.dirty) {

        values.signatures?.forEach(async (s, i) => {
          await saveSignature({ signature: s, index: i })
            .catch(error => {
              console.log({error})
              showMessage({
                message: 'error while saving signature',
                description: error.response?.data?.message ?? '',
                backgroundColor: theme['color-danger-500'],
                floating: true,
                autoHide: false,
                icon: 'danger'
              })
            })
        })
      }
    }
  }

  return (
    <>
      <Formik innerRef={formRef} validateOnMount={true} initialValues={report!} enableReinitialize onSubmit={save}>
        {({ setFieldValue, handleSubmit, errors, handleBlur, values }) =>
          <ScrollView>
            <View style={{ alignSelf: 'center' }}>
              <AutoSave debounceMs={300} />
            </View>
            {values && values.signatures && values.signatures!.map((item, signIndex) => {
              return (<Card key={signIndex} style={styles.card}>

                <Text category='h6'>
                  {`${item.title}`}
                </Text>
                <Text status='warning' category='s2'>
                  {item.principal && !item.drawnSign ? 'This Signature is required. Please complete and sign' : ''}
                </Text>
                <Datepicker
                  label='Date'
                  placeholder='Pick Date'
                  date={typeof item.date === 'string' ? new Date() : item.date}
                  max={new Date()}
                  onSelect={e => setFieldValue(`signatures[${signIndex}].date`, e)}
                  onBlur={() => handleBlur(`signatures[${signIndex}].date`)}
                  accessoryRight={CalendarIcon}
                />
                <Select
                  placeholder='please select...'
                  value={['Supervisor', 'Inspector', 'Witness', 'LEW', 'Other'][item?.responsible?.type ?? 0]}
                  label='Representation Type'
                  onSelect={(e) => { setFieldValue(`signatures[${signIndex}].responsible.type`, e.row) }}
                  status={errors && errors.signatures && errors.signatures[signIndex] && errors.signatures[signIndex].responsible && errors.signatures[signIndex].responsible.type ? 'danger' : 'basic'}
                >
                  {['Supervisor', 'Inspector', 'Witness', 'LEW', 'Other'].map((responsible, index) =>
                    <SelectItem
                      key={index}
                      title={responsible}
                    ></SelectItem>)}
                </Select>
                <Input label='Name' value={item.responsible!.name!} onChangeText={(e) => setFieldValue(`signatures[${signIndex}].responsible.name`, e)} status={errors && errors.signatures && errors.signatures[signIndex] && errors.signatures[signIndex].responsible && errors.signatures[signIndex].responsible.name ? 'danger' : 'basic'} />
                <Input label='Designation' value={item.designation!} onChangeText={(e) => setFieldValue(`signatures[${signIndex}].designation`, e)} />
                <Input label='Remarks' multiline value={item.remarks!} onChangeText={(e) => setFieldValue(`signatures[${signIndex}].remarks`, e)} />
                <View style={{ flex: 2, flexDirection: 'row' }}>
                  {item && item.drawnSign && item.drawnSign!.length > 0 && <Image style={{ flex: 1, alignSelf: 'center', borderColor: 'black', width: 150, height: 100, resizeMode: 'stretch' }} source={{ uri: item.drawnSign! }} />}
                  <View style={{ flex: 1, flexDirection: 'row' }}>
                    <Button style={{ flex: 1, margin: 10, marginTop: 20 }} status='warning' size='small' appearance='outline' onPress={() => navigation.navigate('ModalSignatures', { index: signIndex, existentSign: item.drawnSign, id: item.id })} accessoryLeft={EditSignatureIcon} />
                    <Button disabled={!item.drawnSign} style={{ flex: 1, margin: 10, marginTop: 20 }} status='danger' size='small' appearance='outline' onPress={() => setFieldValue(`signatures[${signIndex}].drawnSign`, '')} accessoryLeft={CrossIcon} />
                  </View>
                </View>
              </Card>
              )
            }
            )
            }
          </ScrollView>
        }
      </Formik>

    </>
  )
}

export { Signatures }

const styles = StyleSheet.create({
  card: { padding: 0, marginHorizontal: 5, marginVertical: 2 },
  modalBackdrop: { backgroundColor: 'rgba(0, 0, 0, 0.5)' },
})
