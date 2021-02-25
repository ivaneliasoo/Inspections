import React from 'react'
import { Layout, Text } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'

const Checklists = (props) => {
  return (
    <Layout style={styles.container}>
      <Text>Checklists</Text>
    </Layout>
  )
}

export {Checklists}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center'
  }
})
