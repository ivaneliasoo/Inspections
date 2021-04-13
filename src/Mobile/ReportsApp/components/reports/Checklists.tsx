import React, { useState } from 'react'
import { Layout, Text, Icon, Spinner } from '@ui-kitten/components'
import { StyleSheet, TouchableOpacity, View } from 'react-native'
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
  index: number,
  onChange: (checked: any) => any
}
const CheckListLine = ({ item, index, onChange, ...props }: CheckListItemProps) => {
  const [checked, setChecked] = useState(2)
  const onPress = () => {

    if(checked < 2)
      setChecked(checked + 1)
    else
      setChecked(0)
    
    onChange(checked)

  }

  return (
    <>
        <TouchableOpacity key={index} onPress={onPress} style={styles.line}>
            <Text style={styles.checkListTitle} category='s1' >{`${index + 1} - ${item.text}`}</Text>
            <Text style={styles.checkListSubtitle} category='c1'>{CheckValue[checked]}<Icon name={checkItemIcon[checked].name} fill={checkItemIcon[checked].color} style={styles.icon} /></Text>
        </TouchableOpacity>
      {
        item && item.checks && item.checks.map((checkItem, checkIndex) => {
          return <CheckListItemCheck key={`checkIcon${checkIndex}`} checkItem={checkItem} itemIndex={index} checkIndex={checkIndex} {...props} />
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
const CheckListItemCheck = ({ checkItem, checkIndex, itemIndex }: CheckListItemCheckProps) => {
  if(checkItem.checked === 3) 
    checkItem.checked = 2

  return <TouchableOpacity style={styles.line} key={`${itemIndex}-${checkIndex}`}>
        <Text style={} category='s2'>{`${itemIndex + 1}.${checkIndex + 1} - ${checkItem.text} (${CheckValue[checkItem.checked!].toLocaleUpperCase()})`}</Text>
        <Icon name={checkItemIcon[checkItem.checked!].name} fill={checkItemIcon[checkItem.checked!].color} style={styles.lineIcon} />
    </TouchableOpacity>
}

const Checklists = () => {

  const { values, setFieldValue } = useFormikContext<Report>();

  return (
    <Layout style={styles.container}>
      {values && values.checkList ? values.checkList.map((checkList, index) => {
        return (
          <CheckListLine item={checkList} index={index} key={index} onChange={(checked) => { 
            checked = checked === 2 ? 0 : checked + 1
            setFieldValue(`checkList[${index}].checked`, checked)
            checkList.checks?.forEach((check, checkIndex) => setFieldValue(`checkList[${index}].checks[${checkIndex}].checked`, checked))
        }} />
        )
      }) : 
      <>
        <View style={styles.checkListLoading}>
          <Spinner size="medium" status="primary" />
          <Text>   Loading Checks...</Text>
        </View>
      </>
      }
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
  },
  line: {
    flex: 1,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center' 
  },
  icon: {
    width: 30, 
    height: 30
  },
  lineIcon: {
    width: 35,
    height: 35 
  },
  checkListLoading: {
    flex: 1,
    flexDirection: 'row',
    alignSelf: 'center',
    justifyContent: 'space-around',
  },
  checkListItem: { flex: 10 },
  checkListTitle: {flex: 10,  fontWeight: '900' },
  checkListSubtitle: { alignSelf: 'center', alignContent: 'center' },
})
