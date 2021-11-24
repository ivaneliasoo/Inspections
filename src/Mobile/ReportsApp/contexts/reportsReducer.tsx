import { ReportQueryResult, CheckListQueryResult, SignatureQueryResult } from '../services/api';
import { ReportsState } from './ReportsContext';

export interface ReportsFilterPayload {
  myReports: boolean;
  isClosed: boolean;
  filter: string;
  descendingSort: boolean;
  orderBy: string;
}

type ReportsAction =
  | { type: 'SET_REFRESHING'; isRefreshing: boolean; }
  | { type: 'SET_REPORTS'; payload: { reports: ReportQueryResult[]; }; }
  | { type: 'REMOVE_REPORT'; payload: { id: number; }; }
  | { type: 'COMPLETE_REPORT'; payload: { id: number; }; }
  | { type: 'SET_WORKING_REPORT', payload: { report: ReportQueryResult } }
  | { type: 'SET_WORKING_CHECKS', payload: { checklists: CheckListQueryResult[] }}
  | { type: 'CLEAR_WORKING_REPORT' }
  | { type: 'SET_FILTER'; payload: ReportsFilterPayload }
  | { type: 'SET_OPERATIONAL_READINGS'; payload: any }
  | { type: 'UPDATE_CHECKLIST'; payload: CheckListQueryResult }
  | { type: 'UPDATE_DRAWNSIGNATURE'; payload: { signature: SignatureQueryResult, index: number } }
  | { type: 'UPDATE_CHECKLIST_ITEMS'; payload: { checklistId: number, newValue: number } }
  | { type: 'UPDATE_CHECKLIST_ITEM'; payload: { checklistItemId: number, newValue: number } };

export const reportsReducer = (prevState: ReportsState, action: ReportsAction) => {
  switch (action.type) {
    case 'SET_REFRESHING': {
      return { ...prevState, refreshing: action.isRefreshing }
    }

    case 'SET_REPORTS': {
      return { ...prevState, reports: action.payload.reports };
    }
    case 'REMOVE_REPORT': {
      return { ...prevState, reports: prevState.reports!.filter((r: ReportQueryResult) => r.id !== action.payload.id) };
    }
    case 'COMPLETE_REPORT': {
      const index = prevState.reports!.findIndex((r: ReportQueryResult) => r.id === action.payload.id)
      const report = prevState.reports![index]
      report.isClosed = true;
      prevState.reports!.splice(index, 1, report)
      return { ...prevState, reports: [...prevState.reports!] };
    }
    case 'SET_OPERATIONAL_READINGS': {
      return { ...prevState, workingOperationalReadings: action.payload };
    }
    case 'SET_FILTER': {
      return { ...prevState, ...action.payload };
    }
    case 'SET_WORKING_REPORT': {
      return { ...prevState, workingReport: action.payload.report };
    }
    case 'SET_WORKING_CHECKS': {
      return { ...prevState, workingCheckList: action.payload.checklists };
    }
    case 'CLEAR_WORKING_REPORT': {
      return { ...prevState, workingReport: {}, workingCheckList: [] };
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
      const index = prevState.workingCheckList?.findIndex(ck => ck.id === action.payload.id)
      return {
        ...prevState,
        workingCheckList: [...prevState.workingCheckList!.slice(0, index), { ...action.payload }, ...prevState.workingCheckList!.slice(index)]
      }
    }
    case 'UPDATE_CHECKLIST_ITEMS': {
      const index = prevState.workingCheckList!.findIndex(ck => ck.id === action.payload.checklistId)
      const checklist = prevState.workingCheckList![index!]
      checklist.checked = action.payload.newValue as unknown as boolean
      if (checklist.checks) {
        checklist.checks.forEach(check => {
          check.checked = action.payload.newValue
          check.touched = true
        })
      }
      return {
        ...prevState,
      }
    }
    default:
      return prevState
  }
};
