import { provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { App } from './app/app';
import { LoginPage } from './app/auth/login-page/login-page';
import { CatalogPage } from './app/catalog/catalog-page/catalog-page';

bootstrapApplication(App, {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(),
    provideRouter([
      { path: '', component: LoginPage },
      { path: 'catalog', component: CatalogPage }
    ]),
  ]
})
  .catch(err => console.error(err));
