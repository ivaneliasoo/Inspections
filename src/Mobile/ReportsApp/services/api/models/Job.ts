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
 * @interface Job
 */
export interface Job {
    /**
     * 
     * @type {number}
     * @memberof Job
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    status?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    value?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    salesPerson?: string | null;
    /**
     * 
     * @type {number}
     * @memberof Job
     */
    priority?: number;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    scope?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    tag?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    comments?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    teams?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    teamCount?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    duration?: string | null;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    shift?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof Job
     */
    updated?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof Job
     */
    lastUpdate?: Date | null;
    /**
     * 
     * @type {Date}
     * @memberof Job
     */
    lastEdit?: Date;
    /**
     * 
     * @type {string}
     * @memberof Job
     */
    lastEditUser?: string | null;
}

export function JobFromJSON(json: any): Job {
    return JobFromJSONTyped(json, false);
}

export function JobFromJSONTyped(json: any, ignoreDiscriminator: boolean): Job {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'status': !exists(json, 'status') ? undefined : json['status'],
        'value': !exists(json, 'value') ? undefined : json['value'],
        'salesPerson': !exists(json, 'salesPerson') ? undefined : json['salesPerson'],
        'priority': !exists(json, 'priority') ? undefined : json['priority'],
        'scope': !exists(json, 'scope') ? undefined : json['scope'],
        'tag': !exists(json, 'tag') ? undefined : json['tag'],
        'comments': !exists(json, 'comments') ? undefined : json['comments'],
        'teams': !exists(json, 'teams') ? undefined : json['teams'],
        'teamCount': !exists(json, 'teamCount') ? undefined : json['teamCount'],
        'duration': !exists(json, 'duration') ? undefined : json['duration'],
        'shift': !exists(json, 'shift') ? undefined : json['shift'],
        'updated': !exists(json, 'updated') ? undefined : json['updated'],
        'lastUpdate': !exists(json, 'lastUpdate') ? undefined : (json['lastUpdate'] === null ? null : new Date(json['lastUpdate'])),
        'lastEdit': !exists(json, 'lastEdit') ? undefined : (new Date(json['lastEdit'])),
        'lastEditUser': !exists(json, 'lastEditUser') ? undefined : json['lastEditUser'],
    };
}

export function JobToJSON(value?: Job | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'status': value.status,
        'value': value.value,
        'salesPerson': value.salesPerson,
        'priority': value.priority,
        'scope': value.scope,
        'tag': value.tag,
        'comments': value.comments,
        'teams': value.teams,
        'teamCount': value.teamCount,
        'duration': value.duration,
        'shift': value.shift,
        'updated': value.updated,
        'lastUpdate': value.lastUpdate === undefined ? undefined : (value.lastUpdate === null ? null : value.lastUpdate.toISOString()),
        'lastEdit': value.lastEdit === undefined ? undefined : (value.lastEdit.toISOString()),
        'lastEditUser': value.lastEditUser,
    };
}


