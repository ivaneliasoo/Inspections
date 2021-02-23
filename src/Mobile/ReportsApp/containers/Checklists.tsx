import React from 'react'
import { Layout, Text } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'

const Checklists = () => {
  return (
    <Layout style={styles.container}>
      <Text>Checklists</Text>
    </Layout>
  )
}

export {Checklists}

const styles = StyleSheet.create({
  container: {
    height: 100,
    alignItems: 'center',
    justifyContent: 'center'
  }
})
