import React from 'react'
import { Text, Card } from '@ui-kitten/components'
import { StyleSheet } from 'react-native'
import { Checklists } from './Checklists'
import { ScrollView } from 'react-native-gesture-handler'
import { useReports } from '../../hooks/useReports';

const ReportForm = () => {

  const { workingReport: reportData, saveReport, updateCheckList, updateCheckListItem } = useReports()

  return (
    <ScrollView>
      <Card
        header={() => <Text style={styles.header} appearance="hint" category="h5">Checks Legend {reportData?.checkList ? reportData?.checkList![0]?.annotation! : ''}</Text>}
      >
        <Checklists
          checkLists={reportData?.checkList}
          onCheckListUpdated={(item: any) => updateCheckList({ ...item, reportId: reportData?.id })}
          onCheckListItemUpdated={updateCheckListItem} />
      </Card>
    </ScrollView>
  )
}

export { ReportForm }

const styles = StyleSheet.create({
  header: {
    marginHorizontal: 5, marginTop: 20
  }
})
