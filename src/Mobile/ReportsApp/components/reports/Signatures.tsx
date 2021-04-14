import { Card, Datepicker, Input, Layout, List, Select, SelectItem, Text, Button } from '@ui-kitten/components'
import { CalendarIcon, CrossIcon, EditSignatureIcon } from '../Icons'
import { useFormikContext } from 'formik'
import moment from 'moment'
import React, { useState } from 'react'
import { Image, StyleSheet, View } from 'react-native'
import { Signature } from 'services/api'
import { ScrollView } from 'react-native-gesture-handler'
import { useNavigation, useRoute } from '@react-navigation/native'

const Signatures = () => {
  const navigation = useNavigation()
  const route = useRoute()
  const { setFieldValue, values, errors, handleChange } = useFormikContext()

    console.log('hola mundo', route.params)

  return (
    <>
      <ScrollView>
        {values.signatures.map((item, signIndex) => {
          return (<Card key={signIndex} style={styles.card}>
            <Text category='s1'>
              {item.title}
            </Text>
            <Text status='warning' category='s2'>
              {item.principal && !item.drawedSign ? 'This Signature is required. Please complete and sign' : ''}
            </Text>
            <Datepicker
              label='Date'
              placeholder='Pick Date'
              date={typeof item.date === 'string' ? new Date() : item.date}
              max={new Date()}
              onSelect={(e) => setFieldValue(`signatures[${signIndex}].date`, e)}
              accessoryRight={CalendarIcon}
            />
            <Select
              placeholder='please select...'
              value={item.responsable.type}
              label='Representation Type'
              onSelect={(e) => { setFieldValue(`signatures[${signIndex}].responsable.type`, ['Supervisor', 'Inspector', 'Witness', 'LEW', 'Other'][e.row]) }}
              status={errors.responsable ? 'danger' : 'basic'}
              caption={errors.responsable}
            >
              {['Supervisor', 'Inspector', 'Witness', 'LEW', 'Other'].map((responsible, index) =>
                <SelectItem
                  key={index}
                  title={responsible}
                ></SelectItem>)}
            </Select>
            <Input label='Name' value={item.responsible} />
            <Input label='Designation' value={item.designation} />
            <Input label='Remarks' multiline value={item.remarks} onChangeText={handleChange('remarks')} />
            <View style={{ flex: 2, flexDirection: 'row' }}>
              {item.drawedSign?.length > 0 && <Image style={{ flex: 1, alignSelf: 'center', borderColor: 'black', width: 150, height: 100, resizeMode: 'stretch' }} source={{ uri: item.drawedSign }} />}
              <View style={{ flex: 1, flexDirection: 'row' }}>
                <Button style={{ flex: 1, margin: 10, marginTop: 20 }} status='warning' size='small' appearance='outline' onPress={() => navigation.navigate('ModalSignatures')} accessoryLeft={EditSignatureIcon} />
                <Button disabled={!item.drawedSign} style={{ flex: 1, margin: 10, marginTop: 20 }} status='danger' size='small' appearance='outline' accessoryLeft={CrossIcon} />
              </View>
            </View>
          </Card>
          )
        }
        )
        }
      </ScrollView>
      
    </>
  )
}

export { Signatures }

const styles = StyleSheet.create({
  card: { padding: 0, marginHorizontal: 5, marginVertical: 2 },
})
