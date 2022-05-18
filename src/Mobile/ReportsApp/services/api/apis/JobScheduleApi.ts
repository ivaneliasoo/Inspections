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


import * as runtime from '../runtime';
import {
    Job,
    JobFromJSON,
    JobToJSON,
    Options,
    OptionsFromJSON,
    OptionsToJSON,
    ProblemDetails,
    ProblemDetailsFromJSON,
    ProblemDetailsToJSON,
    SchedJob,
    SchedJobFromJSON,
    SchedJobToJSON,
    ScheduleData,
    ScheduleDataFromJSON,
    ScheduleDataToJSON,
    Team,
    TeamFromJSON,
    TeamToJSON,
} from '../models';

export interface ApiJobScheduleJobPutRequest {
    job?: Array<Job>;
}

export interface ApiJobScheduleOptionsPutRequest {
    options?: Options;
}

export interface ApiJobSchedulePutRequest {
    scheduleData?: ScheduleData;
}

export interface ApiJobScheduleSchedJobPutRequest {
    schedJob?: Array<SchedJob>;
}

export interface ApiJobScheduleTeamPutRequest {
    team?: Array<Team>;
}

/**
 * 
 */
export class JobScheduleApi extends runtime.BaseAPI {

    /**
     */
    async apiJobScheduleGetRaw(initOverrides?: RequestInit): Promise<runtime.ApiResponse<ScheduleData>> {
        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.accessToken) {
            const token = this.configuration.accessToken;
            const tokenString = await token("jwt_auth", []);

            if (tokenString) {
                headerParameters["Authorization"] = `Bearer ${tokenString}`;
            }
        }
        const response = await this.request({
            path: `/api/JobSchedule`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => ScheduleDataFromJSON(jsonValue));
    }

    /**
     */
    async apiJobScheduleGet(initOverrides?: RequestInit): Promise<ScheduleData> {
        const response = await this.apiJobScheduleGetRaw(initOverrides);
        return await response.value();
    }

    /**
     */
    async apiJobScheduleJobPutRaw(requestParameters: ApiJobScheduleJobPutRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        if (this.configuration && this.configuration.accessToken) {
            const token = this.configuration.accessToken;
            const tokenString = await token("jwt_auth", []);

            if (tokenString) {
                headerParameters["Authorization"] = `Bearer ${tokenString}`;
            }
        }
        const response = await this.request({
            path: `/api/JobSchedule/job`,
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: requestParameters.job.map(JobToJSON),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiJobScheduleJobPut(requestParameters: ApiJobScheduleJobPutRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiJobScheduleJobPutRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiJobScheduleOptionsPutRaw(requestParameters: ApiJobScheduleOptionsPutRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        if (this.configuration && this.configuration.accessToken) {
            const token = this.configuration.accessToken;
            const tokenString = await token("jwt_auth", []);

            if (tokenString) {
                headerParameters["Authorization"] = `Bearer ${tokenString}`;
            }
        }
        const response = await this.request({
            path: `/api/JobSchedule/options`,
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: OptionsToJSON(requestParameters.options),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiJobScheduleOptionsPut(requestParameters: ApiJobScheduleOptionsPutRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiJobScheduleOptionsPutRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiJobSchedulePutRaw(requestParameters: ApiJobSchedulePutRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<ScheduleData>> {
        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        if (this.configuration && this.configuration.accessToken) {
            const token = this.configuration.accessToken;
            const tokenString = await token("jwt_auth", []);

            if (tokenString) {
                headerParameters["Authorization"] = `Bearer ${tokenString}`;
            }
        }
        const response = await this.request({
            path: `/api/JobSchedule`,
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: ScheduleDataToJSON(requestParameters.scheduleData),
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => ScheduleDataFromJSON(jsonValue));
    }

    /**
     */
    async apiJobSchedulePut(requestParameters: ApiJobSchedulePutRequest, initOverrides?: RequestInit): Promise<ScheduleData> {
        const response = await this.apiJobSchedulePutRaw(requestParameters, initOverrides);
        return await response.value();
    }

    /**
     */
    async apiJobScheduleSchedJobPutRaw(requestParameters: ApiJobScheduleSchedJobPutRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        if (this.configuration && this.configuration.accessToken) {
            const token = this.configuration.accessToken;
            const tokenString = await token("jwt_auth", []);

            if (tokenString) {
                headerParameters["Authorization"] = `Bearer ${tokenString}`;
            }
        }
        const response = await this.request({
            path: `/api/JobSchedule/sched-job`,
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: requestParameters.schedJob.map(SchedJobToJSON),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiJobScheduleSchedJobPut(requestParameters: ApiJobScheduleSchedJobPutRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiJobScheduleSchedJobPutRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiJobScheduleTeamPutRaw(requestParameters: ApiJobScheduleTeamPutRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        if (this.configuration && this.configuration.accessToken) {
            const token = this.configuration.accessToken;
            const tokenString = await token("jwt_auth", []);

            if (tokenString) {
                headerParameters["Authorization"] = `Bearer ${tokenString}`;
            }
        }
        const response = await this.request({
            path: `/api/JobSchedule/team`,
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: requestParameters.team.map(TeamToJSON),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiJobScheduleTeamPut(requestParameters: ApiJobScheduleTeamPutRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiJobScheduleTeamPutRaw(requestParameters, initOverrides);
    }

}
