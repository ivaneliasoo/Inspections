import { CheckValue } from "..";

export interface UpdateCheckListItemCommand {
    id: number;
    checkListId: number;
    text: string;
    checked: CheckValue    
    editable: boolean;
    required: boolean;
    remarks: string;
}
