import { Card, Modal, Input, Text, useTheme } from '@ui-kitten/components'
import { CheckIcon, CrossIcon } from '../../components/Icons'
import React, { useState } from 'react'
import { View, TextInput, TouchableOpacity, ImageBackground } from 'react-native'

const PhotoLabelerHeader = ({ onClose }) => (
  <View style={{ flexDirection: 'row', justifyContent: 'space-between', margin: 10 }}>
    <Text appearance='hint' category='h6'>Photo Labeling</Text>
    <TouchableOpacity onPress={onClose}>
      <CrossIcon fill={'black'} style={{ width: 24, height: 24 }} />
    </TouchableOpacity>
  </View>
)


export interface PhotoLabelerProps {
  showLabeler: boolean;
  lastPhoto: string;
  label: string;
  onLabelChanged: (label: string) => void;
  onSave: () => void;
  onClose: () => void;
}

const PhotoLabeler = ({ showLabeler, lastPhoto, label, onLabelChanged, onSave, onClose }: PhotoLabelerProps) => {
  return (
    // <Modal visible={showLabeler}>
    //   <Card style={{ flex: 1 }}
    //     header={() => }
    //     footer={() => <View style={{ flex: 4, flexDirection: 'row' }}>
    <Modal visible={showLabeler}>
      <Card style={{ flex: 1 }}
        header={() => <PhotoLabelerHeader onClose={onClose} />}
      >
        <ImageBackground style={{ width: '100%', height: 150 }} resizeMode='stretch' source={{ uri: lastPhoto }} />        
        <Input style={{ flex: 3, marginVertical: 5, marginHorizontal: 5 }}
          onChangeText={onLabelChanged}
          multiline={true}
          value={label} />
          <TouchableOpacity
            style={{ flex: 1, justifyContent: 'center', alignItems: 'center', marginVertical: 5, marginHorizontal: 5 }}
            onPress={onSave}>
            <CheckIcon fill={'green'} style={{ width: 24, height: 24 }} />
            <Text>Save</Text>
          </TouchableOpacity>
      </Card>
    </Modal>
  )
}

export default PhotoLabeler
