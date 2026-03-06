import { Routes } from '@angular/router';
import { Catalog } from './pages/catalog/catalog';
import { Produtos } from './pages/produtos/produtos';

export const CATALOG_ROUTES: Routes = [
  { path: '', component: Catalog },
  { path: 'products', component: Produtos },
  { path: '**', redirectTo: '' },
];
