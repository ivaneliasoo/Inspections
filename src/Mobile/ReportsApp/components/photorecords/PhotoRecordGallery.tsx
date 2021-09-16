import { CrossIcon, EditIcon, TrashIcon } from '../Icons';
import React, { useEffect, useState } from 'react'
import { Pressable, StyleSheet, Text, TouchableOpacity, View } from 'react-native'
import Gallery from 'react-native-image-gallery';
import { Rect } from 'react-native-svg';
import { useNavigation } from '@react-navigation/core';
import { usePhotoRecords } from '../../hooks/usePhotoRecords';

export default function PhotoRecordGallery() {
  const { goBack } = useNavigation()
  const [selectedImage, setSelectedImage] = useState(0)
  const { GetByReportId } = usePhotoRecords()
  const [images, setImages] = useState([
    // { source: require('yourApp/image.png'), dimensions: { width: 150, height: 150 } },
    { source: { uri: 'http://i.imgur.com/XP2BE7q.jpg', label: 'asasdasdasdasdasd' } },
    { source: { uri: 'http://i.imgur.com/5nltiUd.jpg', label: 'Teasdasdasdst2' } },
    { source: { uri: 'http://i.imgur.com/6vOahbP.jpg', label: 'Teasdasdast3' } },
    { source: { uri: 'http://i.imgur.com/kj5VXasdadastG.jpg', label: 'Tedasdasdst4' } }
  ])

  useEffect(() => {
    GetByReportId(2)
      .then(resp => {
        const mappedData = resp.map((item, index) => {
          return {
            source: {
              id: item.id,
              photoStorageId: item.photoStorageId,
              uri: item.photoUrl,
              thumbnail: item.thumbnailUrl,
              thumbnailStorageId: item.thumbnailStorageId,
              label: item.label
            }
          }
        })
        setImages(mappedData)
      })
  }, [])

  return (
    <View style={{ flex: 1, flexDirection: 'column' }}>
      <View style={{ flex: 1, zIndex: 2, backgroundColor: 'transparent', position: 'absolute', top: 20, right: 10 }}>
        <TouchableOpacity onPress={() => goBack()}>
          <CrossIcon fill={'white'} style={{ width: 48, height: 48 }} />
        </TouchableOpacity>
      </View>
      <Gallery
        onPageSelected={(e) => setSelectedImage(e)}
        initialPage={selectedImage}
        style={{ backgroundColor: 'grey' }}
        images={images}
      />
      <View style={{ backgroundColor: 'black', position: 'absolute', bottom: 0, height: '10%', width: '100%', opacity: .7, flex: 1 }}>
        <View style={{ marginVertical: 20, flexDirection: 'row', flex: 1, justifyContent: 'space-around' }}>
          <EditIcon fill={'white'} style={{ width: 36, height: 36 }} />
          <TrashIcon fill={'white'} style={{ width: 36, height: 36 }} />
        </View>
        <Text style={{ color: 'white', textAlign: 'center', fontSize: 18 }}>{images[selectedImage].source.label}</Text>
      </View>
    </View>
  )
}

const styles = StyleSheet.create({})
