import { CheckListParamDTO } from '../CheckListParamDTO'
import { CheckValue } from '../Models'

export interface AddCheckListItemCommand {
    idCheckList: number;
    text: string;
    checked: CheckValue;
    required: boolean;
    remarks: string;
    checklistParams: CheckListParamDTO[];
}
