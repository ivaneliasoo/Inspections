import { AuthContext } from "authentication-context";

import React, { useReducer } from 'react';
import { Report } from "services/api";
export const ReportsContext = React.createContext({});

function reportsReducer(state, action) {
  switch (action.type) {
    case 'SET_REPORTS': {
      return { ...state, reports: action.payload.reports }
    }
    case 'SET_FILTERS_FILTER': {
      return { ...state, filter: action.payload.filter }
    }
    case 'SET_FILTERS_MYREPORTS': {
      console.log(action.payload.myReports)
      return { ...state, myReports: action.payload.myReports }
    }
    case 'SET_FILTERS_ISCLOSED': {
      return { ...state, isClosed: action.payload.isClosed }
    }
    default:
      throw new Error(`Unhandled action type: ${action.type}`);
  }
}

const initialState = { reports: [],  myReports: true, isClosed: false, filter: '' }

const ReportsProvider= ({ children }: any) => {
  const [state, dispatch] = useReducer(reportsReducer, initialState)
  
  //Actions
  const setFilter = (filter: string) => {
    dispatch({
      type: 'SET_FILTERS_FILTER',
      payload: {filter}
    })
  }

  const setMyReports = (myReports: boolean) => {
    dispatch({
      type: 'SET_FILTERS_MYREPORTS',
      payload: {myReports}
    })
  }

  const setIsClosed = (isClosed: boolean) => {
    dispatch({
      type: 'SET_FILTERS_ISCLOSED',
      payload: {isClosed}
    })
  }

  const getAll = (reports) => {
    dispatch({
      type: 'SET_REPORTS',
      payload: { reports }
    })
  }

  const getById = (id: number) => {
    const report: Report = {}
    dispatch({
      type: 'ADD_REPORT',
      payload: report
    })
  }

  const addReport = (report: Report) => {
    dispatch({
      type: 'ADD_REPORT',
      payload: report
    })
  }

  const editReport = (report: Report) => {
    dispatch({
      type: 'EDIT_REPORT',
      payload: report
    })
  }

  const deleteReport = (report: Report) => {
    dispatch({
      type: 'DELETE_REPORT',
      payload: report
    })
  }

  return (
    <ReportsContext.Provider value={{
      reports: state.reports,
      filter: state.filter,
      myReports: state.myReports,
      isClosed: state.isClosed,
      addReport,
      getAll,
      getById,
      editReport,
      deleteReport,
      setFilter,
      setMyReports,
      setIsClosed
      }}>
        {children}
    </ReportsContext.Provider>
  )
}

export {reportsReducer, ReportsProvider}