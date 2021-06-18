import { Card, Modal, Input, Text, useTheme } from '@ui-kitten/components'
import { CheckIcon, CrossIcon } from '../../components/Icons'
import React, { useState } from 'react'
import { View, TextInput, TouchableOpacity, ImageBackground } from 'react-native'

export interface PhotoLabelerHeaderProps {
  onClose: () => void;
}
const PhotoLabelerHeader = ({ onClose }: PhotoLabelerHeaderProps) => (
  <View style={{ flexDirection: 'row', justifyContent: 'space-between', margin: 10 }}>
    <Text appearance='hint' category='h6'>Photo Labeling</Text>
    <TouchableOpacity onPress={onClose}>
      <CrossIcon fill={'black'} style={{ width: 24, height: 24 }} />
    </TouchableOpacity>
  </View>
)


export interface PhotoLabelerFooterProps {
  label: string;
  onLabelChanged: (label: string) => void;
  onSave: () => void;
}
const PhotoLabelerFooter = ({ onLabelChanged, label, onSave }: PhotoLabelerFooterProps) => (
  <View style={{ flex: 1, flexDirection: 'row' }}>
    <Input style={{ flex: 3, marginVertical: 5, marginHorizontal: 5 }}
      maxLength={20}
      label='Photo Record Label'
      caption={`${label.length} / 20`}
      onChangeText={onLabelChanged}
      value={label} />
    <TouchableOpacity
      style={{ flex: 1, justifyContent: 'center', alignItems: 'center', marginVertical: 5, marginHorizontal: 5 }}
      onPress={onSave}>
      <CheckIcon fill={'green'} style={{ width: 24, height: 24 }} />
      <Text>Save</Text>
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
    <Modal visible={showLabeler} style={{width: '50%'}}>
      <Card style={{ flex: 1 }}
        header={() => <PhotoLabelerHeader onClose={onClose} />}
      >
        <ImageBackground style={{ width: '100%', height: 250 }} resizeMode='cover' source={{ uri: lastPhoto }} />
        <PhotoLabelerFooter onSave={onSave} label={label} onLabelChanged={onLabelChanged} />
      </Card>
    </Modal>
  )
}

export default PhotoLabeler
