import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Banner } from './shared/banner/banner';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '',
    component: Banner,
    children: [
      {
        path: 'login',
        loadComponent: () => import('./auth/login/login').then(m => m.Login)
      },
      {
        path: 'dashboard',
        canActivate: [authGuard],
        loadComponent: () => import('./dashboard/dashboard').then(m => m.Dashboard)
      },
      {
        path: 'auftraege',
        canActivate: [authGuard],
        loadComponent: () => import('./auftraege/liste/liste').then(m => m.Liste)
      },
      {
        path: 'auftraege/anlegen',
        canActivate: [authGuard],
        loadComponent: () => import('./auftraege/anlegen/anlegen').then(m => m.Anlegen)
      },
      {
        path: 'auftraege/bearbeiten/:id',
        canActivate: [authGuard],
        loadComponent: () => import('./auftraege/anlegen/anlegen').then(m => m.Anlegen)
      }
    ]
  }
];
