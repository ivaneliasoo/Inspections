import { ReportsFilterPayload } from '../contexts/reportsReducer';
import { useContext, useState } from 'react';
import { ReportsContext } from '../contexts/ReportsContext';
import { API_HOST, API_KEY } from '../config/config';
import { Configuration, ReportsApi } from '../services/api';
import { AuthContext } from '../contexts/AuthContext';
import { Report } from '../services/api/api';


export const useReports = () => {
  const { authState: { userToken } } = useContext(AuthContext)
  const { getAll, setFilter, reportsState } = useContext(ReportsContext)

  const reportsApi = new ReportsApi({ accessToken: userToken, basePath: API_HOST, apiKey: API_KEY } as Configuration)

  const [refreshing, setRefreshing] = useState(false)

  const getReports = async () => {
    try {
      setRefreshing(true)
      const resp = await reportsApi.reportsGet(reportsState.filter, reportsState.isClosed, reportsState.myReports)
      getAll(resp.data as unknown as Report[])
    } catch (error) {
      console.log(error)
    } finally {
      setRefreshing(false)
    }
  }

  const completeReport = async (reportId: number) => {
    try {
      setRefreshing(true)
      const result = await reportsApi.completeReport(reportId).catch(error => console.log(error.response.message))
      return result
    } catch (error) {
      console.log(error)      
    } finally {
      setRefreshing(false)
    }
  }

  const deleteReport = async (reportId: number) => {
    try {
      setRefreshing(true)
      const result = await reportsApi.reportsIdDelete(reportId).catch(error => console.log(error.response.message))
      return result
    } catch (error) {
      console.log(error)      
    } finally {
      setRefreshing(false)
    }
  }

  const setFilterText = (text: string) => {
    setFilter({ filter: text, myReports: reportsState.myReports, isClosed: reportsState.isClosed })
  }

  return {
    getReports,
    completeReport,
    deleteReport,
    refreshing,
    filter: reportsState.filter,
    setFilterText,
    reports: reportsState.reports || []
  }
}