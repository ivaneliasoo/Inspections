import { ReportQueryResult, CheckListQueryResult } from '../services/api';
import { ReportsState } from './ReportsContext';
import { SignatureQueryResult } from '../services/api';

export interface ReportsFilterPayload {
  myReports: boolean;
  isClosed: boolean;
  filter: string;
  descendingSort: boolean;
  orderBy: string;
}

type ReportsAction =
  | { type: 'SET_REFRESHING'; isRefreshing: boolean; }
  | { type: 'SET_REPORTS'; payload: { reports: any[]; }; }
  | { type: 'REMOVE_REPORT'; payload: { id: number; }; }
  | { type: 'COMPLETE_REPORT'; payload: { id: number; }; }
  | { type: 'SET_WORKING_REPORT', report: ReportQueryResult }
  | { type: 'CLEAR_WORKING_REPORT' }
  | { type: 'SET_FILTER'; payload: ReportsFilterPayload }
  | { type: 'SET_OPERATIONAL_READINGS'; payload: any }
  | { type: 'UPDATE_CHECKLIST'; payload: CheckListQueryResult }
  | { type: 'UPDATE_DRAWNSIGNATURE'; payload: { signature: SignatureQueryResult, index: number } };

export const reportsReducer = (prevState: ReportsState, action: ReportsAction) => {
  switch (action.type) {
    case 'SET_REFRESHING': {
      return {...prevState, refreshing: action.isRefreshing}
    }

    case 'SET_REPORTS': {
      return { ...prevState, reports: action.payload.reports };
    }
    case 'REMOVE_REPORT': {
      return { ...prevState, reports: prevState.reports!.filter((r: ReportQueryResult) =>r.id !== action.payload.id) };
    }
    case 'COMPLETE_REPORT': {
      const index = prevState.reports!.findIndex((r: ReportQueryResult) =>r.id === action.payload.id)
      const report = prevState.reports![index]
      report.isClosed = true;
      prevState.reports!.splice(index, 1, report)
      return { ...prevState, reports: [...prevState.reports!]};
    }
    case 'SET_OPERATIONAL_READINGS': {
      return { ...prevState, workingOperationalReadings: action.payload };
    }
    case 'SET_FILTER': {
      console.log({ ...prevState, ...action.payload })
      return { ...prevState, ...action.payload };
    }
    case 'SET_WORKING_REPORT': {
      return { ...prevState, workingReport: action.report };
    }
    case 'CLEAR_WORKING_REPORT': {
      return { ...prevState, workingReport: {} };
    }
    case 'UPDATE_DRAWNSIGNATURE': {
      let signature = {};
      if (prevState.workingReport?.signatures) {
        signature = action.payload.signature || {}
        prevState.workingReport.signatures.splice(action.payload.index, 1, signature)
      }
      return {
        ...prevState,
        workingReport: {
          ...prevState.workingReport,
          signatures: [...prevState.workingReport?.signatures!]
        }
      }
    }
    case 'UPDATE_CHECKLIST': {
      const index = prevState.workingReport!.checkLists?.findIndex(ck => ck.id === action.payload.id)
      return {
        ...prevState,
        workingReport: {
          ...prevState.workingReport,
          checkList: [...prevState.workingReport!.checkLists!.slice(0, index), { ...action.payload }, ...prevState.workingReport!.checkLists!.slice(index)]
        }
      }
    }
    default:
      return prevState
  }
};
