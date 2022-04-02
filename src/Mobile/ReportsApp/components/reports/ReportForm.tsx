import React from 'react'
import { Text, Card } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'
import { Checklists } from './Checklists'
import { ScrollView } from 'react-native-gesture-handler'
import { useReports } from '../../hooks/useReports';
import { ParticullarOfInstallation } from './ParticullarOfInstallation'

const ReportForm = () => {

  const { workingCheckList, workingReport, updateCheckList, updateCheckListItem  } = useReports()
  return (
    <ScrollView style={{ backgroundColor: 'white'}}>
      <ParticullarOfInstallation />
      <Card
        style={{ marginHorizontal: -20, marginVertical: 0 }}
        
        header={() => <Text style={styles.header} category="s2">Checks Legend {workingCheckList ? workingCheckList[0]?.annotation! : ''}</Text>}
      >
        <Checklists
          checkLists={workingCheckList}
          onCheckListUpdated={(item: any) => updateCheckList({ ...item, reportId: workingReport!.id })}
          onCheckListItemUpdated={updateCheckListItem} />
      </Card>
    </ScrollView>
  )
}

export { ReportForm }

const styles = StyleSheet.create({
  header: {
    marginHorizontal: 25, marginTop: 20
  }
})
