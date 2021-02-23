import React from 'react'
import { Layout, Text } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'

const Signatures = () => {
  return (
    <Layout style={styles.container}>
      <Text>Report Sugnatures</Text>
    </Layout>
  )
}

export default Signatures

const styles = StyleSheet.create({
  container: {
    height: 100,
    alignItems: 'center',
    justifyContent: 'center'
  }
})
