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
 * @interface UpdateReportCommand
 */
export interface UpdateReportCommand {
    /**
     * 
     * @type {number}
     * @memberof UpdateReportCommand
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof UpdateReportCommand
     */
    name?: string | null;
    /**
     * 
     * @type {string}
     * @memberof UpdateReportCommand
     */
    address?: string | null;
    /**
     * 
     * @type {string}
     * @memberof UpdateReportCommand
     */
    licenseNumber?: string | null;
    /**
     * 
     * @type {Date}
     * @memberof UpdateReportCommand
     */
    date?: Date | string;
    /**
     * 
     * @type {boolean}
     * @memberof UpdateReportCommand
     */
    isClosed?: boolean;
}

export function UpdateReportCommandFromJSON(json: any): UpdateReportCommand {
    return UpdateReportCommandFromJSONTyped(json, false);
}

export function UpdateReportCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): UpdateReportCommand {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'address': !exists(json, 'address') ? undefined : json['address'],
        'licenseNumber': !exists(json, 'licenseNumber') ? undefined : json['licenseNumber'],
        'date': !exists(json, 'date') ? undefined : (new Date(json['date'])),
        'isClosed': !exists(json, 'isClosed') ? undefined : json['isClosed'],
    };
}

export function UpdateReportCommandToJSON(value?: UpdateReportCommand | null): any {
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
        'licenseNumber': value.licenseNumber,
        'date': value.date === undefined ? undefined : (value.date),//.toISOString()),
        'isClosed': value.isClosed,
    };
}


