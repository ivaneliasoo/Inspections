import { Card, Icon, Layout, Text } from '@ui-kitten/components'
import { NewReport } from '../components/home/NewReport'
import React, { useState } from 'react'
import { StyleSheet } from 'react-native'

export const Home = ({ navigation }) => {
  const [isCreatingReport, setIsCreatingReport] = useState(false)

  const navigateToDetails = (reportId: any) => {
    console.log({reportId})
    setIsCreatingReport(false)
    navigation.navigate('Details', { reportId })
  }
  
  const cardOptions: any[] = [
    {
      name: 'new',
      text: 'New Report',
      helpText: 'Creates a configured and empty report',
      icon: 'plus-outline',
      color: 'accent',
      path: '',
      action: () => setIsCreatingReport(true)
    },
    {
      name: 'edit',
      text: 'Edit/View Report',
      helpText: 'Click one of the Options bellow',
      icon: 'edit-outline',
      color: 'accent',
      path: 'MyReports'
    }
  ]


  return (
    <Layout>
      <NewReport isOpen={isCreatingReport} onClose={() => setIsCreatingReport(false)} onCreate={navigateToDetails}/>
      {cardOptions.map(option => {
          return <Card key={option.name} style={{alignItems: 'center', borderRadius: 5, margin: 20}} onPress={() => {
            if(!option.action) navigation.navigate(option.path)
            else option.action.call()
          }}>
            <Text category="h1">{option.text}</Text>
            <Text category="s1">{option.helpText}</Text>
            <Icon name={option.icon} style={{width: 48, height: 48}} />
          </Card>
        })}
    </Layout>
  )
}

const styles = StyleSheet.create({})
