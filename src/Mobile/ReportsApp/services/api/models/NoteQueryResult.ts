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
/**
 * 
 * @export
 * @interface NoteQueryResult
 */
export interface NoteQueryResult {
    /**
     * 
     * @type {number}
     * @memberof NoteQueryResult
     */
    id?: number;
    /**
     * 
     * @type {number}
     * @memberof NoteQueryResult
     */
    reportId?: number;
    /**
     * 
     * @type {string}
     * @memberof NoteQueryResult
     */
    text?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof NoteQueryResult
     */
    checked?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof NoteQueryResult
     */
    needsCheck?: boolean;
}

export function NoteQueryResultFromJSON(json: any): NoteQueryResult {
    return NoteQueryResultFromJSONTyped(json, false);
}

export function NoteQueryResultFromJSONTyped(json: any, ignoreDiscriminator: boolean): NoteQueryResult {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'reportId': !exists(json, 'reportId') ? undefined : json['reportId'],
        'text': !exists(json, 'text') ? undefined : json['text'],
        'checked': !exists(json, 'checked') ? undefined : json['checked'],
        'needsCheck': !exists(json, 'needsCheck') ? undefined : json['needsCheck'],
    };
}

export function NoteQueryResultToJSON(value?: NoteQueryResult | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'reportId': value.reportId,
        'text': value.text,
        'checked': value.checked,
        'needsCheck': value.needsCheck,
    };
}


