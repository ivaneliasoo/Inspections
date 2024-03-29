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
export enum CheckValue {
    NotAcceptableFalse = 0,
    Acceptable = 1,
    NotApplicable = 2,
    None = 3
}

export function CheckValueFromJSON(json: any): CheckValue {
    return CheckValueFromJSONTyped(json, false);
}

export function CheckValueFromJSONTyped(json: any, ignoreDiscriminator: boolean): CheckValue {
    return json as CheckValue;
}

export function CheckValueToJSON(value?: CheckValue | null): any {
    return value as any;
}

