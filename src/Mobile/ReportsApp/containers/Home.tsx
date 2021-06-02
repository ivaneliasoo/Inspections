import {useContext} from 'react';
import { Card, Icon, Layout, Text } from '@ui-kitten/components'
import { NewReport } from '../components/home/NewReport'
import React, { useState } from 'react'
import { StyleSheet } from 'react-native'
import { AuthContext } from '../contexts/AuthContext';

export const Home = ({ navigation }: any) => {
  const [isCreatingReport, setIsCreatingReport] = useState(false)
  const { authState } = useContext(AuthContext)

  const navigateToDetails = (reportId: any) => {
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
      name: 'pending',
      text: 'Edit/View Pending Reports',
      helpText: '',
      icon: 'edit-outline',
      color: 'accent',
      path: 'MyReports'
    },
    {
      name: 'completed',
      text: 'View Completed Reports',
      helpText: '',
      icon: 'edit-outline',
      color: 'accent',
      path: 'MyReports'
    }
  ]


  return (
    <Layout>
      <Text category="h4">Welcome, {authState.userInfo.lastName} {authState.userInfo.name}.</Text>
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
