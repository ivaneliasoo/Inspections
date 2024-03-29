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
    ReportType,
    ReportTypeFromJSON,
    ReportTypeFromJSONTyped,
    ReportTypeToJSON,
} from './';

/**
 * 
 * @export
 * @interface CreateReportCommand
 */
export interface CreateReportCommand {
    /**
     * 
     * @type {number}
     * @memberof CreateReportCommand
     */
    configurationId?: number;
    /**
     * 
     * @type {ReportType}
     * @memberof CreateReportCommand
     */
    reportType?: ReportType;
}

export function CreateReportCommandFromJSON(json: any): CreateReportCommand {
    return CreateReportCommandFromJSONTyped(json, false);
}

export function CreateReportCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): CreateReportCommand {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'configurationId': !exists(json, 'configurationId') ? undefined : json['configurationId'],
        'reportType': !exists(json, 'reportType') ? undefined : ReportTypeFromJSON(json['reportType']),
    };
}

export function CreateReportCommandToJSON(value?: CreateReportCommand | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'configurationId': value.configurationId,
        'reportType': ReportTypeToJSON(value.reportType),
    };
}


