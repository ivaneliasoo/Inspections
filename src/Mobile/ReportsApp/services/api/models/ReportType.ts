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

/**
 * 
 * @export
 * @enum {string}
 */
export enum ReportType {
    Inspection = 'Inspection'
}

export function ReportTypeFromJSON(json: any): ReportType {
    return ReportTypeFromJSONTyped(json, false);
}

export function ReportTypeFromJSONTyped(json: any, ignoreDiscriminator: boolean): ReportType {
    return json as ReportType;
}

export function ReportTypeToJSON(value?: ReportType | null): any {
    return value as any;
}

