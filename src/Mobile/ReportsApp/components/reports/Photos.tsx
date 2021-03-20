import React from 'react'
import { Layout, Text } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'

const Photos = () => {
  return (
    <Layout style={styles.container}>
      <Text>PhotoRecord and camera Page</Text>
    </Layout>
  )
}

export default Photos

const styles = StyleSheet.create({
  container: {
    height: 100,
    alignItems: 'center',
    justifyContent: 'center'
  }
})
