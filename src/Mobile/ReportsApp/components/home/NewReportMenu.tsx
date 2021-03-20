import AsyncStorage from "@react-native-async-storage/async-storage"
import { Button, Card, Layout, Modal, Spinner, Text } from "@ui-kitten/components"
import { AddIcon } from "../Icons"
import React, { useContext, useEffect, useState } from "react"
import {StyleSheet, View} from 'react-native'
import { API_HOST, API_KEY} from '../../config/config';
import { CreateReportCommand, ReportConfigurationApi, ReportsApi } from "../../services/api"
import { useNavigation } from "@react-navigation/core"
import { ReportsContext } from "../../reports-contexts"

export const NewReportMenu = () => {
  const [showAddReport, setShowAddReport] = useState(false)
  const [templates, setTemplates] = useState([])
  const [creatingReport, setCreatingReport] = useState(false)
  const navigation = useNavigation()
  const { getAll, reports, filter, myReports, isClosed, setFilter } = useContext<any>(ReportsContext)

  async function getReports() {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY })
    const resp = await reportsApi.reportsGet(filter, isClosed, myReports)
    getAll(resp.data)
  }

  const navigateToDetails = ({ reportId }) => {
    navigation.navigate('Details', { reportId })
  }

  async function getConfigurationTemplates() {
    setCreatingReport(true)
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportConfigurationApi = new ReportConfigurationApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY })
    const resp = await reportConfigurationApi.reportConfigurationGet();
    setTemplates(resp.data)
    setCreatingReport(false)
    
  }

  async function createReport(configuration: CreateReportCommand) {
    setCreatingReport(true)
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY })
    const reportId = await reportsApi.reportsPost(configuration)
    await getReports();
    setCreatingReport(false)
    setShowAddReport(false)
    navigateToDetails({reportId: reportId.data})
  }

  useEffect(() => {
    getConfigurationTemplates()
    return () => {

    }
  }, [])

  return (
    <>
      <Button accessoryLeft={AddIcon} appearance='ghost' status='primary' onPress={() => setShowAddReport(!showAddReport)}></Button>
      <Modal visible={showAddReport} backdropStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.5)' }}>
        <Card disabled={true} header={() => <View style={{ flex: 1 }}>{!creatingReport && <Text category='h5'>Create Report</Text>}</View>} footer={() =>
        (
          !creatingReport && <Button style={{ margin: 3 }} disabled={creatingReport} size='small' status='danger' onPress={(e) => setShowAddReport(false)}>
             Cancel
            </Button>
        )}>
        {!creatingReport ?
          <>
          <Text>Select a template</Text>
          <Layout>
            {templates.map((tmpl, index) => {
              return (
                <Card key={index} style={{ margin: 5 }} onPress={async () => await createReport({ configurationId: tmpl.id, reportType: 0 })}>
                  <Text>{tmpl.title}</Text>
                  <Text> Creates a new Report With {tmpl.title} Configuration</Text>
                </Card>);
            })}
          </Layout>
          </>:<View style={styles.progress}><Spinner status='info' size='giant' /><Text>Please Wait</Text></View>}
        </Card>
      </Modal>
    </>
  );
}

const styles = StyleSheet.create({
  progress: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  }
})