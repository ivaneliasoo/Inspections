import { CheckBox, Divider, Input, Layout, Select, SelectItem, Text } from '@ui-kitten/components'
import NumericPicker from '../NumericPicker'
import { Formik, FormikProps } from 'formik'
import React, { useContext, useEffect, useMemo, useRef, useState } from 'react'
import { ScrollView, StyleSheet, View } from 'react-native'
import { Report } from 'services/api'
import { ReportsContext } from '../../contexts/ReportsContext';
import { ParticullarOfInstallation } from './ParticullarOfInstallation'
import { AutoSave } from '../../components/AutoSave'

const OperationalReading = () => {

  const { reportsState: { workingReport: reportData, workingOperationalReadings } } = useContext(ReportsContext)
  const formRef = useRef<FormikProps<any>>(null)

  useEffect(() => {
    console.log({ workingOperationalReadings })
  }, [])

  const isSingleLine = useMemo(() => {
    return (reportData?.license?.volt <= 230);
  }, [reportData?.license?.volt])

  const isMultiLine = useMemo(() => {
    return (reportData?.license?.volt === 400);
  }, [reportData?.license?.volt])

  return <ScrollView style={{ backgroundColor: 'white', flex: 1 }}>
    <ParticullarOfInstallation />
    <Formik innerRef={formRef} initialValues={workingOperationalReadings!} enableReinitialize onSubmit={() => console.log('saved')}>
      {({ values, setFieldValue }) => (
        <View style={styles.container}>
          <View style={{ alignSelf: 'center' }}>
            <AutoSave debounceMs={300} />
          </View>
          <Text category='h6' style={{ fontWeight: '900' }}>Operational Readings</Text>
          <Text category='s1' appearance='hint'>Voltage</Text>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: !isSingleLine ? 'space-evenly' : 'flex-start', alignContent: 'flex-start', flexDirection: 'row' }}>
            {(isSingleLine || isMultiLine) && <NumericPicker defaultValue={[2, 3, 0]} preppendLabel="L1-N2" itemSelected={value => setFieldValue('L1N2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={[2, 3, 0]} preppendLabel="L2-N" itemSelected={value => setFieldValue('L1N2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={[2, 3, 0]} preppendLabel="L3-N" itemSelected={value => setFieldValue('L1N2', value)} />}
          </View>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: 'space-evenly', alignContent: 'center', flexDirection: 'row' }}>
            {isMultiLine && <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="L1-L2" itemSelected={value => setFieldValue('L1N2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="L1-L3" itemSelected={value => setFieldValue('L1N2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="L2-L3" itemSelected={value => setFieldValue('L1N2', value)} />}
          </View>
          <Divider />
          <Text category='s1' appearance='hint'>Running Load</Text>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: !isSingleLine ? 'space-evenly' : 'flex-start', alignContent: 'center', flexDirection: 'row' }}>
            {(isSingleLine || isMultiLine) && <NumericPicker defaultValue={[0, 1, 0]} preppendLabel="L1" appendLabel="A" itemSelected={value => setFieldValue('L1N2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={[0, 1, 0]} preppendLabel="L1" appendLabel="A" itemSelected={value => setFieldValue('L1N2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={[0, 1, 0]} preppendLabel="L2" appendLabel="A" itemSelected={value => setFieldValue('L1N2', value)} />}
          </View>
          <Divider />
          <Text category='h6'>Main Braker Details</Text>
          <View style={{ flexDirection: 'row', margin: 5, alignContent: 'center', justifyContent: 'center' }}>
            <NumericPicker defaultValue={[4, 0, 0]} preppendLabel="Main Breaker" appendLabel="A" itemSelected={value => setFieldValue('L1N2', value)} />
            <Select
              style={{ flex: 1, margin: 5 }}
              size='large'
              value={values.mainBreakerCapacity}
              onSelect={(e) => { setFieldValue('mainBreakerCapacity', [2, 3, 4][e.row]) }}
              placeholder='please select...'
              label='Braking Capacity'
              accessoryRight={() => <Text>Poles</Text>}
            >
              {[2, 3, 4].map((responsible, index) =>
                <SelectItem
                  key={index}
                  title={responsible}
                ></SelectItem>)}
            </Select>
            <Select
              style={{ flex: 1, margin: 5 }}
              size='large'
              placeholder='please select...'
              value={values.mainBreakerPoles}
              onSelect={(e) => { setFieldValue('mainBreakerPoles', [100, 200, 300, 400][e.row]) }}
              label='Poles'
              accessoryRight={() => <Text style={{ justifyContent: 'center' }}>kA</Text>}
            >
              {[100, 200, 300, 400].map((responsible, index) =>
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
              <CheckBox 
                style={{ flex: 1, margin: 5 }} 
                checked={values.overCurrentByMainBreaker}
                onChange={value => setFieldValue('overCurrentByMainBreaker', value)}
              >
                By Main Breaker
              </CheckBox>
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value={values.overCurrentDTLA}
                onChange={value => setFieldValue('overCurrentDTLA', value)}
                accessoryLeft={() => <Text>DTL</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large' value={values.overCurrentDTLSec}
                onChange={value => setFieldValue('overCurrentDTLSec', value)}
                accessoryLeft={() => <Text>@</Text>}
                accessoryRight={() => <Text>sec</Text>} />
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large'value={values.overCurrentIDMTLA}
                onChange={value => setFieldValue('overCurrentIDMTLA', value)}
                accessoryLeft={() => <Text>IDTML</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large' value={values.overCurrentIDMTLTm}
                onChange={value => setFieldValue('overCurrentIDMTLTm', value)}
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
                value={values.earthFaultMA}
                onSelect={(e) => { setFieldValue('earthFaultMA', [10, 30, 100, 300][e.row]) }}
                accessoryRight={() => <Text style={{ justifyContent: 'center' }}>mA</Text>}
              >
                {[10, 30, 100, 300].map((responsible, index) =>
                  <SelectItem
                    key={index}
                    title={responsible}
                  ></SelectItem>)}
              </Select>
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value={values.earthFaultELRA}
                onChange={value => setFieldValue('earthFaultELRA', value)}
                accessoryLeft={() => <Text>ElR</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large' value={values.earthFaultELRSec}
                onChange={value => setFieldValue('earthFaultELRSec', value)}
                accessoryLeft={() => <Text>@</Text>}
                accessoryRight={() => <Text>sec</Text>} />
            </View>
            <Text>OR</Text>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value={values.earthFaultA}
                onChange={value => setFieldValue('earthFaultA', value)}
                accessoryLeft={() => <Text>E/F</Text>}
                accessoryRight={() => <Text>A</Text>} />
              <Input style={{ flex: 1, margin: 5 }} size='large'value={values.earthFaultSec}
                onChange={value => setFieldValue('earthFaultSec', value)}
                accessoryLeft={() => <Text>@</Text>}
                accessoryRight={() => <Text>sec</Text>} />
            </View>
          </View>
        </View>
      )
      }
    </Formik>
  </ScrollView>
}

export { OperationalReading }

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 10,
    justifyContent: 'space-between'
  }
})
