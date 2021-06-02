import React, { createContext, useReducer } from 'react';
import { Report } from "services/api";
import { reportsReducer } from './reportsreducer';
import { ReportsFilterPayload } from './reportsReducer';

export interface ReportsState {
  reports?: any[],
  myReports: boolean,
  isClosed: boolean,
  filter: string
}

export interface ReportsContextProps {
  setFilter: () => void,
  setMyReports: () => void,

}

export const ReportsContext = createContext({});

const initialState: ReportsState = { reports: [], myReports: true, isClosed: false, filter: '' }

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

  // const getById = (id: number) => {
  //   const report: Report = {}
  //   dispatch({
  //     type: 'ADD_REPORT',
  //     payload: report
  //   })
  // }

  // const addReport = (report: Report) => {
  //   dispatch({
  //     type: 'ADD_REPORT',
  //     payload: report
  //   })
  // }

  // const editReport = (report: Report) => {
  //   dispatch({
  //     type: 'EDIT_REPORT',
  //     payload: report
  //   })
  // }

  // const deleteReport = (report: Report) => {
  //   dispatch({
  //     type: 'DELETE_REPORT',
  //     payload: report
  //   })
  // }

  return (
    <ReportsContext.Provider value={{
      reportsState,
      getAll,
      setFilter,
    }}>
      {children}
    </ReportsContext.Provider>
  )
}

export { reportsReducer, ReportsProvider }