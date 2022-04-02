import React, { useState } from 'react'
import { Text, Icon, Input, Card, List, ListItem, Button } from '@ui-kitten/components'
import { SegmentedControlIOSComponent, StyleSheet, TouchableOpacity, View } from 'react-native'
import { CheckListQueryResult, CheckListItemQueryResult, CheckValue } from '../../services/api'

const checkItemIcon = [
  { name: 'close-outline', color: 'red' },
  { name: 'checkmark-outline', color: 'green' },
  { name: 'minus-outline', color: 'orange' },
  { name: 'alert-circle-outline', color: 'blue' },
  { name: 'question-mark-outline', color: 'black' },
  { name: 'message-square-outline', color: 'black' }
]

const computeCheckListValue = (checkList: CheckListQueryResult) => {
  const totalAcceptable = checkList.checks!.filter(ci => ci.checked === CheckValue.Acceptable).length
  const totalNotAcceptable = checkList.checks!.filter(ci => ci.checked === CheckValue.NotAcceptableFalse).length
  const totalNotAplicable = checkList.checks!.filter(ci => ci.checked === CheckValue.NotApplicable || ci.checked === CheckValue.None).length
  const total = checkList.checks!.length

  if (total === totalAcceptable) return CheckValue.Acceptable
  else if (total === totalNotAplicable) return CheckValue.NotApplicable
  else if (total === totalNotAcceptable) return CheckValue.NotAcceptableFalse
  else return CheckValue.NotApplicable
}

const CheckListItemCheck = ({ item, index, onCheckUpdated }: any) => {
  const [showRemark, setShowRemarks] = useState(false)
  const [remark, setRemark] = useState(item.remarks)
  const onPress = () => {
    onCheckUpdated({...item, checked: item.checked, touched: true, remarks: remark})
  }

  const RightActions = (props: any) => {
    return <>
      <TouchableOpacity key={`check-${item.id}`} onPress={(e) => { e.preventDefault(); setShowRemarks(prev => !prev) }}>
        <Icon name='message-square-outline' fill='black' style={styles.lineIcon} />
      </TouchableOpacity>
      <Icon key={`icon-${item.id}`} name={checkItemIcon[item.checked ?? 0].name} {...props} fill={checkItemIcon[item.checked ?? 0].color} style={styles.lineIcon} />
    </>
  }

  return showRemark ?
      <Input
        key={`remark-input-${item.id}`}
        placeholder='type a remark'
        value={remark!}
        onChangeText={(text) => { setRemark(text) }}
        onSubmitEditing={() => { setShowRemarks(prev => !prev); onPress(); }}
        accessoryRight={() => <TouchableOpacity onPress={() => { setShowRemarks(prev => !prev); onPress(); }}><Icon name='close-outline' fill='black' style={styles.icon} /></TouchableOpacity>} />
    : <ListItem
      style={{ marginHorizontal: -15, padding: 0 }}
      key={`text-${item.id}`}
      title={() => <Text category='s2' status={!item.touched ? 'danger':''}>{`.${index + 1} - ${item.text}`}</Text>}
      onPress={onPress}
      description={() => <Text category='s2' appearance='hint'>{`Remarks: ${remark}`}</Text>}
      accessoryRight={RightActions}
    />
}

export interface CheckListItemProps {
  item: CheckListQueryResult;
  index: number;
  onChange: (checked: any) => any;
  onCheckUpdated: (payload: CheckListItemQueryResult) => any
}
const CheckListGroup = React.memo(({ item, index, onChange, onCheckUpdated, ...props }: CheckListItemProps) => {
  item.checked = computeCheckListValue(item)
  return (
    <Card 
      key={`list-${item.id}`}
      style={{ marginVertical: 3, elevation: 1, marginHorizontal: 0 }}
      header={() => <>
        <TouchableOpacity key={`touch-${item.id}`} onPress={(e) => { e.preventDefault(); onChange(item.checked) }} style={styles.line}>
          <Text key={`title-${item.id}`} style={styles.checkListTitle} category='s1' >{`${index + 1} - ${item.text}`}</Text>
          <Icon key={`titleicon-${item.id}`} name={checkItemIcon[item.checked].name} fill={checkItemIcon[item.checked].color} style={styles.icon} />
        </TouchableOpacity>
      </>}
    >
      {item && item.checks!.map((checkList, checkIndex) => {
        return <CheckListItemCheck key={`checkItemsList-${checkList.id}`} item={checkList} index={checkIndex} onCheckUpdated={onCheckUpdated} />
      })}
    </Card>
  )
})

export interface ChecklistsProps {
  checkLists: CheckListQueryResult[];
  onCheckListUpdated: () => void;
  onCheckListItemUpdated: () => void;
}
const Checklists = ({ checkLists = [], onCheckListUpdated, onCheckListItemUpdated }: any) => {
  return (
    <View>
      {checkLists && checkLists.map((checkList: CheckListQueryResult, index: number) => {
        return (
          <CheckListGroup item={checkList} index={index} key={`checklist-${checkList.id}`} onChange={(checked) => {
            onCheckListUpdated({ reportId: checkList.reportId, checkListId: checkList.id, newValue: checked })
          }}
            onCheckUpdated={(payload: CheckListItemQueryResult) => {
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
    paddingHorizontal: 5,
  },
  line: {
    flex: 1,
    marginHorizontal: 10,
    marginVertical: 10,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    fontWeight: '900'
  },
  icon: {
    width: 24,
    height: 24,
  },
  lineIcon: {
    width: 24,
    height: 24,
    marginStart: 14,
    marginEnd: -10
  },
  checkListTitle: { flex: 1, fontWeight: '900' },
  checkListSubtitle: { alignSelf: 'center', alignContent: 'center' },
})
