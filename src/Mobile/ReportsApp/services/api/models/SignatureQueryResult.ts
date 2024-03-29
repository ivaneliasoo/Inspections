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
 * @interface SignatureQueryResult
 */
export interface SignatureQueryResult {
    /**
     * 
     * @type {number}
     * @memberof SignatureQueryResult
     */
    id?: number;
    /**
     * 
     * @type {Date}
     * @memberof SignatureQueryResult
     */
    date?: Date;
    /**
     * 
     * @type {string}
     * @memberof SignatureQueryResult
     */
    annotation?: string | null;
    /**
     * 
     * @type {string}
     * @memberof SignatureQueryResult
     */
    designation?: string | null;
    /**
     * 
     * @type {string}
     * @memberof SignatureQueryResult
     */
    drawnSign?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof SignatureQueryResult
     */
    principal?: boolean;
    /**
     * 
     * @type {string}
     * @memberof SignatureQueryResult
     */
    remarks?: string | null;
    /**
     * 
     * @type {number}
     * @memberof SignatureQueryResult
     */
    responsibleType?: number | null;
    /**
     * 
     * @type {string}
     * @memberof SignatureQueryResult
     */
    readonly responsibleTypeName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof SignatureQueryResult
     */
    responsibleName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof SignatureQueryResult
     */
    title?: string | null;
    /**
     * 
     * @type {number}
     * @memberof SignatureQueryResult
     */
    order?: number;
    /**
     * 
     * @type {boolean}
     * @memberof SignatureQueryResult
     */
    viewSign?: boolean;
}

export function SignatureQueryResultFromJSON(json: any): SignatureQueryResult {
    return SignatureQueryResultFromJSONTyped(json, false);
}

export function SignatureQueryResultFromJSONTyped(json: any, ignoreDiscriminator: boolean): SignatureQueryResult {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'date': !exists(json, 'date') ? undefined : (new Date(json['date'])),
        'annotation': !exists(json, 'annotation') ? undefined : json['annotation'],
        'designation': !exists(json, 'designation') ? undefined : json['designation'],
        'drawnSign': !exists(json, 'drawnSign') ? undefined : json['drawnSign'],
        'principal': !exists(json, 'principal') ? undefined : json['principal'],
        'remarks': !exists(json, 'remarks') ? undefined : json['remarks'],
        'responsibleType': !exists(json, 'responsibleType') ? undefined : json['responsibleType'],
        'responsibleTypeName': !exists(json, 'responsibleTypeName') ? undefined : json['responsibleTypeName'],
        'responsibleName': !exists(json, 'responsibleName') ? undefined : json['responsibleName'],
        'title': !exists(json, 'title') ? undefined : json['title'],
        'order': !exists(json, 'order') ? undefined : json['order'],
        'viewSign': !exists(json, 'viewSign') ? undefined : json['viewSign'],
    };
}

export function SignatureQueryResultToJSON(value?: SignatureQueryResult | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'date': value.date === undefined ? undefined : (value.date.toISOString()),
        'annotation': value.annotation,
        'designation': value.designation,
        'drawnSign': value.drawnSign,
        'principal': value.principal,
        'remarks': value.remarks,
        'responsibleType': value.responsibleType,
        'responsibleName': value.responsibleName,
        'title': value.title,
        'order': value.order,
        'viewSign': value.viewSign,
    };
}


