import React, { createContext, useReducer } from 'react';
import { Report } from "services/api";
import { ReportsFilterPayload, reportsReducer } from './reportsReducer';

export interface ReportsState {
  reports?: any[];
  myReports: boolean;
  isClosed: boolean;
  filter: string;
  workingReport?: number;
}

export interface ReportsContextProps {
  reportsState: ReportsState,
  setFilter: (filter: ReportsFilterPayload) => void,
  getAll: (reports: Report[]) => void,
  setWorkingReport: (payload: number) => void,
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

  const setWorkingReport = (payload: number) => {
    dispatch({
      type: 'SET_WORKING_REPORT',
      payload
    })
  }

  return (
    <ReportsContext.Provider value={{
      reportsState,
      setFilter,
      getAll,
      setWorkingReport
    }}>
      {children}
    </ReportsContext.Provider>
  )
}

export { reportsReducer, ReportsProvider }