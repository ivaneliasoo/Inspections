import { ReportsState } from './ReportsContext';

export interface ReportsFilterPayload {
  myReports: boolean;
  isClosed: boolean;
  filter: string;
}

type ReportsAction =
  | { type: 'SET_REPORTS'; payload: { reports: any[]; }; }
  | { type: 'SET_FILTER'; payload: ReportsFilterPayload };

export const reportsReducer = (prevState: ReportsState, action: ReportsAction) => {
  switch (action.type) {
    case 'SET_REPORTS': {
      return { ...prevState, reports: action.payload.reports };
    }
    case 'SET_FILTER': {
      return { ...prevState, ...action.payload };
    }
    default:
      return prevState
  }
};
