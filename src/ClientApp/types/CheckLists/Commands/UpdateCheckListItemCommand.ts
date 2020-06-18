import { CheckValue } from '../Models/CheckValue'

export interface UpdateCheckListItemCommand {
    id: number;
    checkListId: number;
    text: string;
    checked: CheckValue;
    required: boolean;
    remarks: string;
}
