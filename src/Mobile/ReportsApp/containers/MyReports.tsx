import React, { useState, useRef, useEffect } from 'react';
import moment from 'moment';
import { Animated, Alert, View, StyleSheet } from 'react-native';
import { Divider, TopNavigation, Layout, Input, List, Text, Card, TopNavigationAction, Toggle, Select, SelectItem, Button } from '@ui-kitten/components';
import { SearchIcon, Camera, EditSignatureIcon, CheckIcon, AlertTriangle, Printer, ImageIcon } from '../components/Icons';
import { OptionsMenu } from '../components/home/OptionsMenu'
import { BackIcon } from '../components/Icons'
import { RectButton, Swipeable } from 'react-native-gesture-handler';
import Empty from '../assets/images/empty.svg'
import { useReports } from '../hooks/useReports';
import { Badge } from '../components/Badge';

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
      <Badge color='grey' count={item.photosCount}>
        <Camera fill={'blue'} style={styles.footerIcon} />
      </Badge>
      <Badge color='info' count={item.signaturesCount}>
        <EditSignatureIcon fill={'blue'} style={styles.footerIcon} />
      </Badge>
      <Badge color='info' count={item.notesCount}>
        <Camera fill={'blue'} style={styles.footerIcon} />
      </Badge>
      <Badge color='info' count={item.CompletedCheckLists}>
        <Camera fill={'blue'} style={styles.footerIcon} />
      </Badge>
    </View>
    <View style={{ flexDirection: 'row', alignContent: 'center' }}>
      {item.isClosed ? <CheckIcon fill={'green'} style={styles.footerIcon} /> : <AlertTriangle fill={'orange'} style={styles.footerIcon} />}
      <Text>Report is {item.isClosed ? 'Completed' : 'Incomplete'}</Text>
    </View>
  </Layout>
);

export const MyReports = ({ navigation }: any) => {
  const swipe = useRef<Swipeable>(null)
  const [refreshing, setRefreshing] = useState(false)

  const { getReports, deleteReport, completeReport, reports, setSorting, filter, setFilterText, reportsState: { descendingSort, orderBy } } = useReports()

  const handleGetReports = async () => {
    setRefreshing(true)
    await getReports()
    setRefreshing(false)
  }

  useEffect(() => {
    handleGetReports()
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
                onPress: () => { completeReport(item.id).then(() => handleGetReports()); swipe.current?.close(); }
              },
              {
                text: 'No',
                // onPress: () => console.log('canceled')
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
                onPress: () => { deleteReport(item.id).then(() => handleGetReports()); swipe.current?.close(); }
              },
              {
                text: 'No',
                // onPress: () => console.log('canceled')
              }
            ]
          );
        }}
        renderLeftActions={!item.isClosed ? renderLeftActions : () => null} renderRightActions={!item.isClosed ? renderRightActions : () => null}>

        <View style={{
          flexDirection: 'row',
          justifyContent: 'flex-start',
          borderBottomRightRadius: 20,
          borderTopRightRadius: 20,
          shadowColor: "#000",
          shadowOffset: { width: 0, height: 1, },
          shadowOpacity: 0.83,
          shadowRadius: 20
        }}>
          <Card
            key={item.index}
            style={styles.card}
            onPress={navigateDetails}
            status={item.isClosed ? 'success' : 'info'}
            header={() => <Text category='s1' >{`${item.licenseName} - ${item.licenseNumber ?? 'Not specified'} Validity: ${moment(item.licenseValidityStart).format('DD-MM-YYYY')} - ${moment(item.licenseValidityEnd).format('DD-MM-YYYY')}`} </Text>}
            footer={footerProps => renderItemFooter(footerProps, item)} >
            <View style={{ flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }}>
              <View>
                <Text category='s2'>{`Report Name: ${item.name}`}</Text>
                <Text category='s2'>{`Date: ${moment(item.date).format('DD/MM/YYYY')}`}</Text>
              </View>
              <View style={{
                justifyContent: 'space-around',
                alignItems: 'center',
              }}>
                <Button
                  style={{margin: 5}}
                  appearance='outline'
                  accessoryLeft={Printer}
                />
                <Button
                style={{margin: 5}}
                  appearance='outline'
                  accessoryLeft={ImageIcon}
                />
              </View>
            </View>
          </Card>
        </View>
      </Swipeable>
    );
  };

  return (
    <>
      <TopNavigation
        title={`Reports (total: ${reports.length})`}
        alignment='center'
        accessoryRight={() => <OptionsMenu onChanged={handleGetReports} />}
        accessoryLeft={() =>
          <TopNavigationAction icon={BackIcon} onPress={() => navigation.goBack()} />
        }
      />
      <Divider />
      <Layout style={styles.cardList}>
        <Input style={styles.inpustSearch} status="info" accessoryLeft={SearchIcon} value={filter} onChangeText={setFilterText} onEndEditing={handleGetReports} />
        <View style={{ marginVertical: 20, marginHorizontal: 20, flexDirection: 'row', justifyContent: 'space-between' }}>
          <Select
            style={{ minWidth: 200 }}
            size='large'
            placeholder='select an item to sort by...'
            label='Sort By'
            value={orderBy}
            onSelect={(e) => { setRefreshing(true); setSorting(descendingSort, ['date', 'name', 'address', 'company'][e.row]); setRefreshing(false); }}
          >
            {['date', 'name', 'address', 'company'].map((column, index) =>
              <SelectItem
                key={index}
                title={column}
              ></SelectItem>)}
          </Select>
          <Toggle checked={descendingSort} onChange={() => { setRefreshing(true); setSorting(!descendingSort, orderBy); setRefreshing(false); }}>
            Descending
          </Toggle>
        </View>
        {reports.length > 0 ?
          <List style={{ marginHorizontal: 10, marginVertical: 10 }} data={reports} renderItem={renderReport} onRefresh={handleGetReports} refreshing={refreshing} />
          :
          <View style={styles.noDataLayout}>
            <Empty height={200} width={300} />
            <Text status='warning' category='h1'>No results</Text>
            <Text status='info' category='s2'>brrr. its cold in here</Text>
            <Button onPress={handleGetReports}>Try Again</Button>
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