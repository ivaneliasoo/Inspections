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
    ResponsibleType,
    ResponsibleTypeFromJSON,
    ResponsibleTypeFromJSONTyped,
    ResponsibleTypeToJSON,
} from './';

/**
 * 
 * @export
 * @interface AddSignatureCommand
 */
export interface AddSignatureCommand {
    /**
     * 
     * @type {string}
     * @memberof AddSignatureCommand
     */
    title?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddSignatureCommand
     */
    annotation?: string | null;
    /**
     * 
     * @type {ResponsibleType}
     * @memberof AddSignatureCommand
     */
    responsableType?: ResponsibleType;
    /**
     * 
     * @type {string}
     * @memberof AddSignatureCommand
     */
    responsibleName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddSignatureCommand
     */
    designation?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddSignatureCommand
     */
    remarks?: string | null;
    /**
     * 
     * @type {Date}
     * @memberof AddSignatureCommand
     */
    date?: Date;
    /**
     * 
     * @type {boolean}
     * @memberof AddSignatureCommand
     */
    principal?: boolean;
    /**
     * 
     * @type {number}
     * @memberof AddSignatureCommand
     */
    reportId?: number | null;
    /**
     * 
     * @type {number}
     * @memberof AddSignatureCommand
     */
    reportConfigurationId?: number | null;
    /**
     * 
     * @type {number}
     * @memberof AddSignatureCommand
     */
    order?: number;
    /**
     * 
     * @type {string}
     * @memberof AddSignatureCommand
     */
    signature?: string | null;
}

export function AddSignatureCommandFromJSON(json: any): AddSignatureCommand {
    return AddSignatureCommandFromJSONTyped(json, false);
}

export function AddSignatureCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): AddSignatureCommand {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'title': !exists(json, 'title') ? undefined : json['title'],
        'annotation': !exists(json, 'annotation') ? undefined : json['annotation'],
        'responsableType': !exists(json, 'responsableType') ? undefined : ResponsibleTypeFromJSON(json['responsableType']),
        'responsibleName': !exists(json, 'responsibleName') ? undefined : json['responsibleName'],
        'designation': !exists(json, 'designation') ? undefined : json['designation'],
        'remarks': !exists(json, 'remarks') ? undefined : json['remarks'],
        'date': !exists(json, 'date') ? undefined : (new Date(json['date'])),
        'principal': !exists(json, 'principal') ? undefined : json['principal'],
        'reportId': !exists(json, 'reportId') ? undefined : json['reportId'],
        'reportConfigurationId': !exists(json, 'reportConfigurationId') ? undefined : json['reportConfigurationId'],
        'order': !exists(json, 'order') ? undefined : json['order'],
        'signature': !exists(json, 'signature') ? undefined : json['signature'],
    };
}

export function AddSignatureCommandToJSON(value?: AddSignatureCommand | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'title': value.title,
        'annotation': value.annotation,
        'responsableType': ResponsibleTypeToJSON(value.responsableType),
        'responsibleName': value.responsibleName,
        'designation': value.designation,
        'remarks': value.remarks,
        'date': value.date === undefined ? undefined : (value.date.toISOString()),
        'principal': value.principal,
        'reportId': value.reportId,
        'reportConfigurationId': value.reportConfigurationId,
        'order': value.order,
        'signature': value.signature,
    };
}


