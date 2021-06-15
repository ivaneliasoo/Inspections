import { useIsFocused } from '@react-navigation/core';
import React, { useRef, useState } from 'react';
import { PermissionsAndroid, StyleSheet, TouchableOpacity, View, Image, Alert, ImageBackground } from 'react-native';
import { RNCamera } from 'react-native-camera';
import CameraRoll from '@react-native-community/cameraroll';
import { Card, Text, Input, Modal, useTheme } from '@ui-kitten/components';
import { CheckIcon, CrossIcon } from '../components/Icons';
import { usePhotoRecords } from '../hooks/usePhotoRecords';
import { showMessage } from 'react-native-flash-message'

export const CameraScreen = () => {
  const cameraRef = useRef<RNCamera>()
  const [lastPhoto, setLastPhoto] = useState('')
  const [lastLabel, setLastLabel] = useState('')
  const [showLabeler, setShowLabeler] = useState(false)

  const isFocused = useIsFocused()
  const theme = useTheme()
  const { EnqueuePhotoUpload } = usePhotoRecords()

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
      //photoLabelerSheet.current?.setModalVisible()
    }
  };
  return (
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
          }} /><View style={styles.shooter}>
          <TouchableOpacity onPress={takePicture} style={styles.capture}>
            <Text style={styles.shooterText}> Shot</Text>
          </TouchableOpacity>
        </View>
      </> : <Text>Camera and Record Audio permission not consented</Text>
      }
      <Modal visible={showLabeler}>
        <Card style={{ flex: 1 }}
          header={() => <View style={{ flexDirection: 'row', justifyContent: 'space-between', margin: 10 }}><Text appearance='hint' category='h6'>Photo Labeling</Text><CrossIcon fill={'black'} style={{ width: 24, height: 24 }} /></View>}
          footer={() => <View style={{ flex: 4, flexDirection: 'row' }}>
            <Input style={{ flex: 3, marginVertical: 5, marginHorizontal: 5 }} caption='Write a label to identify the image'
              onChangeText={(e) => setLastLabel(e)}
              value={lastLabel} />
            <TouchableOpacity style={{ flex: 1, justifyContent: 'center', alignItems: 'center', marginVertical: 5, marginHorizontal: 5 }} onPress={() => {
              EnqueuePhotoUpload(27, lastPhoto, lastLabel)
              setShowLabeler(false)
              showMessage({
                message: 'Photo Upload has been enqueued',
                autoHide: true,
                color: theme['color-success-500']
              })
            }}>
              <CheckIcon fill={'green'} style={{ width: 24, height: 24 }} />
              <Text>Save</Text>
            </TouchableOpacity>
          </View>}
        >
          <ImageBackground style={{ width: 250, height: 150 }} resizeMode='cover' source={{ uri: lastPhoto }} />
        </Card>
      </Modal>
    </View>
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