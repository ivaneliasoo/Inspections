import React from 'react'
import SignatureCapture from 'react-native-signature-capture'
import { StyleSheet, View } from 'react-native'
import { createRef } from 'react';
import { useNavigationState } from '@react-navigation/native'
import { useReports } from '../../hooks/useReports';

/**
 *
 *
 * @returns
 */
const SignaturePad = ({ route, navigation, saved }: any) => {
  const params  = useNavigationState(state => state.routes[state.index].params)

  const { saveSignature, workingReport } = useReports()
  const sign = createRef()
  
  const _onSaveEvent = async (result) => {
    const signature = workingReport?.signatures!.find(s=>s.id === params!.id)
    signature!.drawnSign = `data:image/png;base64,${result.encoded}`
    await saveSignature({ signature, index: params!.index })
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
