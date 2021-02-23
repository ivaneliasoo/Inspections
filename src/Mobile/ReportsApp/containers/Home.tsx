import React, { useContext, useState, useEffect } from 'react';
import {ReportsApi} from '../services/api'
import { Divider, TopNavigation, Layout, Input, Icon, Button, List, ListItem, Text, Card } from '@ui-kitten/components';
import { ThemeContext } from '../theme-context';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { RefreshControl, TouchableOpacity } from 'react-native';

const SearchIcon = (props) => (<Icon name='search-outline' {...props} />)
const DarkIcon = (props) => (<Icon name='moon-outline' {...props} />)
const LightIcon = (props) => (<Icon name='sun-outline' {...props} />)


const ThemeToggle = () => {
  const context = useContext(ThemeContext)
  return (
      <Button appearance='ghost' onPress={context.toggleTheme} accessoryRight={context.theme === 'dark' ? DarkIcon:LightIcon} />
  )
}

const renderReport = ({item, index}) => {
  return <Card style={{padding: 0, marginHorizontal: 15, marginVertical: 10}} status={ item.isClosed ? 'success' : 'warning'}><ListItem title={`${item.name}`} description={`${item.address}`} /></Card>
};

export const HomeScreen = () => {
  const [searchText, setSearchText] = useState('')
  const [onlyMyReports, setOnlyMyReports] = useState(true)
  const [reports, setReports] = useState([])

  useEffect(() => {
    async function getReports() {
      const userToken: string = await AsyncStorage.getItem('userToken') as string;
      const reportsApi =  new ReportsApi({ accessToken: userToken, basePath: 'http://192.168.88.250:5000', password: undefined, username: undefined, apiKey: 'falskjdufghasjdghfaskjdhgfa' })
      const resp = await reportsApi.reportsGet(searchText)
      setReports(resp.data)
    }
    getReports()
    return () => {
      
    }
  }, [searchText])

  return (
    <>
      <TopNavigation title='Report Detail' alignment='center' accessoryRight={ThemeToggle} />
      <Divider />
      <Layout>
        <Input  status="info" accessoryLeft={SearchIcon} />
        <Text>{reports.length}</Text>
        { reports.length> 0 ? <List data={reports} renderItem={renderReport} refreshControl={<RefreshControl refreshing={true}/>} /> : <Text>Nothing to see</Text> }
      </Layout>
    </>
  );
};