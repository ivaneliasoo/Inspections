import { CheckValue } from '../Models/CheckValue'
import { CheckListParamDTO } from '../ViewModels/CheckListParamDTO'

export interface AddCheckListItemCommand {
    idCheckList: number;
    text: string;
    checked: CheckValue;
    required: boolean;
    remarks: string;
    checklistParams: CheckListParamDTO[];
}
