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
    CheckListPrintingMetadata,
    CheckListPrintingMetadataFromJSON,
    CheckListPrintingMetadataFromJSONTyped,
    CheckListPrintingMetadataToJSON,
    FormDefinitionResponse,
    FormDefinitionResponseFromJSON,
    FormDefinitionResponseFromJSONTyped,
    FormDefinitionResponseToJSON,
    ReportType,
    ReportTypeFromJSON,
    ReportTypeFromJSONTyped,
    ReportTypeToJSON,
} from './';

/**
 * 
 * @export
 * @interface ReportConfigurationDto
 */
export interface ReportConfigurationDto {
    /**
     * 
     * @type {number}
     * @memberof ReportConfigurationDto
     */
    readonly id?: number;
    /**
     * 
     * @type {ReportType}
     * @memberof ReportConfigurationDto
     */
    type?: ReportType;
    /**
     * 
     * @type {string}
     * @memberof ReportConfigurationDto
     */
    readonly title?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ReportConfigurationDto
     */
    readonly formName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ReportConfigurationDto
     */
    readonly remarksLabelText?: string | null;
    /**
     * 
     * @type {Array<number>}
     * @memberof ReportConfigurationDto
     */
    readonly checksDefinition?: Array<number> | null;
    /**
     * 
     * @type {Array<number>}
     * @memberof ReportConfigurationDto
     */
    readonly signatureDefinitions?: Array<number> | null;
    /**
     * 
     * @type {Array<FormDefinitionResponse>}
     * @memberof ReportConfigurationDto
     */
    readonly forms?: Array<FormDefinitionResponse> | null;
    /**
     * 
     * @type {CheckListPrintingMetadata}
     * @memberof ReportConfigurationDto
     */
    checkListMetadata?: CheckListPrintingMetadata;
    /**
     * 
     * @type {number}
     * @memberof ReportConfigurationDto
     */
    readonly printSectionId?: number | null;
    /**
     * 
     * @type {string}
     * @memberof ReportConfigurationDto
     */
    readonly templateName?: string | null;
}

export function ReportConfigurationDtoFromJSON(json: any): ReportConfigurationDto {
    return ReportConfigurationDtoFromJSONTyped(json, false);
}

export function ReportConfigurationDtoFromJSONTyped(json: any, ignoreDiscriminator: boolean): ReportConfigurationDto {
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
        'forms': !exists(json, 'forms') ? undefined : (json['forms'] === null ? null : (json['forms'] as Array<any>).map(FormDefinitionResponseFromJSON)),
        'checkListMetadata': !exists(json, 'checkListMetadata') ? undefined : CheckListPrintingMetadataFromJSON(json['checkListMetadata']),
        'printSectionId': !exists(json, 'printSectionId') ? undefined : json['printSectionId'],
        'templateName': !exists(json, 'templateName') ? undefined : json['templateName'],
    };
}

export function ReportConfigurationDtoToJSON(value?: ReportConfigurationDto | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'type': ReportTypeToJSON(value.type),
        'checkListMetadata': CheckListPrintingMetadataToJSON(value.checkListMetadata),
    };
}


