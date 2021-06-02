import React, { useState, useContext, useRef } from 'react';
import moment from 'moment';
import { Animated, Alert, View, StyleSheet, Button } from 'react-native';
import { Divider, TopNavigation, Layout, Input, List, Text, Card, TopNavigationAction } from '@ui-kitten/components';
import { ClosedIcon, SearchIcon, NotClosedIcon } from '../components/Icons'
import { OptionsMenu } from '../components/home/OptionsMenu'
import { useNavigation } from '@react-navigation/native';
import { BackIcon } from '../components/Icons'
import { RectButton, Swipeable } from 'react-native-gesture-handler';
import Empty from '../assets/images/empty.svg'
import { useReports } from '../hooks/useReports';
import { ReportsState } from '../contexts/ReportsContext';

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
  const swipe = useRef<Swipeable>(null)
  const navigation = useNavigation()
  const { getReports, deleteReport, completeReport, reports, filter, setFilterText, refreshing } = useReports()

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
                onPress: () => { completeReport(item.id).then(() => getReports()); swipe.current?.close();  }
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
                onPress: () => { deleteReport(item.id).then(() => getReports()); swipe.current?.close(); }
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
        <Input style={styles.inpustSearch} status="info" accessoryLeft={SearchIcon} value={filter} onChangeText={setFilterText} onEndEditing={getReports} />
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
          <List data={reports} renderItem={renderReport} onRefresh={getReports} refreshing={refreshing} />
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