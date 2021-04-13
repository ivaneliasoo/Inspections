import React from "react"
import { Button, Card, Layout, Modal, Spinner, Text } from "@ui-kitten/components"
import { AddIcon } from "../Icons"
import {StyleSheet, View} from 'react-native'
import { ResumenReportConfiguration } from "../../services/api"

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
  templates: ResumenReportConfiguration[]
  isOpen: boolean
  isBusy: boolean
  onTemplatePress: any
  onCreatePress: any
  onCancelPress: any
}
export const NewReportMenu = ({ templates, isOpen, isBusy, onTemplatePress, onCreatePress, onCancelPress }: NewReportProps) => {
  return (
    <>
      <Button accessoryLeft={AddIcon} appearance='ghost' status='primary' onPress={onCreatePress}></Button>
      <Modal visible={isOpen} backdropStyle={styles.modalBackdrop}>
        <Card disabled={true} header={() => <View style={styles.modalHeader}>{!isBusy && <Text category='h5'>Create Report</Text>}</View>} footer={() => <Footer onCancelPress={onCancelPress} isBusy={isBusy}/>}>
        {!isBusy ?
          <>
          <Text>Select a template</Text>
          <Layout>
            {templates.map((tmpl: any, index) => (
              <Card key={index} style={styles.modalCard} onPress={() => onTemplatePress(tmpl.id)}>
                <Text>{tmpl.title}</Text>
                <Text> Creates a new Report With {tmpl.title} Configuration</Text>
              </Card>))}
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
  },
  cancelButton: { margin: 3 },
  modalBackdrop: { backgroundColor: 'rgba(0, 0, 0, 0.5)' },
  modalHeader: { flex: 1 },
  modalCard: { margin: 5 }
})