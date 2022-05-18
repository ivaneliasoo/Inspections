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
 * @interface LicenseDTO
 */
export interface LicenseDTO {
    /**
     * 
     * @type {number}
     * @memberof LicenseDTO
     */
    licenseId?: number;
    /**
     * 
     * @type {string}
     * @memberof LicenseDTO
     */
    number?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LicenseDTO
     */
    name?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LicenseDTO
     */
    personInCharge?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LicenseDTO
     */
    contact?: string | null;
    /**
     * 
     * @type {string}
     * @memberof LicenseDTO
     */
    email?: string | null;
    /**
     * 
     * @type {number}
     * @memberof LicenseDTO
     */
    amp?: number;
    /**
     * 
     * @type {number}
     * @memberof LicenseDTO
     */
    volt?: number;
    /**
     * 
     * @type {number}
     * @memberof LicenseDTO
     */
    kva?: number;
    /**
     * 
     * @type {Date}
     * @memberof LicenseDTO
     */
    validityStart?: Date;
    /**
     * 
     * @type {Date}
     * @memberof LicenseDTO
     */
    validityEnd?: Date;
}

export function LicenseDTOFromJSON(json: any): LicenseDTO {
    return LicenseDTOFromJSONTyped(json, false);
}

export function LicenseDTOFromJSONTyped(json: any, ignoreDiscriminator: boolean): LicenseDTO {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'licenseId': !exists(json, 'licenseId') ? undefined : json['licenseId'],
        'number': !exists(json, 'number') ? undefined : json['number'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'personInCharge': !exists(json, 'personInCharge') ? undefined : json['personInCharge'],
        'contact': !exists(json, 'contact') ? undefined : json['contact'],
        'email': !exists(json, 'email') ? undefined : json['email'],
        'amp': !exists(json, 'amp') ? undefined : json['amp'],
        'volt': !exists(json, 'volt') ? undefined : json['volt'],
        'kva': !exists(json, 'kva') ? undefined : json['kva'],
        'validityStart': !exists(json, 'validityStart') ? undefined : (new Date(json['validityStart'])),
        'validityEnd': !exists(json, 'validityEnd') ? undefined : (new Date(json['validityEnd'])),
    };
}

export function LicenseDTOToJSON(value?: LicenseDTO | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'licenseId': value.licenseId,
        'number': value.number,
        'name': value.name,
        'personInCharge': value.personInCharge,
        'contact': value.contact,
        'email': value.email,
        'amp': value.amp,
        'volt': value.volt,
        'kva': value.kva,
        'validityStart': value.validityStart === undefined ? undefined : (value.validityStart.toISOString()),
        'validityEnd': value.validityEnd === undefined ? undefined : (value.validityEnd.toISOString()),
    };
}


