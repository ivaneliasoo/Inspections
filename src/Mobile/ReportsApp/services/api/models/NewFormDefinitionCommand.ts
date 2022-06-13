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
 * @interface NewFormDefinitionCommand
 */
export interface NewFormDefinitionCommand {
    /**
     * 
     * @type {number}
     * @memberof NewFormDefinitionCommand
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof NewFormDefinitionCommand
     */
    name?: string | null;
    /**
     * 
     * @type {string}
     * @memberof NewFormDefinitionCommand
     */
    title?: string | null;
    /**
     * 
     * @type {string}
     * @memberof NewFormDefinitionCommand
     */
    icon?: string | null;
    /**
     * 
     * @type {DynamicFields}
     * @memberof NewFormDefinitionCommand
     */
    fields?: DynamicFields;
    /**
     * 
     * @type {any}
     * @memberof NewFormDefinitionCommand
     */
    defaultValues?: any | null;
    /**
     * 
     * @type {boolean}
     * @memberof NewFormDefinitionCommand
     */
    enabled?: boolean;
    /**
     * 
     * @type {number}
     * @memberof NewFormDefinitionCommand
     */
    readonly order?: number;
    /**
     * 
     * @type {Array<number>}
     * @memberof NewFormDefinitionCommand
     */
    reports?: Array<number> | null;
    /**
     * 
     * @type {Array<number>}
     * @memberof NewFormDefinitionCommand
     */
    reportConfigurations?: Array<number> | null;
}

export function NewFormDefinitionCommandFromJSON(json: any): NewFormDefinitionCommand {
    return NewFormDefinitionCommandFromJSONTyped(json, false);
}

export function NewFormDefinitionCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): NewFormDefinitionCommand {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'name': !exists(json, 'name') ? undefined : json['name'],
        'title': !exists(json, 'title') ? undefined : json['title'],
        'icon': !exists(json, 'icon') ? undefined : json['icon'],
        'fields': !exists(json, 'fields') ? undefined : DynamicFieldsFromJSON(json['fields']),
        'defaultValues': !exists(json, 'defaultValues') ? undefined : json['defaultValues'],
        'enabled': !exists(json, 'enabled') ? undefined : json['enabled'],
        'order': !exists(json, 'order') ? undefined : json['order'],
        'reports': !exists(json, 'reports') ? undefined : json['reports'],
        'reportConfigurations': !exists(json, 'reportConfigurations') ? undefined : json['reportConfigurations'],
    };
}

export function NewFormDefinitionCommandToJSON(value?: NewFormDefinitionCommand | null): any {
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
        'defaultValues': value.defaultValues,
        'enabled': value.enabled,
        'reports': value.reports,
        'reportConfigurations': value.reportConfigurations,
    };
}


