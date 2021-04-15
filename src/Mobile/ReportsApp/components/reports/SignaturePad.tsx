import React, { useRef } from 'react'
import SignatureCapture from 'react-native-signature-capture'
import { StyleSheet, Text, TouchableHighlight, View } from 'react-native'
import { createRef } from 'react'
import { useNavigation, useNavigationState } from '@react-navigation/native'
import AsyncStorage from '@react-native-async-storage/async-storage'

/**
 *
 *
 * @returns
 */
const SignaturePad = ({ route, navigation, saved }: any) => {
  const params  = useNavigationState(state => state.routes[state.index].params)
  const sign = createRef()
  
  const saveSign = () => {
    sign.current.saveImage();
  }

  const resetSign = () => {
    sign.current.resetImage();
  }

  const _onSaveEvent = (result) => {
    //result.encoded - for the base64 encoded png
    //result.pathName - for the file path name
    params.onGoBack({...result, index: params.index})
    navigation.goBack()

  }

  return (
    <View style={styles.container}>
      <SignatureCapture
        style={[styles.signature]}
        ref={sign}
        label='Please sign'
        saveImageFileInExtStorage={false}
        showTitleLabel={true}
        showNativeButtons={true}
        showBorder={false}
        onSaveEvent={_onSaveEvent}
        backgroundColor="#FFFFFF"
        strokeColor="#000000"
        minStrokeWidth={4}
        maxStrokeWidth={4}
      />
    </View>
  )
}

export { SignaturePad }

const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: "column"
  },
  signature: {
    flex: 14,
    borderColor: '#000',
    borderWidth: 10,
  },
  buttonStyle: {
    flex: 1, justifyContent: "center", alignItems: "center",
    backgroundColor: "#eeeeee",
    margin: 10
  }
});
