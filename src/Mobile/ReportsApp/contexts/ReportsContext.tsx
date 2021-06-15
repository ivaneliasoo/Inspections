import React, { createContext, useReducer } from 'react';
import { ReportQueryResult } from "../services/api";
import { ReportsFilterPayload, reportsReducer } from './reportsReducer';
import { CheckListQueryResult, SignatureQueryResult } from '../services/api';

export interface ReportsState {
  reports?: ReportQueryResult[];
  myReports: boolean;
  isClosed: boolean;
  filter: string;
  workingReport?: ReportQueryResult;
}

export interface ReportsContextProps {
  reportsState: ReportsState,
  setFilter: (filter: ReportsFilterPayload) => void,
  getAll: (reports: ReportQueryResult[]) => void,
  setWorkingReport: (payload: ReportQueryResult) => void,
  updateCheckList: (payload: CheckListQueryResult) => void,
  updateSignature: (payload: { signature: SignatureQueryResult, index: number }) => void,
  clearWorkingReport: () => void,
}
const initialState: ReportsState = { reports: [], myReports: true, isClosed: false, filter: '', workingReport: undefined! }

export const ReportsContext = createContext({} as ReportsContextProps);
  const ReportsProvider = ({ children }: any) => {
  const [reportsState, dispatch] = useReducer(reportsReducer, initialState)

  //Actions
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
      reportsState,
      setFilter,
      getAll,
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