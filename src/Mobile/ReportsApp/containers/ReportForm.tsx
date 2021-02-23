import React from 'react'
import { Layout, Text } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'

const ReportForm = () => {
  return (
    <Layout style={styles.container}>
      <Text>Report Principal form fields</Text>
    </Layout>
  )
}

export {ReportForm}

const styles = StyleSheet.create({
  container: {
    height: 100,
    alignItems: 'center',
    justifyContent: 'center'
  }
})
