import React, { createContext, useReducer } from 'react';
import { ReportQueryResult } from "../services/api";
import { ReportsFilterPayload, reportsReducer } from './reportsReducer';
import { CheckListQueryResult, SignatureQueryResult } from '../services/api';

export interface ReportsState {
  reports?: ReportQueryResult[];
  myReports: boolean;
  isClosed: boolean;
  filter: string;
  descendingSort: boolean;
  orderBy: string;
  workingReport?: ReportQueryResult;
  refreshing: boolean;
}

export interface ReportsContextProps {
  reportsState: ReportsState,
  setRefreshing: (isRefreshing: boolean) => void
  setFilter: (filter: ReportsFilterPayload) => void,
  getAll: (reports: ReportQueryResult[]) => void,
  removeReport: (id: number) => void,
  completeReport: (id: number) => void,
  setWorkingReport: (payload: ReportQueryResult) => void,
  updateCheckList: (payload: CheckListQueryResult) => void,
  updateSignature: (payload: { signature: SignatureQueryResult, index: number }) => void,
  clearWorkingReport: () => void,
}
const initialState: ReportsState = { reports: [], myReports: true, isClosed: false, filter: '', descendingSort: true, orderBy: 'date', workingReport: undefined!, refreshing: false }

export const ReportsContext = createContext({} as ReportsContextProps);
  const ReportsProvider = ({ children }: any) => {
  const [reportsState, dispatch] = useReducer(reportsReducer, initialState)

  //Actions
  const setRefreshing = (isRefreshing: boolean) => {
    dispatch({
      type: 'SET_REFRESHING',
      isRefreshing
    })
  }

  const setFilter = (payload: ReportsFilterPayload) => {
    dispatch({
      type: 'SET_FILTER',
      payload
    })
  }

  const getAll = (reports: ReportQueryResult[]) => {
    dispatch({
      type: 'SET_REPORTS',
      payload: { reports }
    })
  }

  const removeReport = (id: number) => {
    dispatch({
      type: 'REMOVE_REPORT',
      payload: { id }
    })
  }

  const completeReport = (id: number) => {
    dispatch({
      type: 'COMPLETE_REPORT',
      payload: { id }
    })
  }

  const setWorkingReport = (payload: ReportQueryResult) => {
    dispatch({
      type: 'SET_WORKING_REPORT',
      report: payload
    })
  }

  const clearWorkingReport = () => {
    dispatch({
      type: 'CLEAR_WORKING_REPORT',
    })
  }


  const updateCheckList = (payload: CheckListQueryResult) => {
    dispatch({
      type: 'UPDATE_CHECKLIST',
      payload
    })
  }

  const updateSignature = (payload: { signature: SignatureQueryResult, index: number }) => {
    dispatch({
      type: 'UPDATE_DRAWNSIGNATURE',
      payload
    })
  }

  return (
    <ReportsContext.Provider value={{
      setRefreshing,
      reportsState,
      setFilter,
      getAll,
      completeReport,
      removeReport,
      setWorkingReport,
      updateCheckList,
      updateSignature,
      clearWorkingReport,
    }}>
      {children}
    </ReportsContext.Provider>
  )
}

export { reportsReducer, ReportsProvider }