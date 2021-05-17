import React, { useState, useEffect, useContext, useRef } from 'react';
import { Configuration, CreateReportCommand, ReportConfigurationApi, ReportsApi, ResumenReportConfiguration } from '../services/api'
import moment from 'moment';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Animated, Alert, View, StyleSheet, Button } from 'react-native';
import { Icon, Divider, TopNavigation, Layout, Input, List, Text, Card, TopNavigationAction, Select } from '@ui-kitten/components';
import { ClosedIcon, SearchIcon, NotClosedIcon } from '../components/Icons'
import { OptionsMenu } from '../components/home/OptionsMenu'
import { useNavigation } from '@react-navigation/native';
import { BackIcon } from '../components/Icons'
import { API_HOST, API_KEY } from '../config/config';
import { ReportsContext } from '../reports-contexts';
import { RectButton, Swipeable } from 'react-native-gesture-handler';
import Empty from '../assets/images/empty.svg'

const renderLeftActions = () => {
  return (
    <RectButton style={styles.leftActions}>
      <Animated.Text style={[styles.swipeActionAnimatedText]}>
        Complete
      </Animated.Text>
    </RectButton>
  )
}

const renderRightActions = (_progress: any, dragX: any) => {
  const trans = dragX.interpolate({
    inputRange: [-80, 0],
    outputRange: [1, 0],
    extrapolate: 'clamp',
  })

  return (
    <RectButton style={styles.swipeActionLeft}>
      <Animated.Text
        style={[
          {
            transform: [{ translateX: trans }],
          },
          styles.swipeActionAnimatedText
        ]}
      >
        Delete
      </Animated.Text>
    </RectButton>
  )
}

const renderItemFooter = (footerProps: any, item: any) => (
  <Layout {...footerProps} style={styles.cardFooter}>
    {item.isClosed ? <ClosedIcon fill={'green'} style={styles.footerIcon} /> : <NotClosedIcon fill={'orange'} style={styles.footerIcon} />}
    <Text>{item.isClosed ? 'Completed': 'Incomplete'}</Text>
  </Layout>
);

export const MyReports = () => {
  const [refreshing, setRefreshing] = useState(true)
  const { getAll, reports, filter, setMyReports, myReports, setIsClosed, isClosed, setFilter } = useContext<any>(ReportsContext)
  const swipe = useRef<Swipeable>(null)
  const mountedRef = useRef(true)

  const navigation = useNavigation()
  async function getReports(options: any) {
    setRefreshing(true)
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY } as Configuration)
    const resp = await reportsApi.reportsGet(filter, options.isClosed, options.myReports)
    getAll(resp.data)
    setRefreshing(false)
  }

  async function completeReport(reportId: number) {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const api = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY } as Configuration)
    const result = await api.completeReport(reportId).catch(error => console.log(error.response.message))
    return result
  }

  async function deleteReport(reportId: number) {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const api = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY } as Configuration)
    const result = await api.reportsIdDelete(reportId).catch(error => console.log(error.response.message))
    return result
  }

  useEffect(() => {
    if (mountedRef.current) {
      getReports({})
      if (isClosed === undefined)
        setIsClosed(false)
      if (myReports === undefined)
        setMyReports(true)
    }
    return () => {
      mountedRef.current = false
    }
  }, [])

  const renderReport = ({ item }: any) => {
    const navigateDetails = () => {
      navigation.navigate('Details', { reportId: item.id, title: item.title });
    };
    return (
      <Swipeable ref={swipe} friction={2}
        onSwipeableLeftOpen={() => {
          if (item.isClosed) return;
          Alert.alert('Complete / Close Report', `You are about to complete the report ${item.name} (${item.id}). Are you sure?`,
            [
              {
                text: 'Yes',
                onPress: () => { completeReport(item.id).then(() => getReports({})); swipe.current?.close();  }
              },
              {
                text: 'No',
                onPress: () => console.log('canceled')
              }
            ]
          );
        }}
        onSwipeableRightOpen={() => {
          if (item.isClosed) return;
          Alert.alert('Delete Report', `You are about to delete the report ${item.name} (${item.id}). Are you sure?`,
            [
              {
                text: 'Yes',
                onPress: () => { deleteReport(item.id).then(() => getReports({})); swipe.current?.close(); }
              },
              {
                text: 'No',
                onPress: () => console.log('canceled')
              }
            ]
          );
        }}
        renderLeftActions={!item.isClosed ? renderLeftActions : () => null} renderRightActions={!item.isClosed ? renderRightActions : () => null}>

        <Card key={item.index} style={styles.card} onPress={navigateDetails} status={item.isClosed ? 'success' : 'warning'}
          footer={footerProps => renderItemFooter(footerProps, item)} >
          <Text category='s1'>{`${moment(item.date).format('DD/MM/YYYY HH:mm')} License ${item.license?.number ?? 'Not specified'}`}</Text>
          <Text category='s2'>{item.address === '' ? 'address not specified' : item.address}</Text>
          <Text category='c1'>{API_HOST}</Text>
        </Card>
      </Swipeable>
    );
  };

  return (
    <>
      <TopNavigation
        title={`Reports (total: ${reports.length})`}
        alignment='center'
        accessoryRight={() => <OptionsMenu onChanged={getReports} />}
        accessoryLeft={() =>
          <TopNavigationAction icon={BackIcon} onPress={() => navigation.goBack()} />
        }
      />
      <Divider />
      <Layout style={styles.cardList}>
        <Input style={styles.inpustSearch} status="info" accessoryLeft={SearchIcon} value={filter} onChangeText={setFilter} onEndEditing={getReports} />
        {/* <Select
          placeholder='type to search an address'
          value={}
          label='Sort By' 
          onSelect={(e) => { setFieldValue('address', addresses[e.row].formatedAddress); setFieldValue('license.number', addresses[e.row].number)}}
          status={errors.address ? 'danger':'basic'}
          caption={errors.address}
        >
          {[].map(reanderOption)}
        </Select> */}
        {reports.length > 0 ?
          <List data={reports} renderItem={renderReport} onRefresh={() => getReports({})} refreshing={refreshing} />
          :
          <View style={styles.noDataLayout}>
            <Empty height={200} width={300} />
            <Text status='warning' category='h1'>No results</Text>
            <Text status='info' category='s2'>try again later or try with a new search</Text>
            <Button title="Try Again" onPress={getReports} />
          </View>}
      </Layout>
    </>
  );
};

const styles = StyleSheet.create({
  leftActions: {
    backgroundColor: 'green',
    flex: 1,
    justifyContent: 'center',
    alignContent: 'stretch',
  },
  card: { padding: 0, marginHorizontal: 5, marginVertical: 2 },
  cardFooter: { flexDirection: 'row', justifyContent: 'space-between', margin: 5 },
  cardList: { flex: 1, justifyContent: 'flex-start' },
  inpustSearch: { paddingHorizontal: 15 },
  noDataLayout: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  swipeAction: {
    backgroundColor: 'blue',
  },
  swipeActionLeft: {
    backgroundColor: 'red',
    justifyContent: 'center',
    flex: 1,
    alignContent: 'stretch',
    textAlign: 'right'
  },
  swipeActionAnimatedText: {
    padding: 0,
    marginHorizontal: 5,
    marginVertical: 2,
    color: 'white',
    fontSize: 24,
    textAlign: 'left'
  },
  footerIcon: {
    width: 24,
    height: 24
  }
})