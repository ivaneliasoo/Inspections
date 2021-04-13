import React, { useRef } from 'react'
import SignatureCapture from 'react-native-signature-capture'
import { StyleSheet, View } from 'react-native'

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
    <View style={styles.container}>
      <SignatureCapture
        style={[{ flex: 1 }, styles.signature]}
        ref={sign}
        label='a pedo'
        saveImageFileInExtStorage={true}
        showNativeButtons={true}
        showBorder ={true}
        onSaveEvent={_onSaveEvent}
        onDragEvento={_onDragEvent}
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
