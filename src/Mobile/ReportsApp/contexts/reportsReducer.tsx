import { Report, CheckList } from 'services/api';
import { ReportsState } from './ReportsContext';
import { Signature } from '../services/api/api';

export interface ReportsFilterPayload {
  myReports: boolean;
  isClosed: boolean;
  filter: string;
}

type ReportsAction =
  | { type: 'SET_REPORTS'; payload: { reports: any[]; }; }
  | { type: 'SET_WORKING_REPORT', report: Report }
  | { type: 'CLEAR_WORKING_REPORT' }
  | { type: 'SET_FILTER'; payload: ReportsFilterPayload }
  | { type: 'SET_OPERATIONAL_READINGS'; payload: any }
  | { type: 'UPDATE_CHECKLIST'; payload: CheckList }
  | { type: 'UPDATE_DRAWNSIGNATURE'; payload: { signature: Signature, index: number } };

export const reportsReducer = (prevState: ReportsState, action: ReportsAction) => {
  switch (action.type) {
    case 'SET_REPORTS': {
      return { ...prevState, reports: action.payload.reports };
    }
    case 'SET_OPERATIONAL_READINGS': {
      console.log('por aqui pase')
      return { ...prevState, workingOperationalReadings: action.payload };
    }
    case 'SET_FILTER': {
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
      const index = prevState.workingReport!.checkList?.findIndex(ck => ck.id === action.payload.id)
      return {
        ...prevState,
        workingReport: {
          ...prevState.workingReport,
          checkList: [...prevState.workingReport!.checkList!.slice(0, index), { ...action.payload }, ...prevState.workingReport!.checkList!.slice(index)]
        }
      }
    }
    default:
      return prevState
  }
};
