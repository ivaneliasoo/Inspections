import { useIsFocused } from '@react-navigation/core';
import React, { useEffect, useRef, useState } from 'react';
import { PermissionsAndroid, StyleSheet, TouchableOpacity, View } from 'react-native';
import { RNCamera } from 'react-native-camera';
import CameraRoll from '@react-native-community/cameraroll';
import { Text, useTheme } from '@ui-kitten/components';
import PhotoLabeler from '../components/photorecords/PhotoLabeler';
import { showMessage } from 'react-native-flash-message';
import { usePhotoRecords } from '../hooks/usePhotoRecords';
import { useContext } from 'react';
import { ReportsContext } from '../contexts/ReportsContext';
import { FilmIcon, Camera, SyncIcon, AddIcon, FlipIcon, ImageIcon } from '../components/Icons';
import { useNavigation } from '@react-navigation/native';
import { Asset, ImageLibraryOptions, launchImageLibrary } from 'react-native-image-picker'

export const CameraScreen = () => {
  const cameraRef = useRef<RNCamera>()
  const { reportsState: { workingReport } } = useContext(ReportsContext);
  const [lastLabel, setLastLabel] = useState('')
  const [lastPhoto, setLastPhoto] = useState('')
  const theme = useTheme()
  const { EnqueuePhotoUpload } = usePhotoRecords()
  const [showLabeler, setShowLabeler] = useState(false)
  const [cameraType, setCameraType] = useState(RNCamera.Constants.Type.back)

  const isFocused = useIsFocused()
  const navigation = useNavigation()

  const hasCameraPermission = async () => {
    const camera = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.CAMERA)
    const recordAudio = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.RECORD_AUDIO)
    const readExternalStorage = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.READ_EXTERNAL_STORAGE)
    const writeExternalStorage = await PermissionsAndroid.request(
      PermissionsAndroid.PERMISSIONS.WRITE_EXTERNAL_STORAGE,
      {
        title: 'Permission to use your storage',
        message: 'We need your permission to use your storage to backup images taken by camera',
        buttonPositive: 'Ok',
        buttonNegative: 'Cancel',
      }
    )
    return camera && recordAudio && readExternalStorage && writeExternalStorage
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

  const saveFromGallery = async () => {
    const options = { mediaType: 'photo', quality: 0.5, base64: false } as ImageLibraryOptions;
    launchImageLibrary(options, async (resp) => {
      if (resp && resp.assets) {
        const data = resp.assets[0] || {} as Asset
        setLastPhoto(data.uri!)
        setShowLabeler(true)
        await CameraRoll.save(data.uri!, {
          album: 'CSE Inspections',
          type: 'photo'
        })
      }
    })
  }

  useEffect(() => {
    hasCameraPermission()
  }, [])

  return (
    <>
      <View style={styles.container}>
        {!isFocused ? null : <>
          <RNCamera
            ref={cameraRef as any}
            style={styles.preview}
            type={cameraType}
            // onTap={requestStoragePermission}
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
          <View style={styles.header}>
            <TouchableOpacity onPress={() => {
              if (!cameraType == RNCamera.Constants.Type.front)
                setCameraType(RNCamera.Constants.Type.front)
              else
                setCameraType(RNCamera.Constants.Type.back)
            }}>
              <FlipIcon fill={'white'} style={{ width: 32, height: 32 }} />
            </TouchableOpacity>
          </View>
          <View style={styles.shooter}>
            <TouchableOpacity onPress={() => {
              navigation.navigate('PhotoRecordGallery')
            }}>
              <FilmIcon fill={'white'} style={{ width: 32, height: 32 }} />
            </TouchableOpacity>
            <TouchableOpacity onPress={takePicture} style={styles.snapButton}>
              <View style={styles.centerSnapButton}>
                <Camera fill={'white'} style={{ width: 32, height: 32 }} />
              </View>
            </TouchableOpacity>
            <TouchableOpacity onPress={() => saveFromGallery()} >
              <ImageIcon fill={'white'} style={{ width: 32, height: 32 }} />
            </TouchableOpacity>
          </View>
        </>// : <Text>Camera and Record Audio permission not consented</Text>
        }
        <PhotoLabeler showLabeler={showLabeler} lastPhoto={lastPhoto} label={lastLabel} onLabelChanged={(label) => {
          setLastLabel(label)
        }}
          onClose={() => setShowLabeler(false)}
          onSave={() => {
            EnqueuePhotoUpload(workingReport!.id!, lastPhoto, lastLabel)
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
  header: {
    position: 'absolute',
    top: 0,
    width: '100%',
    flexDirection: 'row',
    justifyContent: 'flex-end'
  },
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
  shooter: {
    position: 'absolute',
    bottom: 0,
    width: '100%',
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    marginBottom: 20
  },
  snapButton: {
    width: 72,
    height: 72,
    borderRadius: 36,
    borderWidth: 4,
    borderColor: 'rgba(204, 31, 74, .5)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  centerSnapButton: {
    backgroundColor: '#cc1f4a',
    width: 60,
    height: 60,
    borderRadius: 32,
    justifyContent: 'center',
    alignItems: 'center'
  }
});