import React, { ReactChild, useEffect, useState } from "react"
import { Button, Card, Layout, Modal, Spinner, Text } from "@ui-kitten/components"
import { StyleSheet, View } from 'react-native'
import { Configuration, CreateReportCommand, ReportConfigurationApi, ReportsApi, ResumenReportConfiguration } from "../../services/api"
import { API_HOST, API_KEY } from "../../config/config"
import { useNavigation } from "@react-navigation/native"
import AsyncStorage from "@react-native-async-storage/async-storage"

type FooterProps = {
  isBusy: boolean
  onCancelPress: any
}
const Footer = ({ isBusy, onCancelPress }: FooterProps) => {
  return <Button style={styles.cancelButton} disabled={isBusy} size='small' status='danger' onPress={onCancelPress}>
    Cancel
  </Button>
}

type NewReportProps = {
  isOpen: boolean;
  onClose: any;
  onCreate: any;
}
export const NewReport = ({ isOpen, onClose, onCreate }: NewReportProps) => {
  const [isBusy, setIsBusy] = useState(false)
  const [templates, setTemplates] = useState<ResumenReportConfiguration[]>([])

  async function createReport(configuration: CreateReportCommand) {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportsApi = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY } as Configuration)
    const reportId = await reportsApi.apiReportsPost(configuration)
    return reportId
  }

  const callCreateReport = async (id: number) => {
    setIsBusy(true)
    await createReport({ configurationId: id, reportType: 0 })
      .then((resp) => onCreate(resp.data))
      .finally(() => { setIsBusy(false); })
  }

  async function getConfigurationTemplates() {
    const userToken: string = await AsyncStorage.getItem('userToken') as string;
    const reportConfigurationApi = new ReportConfigurationApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY } as Configuration)
    const result = await reportConfigurationApi.apiReportConfigurationGet();
    setTemplates(result.data)
    return result.data
  }

  

  useEffect(() => {
    getConfigurationTemplates()
  }, [])
  return (
    <>
      <Modal visible={isOpen} backdropStyle={styles.modalBackdrop}>
        <Card disabled={true} header={() => <View style={styles.modalHeader}>{!isBusy && <Text category='h5'>Create Report</Text>}</View>} footer={() => <Footer onCancelPress={onClose} isBusy={isBusy} />}>
          {!isBusy ?
            <>
              <Text>Select a template</Text>
              <Layout>
                {templates.map((tmpl: any, index) => (
                  <Card key={index} style={styles.modalCard} onPress={() => callCreateReport(tmpl.id)}>
                    <Text>{tmpl.title}</Text>
                    <Text> Creates a new {tmpl.title} With {tmpl.formName} Configuration</Text>
                  </Card>))}
              </Layout>
            </> : <View style={styles.progress}><Spinner status='info' size='giant' /><Text>Please Wait</Text></View>}
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
  },
  cancelButton: { margin: 3 },
  modalBackdrop: { backgroundColor: 'rgba(0, 0, 0, 0.5)' },
  modalHeader: { flex: 1 },
  modalCard: { margin: 5 }
})