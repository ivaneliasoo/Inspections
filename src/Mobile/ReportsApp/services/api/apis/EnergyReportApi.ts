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
    CurrentTable,
    CurrentTableFromJSON,
    CurrentTableToJSON,
    ProblemDetails,
    ProblemDetailsFromJSON,
    ProblemDetailsToJSON,
} from '../models';

export interface ApiEnergyReportCurrentTableStartDateEndDateGetRequest {
    startDate: string;
    endDate: string;
}

/**
 * 
 */
export class EnergyReportApi extends runtime.BaseAPI {

    /**
     */
    async apiEnergyReportBackgroundGetRaw(initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
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
            path: `/api/EnergyReport/background`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiEnergyReportBackgroundGet(initOverrides?: RequestInit): Promise<void> {
        await this.apiEnergyReportBackgroundGetRaw(initOverrides);
    }

    /**
     */
    async apiEnergyReportCategoryGetRaw(initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
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
            path: `/api/EnergyReport/category`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiEnergyReportCategoryGet(initOverrides?: RequestInit): Promise<void> {
        await this.apiEnergyReportCategoryGetRaw(initOverrides);
    }

    /**
     */
    async apiEnergyReportCategoryPostRaw(initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
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
            path: `/api/EnergyReport/category`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiEnergyReportCategoryPost(initOverrides?: RequestInit): Promise<void> {
        await this.apiEnergyReportCategoryPostRaw(initOverrides);
    }

    /**
     */
    async apiEnergyReportCurrentTablePostRaw(initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
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
            path: `/api/EnergyReport/current-table`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiEnergyReportCurrentTablePost(initOverrides?: RequestInit): Promise<void> {
        await this.apiEnergyReportCurrentTablePostRaw(initOverrides);
    }

    /**
     */
    async apiEnergyReportCurrentTableStartDateEndDateGetRaw(requestParameters: ApiEnergyReportCurrentTableStartDateEndDateGetRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<Array<CurrentTable>>> {
        if (requestParameters.startDate === null || requestParameters.startDate === undefined) {
            throw new runtime.RequiredError('startDate','Required parameter requestParameters.startDate was null or undefined when calling apiEnergyReportCurrentTableStartDateEndDateGet.');
        }

        if (requestParameters.endDate === null || requestParameters.endDate === undefined) {
            throw new runtime.RequiredError('endDate','Required parameter requestParameters.endDate was null or undefined when calling apiEnergyReportCurrentTableStartDateEndDateGet.');
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
            path: `/api/EnergyReport/current-table/{startDate}/{endDate}`.replace(`{${"startDate"}}`, encodeURIComponent(String(requestParameters.startDate))).replace(`{${"endDate"}}`, encodeURIComponent(String(requestParameters.endDate))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => jsonValue.map(CurrentTableFromJSON));
    }

    /**
     */
    async apiEnergyReportCurrentTableStartDateEndDateGet(requestParameters: ApiEnergyReportCurrentTableStartDateEndDateGetRequest, initOverrides?: RequestInit): Promise<Array<CurrentTable>> {
        const response = await this.apiEnergyReportCurrentTableStartDateEndDateGetRaw(requestParameters, initOverrides);
        return await response.value();
    }

}