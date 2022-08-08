// Type definitions for @nuxtjs/auth 4.8
// Project: https://auth.nuxtjs.org
// Definitions by: Ruskin Constant
//                Daniel Leal
//                Nick Bolles
// Definitions: https://github.com/DefinitelyTyped/DefinitelyTyped
// TypeScript Version: 3.1

import {
  FormsApi,
  ReportConfigurationApi,
  ReportsApi,
} from './services/api/api'

export interface Storage {
  setUniversal(key: string, value: any, isJson?: boolean): string
  getUniversal(key: string, isJson?: boolean): any
  syncUniversal(key: string, defaultValue: any, isJson?: boolean): any
  // Local State
  setState(key: string, val: any): string
  getState(key: string): string
  watchState(key: string, handler: (newValue: any) => void): any
  // Cookies
  setCookie(key: string, val: any, options?: object): any
  getCookie(key: string, isJson?: boolean): any
  // Local Storage
  setLocalStorage(key: string, val: any, isJson?: boolean): any
  getLocalStorage(key: string, isJson?: boolean): any
}

declare module 'vue/types/vue' {
  interface Vue {
    $notificationsHub: any
    $reportsApi: ReportsApi
    $formsApi: FormsApi
    $reportsConfigApi: ReportConfigurationApi
    $img: any
    auth: any
    $device: any
  }
}

declare module '@nuxt/types' {
  interface Context {
    // $auth: Auth;
    $reportsApi: ReportsApi
    $formsApi: FormsApi
    $reportsConfigApi: ReportConfigurationApi
    $device: any
  }
}
