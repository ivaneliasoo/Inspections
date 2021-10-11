import { CheckBox, Divider, Input, Select, SelectItem, SelectProps, Text, IndexPath } from '@ui-kitten/components';
import NumericPicker from '../NumericPicker'
import NumericPicker2 from '../NumericPicker2';
import { Formik, FormikProps } from 'formik'
import React, { useMemo, useRef } from 'react'
import { Alert, ScrollView, StyleSheet, View } from 'react-native'
import { ReportQueryResult } from '../../services/api'
import { AutoSave } from '../../components/AutoSave'
import { UpdateOperationalReadingsCommand } from '../../services/api/api';
import { useReports } from '../../hooks/useReports';
import { showMessage } from 'react-native-flash-message';

const formatPickerValue = (value: number) => {
  if (!value) return [0, 0, 0]
  const temp = value.toString().padStart(3, '0')
  return [parseInt(temp[0]), parseInt(temp[1]), parseInt(temp[2])]
}



const OperationalReading = () => {

  const { workingReport: reportData, saveOperationalreadings } = useReports()
  const formRef = useRef<FormikProps<ReportQueryResult>>(null)

  const saveReadings = async () => {
    if (formRef.current) {
      if (formRef.current.isValid && formRef.current.dirty) {
        const updateCmd: UpdateOperationalReadingsCommand = {
          id: formRef.current.values.operationalReadingsId!,
          reportId: formRef.current.values.id!,
          voltageL1N: formRef.current.values.operationalReadingsVoltageL1N!,
          voltageL2N: formRef.current.values.operationalReadingsVoltageL2N!,
          voltageL3N: formRef.current.values.operationalReadingsVoltageL3N!,
          voltageL1L2: formRef.current.values.operationalReadingsVoltageL1L2!,
          voltageL1L3: formRef.current.values.operationalReadingsVoltageL1L3!,
          voltageL2L3: formRef.current.values.operationalReadingsVoltageL2L3!,
          runningLoadL1: formRef.current.values.operationalReadingsRunningLoadL1!,
          runningLoadL2: formRef.current.values.operationalReadingsRunningLoadL2!,
          runningLoadL3: formRef.current.values.operationalReadingsRunningLoadL3!,
          mainBreakerAmp: formRef.current.values.operationalReadingsMainBreakerAmp!,
          mainBreakerPoles: formRef.current.values.operationalReadingsMainBreakerPoles!,
          mainBreakerCapacity: formRef.current.values.operationalReadingsMainBreakerCapacity!,
          overCurrentDTLA: formRef.current.values.operationalReadingsOverCurrentDTLA!,
          overCurrentDTLSec: formRef.current.values.operationalReadingsOverCurrentDTLSec!,
          overCurrentIDMTLA: formRef.current.values.operationalReadingsOverCurrentIDMTLA!,
          overCurrentIDMTLTm: formRef.current.values.operationalReadingsOverCurrentIDMTLTm!,
          earthFaultMA: formRef.current.values.operationalReadingsEarthFaultMA!,
          earthFaultELRA: formRef.current.values.operationalReadingsEarthFaultELRA!,
          earthFaultELRSec: formRef.current.values.operationalReadingsEarthFaultELRSec!,
          earthFaultA: formRef.current.values.operationalReadingsEarthFaultA!,
          earthFaultSec: formRef.current.values.operationalReadingsEarthFaultSec!,
        }
        await saveOperationalreadings(updateCmd)
          .catch(error => {
            showMessage(error.response.message)
          })
      } else {
        Alert.alert('Bad Request', `report contains invalid fields: ${Object.keys(formRef.current.errors).map(field => field)}`)
      }
    }
  }

  const isSingleLine = useMemo(() => {
    return (reportData?.licenseVolt! <= 230);
  }, [reportData?.licenseVolt])

  const isMultiLine = useMemo(() => {
    return (reportData?.licenseVolt === 400);
  }, [reportData?.licenseVolt])

  return <ScrollView style={{ backgroundColor: 'white', flex: 1 }}>
    {/* <ParticullarOfInstallation /> */}
    <Formik innerRef={formRef} initialValues={reportData!} enableReinitialize onSubmit={saveReadings}>
      {({ values, setFieldValue }) => (
        <View style={styles.container}>
          <View style={{ alignSelf: 'center' }}>
            <AutoSave debounceMs={600} />
          </View>
          <Text category='h6' style={{ fontWeight: '900' }}>Operational Readings</Text>
          <Text category='s1' appearance='hint'>Voltage</Text>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: !isSingleLine ? 'space-evenly' : 'flex-start', alignContent: 'flex-start', flexDirection: 'row', flexWrap: 'wrap' }}>
            {(isSingleLine || isMultiLine) && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsVoltageL1N!)} preppendLabel="L1-N2" itemSelected={value => setFieldValue('operationalReadingsVoltageL1N', value)} />}
            {isMultiLine && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsVoltageL2N!)} preppendLabel="L2-N" itemSelected={value => setFieldValue('operationalReadingsVoltageL2N', value)} />}
            {isMultiLine && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsVoltageL3N!)} preppendLabel="L3-N" itemSelected={value => setFieldValue('operationalReadingsVoltageL3N', value)} />}
          </View>
          <View style={{ margin: 5, marginVertical: 20, justifyContent: 'space-evenly', alignContent: 'center', flexDirection: 'row', flexWrap: 'wrap' }}>
            {isMultiLine && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsVoltageL1L2!)} preppendLabel="L1-L2" itemSelected={value => setFieldValue('operationalReadingsVoltageL1L2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsVoltageL1L3!)} preppendLabel="L1-L3" itemSelected={value => setFieldValue('operationalReadingsVoltageL1L3', value)} />}
            {isMultiLine && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsVoltageL2L3!)} preppendLabel="L2-L3" itemSelected={value => setFieldValue('operationalReadingsVoltageL2L3', value)} />}
          </View>
          <Divider />
          <Text category='s1' appearance='hint'>Running Load</Text>
          <View style={{ marginVertical: 20, justifyContent: !isSingleLine ? 'space-evenly' : 'flex-start', alignContent: 'center', flexDirection: 'row', flexWrap: 'wrap' }}>
            {(isSingleLine || isMultiLine) && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsRunningLoadL1!)} preppendLabel="L1" appendLabel="A" itemSelected={value => setFieldValue('operationalReadingsRunningLoadL1', value)} />}
            {isMultiLine && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsRunningLoadL2!)} preppendLabel="L2" appendLabel="A" itemSelected={value => setFieldValue('operationalReadingsRunningLoadL2', value)} />}
            {isMultiLine && <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsRunningLoadL3!)} preppendLabel="L3" appendLabel="A" itemSelected={value => setFieldValue('operationalReadingsRunningLoadL3', value)} />}
          </View>
          <Divider />
          <Text category='h6'>Main Breaker Details</Text>
          <View style={{ flexDirection: 'row', margin: 5, alignContent: 'center', justifyContent: 'space-between', flexWrap: 'wrap' }}>
            <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsMainBreakerAmp!)} preppendLabel="Main Breaker" appendLabel="A" itemSelected={value => setFieldValue('operationalReadingsMainBreakerAmp', value)} />
            <NumericPicker2 defaultValue={formatPickerValue(values.operationalReadingsMainBreakerRating!)} preppendLabel="Rating" appendLabel="A" itemSelected={value => setFieldValue('operationalReadingsMainBreakerRating', value)} />
            <Select
              style={{ margin: 5 }}
              size='large'
              value={values.operationalReadingsMainBreakerCapacity!}
              onSelect={(e) => { setFieldValue('operationalReadingsMainBreakerCapacity', [2, 3, 4][(e as IndexPath).row]) }}
              placeholder='please select...'
              label='Poles Capacity'
              accessoryRight={() => <Text>Poles</Text>}
            >
              {[2, 3, 4].map((responsible, index) =>
                <SelectItem
                  key={index}
                  title={responsible}
                ></SelectItem>)}
            </Select>
            <NumericPicker defaultValue={formatPickerValue(values.operationalReadingsMainBreakerPoles!)} preppendLabel="Breaking Capacity (lsc)" appendLabel="A" itemSelected={value => setFieldValue('operationalReadingsMainBreakerPoles', value)} />
          </View>
          <Divider />
          <Text style={{ flex: 1, margin: 5 }} category='h6'>Over current</Text>
          <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', flexWrap: 'wrap' }}>
            <View style={{ flexDirection: 'row', flexWrap: 'wrap' }}>
              <CheckBox
                style={{ flex: 1, margin: 5 }}
                checked={values.operationalReadingsOverCurrentDirectActingEnabled!}
                onChange={value => setFieldValue('operationalReadingsOverCurrentDirectActingEnabled', value)}
              >
                Direct Acting
              </CheckBox>
              {values.operationalReadingsOverCurrentDirectActingEnabled ? <NumericPicker2 defaultValue={formatPickerValue(values.operationalReadingsOverCurrentDirectActing!)} preppendLabel="" appendLabel="A" itemSelected={value => setFieldValue('operationalReadingsRunningLoadL3', value)} /> : null}
            </View>
            <View style={{ flexDirection: 'row' }}>
              <CheckBox
                style={{ flex: 1, margin: 5 }}
                checked={values.operationalReadingsOverCurrentDTLEnabled!}
                onChange={value => setFieldValue('operationalReadingsOverCurrentDTLEnabled', value)}
              >DTL</CheckBox>
              {values.operationalReadingsOverCurrentDTLEnabled ? <><Input style={{ flex: 1, margin: 5 }} size='large' value={values?.operationalReadingsOverCurrentDTLA?.toString() ?? ''}
                onChangeText={value => setFieldValue('operationalReadingsOverCurrentDTLA', value)}
                keyboardType='number-pad'
                accessoryLeft={() => <Text>DTL</Text>}
                accessoryRight={() => <Text>A</Text>} /><Input style={{ flex: 1, margin: 5 }} size='large' value={values?.operationalReadingsOverCurrentDTLSec?.toString() ?? ''}
                  onChangeText={value => setFieldValue('operationalReadingsOverCurrentDTLSec', value)}
                  keyboardType='number-pad'
                  accessoryLeft={() => <Text>@</Text>}
                  accessoryRight={() => <Text>sec</Text>} /></> : null}
            </View>
            <View style={{ flexDirection: 'row' }}>
              <CheckBox
                style={{ flex: 1, margin: 5 }}
                checked={values.operationalReadingsOverCurrentIDTMLEnabled!}
                onChange={value => setFieldValue('operationalReadingsOverCurrentIDTMLEnabled', value)}
              >IDTML</CheckBox>
              {values.operationalReadingsOverCurrentIDTMLEnabled ? <><Input style={{ flex: 1, margin: 5 }} size='large' value={values?.operationalReadingsOverCurrentIDMTLA?.toString() ?? ''}
                onChangeText={value => setFieldValue('operationalReadingsOverCurrentIDMTLA', value)}
                keyboardType='number-pad'
                accessoryLeft={() => <Text>IDTML</Text>}
                accessoryRight={() => <Text>A</Text>} /><Input style={{ flex: 1, margin: 5 }} size='large' value={values?.operationalReadingsOverCurrentIDMTLTm?.toString() ?? ''}
                  onChangeText={value => setFieldValue('operationalReadingsOverCurrentIDMTLTm', value)}
                  keyboardType='number-pad'
                  accessoryLeft={() => <Text>@</Text>}
                  accessoryRight={() => <Text>Tm</Text>} /></> : null}
            </View>
          </View>
          <Divider />
          <Text style={{ flex: 1, margin: 5 }} category='h6'>Earth Fault</Text>
          <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', flexWrap: 'wrap' }}>
            <View style={{ flexDirection: 'row', justifyContent: 'space-between' }}>
              <CheckBox
                style={{ flex: 1, margin: 5 }}
                checked={values.operationalReadingsEarthFaultRoobEnabled!}
                onChange={value => setFieldValue('operationalReadingsEarthFaultRoobEnabled', value)}
              >RccB</CheckBox>
              {values.operationalReadingsEarthFaultRoobEnabled ? <Select
                size='large'
                placeholder='please select...'
                label='RccB'
                value={values.operationalReadingsEarthFaultMA!}
                onSelect={(e) => { setFieldValue('operationalReadingsEarthFaultMA', [10, 30, 100, 300][(e as IndexPath).row]) }}
                accessoryRight={() => <Text style={{ justifyContent: 'center' }}>mA</Text>}
              >
                {[10, 30, 100, 300].map((responsible, index) =>
                  <SelectItem
                    key={index}
                    title={responsible}
                  ></SelectItem>)}
              </Select> : null}
            </View>
            <View style={{ flexDirection: 'row' }}>
              <CheckBox
                style={{ flex: 1, margin: 5 }}
                checked={values.operationalReadingsEarthFaultEIREnabled!}
                onChange={value => setFieldValue('operationalReadingsEarthFaultEIREnabled', value)}
              >EIR</CheckBox>
              {values.operationalReadingsEarthFaultEIREnabled ? <><Input style={{ flex: 1, margin: 5 }} size='large' value={values.operationalReadingsEarthFaultELRA?.toString() ?? ''}
                onChangeText={value => setFieldValue('operationalReadingsEarthFaultELRA', value)}
                keyboardType='number-pad'
                accessoryLeft={() => <Text>ElR</Text>}
                accessoryRight={() => <Text>A</Text>} /><Input style={{ flex: 1, margin: 5 }} size='large' value={values.operationalReadingsEarthFaultELRSec?.toString() ?? ''}
                  onChangeText={value => setFieldValue('operationalReadingsEarthFaultELRSec', value)}
                  keyboardType='number-pad'
                  accessoryLeft={() => <Text>@</Text>}
                  accessoryRight={() => <Text>sec</Text>} /></> : null}
            </View>
            <View style={{ flexDirection: 'row' }}>
              <CheckBox
                style={{ flex: 1, margin: 5 }}
                checked={values.operationalReadingsEarthFaultEFEnabled!}
                onChange={value => setFieldValue('operationalReadingsEarthFaultEFEnabled', value)}
              >EIR</CheckBox>
              {values.operationalReadingsEarthFaultEFEnabled ? <><Input style={{ flex: 1, margin: 5 }} size='large' value={values.operationalReadingsEarthFaultA?.toString() ?? ''}
                onChangeText={value => setFieldValue('operationalReadingsEarthFaultA', value)}
                keyboardType='number-pad'
                accessoryLeft={() => <Text>E/F</Text>}
                accessoryRight={() => <Text>A</Text>} /><Input style={{ flex: 1, margin: 5 }} size='large' value={values.operationalReadingsEarthFaultSec?.toString() ?? ''}
                  onChangeText={value => setFieldValue('operationalReadingsEarthFaultSec', value)}
                  keyboardType='number-pad'
                  accessoryLeft={() => <Text>@</Text>}
                  accessoryRight={() => <Text>sec</Text>} /></> : null}
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
    justifyContent: 'space-between',
  }
})
