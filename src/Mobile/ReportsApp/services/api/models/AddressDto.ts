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
 * @interface AddressDto
 */
export interface AddressDto {
    /**
     * 
     * @type {number}
     * @memberof AddressDto
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    addressLine?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    addressLine2?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    unit?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    country?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    postalCode?: string | null;
    /**
     * 
     * @type {number}
     * @memberof AddressDto
     */
    licenseId?: number;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    number?: string | null;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    name?: string | null;
    /**
     * 
     * @type {number}
     * @memberof AddressDto
     */
    amp?: number;
    /**
     * 
     * @type {number}
     * @memberof AddressDto
     */
    volt?: number;
    /**
     * 
     * @type {number}
     * @memberof AddressDto
     */
    kva?: number;
    /**
     * 
     * @type {DateTimeRange}
     * @memberof AddressDto
     */
    validity?: DateTimeRange;
    /**
     * 
     * @type {string}
     * @memberof AddressDto
     */
    formatedAddress?: string | null;
}

export function AddressDtoFromJSON(json: any): AddressDto {
    return AddressDtoFromJSONTyped(json, false);
}

export function AddressDtoFromJSONTyped(json: any, ignoreDiscriminator: boolean): AddressDto {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'addressLine': !exists(json, 'addressLine') ? undefined : json['addressLine'],
        'addressLine2': !exists(json, 'addressLine2') ? undefined : json['addressLine2'],
        'unit': !exists(json, 'unit') ? undefined : json['unit'],
        'country': !exists(json, 'country') ? undefined : json['country'],
        'postalCode': !exists(json, 'postalCode') ? undefined : json['postalCode'],
        'licenseId': !exists(json, 'licenseId') ? undefined : json['licenseId'],
        'number': !exists(json, 'number') ? undefined : json['number'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'amp': !exists(json, 'amp') ? undefined : json['amp'],
        'volt': !exists(json, 'volt') ? undefined : json['volt'],
        'kva': !exists(json, 'kva') ? undefined : json['kva'],
        'validity': !exists(json, 'validity') ? undefined : DateTimeRangeFromJSON(json['validity']),
        'formatedAddress': !exists(json, 'formatedAddress') ? undefined : json['formatedAddress'],
    };
}

export function AddressDtoToJSON(value?: AddressDto | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'addressLine': value.addressLine,
        'addressLine2': value.addressLine2,
        'unit': value.unit,
        'country': value.country,
        'postalCode': value.postalCode,
        'licenseId': value.licenseId,
        'number': value.number,
        'name': value.name,
        'amp': value.amp,
        'volt': value.volt,
        'kva': value.kva,
        'validity': DateTimeRangeToJSON(value.validity),
        'formatedAddress': value.formatedAddress,
    };
}


