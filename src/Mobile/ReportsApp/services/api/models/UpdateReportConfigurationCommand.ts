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
    CheckListDisplay,
    CheckListDisplayFromJSON,
    CheckListDisplayFromJSONTyped,
    CheckListDisplayToJSON,
    ReportType,
    ReportTypeFromJSON,
    ReportTypeFromJSONTyped,
    ReportTypeToJSON,
} from './';

/**
 * 
 * @export
 * @interface UpdateReportConfigurationCommand
 */
export interface UpdateReportConfigurationCommand {
    /**
     * 
     * @type {number}
     * @memberof UpdateReportConfigurationCommand
     */
    id?: number;
    /**
     * 
     * @type {ReportType}
     * @memberof UpdateReportConfigurationCommand
     */
    type?: ReportType;
    /**
     * 
     * @type {string}
     * @memberof UpdateReportConfigurationCommand
     */
    title?: string | null;
    /**
     * 
     * @type {string}
     * @memberof UpdateReportConfigurationCommand
     */
    formName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof UpdateReportConfigurationCommand
     */
    remarksLabelText?: string | null;
    /**
     * 
     * @type {Array<number>}
     * @memberof UpdateReportConfigurationCommand
     */
    checksDefinition?: Array<number> | null;
    /**
     * 
     * @type {Array<number>}
     * @memberof UpdateReportConfigurationCommand
     */
    signatureDefinitions?: Array<number> | null;
    /**
     * 
     * @type {number}
     * @memberof UpdateReportConfigurationCommand
     */
    printSectionId?: number;
    /**
     * 
     * @type {CheckListDisplay}
     * @memberof UpdateReportConfigurationCommand
     */
    display?: CheckListDisplay;
    /**
     * 
     * @type {string}
     * @memberof UpdateReportConfigurationCommand
     */
    templateName?: string | null;
}

export function UpdateReportConfigurationCommandFromJSON(json: any): UpdateReportConfigurationCommand {
    return UpdateReportConfigurationCommandFromJSONTyped(json, false);
}

export function UpdateReportConfigurationCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): UpdateReportConfigurationCommand {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'type': !exists(json, 'type') ? undefined : ReportTypeFromJSON(json['type']),
        'title': !exists(json, 'title') ? undefined : json['title'],
        'formName': !exists(json, 'formName') ? undefined : json['formName'],
        'remarksLabelText': !exists(json, 'remarksLabelText') ? undefined : json['remarksLabelText'],
        'checksDefinition': !exists(json, 'checksDefinition') ? undefined : json['checksDefinition'],
        'signatureDefinitions': !exists(json, 'signatureDefinitions') ? undefined : json['signatureDefinitions'],
        'printSectionId': !exists(json, 'printSectionId') ? undefined : json['printSectionId'],
        'display': !exists(json, 'display') ? undefined : CheckListDisplayFromJSON(json['display']),
        'templateName': !exists(json, 'templateName') ? undefined : json['templateName'],
    };
}

export function UpdateReportConfigurationCommandToJSON(value?: UpdateReportConfigurationCommand | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'type': ReportTypeToJSON(value.type),
        'title': value.title,
        'formName': value.formName,
        'remarksLabelText': value.remarksLabelText,
        'checksDefinition': value.checksDefinition,
        'signatureDefinitions': value.signatureDefinitions,
        'printSectionId': value.printSectionId,
        'display': CheckListDisplayToJSON(value.display),
        'templateName': value.templateName,
    };
}


