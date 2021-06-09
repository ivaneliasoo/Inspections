import React, { useState } from 'react'
import { Text, Icon, Input, Card, List, ListItem, Button } from '@ui-kitten/components'
import { StyleSheet, TouchableOpacity, View } from 'react-native'
import { CheckList, CheckListItem, CheckValue } from '../../services/api'

const checkItemIcon = [
  { name: 'close-outline', color: 'red' },
  { name: 'checkmark-outline', color: 'green' },
  { name: 'minus-outline', color: 'orange' },
  { name: 'alert-circle-outline', color: 'blue' },
  { name: 'question-mark-outline', color: 'black' },
  { name: 'message-square-outline', color: 'black' }
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

const CheckListItemCheck = ({ item, index }) => {
  const persistedValue = item.checked! >= 3 ? 2 : item.checked ?? 2
  const [showRemark, setShowRemarks] = useState(false)

  const RightActions = (props) => {
    return <>
      <Icon name={checkItemIcon[persistedValue].name} {...props} fill={checkItemIcon[persistedValue].color} style={styles.lineIcon} />
    </>
  }

  const LeftActions = () => {
    return (
      <TouchableOpacity onPress={() => setShowRemarks(prev => !prev)}>
        <Icon name='message-square-outline' fill='black' style={styles.icon} />
      </TouchableOpacity>
    )
  }

  return showRemark ?
    <>
    <Input placeholder='type a remark' value={item.remarks!} accessoryRight={() => <Icon name='close-outline' fill='black' style={styles.icon} />} />
    <Button onPress={() => setShowRemarks(prev => !prev)}>Save</Button>
    </>
    : <ListItem
      title={() => <Text category='h6'>{`.${index + 1} - ${item.text}`}</Text>}
      description={() => <Text category='s1' appearance='hint'>{`Remarks: ${item.remarks}`}</Text>}
      accessoryRight={RightActions}
      accessoryLeft={LeftActions}
    />
}

export interface CheckListItemProps {
  item: CheckList;
  index: number;
  onChange: (checked: any) => any;
  onCheckUpdated: (payload: CheckListItem) => any
}
const CheckListGroup = React.memo(({ item, index, onChange, onCheckUpdated, ...props }: CheckListItemProps) => {
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
    <Card style={{ marginVertical: 5, elevation: 5 }}
      header={() => <>
        <TouchableOpacity key={index} onPress={onPress} style={styles.line}>
          <Text style={styles.checkListTitle} category='h6' >{`${index + 1} - ${item.text}`}</Text>
          <Icon name={checkItemIcon[checked].name} fill={checkItemIcon[checked].color} style={styles.icon} />
        </TouchableOpacity>
      </>}
    >
      {item && item.checks!.map((checkList, checkIndex) => {
        return <CheckListItemCheck item={checkList} index={checkIndex} />
      })}
    </Card>
  )
})

export interface ChecklistsProps {
  checkLists: CheckList[];
  onCheckListUpdated: () => void;
  onCheckListItemUpdated: () => void;
}
const Checklists = ({ checkLists = [], onCheckListUpdated, onCheckListItemUpdated }: any) => {
  return (
    <View>
      {checkLists && checkLists.map((checkList: CheckList, index: number) => {
        return (
          <CheckListGroup item={checkList} index={index} key={index} onChange={(checked) => {
            onCheckListUpdated({ reportId: checkList.reportId, checkListId: checkList.id, newValue: checked + 1 > 2 ? 0 : checked + 1 })
          }}
            onCheckUpdated={(payload: CheckListItem) => {
              onCheckListItemUpdated(payload)
            }}
          />
        )
      })
      }
    </View>
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
    marginHorizontal: 20,
    marginVertical: 20,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center'
  },
  icon: {
    width: 40,
    height: 40,
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
  checkListTitle: { flex: 10, fontWeight: '900' },
  checkListSubtitle: { alignSelf: 'center', alignContent: 'center' },
})
