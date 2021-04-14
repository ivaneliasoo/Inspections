import React, { useRef } from 'react'
import SignatureCapture from 'react-native-signature-capture'
import { StyleSheet, Text, TouchableHighlight, View } from 'react-native'
import { createRef } from 'react'
import { useNavigation } from '@react-navigation/native'

/**
 *
 *
 * @returns
 */
const SignaturePad = ({ navigation, saved }: any) => {
  // const navigator = useNavigation()
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
    console.log(result);
    navigation.navigate('Main', { image: result.encoded })

  }
  const _onDragEvent = () => {
    // This callback will be called when the user enters signature
    console.log("dragged");
  }

  return (
    <View style={styles.container}>
      <SignatureCapture
        style={[{ flex: 1 }, styles.signature]}
        ref={sign}
        label='a pedo'
        saveImageFileInExtStorage={false}
        showNativeButtons={false}
        showBorder={true}
        onSaveEvent={_onSaveEvent}
        onDragEvent={_onDragEvent}
        backgroundColor="#FFFFFF"
        strokeColor="#000000"
        minStrokeWidth={4}
        maxStrokeWidth={4}
      />

      <View style={{ flex: 1, flexDirection: "row" }}>
        <TouchableHighlight style={styles.buttonStyle}
          onPress={() => { saveSign() }} >
          <Text>Save</Text>
        </TouchableHighlight>

        <TouchableHighlight style={styles.buttonStyle}
          onPress={() => { resetSign() }} >
          <Text>Reset</Text>
        </TouchableHighlight>

      </View>
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
    flex: 1,
    borderColor: '#000',
    borderWidth: 10,
  },
  buttonStyle: {
    flex: 1, justifyContent: "center", alignItems: "center", height: 50,
    backgroundColor: "#eeeeee",
    margin: 10
  }
});
