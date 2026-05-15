import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./layout/shell.component').then((m) => m.ShellComponent),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./pages/catalog/catalog.component').then(
            (m) => m.CatalogComponent
          ),
      },
      {
        path: 'cars/:id',
        loadComponent: () =>
          import('./pages/car-detail/car-detail.component').then(
            (m) => m.CarDetailComponent
          ),
      },
      {
        path: 'login',
        loadComponent: () =>
          import('./pages/login/login.component').then((m) => m.LoginComponent),
      },
      {
        path: 'register',
        loadComponent: () =>
          import('./pages/register/register.component').then(
            (m) => m.RegisterComponent
          ),
      },
      {
        path: 'sell',
        canActivate: [authGuard],
        loadComponent: () =>
          import('./pages/sell/sell.component').then((m) => m.SellComponent),
      },
      {
        path: 'my-cars',
        canActivate: [authGuard],
        loadComponent: () =>
          import('./pages/my-cars/my-cars.component').then(
            (m) => m.MyCarsComponent
          ),
      },
      {
        path: 'favorites',
        canActivate: [authGuard],
        loadComponent: () =>
          import('./pages/favorites/favorites.component').then(
            (m) => m.FavoritesComponent
          ),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
