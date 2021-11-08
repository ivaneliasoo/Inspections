import React, { useState, useEffect } from 'react';
import moment from 'moment';
import { Animated, Alert, View, StyleSheet } from 'react-native';
import { Divider, TopNavigation, Layout, Input, List, Text, Card, TopNavigationAction, Toggle, Select, SelectItem, Button, ListItem, IndexPath } from '@ui-kitten/components';
import { Camera, EditSignatureIcon, CheckIcon, AlertTriangle, Printer, ImageIcon } from '../components/Icons';
import { OptionsMenu } from '../components/home/OptionsMenu'
import { BackIcon } from '../components/Icons'
import { RectButton, Swipeable } from 'react-native-gesture-handler';
import Empty from '../assets/images/empty.svg'
import { useReports } from '../hooks/useReports';
import { Badge } from '../components/Badge';
import { ReportsFilter } from '../components/reports/ReportsFilter';
import { useDownloader } from '../hooks/useDownloader';

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
    <View style={{ flexDirection: 'row' }}>
      <Badge color='info' count={item.photosCount}>
        <Camera fill={'blue'} style={styles.footerIcon} />
      </Badge>
      <Badge color='info' count={item.signaturesCount}>
        <EditSignatureIcon fill={'blue'} style={styles.footerIcon} />
      </Badge>
    </View>
    <View style={{ flexDirection: 'row', alignContent: 'center' }}>
      {item.isClosed ? <CheckIcon fill={'green'} style={styles.footerIcon} /> : <AlertTriangle fill={'orange'} style={styles.footerIcon} />}
      <Text>Report is {item.isClosed ? 'Completed' : 'Incomplete'}</Text>
    </View>
  </Layout>
);

export interface SwipeableReportProps {
  children: any;
  isClosed: boolean;
  id: number;
  name: string;
  onSwipedLeft: (reportId: number) => void;
  onSwipedRight: (reportId: number) => void;
}

const SwipeableReport = ({ children, isClosed, id, name, onSwipedLeft, onSwipedRight }: SwipeableReportProps) => {
  const swipe = React.useRef(null)

  const closeSwipe = () => {
    swipe.current?.close()
  }
  return (
    <Swipeable
      ref={swipe}
      friction={2}
      leftThreshold={30}
      rightThreshold={40}
      onSwipeableLeftOpen={() => {
        if (isClosed) return;
        Alert.alert('Complete / Close Report', `You are about to complete the report ${name} (${id}). Are you sure?`,
          [
            {
              text: 'Yes',
              onPress: () => { onSwipedLeft(id); closeSwipe() }
            },
            {
              text: 'No',
            }
          ]
        );
      }}
      onSwipeableRightOpen={() => {
        if (isClosed) return;
        Alert.alert('Delete Report', `You are about to delete the report ${name} (${id}). Are you sure?`,
          [
            {
              text: 'Yes',
              onPress: () => { onSwipedRight(id); closeSwipe(); }
            },
            {
              text: 'No',
            }
          ]
        );
      }}
      renderLeftActions={!isClosed ? renderLeftActions : () => null} renderRightActions={!isClosed ? renderRightActions : () => null}>

      {children}
    </Swipeable>
  );
};

export const MyReports = ({ navigation }: any) => {
  const { getReports, reports, deleteReport, completeReport, refreshing, reportsState, generatePdf } = useReports()

  useEffect(() => {
    getReports()
  }, [])

  useEffect(() => {
    getReports()
  }, [reportsState.filter,reportsState.isClosed, reportsState.orderBy, reportsState.descendingSort, reportsState.myReports])
  
  const {downloadPdf} = useDownloader()

  const ReportItem = ({ item }: { item: any }) => {

    const navigateDetails = () => {
      navigation.navigate('Details', { reportId: item.id, title: item.title });
    };

    return (
      <SwipeableReport isClosed={item.isClosed} name={item.name} id={item.id} onSwipedRight={deleteReport} onSwipedLeft={completeReport}>
        <View style={{
          flexDirection: 'row',
          justifyContent: 'flex-start',
          borderBottomRightRadius: 20,
          borderTopRightRadius: 20,
          shadowColor: "#000",
          shadowOffset: { width: 0, height: 1, },
          shadowOpacity: 0.83,
          shadowRadius: 20,
        }}>
          <Card
            key={item.index}
            style={styles.card}
            onPress={navigateDetails}
            status={item.isClosed ? 'success' : 'info'}
            header={() => <Text category='s1' >{`${item.licenseName} - ${item.licenseNumber ?? 'Not specified'}`} </Text>}
            footer={footerProps => renderItemFooter(footerProps, item)} >
            <View style={{ flex: 2, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }}>
              <View style={{ flex: 1 }}>
                <Text category='s2'>{`Report Name: ${item.name}`}</Text>
                <Text category='s2'>{`Date: ${moment(item.date).format('DD/MM/YYYY')}`}</Text>
                <Text category='s2'>Validity: {moment(item.licenseValidityStart).format('DD-MM-YYYY')} - {moment(item.licenseValidityEnd).format('DD-MM-YYYY')}</Text>
              </View>
              <View style={{
                justifyContent: 'space-around',
                alignItems: 'center',
              }}>
                <Button
                  style={{ margin: 5 }}
                  appearance='outline'
                  accessoryLeft={Printer}
                  onPress={() => downloadPdf(item.id)}
                />
                <Button
                  style={{ margin: 5 }}
                  appearance='outline'
                  accessoryLeft={ImageIcon}
                  onPress={() => downloadPdf(item.id, true)}
                />
              </View>
            </View>
          </Card>
        </View>
      </SwipeableReport>
    )
  }

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
        <ReportsFilter />
        {reports.length > 0 ?
          <List style={{ marginHorizontal: 10, marginVertical: 10 }}
            data={reports} renderItem={ReportItem} onRefresh={getReports} refreshing={refreshing}
            keyExtractor={(item, index) => `report${item.id}`} />
          :
          <View style={styles.noDataLayout}>
            <Empty height={200} width={300} />
            <Text status='warning' category='h1'>No results</Text>
            <Text status='info' category='s2'>brrr. its cold in here. try adding some reports or refresh the list</Text>
            <Button onPress={getReports}>Refresh</Button>
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
  card: {
    padding: 0,
    marginVertical: 2,
    elevation: 5,
    paddingHorizontal: 5,
    paddingVertical: 5,
    width: '100%',
  },
  cardFooter: { flexDirection: 'row', justifyContent: 'space-between', margin: 5, alignContent: 'center', alignItems: 'center', flexWrap: 'wrap' },
  cardList: { flex: 1, justifyContent: 'flex-start' },
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