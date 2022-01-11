export * from './Models'
export * from './Commands'

export enum CheckListDisplay {
  Numbered,
  Inline,
};

export interface SelectItem {
  id: number;
  text: string;
}

export const ChecklistDisplayList = [
  { id: CheckListDisplay.Numbered, text: CheckListDisplay[CheckListDisplay.Numbered] },
  { id: CheckListDisplay.Inline, text: CheckListDisplay[CheckListDisplay.Inline] }
]

export interface CheckListPrintingMetadata {
  display: CheckListDisplay;
}
