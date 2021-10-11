import { useContext } from 'react';
import { Card, Icon, Layout, Text, TopNavigation, TopNavigationAction, useTheme } from '@ui-kitten/components'
import { NewReport } from '../components/home/NewReport'
import React, { useState } from 'react'
import { ScrollView, StyleSheet, TouchableOpacity } from 'react-native'
import { AuthContext } from '../contexts/AuthContext';
import { useReports } from '../hooks/useReports';
import { ReportsState } from '../contexts/ReportsContext';

export const Home = ({ navigation }: any) => {
  const [isCreatingReport, setIsCreatingReport] = useState(false)
  const { authState } = useContext(AuthContext)
  const theme = useTheme()
  const { setOptions, getReports } = useReports()

  const navigateToDetails = (reportId: any) => {
    setIsCreatingReport(false)
    navigation.navigate('Details', { reportId })
  }

  const cardOptions: any[] = [
    {
      name: 'new',
      text: 'Create a New Report',
      helpText: 'Creates a configured and empty report',
      icon: 'plus-outline',
      color: 'accent',
      path: '',
      action: () => setIsCreatingReport(true)
    },
    {
      name: 'pending',
      text: 'Continue existing Report',
      helpText: 'allows editing incompletes reports',
      icon: 'edit-outline',
      color: 'accent',
      path: 'MyReports'
    },
    {
      name: 'completed',
      text: 'View',
      helpText: 'Visualize Completed Reports',
      icon: 'eye-outline',
      color: 'accent',
      path: 'MyReports'
    }
  ]


  return (
    <Layout style={styles.container}>
      <TopNavigation title={`Welcome, ${authState.userInfo?.lastName} ${authState.userInfo?.name}.`} alignment='center' accessoryLeft={() =>
        <TopNavigationAction onPress={() => navigation.openDrawer()} icon={(props) => <Icon name='menu-outline' style={{ width: 50, height: 50 }} {...props} />} />} />
      <NewReport isOpen={isCreatingReport} onClose={() => setIsCreatingReport(false)} onCreate={navigateToDetails} />
      <ScrollView>
        {cardOptions.map(option => {
          return <Card key={option.name} style={styles.card} onPress={() => {
            if (!option.action) {
              setOptions(option.name === 'completed', true)
              navigation.navigate(option.path)
            }
            else option.action.call()
          }}>
            <Text category="h5" style={{ alignSelf: 'center' }}>{option.text}</Text>
            <TouchableOpacity style={[styles.button, { backgroundColor: theme['color-primary-500'], alignSelf: 'center' }]}
              onPress={() => {
                if (!option.action) {
                  setOptions(option.name === 'completed', true)
                  navigation.navigate(option.path)
                } else { option.action.call() }
              }}
            >
              {option.icon && <Icon name={option.icon} fill='#ffffff' style={{ width: 46, height: 46 }} />}
            </TouchableOpacity>
            <Text category="s1" appearance='hint' style={{ alignSelf: 'center', marginTop: 10 }}>{option.helpText}</Text>
          </Card>
        })}
      </ScrollView>
    </Layout>
  )
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  card: {
    padding: 15,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 3,
    alignItems: 'stretch',
    margin: 10,
  },
  button: {
    alignItems: 'center',
    justifyContent: 'center',
    marginVertical: 2,
    margin: 2,
    width: 80,
    height: 80,
    elevation: 10,
    borderRadius: 200
  },
});
