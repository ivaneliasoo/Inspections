/* tslint:disable */
/* eslint-disable */
/**
 * Inspections API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { exists, mapValues } from '../runtime';
import {
    CheckListQueryResult,
    CheckListQueryResultFromJSON,
    CheckListQueryResultFromJSONTyped,
    CheckListQueryResultToJSON,
    DateTimeRange,
    DateTimeRangeFromJSON,
    DateTimeRangeFromJSONTyped,
    DateTimeRangeToJSON,
    FormResult,
    FormResultFromJSON,
    FormResultFromJSONTyped,
    FormResultToJSON,
    NoteQueryResult,
    NoteQueryResultFromJSON,
    NoteQueryResultFromJSONTyped,
    NoteQueryResultToJSON,
    SignatureQueryResult,
    SignatureQueryResultFromJSON,
    SignatureQueryResultFromJSONTyped,
    SignatureQueryResultToJSON,
} from './';

/**
 * 
 * @export
 * @interface ReportQueryResult
 */
export interface ReportQueryResult {
    /**
     * 
     * @type {number}
     * @memberof ReportQueryResult
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof ReportQueryResult
     */
    name?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ReportQueryResult
     */
    address?: string | null;
    /**
     * 
     * @type {Date}
     * @memberof ReportQueryResult
     */
    date?: Date;
    /**
     * 
     * @type {string}
     * @memberof ReportQueryResult
     */
    licenseNumber?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ReportQueryResult
     */
    licenseName?: string | null;
    /**
     * 
     * @type {number}
     * @memberof ReportQueryResult
     */
    licenseKVA?: number | null;
    /**
     * 
     * @type {number}
     * @memberof ReportQueryResult
     */
    licenseVolt?: number | null;
    /**
     * 
     * @type {number}
     * @memberof ReportQueryResult
     */
    licenseAmp?: number | null;
    /**
     * 
     * @type {number}
     * @memberof ReportQueryResult
     */
    reportConfigurationId?: number;
    /**
     * 
     * @type {DateTimeRange}
     * @memberof ReportQueryResult
     */
    licenseValidity?: DateTimeRange;
    /**
     * 
     * @type {string}
     * @memberof ReportQueryResult
     */
    title?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ReportQueryResult
     */
    formName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ReportQueryResult
     */
    remarksLabelText?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof ReportQueryResult
     */
    isClosed?: boolean;
    /**
     * 
     * @type {Array<FormResult>}
     * @memberof ReportQueryResult
     */
    forms?: Array<FormResult> | null;
    /**
     * 
     * @type {Array<SignatureQueryResult>}
     * @memberof ReportQueryResult
     */
    signatures?: Array<SignatureQueryResult> | null;
    /**
     * 
     * @type {Array<CheckListQueryResult>}
     * @memberof ReportQueryResult
     */
    checkLists?: Array<CheckListQueryResult> | null;
    /**
     * 
     * @type {Array<NoteQueryResult>}
     * @memberof ReportQueryResult
     */
    notes?: Array<NoteQueryResult> | null;
}

export function ReportQueryResultFromJSON(json: any): ReportQueryResult {
    return ReportQueryResultFromJSONTyped(json, false);
}

export function ReportQueryResultFromJSONTyped(json: any, ignoreDiscriminator: boolean): ReportQueryResult {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'address': !exists(json, 'address') ? undefined : json['address'],
        'date': !exists(json, 'date') ? undefined : (new Date(json['date'])),
        'licenseNumber': !exists(json, 'licenseNumber') ? undefined : json['licenseNumber'],
        'licenseName': !exists(json, 'licenseName') ? undefined : json['licenseName'],
        'licenseKVA': !exists(json, 'licenseKVA') ? undefined : json['licenseKVA'],
        'licenseVolt': !exists(json, 'licenseVolt') ? undefined : json['licenseVolt'],
        'licenseAmp': !exists(json, 'licenseAmp') ? undefined : json['licenseAmp'],
        'reportConfigurationId': !exists(json, 'reportConfigurationId') ? undefined : json['reportConfigurationId'],
        'licenseValidity': !exists(json, 'licenseValidity') ? undefined : DateTimeRangeFromJSON(json['licenseValidity']),
        'title': !exists(json, 'title') ? undefined : json['title'],
        'formName': !exists(json, 'formName') ? undefined : json['formName'],
        'remarksLabelText': !exists(json, 'remarksLabelText') ? undefined : json['remarksLabelText'],
        'isClosed': !exists(json, 'isClosed') ? undefined : json['isClosed'],
        'forms': !exists(json, 'forms') ? undefined : (json['forms'] === null ? null : (json['forms'] as Array<any>).map(FormResultFromJSON)),
        'signatures': !exists(json, 'signatures') ? undefined : (json['signatures'] === null ? null : (json['signatures'] as Array<any>).map(SignatureQueryResultFromJSON)),
        'checkLists': !exists(json, 'checkLists') ? undefined : (json['checkLists'] === null ? null : (json['checkLists'] as Array<any>).map(CheckListQueryResultFromJSON)),
        'notes': !exists(json, 'notes') ? undefined : (json['notes'] === null ? null : (json['notes'] as Array<any>).map(NoteQueryResultFromJSON)),
    };
}

export function ReportQueryResultToJSON(value?: ReportQueryResult | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'name': value.name,
        'address': value.address,
        'date': value.date === undefined ? undefined : (value.date.toISOString()),
        'licenseNumber': value.licenseNumber,
        'licenseName': value.licenseName,
        'licenseKVA': value.licenseKVA,
        'licenseVolt': value.licenseVolt,
        'licenseAmp': value.licenseAmp,
        'reportConfigurationId': value.reportConfigurationId,
        'licenseValidity': DateTimeRangeToJSON(value.licenseValidity),
        'title': value.title,
        'formName': value.formName,
        'remarksLabelText': value.remarksLabelText,
        'isClosed': value.isClosed,
        'forms': value.forms === undefined ? undefined : (value.forms === null ? null : (value.forms as Array<any>).map(FormResultToJSON)),
        'signatures': value.signatures === undefined ? undefined : (value.signatures === null ? null : (value.signatures as Array<any>).map(SignatureQueryResultToJSON)),
        'checkLists': value.checkLists === undefined ? undefined : (value.checkLists === null ? null : (value.checkLists as Array<any>).map(CheckListQueryResultToJSON)),
        'notes': value.notes === undefined ? undefined : (value.notes === null ? null : (value.notes as Array<any>).map(NoteQueryResultToJSON)),
    };
}


