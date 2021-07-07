import React from 'react';
import * as eva from '@eva-design/eva';
import {EvaIconsPack} from '@ui-kitten/eva-icons';
import {ApplicationProvider, IconRegistry} from '@ui-kitten/components';
import {ThemeContext} from './contexts/ThemeContext';
import {AuthProvider} from './contexts/AuthContext';
import {AppNavigator} from './navigations/AppNavigator';
import { ReportsProvider } from './contexts/ReportsContext';
import FlashMessage from 'react-native-flash-message';


export default () => {
  const [theme, setTheme] = React.useState('light');

  const toggleTheme = () => {
    const nextTheme = theme === 'light' ? 'dark' : 'light';
    setTheme(nextTheme);
  };

  return (
    <>
      <IconRegistry icons={EvaIconsPack} />
      <ThemeContext.Provider value={{theme, toggleTheme}}>
        <ApplicationProvider {...eva} theme={eva[theme]}>
          <AuthProvider>
            <ReportsProvider>
              <AppNavigator />
              <FlashMessage position="top" />
            </ReportsProvider>
          </AuthProvider>
        </ApplicationProvider>
      </ThemeContext.Provider>
    </>
  );
};
