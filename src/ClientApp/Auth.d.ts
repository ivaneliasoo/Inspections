// Type definitions for @nuxtjs/auth 4.8
// Project: https://auth.nuxtjs.org
// Definitions by: Ruskin Constant
//                Daniel Leal
//                Nick Bolles
// Definitions: https://github.com/DefinitelyTyped/DefinitelyTyped
// TypeScript Version: 3.1

import { ReportsApi } from './services/api/api'

export interface Storage {
  setUniversal(key: string, value: any, isJson?: boolean): string;
  getUniversal(key: string, isJson?: boolean): any;
      syncUniversal(key: string, defaultValue: any, isJson?: boolean): any;
  // Local State
  setState(key: string, val: any): string;
  getState(key: string): string;
  watchState(key: string, handler: (newValue: any) => void): any;
  // Cookies
  setCookie(key: string, val: any, options?: object): any;
  getCookie(key: string, isJson?: boolean): any;
  // Local Storage
  setLocalStorage(key: string, val: any, isJson?: boolean): any;
  getLocalStorage(key: string, isJson?: boolean): any;
}

export interface Auth {
  ctx: any;
  $state: any;
  $storage: Storage;
  user: any;
  loggedIn: boolean;
  loginWith(strategyName: string, ...args: any): Promise<void>;
  login(...args: any): Promise<void>;
  logout(): Promise<void>;
  setUserToken(token: string): Promise<void>;
  fetchUser(): Promise<void>;
  fetchUserOnce(): Promise<void>;
  hasScope(scopeName: string): boolean;
  setToken(strategyName: string, token?: string): string;
  getToken(strategyName: string): string;
  syncToken(strategyName: string): string;
  onError(handler: (error: Error, name: string, endpoint: any) => void): any;
  setUser(user?: any): any;
  reset(): Promise<void>;
  redirect(name: string): any;
}

declare module 'vue/types/vue' {
  interface Vue {
    $auth: Auth;
    $notificationsHub: any;
    $reportsApi: ReportsApi;
    $img: any;
    auth: any;
  }
}

declare module '@nuxt/types' {
  interface Context {
    $auth: Auth;
    $reportsApi: ReportsApi;
  }
}
