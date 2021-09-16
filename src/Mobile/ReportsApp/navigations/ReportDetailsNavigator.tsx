import React from 'react';
import {createBottomTabNavigator} from '@react-navigation/bottom-tabs'
import { OperationalReading } from '../components/reports/OperationalReading';
import { ReportForm } from '../components/reports/ReportForm';
import { Signatures } from '../components/reports/Signatures';
import { CameraScreen } from '../containers/CameraScreen';
import { Details } from '../containers/Details';
import PhotoRecordGallery from 'components/photorecords/PhotoRecordGallery';

const { Navigator, Screen } = createBottomTabNavigator()


export const ReportDetailsNavigator = () => (
  <Navigator>
    <Screen name="ReportForm" component={ReportForm} />
    <Screen name="CameraScreen" component={CameraScreen} />
    <Screen name="OperationalReadings" component={OperationalReading} />
    <Screen name="Signatures" component={Signatures} />
  </Navigator>
)