import React, { useState, useEffect, useContext, useRef } from 'react';
import { ReportsApi } from '../services/api'
import moment from 'moment';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Animated, Alert } from 'react-native';
import { Icon, Divider, TopNavigation, Layout, Input, List, Text, Card } from '@ui-kitten/components';
import { ClosedIcon, SearchIcon, NotClosedIcon } from '../components/Icons'
import { OptionsMenu } from '../components/home/OptionsMenu'
import { NewReportMenu } from '../components/home/NewReportMenu'
import { useNavigation } from '@react-navigation/native';
import { API_HOST, API_KEY } from '../config/config';
import { ReportsContext } from '../reports-contexts';
import { RectButton, Swipeable } from 'react-native-gesture-handler';

const AnimatedIcon = Animated.createAnimatedComponent(Icon);

const renderLeftActions = (progress, dragX) => {
  return (
    <RectButton style={{
      backgroundColor: 'green',
      flex: 1,
      justifyContent: 'center',
      alignContent: 'stretch',
    }}>
      <Animated.Text style={[
        {
          padding: 0,
          marginHorizontal: 5,
          marginVertical: 2,
          color: 'white',
          fontSize: 24,
          textAlign: 'left'
        }
      ]}>
        Complete
      </Animated.Text>
    </RectButton>
  )
}

const renderRightActions = (progress, dragX) => {
  const trans = dragX.interpolate({
    inputRange: [-80, 0],
    outputRange: [1, 0],
    extrapolate: 'clamp',
  })

  return (
    <RectButton style={{
      backgroundColor: 'red',
      justifyContent: 'center',
      flex: 1,
      alignContent: 'stretch',
    }}>
      <Animated.Text
        style={[
          {
            transform: [{ translateX: trans }],
            padding: 0,
            marginHorizontal: 5,
            marginVertical: 2,
            color: 'white',
            fontSize: 24,
            textAlign: 'right'
          }
        ]}
      >
        Delete
      </Animated.Text>
    </RectButton>
  )
}

const renderItemFooter = (footerProps, item) => (
  <Layout {...footerProps} style={{ flexDirection: 'row', justifyContent: 'space-between', margin: 5 }}>
    { item.item.isClosed ? <ClosedIcon fill={'green'} style={{ width: 24, height: 24 }} /> : <NotClosedIcon fill={'orange'} style={{ width: 24, height: 24 }} />}
    <Layout key={item.name}>
      <Text>Not Synced</Text>
      <Icon name='wifi-off-outline' fill={'red'} style={{ width: 24, height: 24 }} />
    </Layout>
  </Layout>
);



export const HomeScreen = () => {
  const [refreshing, setRefreshing] = useState(true)
  const { getAll, reports, filter, myReports, isClosed, setFilter } = useContext<any>(ReportsContext)
  const swipe = useRef<Swipeable>(null)

  const navigation = useNavigation()
  async function getReports() {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY })
    const resp = await reportsApi.reportsGet(filter, isClosed, myReports)
    getAll(resp.data)
    setRefreshing(false)
  }

  useEffect(() => {
    getReports()
    return () => {
    }
  }, [])

  const renderReport = ({ navigation, item, index }) => {
    const navigateDetails = () => {
      navigation.navigate('Details', { reportId: item.item.id, title: item.item.title });
    };
    return (
      <Swipeable ref={swipe} friction={2} onSwipeableLeftOpen={() => {
        if (item.item.isClosed) return;
        Alert.alert('Complete / Close Report', `You are about to complete the report ${item.item.name} (${item.item.id}). Are you sure?`,
          [
            {
              text: 'Yes',
              onPress: () => { swipe.current?.close() }
            },
            {
              text: 'No',
              onPress: () => console.log('canceled')
            }
          ]
        ); 
      }} renderLeftActions={ !item.item.isClosed ? renderLeftActions : () => null} renderRightActions={!item.item.isClosed ? renderRightActions : () => null}>

        <Card key={index} style={{ padding: 0, marginHorizontal: 5, marginVertical: 2 }} onPress={navigateDetails} status={item.item.isClosed ? 'success' : 'warning'}
          footer={footerProps => renderItemFooter(footerProps, item)} >
          <Text category='s1'>{`${moment(item.item.date).format('DD/MM/YYYY HH:mm')} License ${item.item.license?.number ?? 'Not specified'}`}</Text>
          <Text category='s2'>{item.item.address === '' ? 'address not specified' : item.item.address}</Text>
          <Text category='c1'>{ API_HOST }</Text>
        </Card>
      </Swipeable>
    );
  };

  return (
    <>
      <TopNavigation title={`Reports (total: ${reports.length})`} alignment='center' accessoryRight={() => <OptionsMenu onChanged={() => getReports()} />} accessoryLeft={NewReportMenu} />
      <Divider />
      <Layout style={{ flex: 1, justifyContent: 'flex-start' }}>
        <Input style={{ paddingHorizontal: 15 }} status="info" accessoryLeft={SearchIcon} value={filter} onChangeText={setFilter} onEndEditing={getReports} />
        {reports.length > 0 ? <List data={reports} renderItem={(item: any, index: any) => renderReport({ navigation, item, index })} onRefresh={getReports} refreshing={refreshing} /> : <Text style={{ alignSelf: 'center' }}>Nothing to see</Text>}
      </Layout>
    </>
  );
};