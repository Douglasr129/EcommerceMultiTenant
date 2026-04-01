import { loadRemoteModule } from '@angular-architects/module-federation';
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./pages/pages.routes').then((m)=> m.PAGES_ROUTES),
  },
  {
    path: 'features',
    loadChildren: () => import('./features/features.routes').then((m)=> m.FEATURES_ROUTE),
  },
    {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then((m)=> m.AUTH_ROUTES),
  },

];
