import { PhotoRecord } from './PhotoRecord'
import { Note } from './Note'
import { EMALicense } from './EMALicense'
import { Signature, CheckList } from '~/types'

export interface Report {
    id:number;
    name: string;
    address: string;
    license: EMALicense;
    date: string;
    isClosed: boolean;
    title: string;
    formName: string;
    remarksLabelText: string;
    signatures: Signature[];
    notes: Note[];
    checkList: CheckList[];
    photoRecords: PhotoRecord[];
    completed: boolean;
    reportConfigurationId: number;
}
