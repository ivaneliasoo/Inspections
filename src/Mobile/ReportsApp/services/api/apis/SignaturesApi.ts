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
    AddSignatureCommand,
    AddSignatureCommandFromJSON,
    AddSignatureCommandToJSON,
    EditSignatureCommand,
    EditSignatureCommandFromJSON,
    EditSignatureCommandToJSON,
    ProblemDetails,
    ProblemDetailsFromJSON,
    ProblemDetailsToJSON,
    SignatureDto,
    SignatureDtoFromJSON,
    SignatureDtoToJSON,
} from '../models';

export interface ApiSignaturesGetRequest {
    filter?: string;
    reportConfigurationId?: number;
    reportId?: number;
    inConfigurationOnly?: boolean;
}

export interface ApiSignaturesIdDeleteRequest {
    id: number;
}

export interface ApiSignaturesIdGetRequest {
    id: number;
}

export interface ApiSignaturesIdPutRequest {
    id: number;
    editSignatureCommand?: EditSignatureCommand;
}

export interface ApiSignaturesPostRequest {
    addSignatureCommand?: AddSignatureCommand;
}

/**
 * 
 */
export class SignaturesApi extends runtime.BaseAPI {

    /**
     */
    async apiSignaturesGetRaw(requestParameters: ApiSignaturesGetRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<Array<SignatureDto>>> {
        const queryParameters: any = {};

        if (requestParameters.filter !== undefined) {
            queryParameters['filter'] = requestParameters.filter;
        }

        if (requestParameters.reportConfigurationId !== undefined) {
            queryParameters['reportConfigurationId'] = requestParameters.reportConfigurationId;
        }

        if (requestParameters.reportId !== undefined) {
            queryParameters['reportId'] = requestParameters.reportId;
        }

        if (requestParameters.inConfigurationOnly !== undefined) {
            queryParameters['inConfigurationOnly'] = requestParameters.inConfigurationOnly;
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
            path: `/api/Signatures`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => jsonValue.map(SignatureDtoFromJSON));
    }

    /**
     */
    async apiSignaturesGet(requestParameters: ApiSignaturesGetRequest, initOverrides?: RequestInit): Promise<Array<SignatureDto>> {
        const response = await this.apiSignaturesGetRaw(requestParameters, initOverrides);
        return await response.value();
    }

    /**
     */
    async apiSignaturesIdDeleteRaw(requestParameters: ApiSignaturesIdDeleteRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiSignaturesIdDelete.');
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
            path: `/api/Signatures/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'DELETE',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiSignaturesIdDelete(requestParameters: ApiSignaturesIdDeleteRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiSignaturesIdDeleteRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiSignaturesIdGetRaw(requestParameters: ApiSignaturesIdGetRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiSignaturesIdGet.');
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
            path: `/api/Signatures/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiSignaturesIdGet(requestParameters: ApiSignaturesIdGetRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiSignaturesIdGetRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiSignaturesIdPutRaw(requestParameters: ApiSignaturesIdPutRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling apiSignaturesIdPut.');
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
            path: `/api/Signatures/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: EditSignatureCommandToJSON(requestParameters.editSignatureCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiSignaturesIdPut(requestParameters: ApiSignaturesIdPutRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiSignaturesIdPutRaw(requestParameters, initOverrides);
    }

    /**
     */
    async apiSignaturesPostRaw(requestParameters: ApiSignaturesPostRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
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
            path: `/api/Signatures`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: AddSignatureCommandToJSON(requestParameters.addSignatureCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async apiSignaturesPost(requestParameters: ApiSignaturesPostRequest, initOverrides?: RequestInit): Promise<void> {
        await this.apiSignaturesPostRaw(requestParameters, initOverrides);
    }

}
