import { useIsFocused } from '@react-navigation/core';
import React, { useRef, useState } from 'react';
import { PermissionsAndroid, StyleSheet, TouchableOpacity, View } from 'react-native';
import { RNCamera } from 'react-native-camera';
import CameraRoll from '@react-native-community/cameraroll';
import { Text, useTheme } from '@ui-kitten/components';
import PhotoLabeler from '../components/photorecords/PhotoLabeler';
import { showMessage } from 'react-native-flash-message';
import { usePhotoRecords } from '../hooks/usePhotoRecords';

export const CameraScreen = () => {
  const cameraRef = useRef<RNCamera>()
  const [lastLabel, setLastLabel] = useState('')
  const [lastPhoto, setLastPhoto] = useState('')
  const theme = useTheme()
  const { EnqueuePhotoUpload } = usePhotoRecords()
  const [showLabeler, setShowLabeler] = useState(false)

  const isFocused = useIsFocused()


  const hasCameraPermission = async () => {
    const camera = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.CAMERA)
    const recordAudio = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.RECORD_AUDIO)
    const readExternalStorage = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.READ_EXTERNAL_STORAGE)
    const writeExternalStorage = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.WRITE_EXTERNAL_STORAGE)
    return camera && recordAudio && readExternalStorage && writeExternalStorage
  }

  const requestStoragePermission = async () => {
    await PermissionsAndroid.request(
      PermissionsAndroid.PERMISSIONS.WRITE_EXTERNAL_STORAGE,
      {
        title: 'Permission to use your storage',
        message: 'We need your permission to use your storage to backup images taken by camera',
        buttonPositive: 'Ok',
        buttonNegative: 'Cancel',
      }
    )
  }

  const takePicture = async () => {
    if (cameraRef) {
      const options = { quality: 0.5, base64: false };
      const data = await cameraRef.current!.takePictureAsync(options);
      setLastPhoto(data.uri)
      setShowLabeler(true)
      await CameraRoll.save(data.uri, {
        album: 'CSE Inspections',
        type: 'photo'
      })
    }
  };
  return (
    <>
      <View style={styles.container}>
        {!isFocused ? null : hasCameraPermission() ? <>
          <RNCamera
            ref={cameraRef}
            style={styles.preview}
            type={RNCamera.Constants.Type.back}
            onTap={requestStoragePermission}
            flashMode={RNCamera.Constants.FlashMode.on}
            androidCameraPermissionOptions={{
              title: 'Permission to use camera',
              message: 'We need your permission to use your camera',
              buttonPositive: 'Ok',
              buttonNegative: 'Cancel',
            }}
            androidRecordAudioPermissionOptions={{
              title: 'Permission to use audio recording',
              message: 'We need your permission to use your audio',
              buttonPositive: 'Ok',
              buttonNegative: 'Cancel',
            }} />
          <View style={styles.shooter}>
            <TouchableOpacity onPress={takePicture} style={styles.capture}>
              <Text style={styles.shooterText}> Shot</Text>
            </TouchableOpacity>
          </View>
        </> : <Text>Camera and Record Audio permission not consented</Text>
        }
        <PhotoLabeler showLabeler={showLabeler} lastPhoto={lastPhoto} label={lastLabel} onLabelChanged={(label) => {
          setLastLabel(label)
        }}
          onClose={() => setShowLabeler(false)}
          onSave={() => {
            EnqueuePhotoUpload(27, lastPhoto, lastLabel)
            setShowLabeler(false)
            showMessage({
              message: 'Photo Upload has been enqueued',
              autoHide: true,
              color: theme['color-success-500']
            })
          }}
        />
      </View>
    </>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: 'column',
    backgroundColor: 'black',
  },
  preview: {
    flex: 1,
    justifyContent: 'flex-end',
    alignItems: 'center',
  },
  capture: {
    flex: 0,
    backgroundColor: '#fff',
    borderRadius: 5,
    padding: 15,
    paddingHorizontal: 20,
    alignSelf: 'center',
    margin: 20,
  },
  shooter: { flex: 0, flexDirection: 'row', justifyContent: 'center' },
  shooterText: { fontSize: 14 }
});