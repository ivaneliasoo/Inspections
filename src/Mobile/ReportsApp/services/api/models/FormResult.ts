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
    DynamicFields,
    DynamicFieldsFromJSON,
    DynamicFieldsFromJSONTyped,
    DynamicFieldsToJSON,
} from './';

/**
 * 
 * @export
 * @interface FormResult
 */
export interface FormResult {
    /**
     * 
     * @type {number}
     * @memberof FormResult
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof FormResult
     */
    name?: string | null;
    /**
     * 
     * @type {string}
     * @memberof FormResult
     */
    title?: string | null;
    /**
     * 
     * @type {string}
     * @memberof FormResult
     */
    icon?: string | null;
    /**
     * 
     * @type {DynamicFields}
     * @memberof FormResult
     */
    fields?: DynamicFields;
    /**
     * 
     * @type {any}
     * @memberof FormResult
     */
    values?: any | null;
    /**
     * 
     * @type {boolean}
     * @memberof FormResult
     */
    enabled?: boolean;
}

export function FormResultFromJSON(json: any): FormResult {
    return FormResultFromJSONTyped(json, false);
}

export function FormResultFromJSONTyped(json: any, ignoreDiscriminator: boolean): FormResult {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'title': !exists(json, 'title') ? undefined : json['title'],
        'icon': !exists(json, 'icon') ? undefined : json['icon'],
        'fields': !exists(json, 'fields') ? undefined : DynamicFieldsFromJSON(json['fields']),
        'values': !exists(json, 'values') ? undefined : json['values'],
        'enabled': !exists(json, 'enabled') ? undefined : json['enabled'],
    };
}

export function FormResultToJSON(value?: FormResult | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'name': value.name,
        'title': value.title,
        'icon': value.icon,
        'fields': DynamicFieldsToJSON(value.fields),
        'values': value.values,
        'enabled': value.enabled,
    };
}

