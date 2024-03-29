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
export enum ResponsibleType {
    Supervisor = 'Supervisor',
    Inspector = 'Inspector',
    Witness = 'Witness',
    Lew = 'LEW',
    Other = 'Other'
}

export function ResponsibleTypeFromJSON(json: any): ResponsibleType {
    return ResponsibleTypeFromJSONTyped(json, false);
}

export function ResponsibleTypeFromJSONTyped(json: any, ignoreDiscriminator: boolean): ResponsibleType {
    return json as ResponsibleType;
}

export function ResponsibleTypeToJSON(value?: ResponsibleType | null): any {
    return value as any;
}

