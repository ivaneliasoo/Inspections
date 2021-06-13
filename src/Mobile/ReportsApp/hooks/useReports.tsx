import { useContext } from 'react';
import { ReportsContext } from '../contexts/ReportsContext';
import { API_HOST, API_KEY } from '../config/config';
import { Configuration, ReportsApi, CheckListsApiFactory } from '../services/api';
import { AuthContext } from '../contexts/AuthContext';
import { CheckListItem, Report, UpdateCheckListItemCommand, UpdateReportCommand, SignaturesApi, Signature } from '../services/api/api';
import moment from 'moment';


export const useReports = () => {
  const { authState: { userToken } } = useContext(AuthContext)
  const { getAll, setFilter, setWorkingReport, updateSignature, clearWorkingReport, reportsState } = useContext(ReportsContext)

  const configuration = new Configuration({
    accessToken: userToken!,
    basePath: API_HOST,
    apiKey: API_KEY
  })

  const checkListsApi = CheckListsApiFactory(configuration)
  const reportsApi = new ReportsApi(configuration)
  const signaturesApi = new SignaturesApi(configuration)

  const getReports = async () => {
    try {
      const resp = await reportsApi.reportsGet(reportsState.filter, reportsState.isClosed, reportsState.myReports)
      getAll(resp.data as unknown as Report[])
    } catch (error) {
      console.log(error)
    } finally {
    }
  }

  const getReportById = async (id: number) => {
    const result: Report = (await reportsApi.reportsIdGet(id)).data as unknown as Report
    result.date = moment(result.date).toDate()
    setWorkingReport(result)
  }

  const completeReport = async (reportId: number) => {
    try {
      const result = await reportsApi.completeReport(reportId).catch(error => console.log(error.response.message))
      return result
    } catch (error) {
      console.log(error)
    } finally {
    }
  }

  const deleteReport = async (reportId: number) => {
    try {
      const result = await reportsApi.reportsIdDelete(reportId).catch(error => console.log(error.response.message))
      return result
    } catch (error) {
      console.log(error)
    } finally {
    }
  }

  const setFilterText = (text: string) => {
    setFilter({ filter: text, myReports: reportsState.myReports, isClosed: reportsState.isClosed })
  }

  const updateCheckList = async (payload: { reportId: number; checkListId: number; newValue: number | undefined }) => {
    await reportsApi.bulkUpdateChecks(payload.reportId, payload.checkListId, payload.newValue)
  }

  const updateCheckListItem = async (payload: CheckListItem) => {
    const command: UpdateCheckListItemCommand = {
      checkListId: payload.checkListId!,
      checked: payload.checked!,
      editable: payload.editable!,
      id: payload.id!,
      remarks: payload.remarks!,
      required: payload.required!,
      text: payload.text!
    }
    await checkListsApi.updateChecklistItem(payload.checkListId ?? -1, payload.id ?? -1, command)
  }

  const saveSignature = async (s: { signature: Signature, index: number }) => {
    await signaturesApi.signaturesIdPut(Number(s.signature.id), {
      id: s.signature.id,
      title: s.signature.title,
      annotation: s.signature.annotation,
      responsibleType: s.signature.responsible?.type,
      responsibleName: s.signature.responsible?.name,
      designation: s.signature.designation,
      remarks: s.signature.remarks,
      date: s.signature.date,
      principal: s.signature.principal,
      drawnSign: s.signature.drawnSign
    }).then(() => {
      updateSignature(s)
    })
  }

  const saveReport = async (updateCmd: UpdateReportCommand) => {
    if (updateCmd && updateCmd.id) {
      await reportsApi.reportsIdPut(updateCmd.id.toString(), updateCmd)
    }
  }

  return {
    getReports,
    getReportById,
    completeReport,
    deleteReport,
    saveReport,
    filter: reportsState.filter,
    setFilterText,
    setWorkingReport,
    workingReport: reportsState.workingReport,
    reports: reportsState.reports || [],
    workingRerport: reportsState.workingReport,
    updateCheckList,
    updateCheckListItem,
    saveSignature,
    clearWorkingReport
  }
}