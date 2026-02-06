import { Routes } from '@angular/router';
import { RoleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  { path: '', loadChildren: () => import('./pages/pages.routes').then(m => m.PAGES_ROUTES) },
  { path: 'admin', loadChildren: () => import('./domain/admin/admin-module').then(m => m.AdminModule), canActivate: [RoleGuard], data: { roles: ['Admin'] } },
  { path: 'seller', loadChildren: () => import('./domain/seller/seller-module').then(m => m.SellerModule), canActivate: [RoleGuard], data: { roles: ['Seller'] } },
  { path: 'client', loadChildren: () => import('./domain/client/client-module').then(m => m.ClientModule), canActivate: [RoleGuard], data: { roles: ['Client'] } },
  { path: '**', redirectTo: '' }

];
