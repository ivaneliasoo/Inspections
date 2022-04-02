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
    AddCheckListCommand,
    AddCheckListCommandFromJSON,
    AddCheckListCommandToJSON,
    AddCheckListItemCommand,
    AddCheckListItemCommandFromJSON,
    AddCheckListItemCommandToJSON,
    CheckListQueryResult,
    ProblemDetails,
    ProblemDetailsFromJSON,
    ProblemDetailsToJSON,
    UpdateCheckListCommand,
    UpdateCheckListCommandFromJSON,
    UpdateCheckListCommandToJSON,
    UpdateCheckListItemCommand,
    UpdateCheckListItemCommandFromJSON,
    UpdateCheckListItemCommandToJSON,
} from '../models';

export interface AddItemToChecklistRequest {
    id: number;
    addCheckListItemCommand?: AddCheckListItemCommand;
}

export interface CreateCheckListRequest {
    addCheckListCommand?: AddCheckListCommand;
}

export interface DeleteChecklistRequest {
    id: number;
}

export interface DeleteChecklistItemRequest {
    id: number;
    idItem: number;
}

export interface GetCheckListbyIdRequest {
    id: number;
}

export interface GetChecklistsRequest {
    filter?: string;
    reportConfigurationId?: number;
    reportId?: number;
    inConfigurationOnly?: boolean;
}

export interface UpdateChecklistRequest {
    id: number;
    updateCheckListCommand?: UpdateCheckListCommand;
}

export interface UpdateChecklistItemRequest {
    id: number;
    idItem: number;
    updateCheckListItemCommand?: UpdateCheckListItemCommand;
}

/**
 * 
 */
export class CheckListsApi extends runtime.BaseAPI {

    /**
     */
    async addItemToChecklistRaw(requestParameters: AddItemToChecklistRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling addItemToChecklist.');
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
            path: `/api/CheckLists/{id}/items`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: AddCheckListItemCommandToJSON(requestParameters.addCheckListItemCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async addItemToChecklist(requestParameters: AddItemToChecklistRequest, initOverrides?: RequestInit): Promise<void> {
        await this.addItemToChecklistRaw(requestParameters, initOverrides);
    }

    /**
     */
    async createCheckListRaw(requestParameters: CreateCheckListRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
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
            path: `/api/CheckLists`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: AddCheckListCommandToJSON(requestParameters.addCheckListCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async createCheckList(requestParameters: CreateCheckListRequest, initOverrides?: RequestInit): Promise<void> {
        await this.createCheckListRaw(requestParameters, initOverrides);
    }

    /**
     */
    async deleteChecklistRaw(requestParameters: DeleteChecklistRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling deleteChecklist.');
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
            path: `/api/CheckLists/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'DELETE',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async deleteChecklist(requestParameters: DeleteChecklistRequest, initOverrides?: RequestInit): Promise<void> {
        await this.deleteChecklistRaw(requestParameters, initOverrides);
    }

    /**
     */
    async deleteChecklistItemRaw(requestParameters: DeleteChecklistItemRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling deleteChecklistItem.');
        }

        if (requestParameters.idItem === null || requestParameters.idItem === undefined) {
            throw new runtime.RequiredError('idItem','Required parameter requestParameters.idItem was null or undefined when calling deleteChecklistItem.');
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
            path: `/api/CheckLists/{id}/items/{idItem}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))).replace(`{${"idItem"}}`, encodeURIComponent(String(requestParameters.idItem))),
            method: 'DELETE',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async deleteChecklistItem(requestParameters: DeleteChecklistItemRequest, initOverrides?: RequestInit): Promise<void> {
        await this.deleteChecklistItemRaw(requestParameters, initOverrides);
    }

    /**
     */
    async getCheckListbyIdRaw(requestParameters: GetCheckListbyIdRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<CheckListQueryResult>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling getCheckListbyId.');
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
            path: `/api/CheckLists/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response);
    }

    /**
     */
    async getCheckListbyId(requestParameters: GetCheckListbyIdRequest, initOverrides?: RequestInit): Promise<CheckListQueryResult> {
        return await (await this.getCheckListbyIdRaw(requestParameters, initOverrides)).value();
    }

    /**
     */
    async getChecklistsRaw(requestParameters: GetChecklistsRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<CheckListQueryResult>> {
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
            path: `/api/CheckLists`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response);
    }

    /**
     */
    async getChecklists(requestParameters: GetChecklistsRequest, initOverrides?: RequestInit): Promise<CheckListQueryResult> {
        return await (await this.getChecklistsRaw(requestParameters, initOverrides)).value();
    }

    /**
     */
    async updateChecklistRaw(requestParameters: UpdateChecklistRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling updateChecklist.');
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
            path: `/api/CheckLists/{id}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))),
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: UpdateCheckListCommandToJSON(requestParameters.updateCheckListCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async updateChecklist(requestParameters: UpdateChecklistRequest, initOverrides?: RequestInit): Promise<void> {
        await this.updateChecklistRaw(requestParameters, initOverrides);
    }

    /**
     */
    async updateChecklistItemRaw(requestParameters: UpdateChecklistItemRequest, initOverrides?: RequestInit): Promise<runtime.ApiResponse<void>> {
        if (requestParameters.id === null || requestParameters.id === undefined) {
            throw new runtime.RequiredError('id','Required parameter requestParameters.id was null or undefined when calling updateChecklistItem.');
        }

        if (requestParameters.idItem === null || requestParameters.idItem === undefined) {
            throw new runtime.RequiredError('idItem','Required parameter requestParameters.idItem was null or undefined when calling updateChecklistItem.');
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
            path: `/api/CheckLists/{id}/items/{idItem}`.replace(`{${"id"}}`, encodeURIComponent(String(requestParameters.id))).replace(`{${"idItem"}}`, encodeURIComponent(String(requestParameters.idItem))),
            method: 'PUT',
            headers: headerParameters,
            query: queryParameters,
            body: UpdateCheckListItemCommandToJSON(requestParameters.updateCheckListItemCommand),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async updateChecklistItem(requestParameters: UpdateChecklistItemRequest, initOverrides?: RequestInit): Promise<void> {
        await this.updateChecklistItemRaw(requestParameters, initOverrides);
    }

}