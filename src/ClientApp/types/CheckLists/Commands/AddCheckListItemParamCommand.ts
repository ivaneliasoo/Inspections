import { CheckListParamDTO } from '../ViewModels/CheckListParamDTO'

export interface AddCheckListItemParamCommand {
    idCheckList: number;
    idCheckListItem: number;
    checkListParams: CheckListParamDTO[];
}
