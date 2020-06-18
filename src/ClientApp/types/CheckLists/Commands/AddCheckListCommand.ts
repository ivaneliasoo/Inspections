import { CheckListParamDTO } from '../ViewModels/CheckListParamDTO'
import { CheckListItemDTO } from '../ViewModels/CheckListItemDTO'

export interface AddCheckListCommand {
    text: string;
    textParams: CheckListParamDTO[];
    items: CheckListItemDTO[];
    annotation: string;
}
