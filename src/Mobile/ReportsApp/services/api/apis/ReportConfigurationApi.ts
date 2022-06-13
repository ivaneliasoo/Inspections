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
    AddReportConfigurationCommand,
    AddReportConfigurationCommandFromJSON,
    AddReportConfigurationCommandToJSON,
    ProblemDetails,
    ProblemDetailsFromJSON,
    ProblemDetailsToJSON,
    ReportConfigurationDto,
    ReportConfigurationDtoFromJSON,
    ReportConfigurationDtoToJSON,
    ResumenReportConfiguration,
    ResumenReportConfigurationFromJSON,
    ResumenReportConfigurationToJSON,
    UpdateReportConfigurationCommand,
    UpdateReportConfigurationCommandFromJSON,
    UpdateReportConfigurationCommandToJSON,
} from '../models';

export interface ApiReportConfigurationGetRequest {
    filter?: string;
}

export interface ApiReportConfigurationIdDeleteRequest {
    id: number;
}

export interface ApiReportConfigurationIdGetRequest {
    id: number;
}

export interface ApiReportConfigurationIdPutRequest {
    id: number;
    updateReportConfigurationCommand?: UpdateReportConfigurationCommand;
}

export interface ApiReportConfigurationPostRequest {
    addReportConfigurationCommand?: AddReportConfigurationCommand;
}

/**
 * 
 */
export class ReportConfigurationApi extends runtime.BaseAPI {

    /**
     */
    async apiReportConfigurationGetRaw(requestParameters: ApiReportConfigurationGetRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<Array<ResumenReportConfiguration>>> {
        const queryParameters: any = {};

        if (requestParameters.filter !== undefined) {
            queryParameters['filter'] = requestParameters.filter;
        }

        const headerParameters: runtime.HTTPHeaders = {};

        if (this.configuration && this.configuration.accessToken) {
            const token = this.configuration.accessToken;
            const tokenString = await token("jwt_auth", []);

            if (tokenString) {
                headerParameters["Authorization"] = `Bearer ${tokenString}`;
            }
        }
        const response = await this.request({
            path: `/api/ReportConfiguration`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => jsonValue.map(ResumenReportConfigurationFromJSON));
    }

    /**
     */
    async apiReportConfigurationGet(requestParameters: ApiReportConfigurationGetRequest, initOverrides?: RequestInit): Promise<Array<ResumenReportConfiguration>> {
        const response = await this.apiReportConfigurationGetRaw(requestParameters, initOverrides);
        return await response.value();
    }

    /**
     */
    async apiReportConfigurationIdDeleteRaw(requestParameters: ApiReportConfigurationIdDeleteRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiReportConfigurationIdDelete.');
        }

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
            path: `/api/ReportConfiguration/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'DELETE',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiReportConfigurationIdDelete(requestParameters: ApiReportConfigurationIdDeleteRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiReportConfigurationIdDeleteRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiReportConfigurationIdGetRaw(requestParameters: ApiReportConfigurationIdGetRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<ReportConfigurationDto>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiReportConfigurationIdGet.');
        }

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
            path: `/api/ReportConfiguration/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => ReportConfigurationDtoFromJSON(jsonValue));
    }

    /**
     */
    async apiReportConfigurationIdGet(requestParameters: ApiReportConfigurationIdGetRequest, initOverrides?: RequestInit): Promise<ReportConfigurationDto> {
        const response = await this.apiReportConfigurationIdGetRaw(requestParameters, initOverrides);
        return await response.value();
    }

    /**
     */
    async apiReportConfigurationIdPutRaw(requestParameters: ApiReportConfigurationIdPutRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiReportConfigurationIdPut.');
        }

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
            path: `/api/ReportConfiguration/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: UpdateReportConfigurationCommandToJSON(requestParameters.updateReportConfigurationCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiReportConfigurationIdPut(requestParameters: ApiReportConfigurationIdPutRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiReportConfigurationIdPutRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiReportConfigurationPostRaw(requestParameters: ApiReportConfigurationPostRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
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
            path: `/api/ReportConfiguration`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: AddReportConfigurationCommandToJSON(requestParameters.addReportConfigurationCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiReportConfigurationPost(requestParameters: ApiReportConfigurationPostRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiReportConfigurationPostRaw(requestParameters, initOverrides);
    }

}
