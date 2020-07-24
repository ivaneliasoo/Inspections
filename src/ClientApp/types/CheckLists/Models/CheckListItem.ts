import { CheckValue } from './CheckValue'
import { CheckListParam } from './CheckListParam'

export interface CheckListItem {
    id: number;
    checkListId: number;
    text: string;
    checked: CheckValue;
    editable: boolean;
    required: boolean;
    remarks: string;
    textParams: CheckListParam[];
}
