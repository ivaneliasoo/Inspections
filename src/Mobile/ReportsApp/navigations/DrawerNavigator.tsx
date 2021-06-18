import React, { useContext } from 'react'
import { Drawer, DrawerItem, IndexPath } from '@ui-kitten/components';
import { createDrawerNavigator } from '@react-navigation/drawer'
import { HomeNavigator } from './HomeNavigator';
import { AuthContext } from '../contexts/AuthContext';
import { Authentication } from '../containers/Authentication';
import { MyReports } from '../containers/MyReports';
import {Avatar} from '@ui-kitten/components'
import { ImageBackground } from 'react-native';


const { Navigator, Screen } = createDrawerNavigator()


export const DrawerNavigator = () => {
  const { authState, signOut } = useContext(AuthContext);

  const DrawerContent = ({ navigation, state }: any) => (
    <Drawer
      header={()=> <ImageBackground style={{
        height: 128,
        flexDirection: 'row',
        alignItems: 'center',
      }} resizeMode='cover' source={require('../assets/images/LoginBackground.jpg')}/>}
      selectedIndex={new IndexPath(state.index)}
      onSelect={index => index.row !== 2 ? navigation.navigate(state.routeNames[index.row]) : null}>
      <DrawerItem style={{ alignSelf: 'center' }} title={`${authState.userInfo.lastName} ${authState.userInfo.name}.`} accessoryLeft={() => <Avatar size='giant' source={{uri: 'https://demos.telerik.com/kendo-ui/content/web/Customers/ANTON.jpg'}} />} />
      <DrawerItem title='Home' onPress={() => navigation.navigate('Home')} />
      <DrawerItem title='MyReports' onPress={() => navigation.navigate('MyReports')} />
      <DrawerItem title='Logout' onPress={() => signOut()} />
    </Drawer>
  );

  if (!authState.userToken) {
    return <Authentication />
  }
  return (
    <Navigator drawerContent={props => <DrawerContent {...props} />}>
      <Screen name="Main" component={HomeNavigator} />
      <Screen name="MyReports" component={MyReports} />
    </Navigator>
  )
}
