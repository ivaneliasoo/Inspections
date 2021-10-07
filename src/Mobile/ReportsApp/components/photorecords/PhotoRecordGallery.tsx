import { CrossIcon, EditIcon, TrashIcon } from '../Icons';
import React, { useEffect, useState } from 'react'
import { Image, Pressable, StyleSheet, Text, TouchableOpacity, View } from 'react-native'
import Gallery from 'react-native-image-gallery';
import { Rect } from 'react-native-svg';
import { useNavigation } from '@react-navigation/core';
import { usePhotoRecords } from '../../hooks/usePhotoRecords';
import { FlatList } from 'react-native-gesture-handler';

export default function PhotoRecordGallery() {
  const { goBack } = useNavigation()
  const [selectedImage, setSelectedImage] = useState(0)
  const { GetByReportId } = usePhotoRecords()
  const [images, setImages] = useState([])

  useEffect(() => {
    GetByReportId(2)
      .then(resp => {
        const mappedData = resp.map(item => {
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
        setImages(mappedData as [])
        setSelectedImage(mappedData.length - 1)
      })
  }, [])

  return (
    <>
      <View style={{ flex: 1, flexDirection: 'column' }}>

        <View style={{ flex: 1, zIndex: 2, backgroundColor: 'transparent', position: 'absolute', top: 20, right: 10 }}>
          <TouchableOpacity onPress={() => goBack()} style={styles.circle}>
            <CrossIcon fill={'white'} style={{ width: 36, height: 36 }} />
          </TouchableOpacity>
        </View>
        <Gallery
          onPageSelected={(e) => setSelectedImage(e)}
          initialPage={selectedImage}
          style={{ backgroundColor: 'grey' }}
          images={images} />
        <View style={{ backgroundColor: 'black', position: 'absolute', bottom: 0, height: '10%', width: '100%', opacity: .7, flex: 1 }}>
          <View style={{ marginVertical: 20, flexDirection: 'row', flex: 1, justifyContent: 'space-around' }}>
            <TouchableOpacity onPress={() => goBack()} style={styles.circle}>
              <EditIcon fill={'white'} style={{ width: 36, height: 36 }} />
            </TouchableOpacity>
            <TouchableOpacity onPress={() => goBack()} style={styles.circle}>
              <TrashIcon fill={'white'} style={{ width: 36, height: 36 }} />
            </TouchableOpacity>
          </View>
          {/* <Text style={{ color: 'white', textAlign: 'center', fontSize: 18 }}>{images[selectedImage].source.label}</Text> */}
        </View>
      </View>
      <View style={{ backgroundColor: 'black' }}>
        <FlatList
          data={images}
          keyExtractor={item => item.source.id.toString()}
          horizontal={true}

          showsHorizontalScrollIndicator={false}
          renderItem={({ item, index }: any) => <TouchableOpacity
            onPress={() => { setSelectedImage(index); }}
            activeOpacity={0.8}
            style={{
              width: 100,
              height: 150,
              marginHorizontal: 2,
              marginVertical: 2,
              paddingBottom: 3,
              paddingHorizontal: 3,
            }}>
            <View style={styles.imageContainer}>
              <Image source={{ uri: item.source.uri }} style={styles.image} />
            </View>
          </TouchableOpacity>} />
      </View>
    </>
  )
}

const styles = StyleSheet.create({
  circle: {
    width: 36,
    height: 36,
    borderRadius: 100 / 2,
    opacity: .8
  },
  image: {
    flex: 1,
    borderRadius: 3,
  },
  imageContainerView: {
    borderColor: 'white'
  },
  imageContainer: {
    flex: 1,
    // justifyContent: 'space-between',
    // backgroundColor: 'black',
    // opacity: .5,
    borderColor: 'white',
    borderRadius: 3,
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 10,
    },
    shadowOpacity: 0.24,
    shadowRadius: 7,

    elevation: 5,
  },
})
