import React from 'react'
import { StyleSheet, Text, View } from 'react-native';
import { Camera } from './Icons';
import { useTheme } from '@ui-kitten/components';
import { useContext } from 'react';
import { ThemeContext } from '../contexts/ThemeContext';

export interface BadgeProps {
  count: number;
  color?: string;
  children: any;
  textColor?: string;
}

export const Badge = ({ count = 1, color = 'primary', textColor = 'white', children }: BadgeProps) => {
  const { theme } = useContext(ThemeContext)
  const themeColors = useTheme()
  return (
    <View style={styles.container}>
      {children}
      <View style={[styles.badge, { backgroundColor: themeColors[`color-${color}-500`] }]}>
        <Text style={[styles.text, { color: textColor }]}>{count}</Text>
      </View>
    </View>
  )
}

const styles = StyleSheet.create({
  container: {
    width: 24,
    height: 24,
    marginTop: 5,
    marginBottom: 5,
    marginHorizontal: 10
  },
  badge: {
    position: 'absolute',
    left: -10,
    top: -6,
    // backgroundColor: 'red',
    borderRadius: 12,
    width: 18,
    height: 18,
    justifyContent: 'center',
    alignItems: 'center',
  },
  text: {
    top: -1,
    left: -1,
    // color: 'white',
    fontSize: 12,
    fontWeight: '900'
  },
  icon: {
    width: 24,
    height: 24
  }
})
