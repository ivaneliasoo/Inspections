import React, { useRef } from 'react'
import SignatureCapture from 'react-native-signature-capture'
import { Layout, Text } from '@ui-kitten/components'
import { StyleSheet, TouchableHighlight, View } from 'react-native'
/**
 *
 *
 * @returns
 */
const Signatures = () => {
  const sign = useRef(null)

  const saveSign = () => {
    sign.saveImage();
  }

  const resetSign = () => {
    sign.resetImage();
  }

  const _onSaveEvent = (result) => {
    //result.encoded - for the base64 encoded png
    //result.pathName - for the file path name
    console.log(result);
  }
  const _onDragEvent = () => {
    // This callback will be called when the user enters signature
    console.log("dragged");
  }

  return (
    <View style={{ flex: 1, flexDirection: "column" }}>
      <Text style={{ alignItems: "center", justifyContent: "center" }}>Signature Capture Extended </Text>
      <SignatureCapture
        style={[{ flex: 1 }, styles.signature]}
        ref={sign}
        label='a pedo'
        saveImageFileInExtStorage={true}
        showNativeButtons={true}
        showBorder ={true}
        showTitleLabel={true}
        backgroundColor="#FFFFFF"
        strokeColor="#000000"
        minStrokeWidth={4}
        maxStrokeWidth={4}
         />
    </View>
  )
}

export { Signatures }

const styles = StyleSheet.create({
  signature: {
      flex: 1,
      borderColor: 'green',
      borderWidth: 10,
  },
  buttonStyle: {
      flex: 1, justifyContent: "center", alignItems: "center", height: 50,
      backgroundColor: "#eeeeee",
      margin: 10
  }
});
