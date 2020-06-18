import { CheckListParamDTO } from '../ViewModels/CheckListParamDTO'

export interface AddCheckListParamCommand {
    idCheckList: number;
    checklistParams: CheckListParamDTO[];
}
