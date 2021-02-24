import React, { useState, useEffect } from 'react';
import { ReportsApi } from '../services/api'
import moment from 'moment';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Divider, TopNavigation, Layout, Input,List, Text, Card } from '@ui-kitten/components';
import { ClosedIcon, SearchIcon, NotClosedIcon} from '../components/Icons'
import { OptionsMenu } from '../components/home/OptionsMenu'
import { NewReportMenu } from '../components/home/NewReportMenu'
import { View } from 'react-native';
import { useNavigation } from '@react-navigation/native';

const renderItemFooter = (footerProps, item) => (
  <View {...footerProps}>
    { item.isClosed ? <ClosedIcon fill={'green'} style={{ width: 24, height: 24 }} /> : <NotClosedIcon fill={'orange'} style={{ width: 24, height: 24 }} />}
  </View>
);

const renderReport = ({ navigation, item, index }) => {
  const navigateDetails = () => {
    navigation.navigate('Details', { reportId: item.item.id, title: item.item.title });
  };
  return (
    <Card key={index} style={{ padding: 0, marginHorizontal: 15, marginVertical: 10 }} onPress={navigateDetails} status={item.isClosed ? 'success' : 'warning'}
      footer={footerProps => renderItemFooter(footerProps, item)} >
      <Text category='s1'>{`${moment(item.date).format('DD/MM/YYYY HH:mm')} License ${item.license?.number ?? 'Not specified'}`}</Text>
      <Text category='s2'>{item.address === '' ? 'address not specified' : item.address}</Text>
    </Card>
  );
};

export const HomeScreen = () => {
  const [searchText, setSearchText] = useState('')
  const [reports, setReports] = useState([])
  const [refreshing, setRefreshing] = useState(true)
  const navigation = useNavigation()
  async function getReports() {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: 'http://192.168.88.250:5000', password: undefined, username: undefined, apiKey: 'falskjdufghasjdghfaskjdhgfa' })
    const resp = await reportsApi.reportsGet(searchText)
    setReports(resp.data)
    setRefreshing(false)
  }

  useEffect(() => {
    getReports()
    return () => {

    }
  }, [])

  return (
    <>
      <TopNavigation title={`Reports (total: ${reports.length})`} alignment='center' accessoryRight={OptionsMenu} accessoryLeft={NewReportMenu} />
      <Divider />
      <Layout style={{ flex: 1 }}>
        <Input style={{ paddingHorizontal: 15 }} status="info" accessoryLeft={SearchIcon} value={searchText} onChangeText={setSearchText} onEndEditing={getReports} />
        {reports.length > 0 ? <List data={reports} renderItem={(item, index) => renderReport({ navigation, item, index})} onRefresh={getReports} refreshing={refreshing} /> : <Text>Nothing to see</Text>}
      </Layout>
    </>
  );
};