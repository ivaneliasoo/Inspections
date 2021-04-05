import React, { useState } from 'react'
import { CheckIcon } from '../Icons'
import { Layout, Text, Icon } from '@ui-kitten/components'
import { Pressable, StyleSheet, TouchableOpacity, View } from 'react-native'
import { useFormikContext } from 'formik'
import { CheckList, CheckListItem, CheckValue, Report } from '../../services/api'

const checkItemIcon = [
  { name: 'close-outline', color: 'red'},
  { name: 'checkmark-outline', color: 'green'},
  { name: 'minus-outline', color: 'orange'},
  { name: 'alert-circle-outline', color: 'blue'}
]

type CheckListItemProps = {
  item: CheckList,
  index: number
}

const CheckListLine = ({ item, index, ...props }: CheckListItemProps) => {
  const [checked, setChecked] = useState(2)

  const onPress = () => {

    if(checked < 2)
      setChecked(checked + 1)
    else
      setChecked(0)
  }

  return (
    <>
        <TouchableOpacity key={index} onPress={onPress} style={[{ flex: 1, flexDirection: 'row', alignItems: 'center'}, props.style]}>
            <Text style={{flex: 10,  fontWeight: '900' }} category='s1' >{`${index + 1} - ${item.text}`}</Text>
            <Text style={{ alignSelf: 'center', alignContent: 'center' }} category='c1'>{CheckValue[checked]}<Icon name={checkItemIcon[checked].name} fill={checkItemIcon[checked].color} style={{ width: 30, height: 30 }} /></Text>
        </TouchableOpacity>
      {
        item && item.checks && item.checks.map((checkItem, checkIndex) => {
          return <CheckListItemCheck checkItem={checkItem} itemIndex={index} checkIndex={checkIndex} {...props} />
        })
      }
    </>
  )
}

type CheckListItemCheckProps = {
  checkItem: CheckListItem,
  checkIndex: number,
  itemIndex: number
}
const CheckListItemCheck = ({ checkItem, checkIndex, itemIndex, ...props }: CheckListItemCheckProps) => {
  const [checked, setChecked] = useState(2)

  const onPress = () => {

    if(checked < 2)
      setChecked(checked + 1)
    else
      setChecked(0)
  }

  return <TouchableOpacity style={[{flex: 1, flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' }, props.style]} key={`${itemIndex}-${checkIndex}`} onPress={onPress}>
        <Text style={{ flex: 10 }} category='s2'>{`${itemIndex + 1}.${checkIndex + 1} - ${checkItem.text} ${checked} (${CheckValue[checked]})`}</Text>
        <Icon name={checkItemIcon[checked].name} fill={checkItemIcon[checked].color} style={{ width: 35, height: 35 }} />
    </TouchableOpacity>
}

const Checklists = () => {

  const { values } = useFormikContext<Report>();

  return (
    <Layout style={styles.container}>
      {values && values.checkList ? values.checkList.map((checkList, index) => {
        return (
          <CheckListLine item={checkList} index={index} />
        )
      }) : <Text>Nada Que ver Aqui</Text>}
    </Layout>
  )
}

export { Checklists }

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'flex-start',
    justifyContent: 'flex-start',
    padding: 5,
  }
})
