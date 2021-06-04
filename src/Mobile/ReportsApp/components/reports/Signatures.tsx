import { Card, Datepicker, Input, Select, SelectItem, Text, Button } from '@ui-kitten/components'
import { CalendarIcon, CrossIcon, EditSignatureIcon } from '../Icons'
import { Formik, FormikProps } from 'formik'
import React, { useContext } from 'react'
import { Alert, Image, StyleSheet, View } from 'react-native'
import { Configuration, Report, SignaturesApi } from '../../services/api'
import { ScrollView } from 'react-native-gesture-handler'
import { useNavigation } from '@react-navigation/native'
import { createRef } from 'react'
import { API_CONFIG } from '../../config/config'
import AsyncStorage from '@react-native-async-storage/async-storage'
import { AutoSave } from '../../components/AutoSave'
import { ReportsContext } from '../../contexts/ReportsContext'


const Signatures = () => {
  const navigation = useNavigation()

  const { reportsState: {workingReport: report} } = useContext(ReportsContext)
  const formRef = createRef<FormikProps<Report>>()

  const onGoBack = (result: any) => {
    formRef.current?.setFieldValue(`signatures[${result.index}].drawnSign`, `data:image/png;base64,${result.encoded}`)
  }

  const saveSignatures = async (values: Report) => {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const apiService = new SignaturesApi({ accessToken: userToken, basePath: API_CONFIG.basePath, apiKey: API_CONFIG.apiKey } as Configuration)
    values.signatures?.forEach(async s => {
      await apiService.signaturesIdPut(Number(s.id), {
        id: s.id,
        title: s.title,
        annotation: s.annotation,
        responsableType: s.responsable?.type,
        responsableName: s.responsable?.name,
        designation: s.designation,
        remarks: s.remarks,
        date: s.date,
        principal: s.principal,
        drawnSign: s.drawnSign
      }).catch(error => {
        Alert.alert('error while saving signature', error.response.message)
      })
    })
  }

  return (
    <>
      <Formik innerRef={formRef} validateOnMount={true} initialValues={report!} enableReinitialize onSubmit={saveSignatures}>
        {({ setFieldValue, handleSubmit, errors, handleBlur, values }) =>
          <ScrollView>
            {values.signatures!.map((item, signIndex) => {
              return (<Card key={signIndex} style={styles.card}>
                <View style={{ alignSelf: 'center' }}>
                  <AutoSave debounceMs={300} />
                </View>
                <Text category='s1'>
                  {`${item.title}${JSON.stringify(errors)}`}
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
                  value={['Supervisor', 'Inspector', 'Witness', 'LEW', 'Other'][item?.responsable?.type ?? 0]}
                  label='Representation Type'
                  onSelect={(e) => { setFieldValue(`signatures[${signIndex}].responsable.type`, [e.row]) }}
                  status={errors && errors.signatures && errors.signatures[signIndex] && errors.signatures[signIndex].responsable && errors.signatures[signIndex].responsable.type ? 'danger' : 'basic'}
                >
                  {['Supervisor', 'Inspector', 'Witness', 'LEW', 'Other'].map((responsible, index) =>
                    <SelectItem
                      key={index}
                      title={responsible}
                    ></SelectItem>)}
                </Select>
                <Input label='Name' value={item.responsable!.name!} onChangeText={(e) => setFieldValue(`signatures[${signIndex}].responsable.name`, e)} status={errors && errors.signatures && errors.signatures[signIndex] && errors.signatures[signIndex].responsable && errors.signatures[signIndex].responsable.name ? 'danger' : 'basic'} />
                <Input label='Designation' value={item.designation!} onChangeText={(e) => setFieldValue(`signatures[${signIndex}].designation`, e)} />
                <Input label='Remarks' multiline value={item.remarks!} onChangeText={(e) => setFieldValue(`signatures[${signIndex}].remarks`, e)} />
                <View style={{ flex: 2, flexDirection: 'row' }}>
                  {item.drawnSign!.length > 0 && <Image style={{ flex: 1, alignSelf: 'center', borderColor: 'black', width: 150, height: 100, resizeMode: 'stretch' }} source={{ uri: item.drawnSign! }} />}
                  <View style={{ flex: 1, flexDirection: 'row' }}>
                    <Button style={{ flex: 1, margin: 10, marginTop: 20 }} status='warning' size='small' appearance='outline' onPress={() => navigation.navigate('ModalSignatures', { index: signIndex, existentSign: item.drawnSign, onGoBack: onGoBack })} accessoryLeft={EditSignatureIcon} />
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
