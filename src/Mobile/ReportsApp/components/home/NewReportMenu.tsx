import AsyncStorage from "@react-native-async-storage/async-storage"
import { Button, Card, Layout, Modal, Text } from "@ui-kitten/components"
import { AddIcon } from "../Icons"
import React, { useEffect, useState } from "react"
import {View} from 'react-native'
import { CreateReportCommand, ReportConfigurationApi, ReportsApi } from "../../services/api"

export const NewReportMenu = () => {
  const [showAddReport, setShowAddReport] = useState(false)
  const [templates, setTemplates] = useState([])
  const [creatingReport, setCreatingReport] = useState(false)

  async function getConfigurationTemplates() {
    setCreatingReport(true)
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportConfigurationApi = new ReportConfigurationApi({ accessToken: userToken, basePath: 'http://192.168.88.250:5000', password: undefined, username: undefined, apiKey: 'falskjdufghasjdghfaskjdhgfa' })
    const resp = await reportConfigurationApi.reportConfigurationGet();
    setTemplates(resp.data)
    setCreatingReport(false)
  }

  async function createReport(configuration: CreateReportCommand) {
    setCreatingReport(true)
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: 'http://192.168.88.250:5000', password: undefined, username: undefined, apiKey: 'falskjdufghasjdghfaskjdhgfa' })
    const reportId = await reportsApi.reportsPost(configuration)
    setCreatingReport(false)
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
        <Card disabled={true} header={() => <View style={{ flex: 1 }}><Text category='h5'>Create Report</Text></View>} footer={() =>
        (
          <Button style={{ margin: 3 }} size='small' status='danger' onPress={() => setShowAddReport(false)}>
            Cancel
            </Button>
        )}>
          <Text>Select a template</Text>
          <Layout>
            {templates.map((tmpl, index) => {
              return (
                <Card key={index} style={{ margin: 5 }} onPress={() => createReport({ configurationId: tmpl.id, reportType: 0 })}>
                  <Text>{tmpl.title}</Text>
                  <Text> Creates a new Report With {tmpl.title} Configuration</Text>
                </Card>);
            })}
          </Layout>
        </Card>
      </Modal>
    </>
  );
}