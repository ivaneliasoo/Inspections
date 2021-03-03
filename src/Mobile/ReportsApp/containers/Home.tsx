import React, { useState, useEffect, useContext } from 'react';
import { ReportsApi } from '../services/api'
import moment from 'moment';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Divider, TopNavigation, Layout, Input, List, Text, Card, Icon } from '@ui-kitten/components';
import { ClosedIcon, SearchIcon, NotClosedIcon } from '../components/Icons'
import { OptionsMenu } from '../components/home/OptionsMenu'
import { NewReportMenu } from '../components/home/NewReportMenu'
import { useNavigation } from '@react-navigation/native';
import { API_HOST, API_KEY } from '../config/config';
import { ReportsContext } from '../reports-contexts';

const renderItemFooter = (footerProps, item) => (
  <Layout {...footerProps} style={{ flexDirection: 'row', justifyContent: 'space-between', margin: 5 }}>
    { item.item.isClosed ? <ClosedIcon fill={'green'} style={{ width: 24, height: 24 }} /> : <NotClosedIcon fill={'orange'} style={{ width: 24, height: 24 }} />}
    <Layout key={item.name}>
      <Text>Not Synced</Text>
      <Icon name='wifi-off-outline' fill={'red'} style={{ width: 24, height: 24 }} />
    </Layout>
  </Layout>
);

const renderReport = ({ navigation, item, index }) => {
  const navigateDetails = () => {
    navigation.navigate('Details', { reportId: item.item.id, title: item.item.title });
  };
  return (
    <Card key={index} style={{ padding: 0, marginHorizontal: 5, marginVertical: 2 }} onPress={navigateDetails} status={item.item.isClosed ? 'success' : 'warning'}
      footer={footerProps => renderItemFooter(footerProps, item)} >
      <Text category='s1'>{`${moment(item.item.date).format('DD/MM/YYYY HH:mm')} License ${item.item.license?.number ?? 'Not specified'}`}</Text>
      <Text category='s2'>{item.item.address === '' ? 'address not specified' : item.item.address}</Text>
    </Card>
  );
};

export const HomeScreen = () => {
  const [refreshing, setRefreshing] = useState(true)
  const { getAll, reports, filter, myReports, isClosed, setFilter} = useContext<any>(ReportsContext)

  const navigation = useNavigation()
  async function getReports() {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY })
    const resp = await reportsApi.reportsGet(filter, isClosed, myReports )
    getAll(resp.data)
    setRefreshing(false)
  }

  useEffect(() => {
    getReports()
    return () => {
    }
  }, [])

  return (
    <>
      <TopNavigation title={`Reports (total: ${reports.length})`} alignment='center' accessoryRight={() => <OptionsMenu onChanged={() => getReports()}/>} accessoryLeft={NewReportMenu} />
      <Divider />
      <Layout style={{ flex: 1, justifyContent: reports.length > 0 ? 'flex-start' : 'center', alignContent: 'center' }}>
        <Input style={{ paddingHorizontal: 15 }} status="info" accessoryLeft={SearchIcon} value={filter} onChangeText={setFilter} onEndEditing={getReports} />
        {reports.length > 0 ? <List data={reports} renderItem={(item, index) => renderReport({ navigation, item, index })} onRefresh={getReports} refreshing={refreshing} /> : <Text>Nothing to see</Text>}
      </Layout>
    </>
  );
};