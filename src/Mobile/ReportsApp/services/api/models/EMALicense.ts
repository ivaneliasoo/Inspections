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
    DateTimeRange,
    DateTimeRangeFromJSON,
    DateTimeRangeFromJSONTyped,
    DateTimeRangeToJSON,
} from './';

/**
 * 
 * @export
 * @interface EMALicense
 */
export interface EMALicense {
    /**
     * 
     * @type {number}
     * @memberof EMALicense
     */
    id?: number;
    /**
     * 
     * @type {Array<object>}
     * @memberof EMALicense
     */
    readonly domainEvents?: Array<object> | null;
    /**
     * 
     * @type {string}
     * @memberof EMALicense
     */
    number?: string | null;
    /**
     * 
     * @type {string}
     * @memberof EMALicense
     */
    name?: string | null;
    /**
     * 
     * @type {string}
     * @memberof EMALicense
     */
    personInCharge?: string | null;
    /**
     * 
     * @type {string}
     * @memberof EMALicense
     */
    contact?: string | null;
    /**
     * 
     * @type {string}
     * @memberof EMALicense
     */
    email?: string | null;
    /**
     * 
     * @type {number}
     * @memberof EMALicense
     */
    amp?: number;
    /**
     * 
     * @type {number}
     * @memberof EMALicense
     */
    volt?: number;
    /**
     * 
     * @type {number}
     * @memberof EMALicense
     */
    kva?: number;
    /**
     * 
     * @type {DateTimeRange}
     * @memberof EMALicense
     */
    validity?: DateTimeRange;
}

export function EMALicenseFromJSON(json: any): EMALicense {
    return EMALicenseFromJSONTyped(json, false);
}

export function EMALicenseFromJSONTyped(json: any, ignoreDiscriminator: boolean): EMALicense {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'domainEvents': !exists(json, 'domainEvents') ? undefined : json['domainEvents'],
        'number': !exists(json, 'number') ? undefined : json['number'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'personInCharge': !exists(json, 'personInCharge') ? undefined : json['personInCharge'],
        'contact': !exists(json, 'contact') ? undefined : json['contact'],
        'email': !exists(json, 'email') ? undefined : json['email'],
        'amp': !exists(json, 'amp') ? undefined : json['amp'],
        'volt': !exists(json, 'volt') ? undefined : json['volt'],
        'kva': !exists(json, 'kva') ? undefined : json['kva'],
        'validity': !exists(json, 'validity') ? undefined : DateTimeRangeFromJSON(json['validity']),
    };
}

export function EMALicenseToJSON(value?: EMALicense | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'number': value.number,
        'name': value.name,
        'personInCharge': value.personInCharge,
        'contact': value.contact,
        'email': value.email,
        'amp': value.amp,
        'volt': value.volt,
        'kva': value.kva,
        'validity': DateTimeRangeToJSON(value.validity),
    };
}


