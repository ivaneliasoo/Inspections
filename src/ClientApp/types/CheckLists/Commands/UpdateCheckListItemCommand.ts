import { CheckValue } from "..";

export interface UpdateCheckListItemCommand {
    id: number;
    checkListId: number;
    text: string;
    checked: CheckValue    
    required: boolean;
    remarks: string;
}
