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
    PrintSectionStatus,
    PrintSectionStatusFromJSON,
    PrintSectionStatusFromJSONTyped,
    PrintSectionStatusToJSON,
} from './';

/**
 * 
 * @export
 * @interface EditPrintSectionCommand
 */
export interface EditPrintSectionCommand {
    /**
     * 
     * @type {number}
     * @memberof EditPrintSectionCommand
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof EditPrintSectionCommand
     */
    code?: string | null;
    /**
     * 
     * @type {string}
     * @memberof EditPrintSectionCommand
     */
    description?: string | null;
    /**
     * 
     * @type {string}
     * @memberof EditPrintSectionCommand
     */
    content?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof EditPrintSectionCommand
     */
    isMainReport?: boolean;
    /**
     * 
     * @type {PrintSectionStatus}
     * @memberof EditPrintSectionCommand
     */
    status?: PrintSectionStatus;
}

export function EditPrintSectionCommandFromJSON(json: any): EditPrintSectionCommand {
    return EditPrintSectionCommandFromJSONTyped(json, false);
}

export function EditPrintSectionCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): EditPrintSectionCommand {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'code': !exists(json, 'code') ? undefined : json['code'],
        'description': !exists(json, 'description') ? undefined : json['description'],
        'content': !exists(json, 'content') ? undefined : json['content'],
        'isMainReport': !exists(json, 'isMainReport') ? undefined : json['isMainReport'],
        'status': !exists(json, 'status') ? undefined : PrintSectionStatusFromJSON(json['status']),
    };
}

export function EditPrintSectionCommandToJSON(value?: EditPrintSectionCommand | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'code': value.code,
        'description': value.description,
        'content': value.content,
        'isMainReport': value.isMainReport,
        'status': PrintSectionStatusToJSON(value.status),
    };
}

