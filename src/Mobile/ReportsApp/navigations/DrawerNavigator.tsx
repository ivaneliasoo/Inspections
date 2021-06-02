import React, { useContext } from 'react'
import { Drawer, DrawerItem, IndexPath } from '@ui-kitten/components';
import { createDrawerNavigator } from '@react-navigation/drawer'
import { HomeNavigator } from './HomeNavigator';
import { AuthContext } from '../contexts/AuthContext';
import { Authentication } from '../containers/Authentication';
import { MyReports } from '../containers/MyReports';


const { Navigator, Screen } = createDrawerNavigator()


export const DrawerNavigator = () => {
  const { authState, signOut } = useContext(AuthContext);

  const DrawerContent = ({ navigation, state }: any) => (
    <Drawer
      selectedIndex={new IndexPath(state.index)}
      onSelect={index => index.row !== 2 ? navigation.navigate(state.routeNames[index.row]) : null}>
      <DrawerItem title='Main' />
      <DrawerItem title='My Reports' />
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
