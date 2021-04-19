import React, { useEffect, useMemo, useState } from 'react'
import { Layout, Text, Icon, Spinner } from '@ui-kitten/components'
import { StyleSheet, TouchableOpacity, View } from 'react-native'
import { useFormikContext } from 'formik'
import { CheckList, CheckListItem, CheckValue, Report } from '../../services/api'

const checkItemIcon = [
  { name: 'close-outline', color: 'red' },
  { name: 'checkmark-outline', color: 'green' },
  { name: 'minus-outline', color: 'orange' },
  { name: 'alert-circle-outline', color: 'blue' },
  { name: 'question-mark-outline', color: 'black' }
]

const computeCheckListValue = (checkList: CheckList) => {
  const totalAcceptable = checkList.checks!.filter(ci => ci.checked === CheckValue.Acceptable).length
  const totalNotAcceptable = checkList.checks!.filter(ci => ci.checked === CheckValue.NotAcceptable).length
  const totalNotAplicable = checkList.checks!.filter(ci => ci.checked === CheckValue.NotAplicable).length
  const total = checkList.checks!.length

  if (total === totalAcceptable) return CheckValue.Acceptable
  else if (total === totalNotAplicable) return CheckValue.NotAplicable
  else if (total === totalNotAcceptable) return CheckValue.NotAcceptable
  else return CheckValue.NotAplicable
}

type CheckListItemProps = {
  item: CheckList,
  index: number,
  onChange: (checked: any) => any,
  onCheckUpdated: (payload: CheckListItem) => any
}
const CheckListLine = React.memo(({ item, index, onChange, onCheckUpdated, ...props }: CheckListItemProps) => {
  const [checked, setChecked] = useState(computeCheckListValue(item) ?? 3)
  const onPress = () => {

    if (checked >= 2)
      setChecked(1)
    else if (checked === 1)
      setChecked(0)
    else if (checked === 0)
      setChecked(2)


    onChange(checked)


  }

  return (
    <>
      <TouchableOpacity key={index} onPress={onPress} style={styles.line}>
        <Text style={styles.checkListTitle} category='s1' >{`${index + 1} - ${item.text}`}</Text>
        <Icon name={checkItemIcon[checked].name} fill={checkItemIcon[checked].color} style={styles.icon} />
      </TouchableOpacity>
      {
        item && item.checks && item.checks.map((checkItem, checkIndex) => {
          return <CheckListItemCheck key={`checkIcon${checkIndex}`} checkItem={checkItem} itemIndex={index} checkIndex={checkIndex} onPress={onCheckUpdated} {...props} />
        })
      }
    </>
  )
})

type CheckListItemCheckProps = {
  checkItem: CheckListItem,
  checkIndex: number,
  itemIndex: number,
  onPress: (payload: CheckListItem) => any
}
const CheckListItemCheck = ({ checkItem, checkIndex, itemIndex, onPress }: CheckListItemCheckProps) => {
  const persistedValue = checkItem.checked! >= 3 ? 2 : checkItem.checked ?? 2
  const [itemChecked, setItemChecked] = useState(persistedValue)


  useEffect(() => {
    setItemChecked(persistedValue)
    checkItem.checked! =  persistedValue
  }, [])

  return <TouchableOpacity style={styles.line} key={`${itemIndex}-${checkIndex}`} onPress={() => {
     if (itemChecked! >= 2) {
      setItemChecked(1)
      checkItem.checked! = 1
    }
    else if (itemChecked === 1) { setItemChecked(0); checkItem.checked! = 0 }
    else if (itemChecked === 0) { setItemChecked(2); checkItem.checked! = 2 }

    onPress(checkItem)
  }}>
    <Text
      style={styles.checkListItem}
      category='s2'>{`${itemIndex + 1}.${checkIndex + 1} - ${checkItem.text} (${CheckValue[persistedValue]}) value: ${checkItem.checked}`}
    </Text>
    <Icon name={checkItemIcon[persistedValue].name} fill={checkItemIcon[persistedValue].color} style={styles.lineIcon} />
  </TouchableOpacity>
}

const Checklists = ({ onCheckListUpdated, onCheckListItemUpdated }: any) => {

  const { values, setFieldValue } = useFormikContext<Report>();



  return (
    <Layout style={styles.container}>
      {values && values.checkList ? values.checkList.map((checkList, index) => {
        return (
          <CheckListLine item={checkList} index={index} key={index} onChange={(checked) => {
            setFieldValue(`checkList[${index}].checked`, checked)
            checkList.checks?.forEach((check, checkIndex) => setFieldValue(`checkList[${index}].checks[${checkIndex}].checked`, checked + 1 > 2 ? 0 : checked + 1))
            onCheckListUpdated({ reportId: checkList.reportId, checkListId: checkList.id, newValue: checked + 1 > 2 ? 0 : checked + 1 })
          }}
            onCheckUpdated={(payload: CheckListItem) => {
              const item = checkList.checks?.findIndex(it => it.id === payload.id)
              setFieldValue(`checkList[${index}].checks[${item}].checked`, payload.checked! + 1 > 2 ? 0 : payload.checked! + 1)

              setFieldValue(`checkList[${index}].checked`, computeCheckListValue(checkList))

              onCheckListItemUpdated(payload)
            }}
          />
        )
      }) :
        <>
          <View style={styles.checkListLoading}>
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
  checkListTitle: { flex: 10, fontWeight: '900' },
  checkListSubtitle: { alignSelf: 'center', alignContent: 'center' },
})
