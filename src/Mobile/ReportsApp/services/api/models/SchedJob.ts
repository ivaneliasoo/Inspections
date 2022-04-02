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
 * @interface SchedJob
 */
export interface SchedJob {
    /**
     * 
     * @type {number}
     * @memberof SchedJob
     */
    id?: number;
    /**
     * 
     * @type {number}
     * @memberof SchedJob
     */
    team?: number;
    /**
     * 
     * @type {Date}
     * @memberof SchedJob
     */
    date: Date;
    /**
     * 
     * @type {string}
     * @memberof SchedJob
     */
    shift?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof SchedJob
     */
    splitShift?: boolean;
    /**
     * 
     * @type {string}
     * @memberof SchedJob
     */
    job1?: string | null;
    /**
     * 
     * @type {string}
     * @memberof SchedJob
     */
    job2?: string | null;
    /**
     * 
     * @type {string}
     * @memberof SchedJob
     */
    teamMembers?: string | null;
    /**
     * 
     * @type {boolean}
     * @memberof SchedJob
     */
    excludeSaturday?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof SchedJob
     */
    excludeSunday?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof SchedJob
     */
    lastUpdate?: Date | null;
    /**
     * 
     * @type {boolean}
     * @memberof SchedJob
     */
    updated?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof SchedJob
     */
    lastEdit?: Date;
    /**
     * 
     * @type {string}
     * @memberof SchedJob
     */
    lastEditUser?: string | null;
}

export function SchedJobFromJSON(json: any): SchedJob {
    return SchedJobFromJSONTyped(json, false);
}

export function SchedJobFromJSONTyped(json: any, ignoreDiscriminator: boolean): SchedJob {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'id': !exists(json, 'id') ? undefined : json['id'],
        'team': !exists(json, 'team') ? undefined : json['team'],
        'date': (new Date(json['date'])),
        'shift': !exists(json, 'shift') ? undefined : json['shift'],
        'splitShift': !exists(json, 'splitShift') ? undefined : json['splitShift'],
        'job1': !exists(json, 'job1') ? undefined : json['job1'],
        'job2': !exists(json, 'job2') ? undefined : json['job2'],
        'teamMembers': !exists(json, 'teamMembers') ? undefined : json['teamMembers'],
        'excludeSaturday': !exists(json, 'excludeSaturday') ? undefined : json['excludeSaturday'],
        'excludeSunday': !exists(json, 'excludeSunday') ? undefined : json['excludeSunday'],
        'lastUpdate': !exists(json, 'lastUpdate') ? undefined : (json['lastUpdate'] === null ? null : new Date(json['lastUpdate'])),
        'updated': !exists(json, 'updated') ? undefined : json['updated'],
        'lastEdit': !exists(json, 'lastEdit') ? undefined : (new Date(json['lastEdit'])),
        'lastEditUser': !exists(json, 'lastEditUser') ? undefined : json['lastEditUser'],
    };
}

export function SchedJobToJSON(value?: SchedJob | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'id': value.id,
        'team': value.team,
        'date': (value.date.toISOString().substr(0,10)),
        'shift': value.shift,
        'splitShift': value.splitShift,
        'job1': value.job1,
        'job2': value.job2,
        'teamMembers': value.teamMembers,
        'excludeSaturday': value.excludeSaturday,
        'excludeSunday': value.excludeSunday,
        'lastUpdate': value.lastUpdate === undefined ? undefined : (value.lastUpdate === null ? null : value.lastUpdate.toISOString()),
        'updated': value.updated,
        'lastEdit': value.lastEdit === undefined ? undefined : (value.lastEdit.toISOString()),
        'lastEditUser': value.lastEditUser,
    };
}

