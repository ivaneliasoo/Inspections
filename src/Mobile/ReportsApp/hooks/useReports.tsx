import { useContext } from 'react';
import { ReportsContext } from '../contexts/ReportsContext';
import { API_HOST, API_KEY } from '../config/config';
import { Configuration, ReportsApi, CheckListsApiFactory } from '../services/api';
import { AuthContext } from '../contexts/AuthContext';
import { CheckListItemQueryResult, ReportQueryResult, UpdateCheckListItemCommand, UpdateReportCommand, SignaturesApi, SignatureQueryResult } from '../services/api';
import { UpdateOperationalReadingsCommand } from '../services/api/api';
import moment from 'moment';0

export const useReports = () => {
  const { authState: { userToken } } = useContext(AuthContext)
  const { getAll, setFilter, setWorkingReport, updateSignature, clearWorkingReport, reportsState, completeReport: complete, removeReport, setRefreshing } = useContext(ReportsContext)

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
      setRefreshing(true)
      const resp = await reportsApi.apiReportsGet(reportsState.filter, reportsState.isClosed, reportsState.myReports,reportsState.orderBy, reportsState.descendingSort)
      getAll(resp.data as unknown as ReportQueryResult[])
    } catch (error) {
      console.log(error)
    } finally {
      setRefreshing(false)
    }
  }

  const getReportById = async (id: number) => {
    const result: ReportQueryResult = (await reportsApi.apiReportsIdGet(id)).data as unknown as ReportQueryResult
    if (!result.date) result.date = new Date()
    else result.date = moment(result.date).toDate()
    setWorkingReport(result)
  }

  const completeReport = async (reportId: number) => {
    try {
      const result = await reportsApi.completeReport(reportId).catch(error => console.log(error.response.message))
      complete(reportId)
      return result
    } catch (error) {
      console.log(error)
    } finally {
    }
  }

  const deleteReport = async (reportId: number) => {
    try {
      const result = await reportsApi.apiReportsIdDelete(reportId).catch(error => console.log(error.response.message))
      removeReport(reportId)
      return result
    } catch (error) {
      console.log(error)
    } finally {
    }
  }

  const setFilterText = (text: string) => {
    setFilter({ ...reportsState, filter: text })
  }

  const setSorting = async (descending: boolean, sortBy: string) => {
    setFilter({ ...reportsState, descendingSort: descending, orderBy: sortBy })
  }

  const setOptions = async (isClosed: boolean, myReports: boolean) => {
    setFilter({ ...reportsState, filter: reportsState.filter, myReports, isClosed })
  }

  const updateCheckList = async (payload: { reportId: number; checkListId: number; newValue: number | undefined }) => {
    await reportsApi.bulkUpdateChecks(payload.reportId, payload.checkListId, payload.newValue)
  }

  const updateCheckListItem = async (payload: CheckListItemQueryResult) => {
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

  const saveSignature = async (s: { signature: SignatureQueryResult, index: number }) => {
    await signaturesApi.apiSignaturesIdPut(Number(s.signature.id), {
      id: s.signature.id!,
      title: s.signature.title!,
      annotation: s.signature.annotation!,
      responsibleType: s.signature.responsibleType!,
      responsibleName: s.signature.responsibleName!,
      designation: s.signature.designation!,
      remarks: s.signature.remarks!,
      date: s.signature.date!,
      principal: s.signature.principal!,
      drawnSign: s.signature.drawnSign!
    }).then(() => {
      updateSignature(s)
    })
  }

  const saveOperationalreadings = async (or: UpdateOperationalReadingsCommand) =>{
    await reportsApi.updateOperationalReadings(or.reportId!, or)
  }

  const saveReport = async (updateCmd: UpdateReportCommand) => {
    if (updateCmd && updateCmd.id) {
      await reportsApi.apiReportsIdPut(updateCmd.id.toString(), updateCmd)
      getReportById(updateCmd.id)
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
    setFilter,
    setWorkingReport,
    workingReport: reportsState.workingReport,
    reports: reportsState.reports || [],
    updateCheckList,
    updateCheckListItem,
    saveSignature,
    clearWorkingReport,
    saveOperationalreadings,
    setSorting,
    setOptions,
    reportsState,
    setRefreshing,
    refreshing: reportsState.refreshing
  }
}