import { Layout, Text } from '@ui-kitten/components'
import React from 'react'
import { StyleSheet } from 'react-native'

const OperationalReading = () => {
  return (
    <Layout style={styles.container}>
      <Text>Operational Reading Page</Text>
    </Layout>
  )
}

export {OperationalReading}

const styles = StyleSheet.create({
  container: {
    height: 100,
    alignItems: 'center',
    justifyContent: 'center'
  }
})
