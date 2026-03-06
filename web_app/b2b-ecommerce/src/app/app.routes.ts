import { Routes } from '@angular/router';
import { RoleGuard } from './core/guards/role.guard';


export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./pages/pages.routes').then((m) => m.PAGES_ROUTES),
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./domain/auth/auth.routes').then((m) => m.AUTH_ROUTES),
  },
  {
    path: 'seller',
    loadChildren: () =>
      import('./domain/seller/seller.routes').then((m) => m.SELLER_ROUTES),
    canActivate: [RoleGuard],
    data: { roles: ['SELLER'] },
  },
  {
    path: 'catalog',
    loadChildren: () =>
      import('./domain/catalog/catalog.routes').then((m) => m.CATALOG_ROUTES),
    canActivate: [RoleGuard],
    data: { roles: ['SELLER'] },
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./domain/admin/admin-module').then((m) => m.AdminModule),
    canActivate: [RoleGuard],
    data: { roles: ['ADMIN'] },
  },
  {
    path: 'client',
    loadChildren: () =>
      import('./domain/client/client.routes').then((m) => m.CLIENT_ROUTES),
    canActivate: [RoleGuard],
    data: { roles: ['CLIENT'] },
  },
  { path: '**', redirectTo: '' },
];
