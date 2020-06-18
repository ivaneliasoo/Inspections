import { CheckListItem } from './CheckListItem'
import { CheckList } from './CheckList'
import { CheckListParamType } from './CheckListParamType'

export interface CheckListParam {
    id: number;
    checkListId: number | null;
    checkListItemId: number | null;
    checkListItem: CheckListItem;
    checkList: CheckList;
    key: string;
    value: string;
    type: CheckListParamType;
}
