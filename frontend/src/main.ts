import { provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { App } from './app/app';
import { LoginPage } from './app/auth/login-page/login-page';
import { CatalogPage } from './app/catalog/catalog-page/catalog-page';
import { HomePage } from './app/home/home-page/home-page';
import { MovieDetailsPage } from './app/movies/movie-details-page/movie-details-page';

bootstrapApplication(App, {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(),
    provideRouter([
      { path: '', component: HomePage },
      { path: 'login', component: LoginPage },
      { path: 'catalog', component: CatalogPage },
      { path: 'movies/:id', component: MovieDetailsPage }
    ]),
  ]
})
  .catch(err => console.error(err));
