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
 * @interface DynamicFieldMetadata
 */
export interface DynamicFieldMetadata {
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    fieldName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    sectionTitle?: string | null;
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    switchableSection?: string | null;
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    label?: string | null;
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    inputType?: string | null;
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    selectOptions?: string | null;
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    suffix?: string | null;
    /**
     * 
     * @type {string}
     * @memberof DynamicFieldMetadata
     */
    prefix?: string | null;
    /**
     * 
     * @type {number}
     * @memberof DynamicFieldMetadata
     */
    min?: number;
    /**
     * 
     * @type {number}
     * @memberof DynamicFieldMetadata
     */
    max?: number;
    /**
     * 
     * @type {number}
     * @memberof DynamicFieldMetadata
     */
    step?: number;
    /**
     * 
     * @type {number}
     * @memberof DynamicFieldMetadata
     */
    maxLength?: number;
    /**
     * 
     * @type {boolean}
     * @memberof DynamicFieldMetadata
     */
    enabled?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof DynamicFieldMetadata
     */
    rollerOnMobile?: boolean;
    /**
     * 
     * @type {number}
     * @memberof DynamicFieldMetadata
     */
    rollerDigits?: number;
    /**
     * 
     * @type {boolean}
     * @memberof DynamicFieldMetadata
     */
    visible?: boolean;
    /**
     * 
     * @type {any}
     * @memberof DynamicFieldMetadata
     */
    defaultValue?: any | null;
    /**
     * 
     * @type {number}
     * @memberof DynamicFieldMetadata
     */
    order?: number;
}

export function DynamicFieldMetadataFromJSON(json: any): DynamicFieldMetadata {
    return DynamicFieldMetadataFromJSONTyped(json, false);
}

export function DynamicFieldMetadataFromJSONTyped(json: any, ignoreDiscriminator: boolean): DynamicFieldMetadata {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'fieldName': !exists(json, 'fieldName') ? undefined : json['fieldName'],
        'sectionTitle': !exists(json, 'sectionTitle') ? undefined : json['sectionTitle'],
        'switchableSection': !exists(json, 'switchableSection') ? undefined : json['switchableSection'],
        'label': !exists(json, 'label') ? undefined : json['label'],
        'inputType': !exists(json, 'inputType') ? undefined : json['inputType'],
        'selectOptions': !exists(json, 'selectOptions') ? undefined : json['selectOptions'],
        'suffix': !exists(json, 'suffix') ? undefined : json['suffix'],
        'prefix': !exists(json, 'prefix') ? undefined : json['prefix'],
        'min': !exists(json, 'min') ? undefined : json['min'],
        'max': !exists(json, 'max') ? undefined : json['max'],
        'step': !exists(json, 'step') ? undefined : json['step'],
        'maxLength': !exists(json, 'maxLength') ? undefined : json['maxLength'],
        'enabled': !exists(json, 'enabled') ? undefined : json['enabled'],
        'rollerOnMobile': !exists(json, 'rollerOnMobile') ? undefined : json['rollerOnMobile'],
        'rollerDigits': !exists(json, 'rollerDigits') ? undefined : json['rollerDigits'],
        'visible': !exists(json, 'visible') ? undefined : json['visible'],
        'defaultValue': !exists(json, 'defaultValue') ? undefined : json['defaultValue'],
        'order': !exists(json, 'order') ? undefined : json['order'],
    };
}

export function DynamicFieldMetadataToJSON(value?: DynamicFieldMetadata | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'fieldName': value.fieldName,
        'sectionTitle': value.sectionTitle,
        'switchableSection': value.switchableSection,
        'label': value.label,
        'inputType': value.inputType,
        'selectOptions': value.selectOptions,
        'suffix': value.suffix,
        'prefix': value.prefix,
        'min': value.min,
        'max': value.max,
        'step': value.step,
        'maxLength': value.maxLength,
        'enabled': value.enabled,
        'rollerOnMobile': value.rollerOnMobile,
        'rollerDigits': value.rollerDigits,
        'visible': value.visible,
        'defaultValue': value.defaultValue,
        'order': value.order,
    };
}


