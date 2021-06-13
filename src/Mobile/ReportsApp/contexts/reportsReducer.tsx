import { Report, CheckList } from 'services/api';
import { ReportsState } from './ReportsContext';

export interface ReportsFilterPayload {
  myReports: boolean;
  isClosed: boolean;
  filter: string;
}

type ReportsAction =
  | { type: 'SET_REPORTS'; payload: { reports: any[]; }; }
  | { type: 'SET_WORKING_REPORT', report: Report }
  | { type: 'SET_FILTER'; payload: ReportsFilterPayload }
  | { type: 'UPDATE_CHECKLIST'; payload: CheckList };

export const reportsReducer = (prevState: ReportsState, action: ReportsAction) => {
  switch (action.type) {
    case 'SET_REPORTS': {
      return { ...prevState, reports: action.payload.reports };
    }
    case 'SET_FILTER': {
      return { ...prevState, ...action.payload };
    }
    case 'SET_WORKING_REPORT': {
      return { ...prevState, workingReport: action.report };
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
