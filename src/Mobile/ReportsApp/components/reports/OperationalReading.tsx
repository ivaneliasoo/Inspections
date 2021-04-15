import { CheckBox, Divider, Input, Layout, Select, SelectItem, Text } from '@ui-kitten/components'
import { Formik, FormikProps } from 'formik'
import React, { useMemo, useRef, useState } from 'react'
import { ScrollView, StyleSheet, View } from 'react-native'
import { Report } from 'services/api'



const OperationalReading = ({ reportData }) => {
  const formRef = useRef<FormikProps<Report>>(null)
  const [amp, setAmp] = useState(20)
  const [volt, setVolt] = useState(220)

  const voltageMoreThen400 = useMemo(() => {

  }, [reportData])

  return (
    <Formik ref={formRef} initialValues={reportData} enableReinitialize onSubmit={() => console.log('saved')}>
      <ScrollView>
        <Layout style={styles.container}>
          <Text category='h2' style={{ alignSelf: 'center', marginBottom: 20 }}>Operational Reading Page</Text>
          <Text category='h4'>Particular of Installation: </Text>
          <View>
            <Text category='h6'>Name: </Text>
            <Input size='large' value={reportData.name} />
            <Text category='h6'>Address: </Text>
            <Input size='large' value='Test Address' />
          </View>
          <View style={{ flexDirection: 'row', justifyContent: 'space-evenly', alignItems: 'center' }}>
            <Text category='h6'>Approved Load: </Text>
            <Input size='large' value='40' accessoryRight={() => <Text>A</Text>} />
            <Input size='large' value='200' accessoryRight={() => <Text>V</Text>} />
          </View>
          <Divider />
          <Text category='h4'>Operational Readings</Text>
          <Text category='h6'>Voltage</Text>
          <View style={{ margin: 5, justifyContent: 'space-evenly', alignContent: 'center', flexDirection: 'row' }}>
            <Input style={{ flex: 1, margin: 5 }} size='large' label='L1-N' value='230' accessoryRight={() => <Text>V</Text>} />
            <Input style={{ flex: 1, margin: 5 }} label='L2-N' size='large' value='230' accessoryRight={() => <Text>V</Text>} />
            <Input style={{ flex: 1, margin: 5 }} label='L3-N' size='large' value='230' accessoryRight={() => <Text>V</Text>} />
          </View>
          <View style={{ margin: 5, justifyContent: 'space-evenly', alignContent: 'center', flexDirection: 'row' }}>
            <Input style={{ flex: 1, margin: 5 }} label='L1-L2' size='large' value='400' accessoryRight={() => <Text>V</Text>} />
            <Input style={{ flex: 1, margin: 5 }} label='L1-L3' size='large' value='400' accessoryRight={() => <Text>V</Text>} />
            <Input style={{ flex: 1, margin: 5 }} label='L2-L3' size='large' value='400' accessoryRight={() => <Text>V</Text>} />
          </View>
          <Divider />
          <Text category='h6'>Running Load</Text>
          <View style={{ flexDirection: 'row', alignContent: 'center', margin: 5 }}>
            <Input style={{ flex: 1, margin: 5 }} label='L1' size='large' value='010' accessoryRight={() => <Text>A</Text>} />
            <Input style={{ flex: 1, margin: 5 }} label='L1' size='large' value='010' accessoryRight={() => <Text>A</Text>} />
            <Input style={{ flex: 1, margin: 5 }} label='L2' size='large' value='010' accessoryRight={() => <Text>A</Text>} />
          </View>
          <Divider />
          <Text category='h6'>Main Braker Details</Text>
          <View style={{ flexDirection: 'row', margin: 5, alignContent: 'center', justifyContent: 'center' }}>
            <Input style={{ flex: 1, margin: 5 }} label='Main Breaker' size='large' value='400' accessoryRight={() => <Text>A</Text>} />
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
          <View style={{ flexDirection: 'row', justifyContent: 'space-evenly', alignItems: 'center' }}>
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
          <View style={{ flexDirection: 'row', justifyContent: 'space-evenly', alignItems: 'center' }}>
            <View>
              <Input style={{ flex: 1, margin: 5 }} size='large' value='400' accessoryRight={() => <Text>V</Text>} />
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
        </Layout>
      </ScrollView>
    </Formik>
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
