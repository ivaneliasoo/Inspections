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
export enum PrintSectionStatus {
    Inactive = 'Inactive',
    Active = 'Active'
}

export function PrintSectionStatusFromJSON(json: any): PrintSectionStatus {
    return PrintSectionStatusFromJSONTyped(json, false);
}

export function PrintSectionStatusFromJSONTyped(json: any, ignoreDiscriminator: boolean): PrintSectionStatus {
    return json as PrintSectionStatus;
}

export function PrintSectionStatusToJSON(value?: PrintSectionStatus | null): any {
    return value as any;
}
