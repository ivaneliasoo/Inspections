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
 * @interface PrintSectionDto
 */
export interface PrintSectionDto {
    /**
     * 
     * @type {number}
     * @memberof PrintSectionDto
     */
    readonly id?: number;
    /**
     * 
     * @type {string}
     * @memberof PrintSectionDto
     */
    readonly code?: string | null;
    /**
     * 
     * @type {string}
     * @memberof PrintSectionDto
     */
    readonly description?: string | null;
    /**
     * 
     * @type {string}
     * @memberof PrintSectionDto
     */
    readonly content?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof PrintSectionDto
     */
    readonly isMainReport?: boolean;
    /**
     * 
     * @type {PrintSectionStatus}
     * @memberof PrintSectionDto
     */
    status?: PrintSectionStatus;
}

export function PrintSectionDtoFromJSON(json: any): PrintSectionDto {
    return PrintSectionDtoFromJSONTyped(json, false);
}

export function PrintSectionDtoFromJSONTyped(json: any, ignoreDiscriminator: boolean): PrintSectionDto {
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

export function PrintSectionDtoToJSON(value?: PrintSectionDto | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'status': PrintSectionStatusToJSON(value.status),
    };
}


