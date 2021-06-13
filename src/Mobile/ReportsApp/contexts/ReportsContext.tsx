import React, { createContext, useReducer } from 'react';
import { Report } from "services/api";
import { ReportsFilterPayload, reportsReducer } from './reportsReducer';
import { CheckList, Signature } from '../services/api/api';

export interface ReportsState {
  reports?: Report[];
  myReports: boolean;
  isClosed: boolean;
  filter: string;
  workingReport?: Report;
}

export interface ReportsContextProps {
  reportsState: ReportsState,
  setFilter: (filter: ReportsFilterPayload) => void,
  getAll: (reports: Report[]) => void,
  setWorkingReport: (payload: Report) => void,
  updateCheckList: (payload: CheckList) => void,
  updateSignature: (payload: { signature: Signature, index: number }) => void,
  clearWorkingReport: () => void
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

  const getAll = (reports: Report[]) => {
    dispatch({
      type: 'SET_REPORTS',
      payload: { reports }
    })
  }

  const setWorkingReport = (payload: Report) => {
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


  const updateCheckList = (payload: CheckList) => {
    dispatch({
      type: 'UPDATE_CHECKLIST',
      payload
    })
  }

  const updateSignature = (payload: { signature: Signature, index: number }) => {
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
      clearWorkingReport
    }}>
      {children}
    </ReportsContext.Provider>
  )
}

export { reportsReducer, ReportsProvider }