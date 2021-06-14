import Picker from '@gregfrench/react-native-wheel-picker'
import { CheckBox, Divider, Input, Layout, Select, SelectItem, Text } from '@ui-kitten/components'
import NumericPicker from '../NumericPicker'
import { Formik, FormikProps } from 'formik'
import React, { useContext, useMemo, useRef, useState } from 'react'
import { ScrollView, StyleSheet, View } from 'react-native'
import { Report } from 'services/api'
import { ReportsContext } from '../../contexts/ReportsContext';

// type OperationalReadingsProps = {
//   reportData: Report
// }
const OperationalReading = () => {

  const { reportsState: { workingReport: reportData } } = useContext(ReportsContext)
  const formRef = useRef<FormikProps<Report>>(null)


  const [testNumber, setTestNumber] = useState<number[]>([])

  return (
    <ScrollView style={{backgroundColor: 'white'}}>
      <Formik ref={formRef} initialValues={reportData!} enableReinitialize onSubmit={() => console.log('saved')}>
        <View>
          <Text category='h4'>Operational Readings</Text>
          <Text category='h6'>Voltage</Text>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: 'space-evenly', alignContent: 'center', flexDirection: 'row' }}>
            <NumericPicker defaultValue={[2, 3, 0]} preppendLabel="L1-N2" itemSelected={value => setTestNumber(value)} />
            <NumericPicker defaultValue={[2, 3, 0]} preppendLabel="L1-N3" itemSelected={value => setTestNumber(value)} />
            <NumericPicker defaultValue={[2, 3, 0]} preppendLabel="L2-N3" itemSelected={value => setTestNumber(value)} />
          </View>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: 'space-evenly', alignContent: 'center', flexDirection: 'row' }}>
            <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="L1-L2" itemSelected={value => setTestNumber(value)} />
            <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="L1-L3" itemSelected={value => setTestNumber(value)} />
            <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="L2-L3" itemSelected={value => setTestNumber(value)} />
          </View>
          <Divider />
          <Text category='h6'>Running Load</Text>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: 'space-evenly', alignContent: 'center', flexDirection: 'row' }}>
            <NumericPicker defaultValue={[0, 1, 0]} preppendLabel="L1" appendLabel="A" itemSelected={value => setTestNumber(value)} />
            <NumericPicker defaultValue={[0, 1, 0]} preppendLabel="L1" appendLabel="A" itemSelected={value => setTestNumber(value)} />
            <NumericPicker defaultValue={[0, 1, 0]} preppendLabel="L2" appendLabel="A" itemSelected={value => setTestNumber(value)} />
          </View>
          <Divider />
          <Text category='h6'>Main Braker Details</Text>
          <View style={{ flexDirection: 'row', margin: 5, alignContent: 'center', justifyContent: 'center' }}>
            <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="Main Breaker" appendLabel="A" itemSelected={value => setTestNumber(value)} />
            <Select
              style={{ flex: 1, margin: 5 }}
              size='large'
              placeholder='please select...'
              label='Braking Capacity'
              accessoryRight={() => <Text>Poles</Text>}
            >
              {['2', '3', '4'].map((responsible, index) =>
                <SelectItem
                  key={index}
                  title={responsible}
                ></SelectItem>)}
            </Select>
            <Select
              style={{ flex: 1, margin: 5 }}
              size='large'
              placeholder='please select...'
              label='Poles'
              accessoryRight={() => <Text style={{ justifyContent: 'center' }}>kA</Text>}
            >
              {['100', '200', '300', '400'].map((responsible, index) =>
                <SelectItem
                  key={index}
                  title={responsible}
                ></SelectItem>)}
            </Select>
          </View>
          <Divider />
          <Text style={{ flex: 1, margin: 5 }} category='h6'>Over current</Text>
          <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' }}>
            <View >
              <CheckBox style={{ flex: 1, margin: 5 }} checked={true}>By Main Breaker</CheckBox>
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>DTL</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>@</Text>}
                accessoryRight={() => <Text>sec</Text>} />
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>IDTML</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>@</Text>}
                accessoryRight={() => <Text>Tm</Text>} />
            </View>
          </View>
          <Divider />
          <Text style={{ flex: 1, margin: 5 }} category='h6'>Earth Fault</Text>
          <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' }}>
            <View style={{ width: 250 }}>
              <Select
                size='large'
                placeholder='please select...'
                label='RccB'
                accessoryRight={() => <Text style={{ justifyContent: 'center' }}>mA</Text>}
              >
                {['10', '30', '100', '300'].map((responsible, index) =>
                  <SelectItem
                    key={index}
                    title={responsible}
                  ></SelectItem>)}
              </Select>
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>ElR</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>@</Text>}
                accessoryRight={() => <Text>sec</Text>} />
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>E/F</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400'
                accessoryLeft={() => <Text>@</Text>}
                accessoryRight={() => <Text>sec</Text>} />
            </View>
          </View>
        </View>
      </Formik>
    </ScrollView>
  )
}

export { OperationalReading }

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 10,
    justifyContent: 'space-between'
  }
})
