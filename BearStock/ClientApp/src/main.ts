import {enableProdMode} from '@angular/core';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';

import {AppModule} from './app/app.module';
import {environment} from './environments/environment';

export function getBaseUrl(): string {
  return document.getElementsByTagName('base')[0].href;
}

export function getApiUrl(): string {
  return environment.api_url;
}

export function getAuthUrl(): string {
  return environment.auth_url;
}

const providers = [
  {provide: 'BASE_URL', useFactory: getBaseUrl, deps: []},
  {provide: 'API_URL', useFactory: getApiUrl, deps: []},
  {provide: 'AUTH_URL', useFactory: getAuthUrl, deps: []}
];


if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.error(err));
